using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using RecentList;

namespace LinqReflection
{
	public class Reflect
	{
		private static readonly Dictionary<string,StaticReflection> s_reflections = new Dictionary<string, StaticReflection>();

		/// <exception cref="ArgumentNullException"><paramref name="instance" /> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">Couldn't get a type name</exception>
		public static StaticReflection ByInstance(object instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");
			var type = instance.GetType();
			var typeName = type.FullName;
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentException("Couldn't get a type name");
			StaticReflection reflection;
			if (!s_reflections.TryGetValue(typeName, out reflection))
			{
				reflection = new StaticReflection(type);
				s_reflections[typeName] = reflection;
			}
			return reflection;
		}

		public static StaticReflection ByType<T>()
		{
			return ByStrongType<T>();
		}

		/// <exception cref="ArgumentException">Couldn't get a type name</exception>
		public static StaticReflection<T> ByStrongType<T>()
		{
			var type = typeof(T);
			var typeName = type.FullName;
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentException("Couldn't get a type name");
			StaticReflection reflection;
			if (!s_reflections.TryGetValue(typeName, out reflection))
			{
				reflection = new StaticReflection<T>();
				s_reflections[typeName] = reflection;
			}
			return (StaticReflection<T>)reflection;
		}
	}

	public class StaticReflection
	{
		protected const int InitialMaxCapacity = 128;
		private readonly MruDictionary<string, Func<object, object>> _Getters;
		protected Type _Type;
		protected string _TypeName;

		public StaticReflection(Type type) : this()
		{
			_Type = type;
			_TypeName = type.FullName;
		}

		public StaticReflection()
		{
			_Getters = new MruDictionary<string, Func<object, object>>(InitialMaxCapacity);			
		}

		public int MaxCapacity
		{
			get { return _Getters.MaxCapacity; }
			set { _Getters.MaxCapacity = value; }
		}

		public int Count
		{
			get { return _Getters.Count; }
		}

		public Func<object, object> GetGetterOrNull(string memberName)
		{
			Func<object, object> getter;
			if (TryGetGetter(memberName, out getter))
				return getter;
			return null;
		}

		/// <exception cref="ArgumentOutOfRangeException"><c>memberName</c> is out of range.</exception>
		public bool TryGetGetter(string memberName, out Func<object, object> getter)
		{
			if (!_Getters.TryGetValue(memberName, out getter))
			{
				var param = Expression.Parameter(typeof(object), "x");
				var convertParam = Expression.Convert(param, _Type);
				var key = (MemberInfo)_Type.GetProperty(memberName) ?? _Type.GetField(memberName);
				if (key == null)
					return false;
				var body = Expression.MakeMemberAccess(convertParam, key);
				var convert = Expression.Convert(body, typeof(object));
				getter = Expression.Lambda<Func<object, object>>(convert, param).Compile();
				_Getters[memberName] = getter;
			}

			return true;
		}


		public bool ContainsKey(string memberName)
		{
			return _Getters.ContainsKey(memberName);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class StaticReflection<T> : StaticReflection
	{
		private readonly MruDictionary<string, Func<T, object>> _Getters;

		public StaticReflection()
		{
			_Type = typeof(T);
			_TypeName = typeof(T).FullName;
			_Getters = new MruDictionary<string, Func<T, object>>(InitialMaxCapacity);
		}


		public new int MaxCapacity
		{
			get { return _Getters.MaxCapacity; }
			set { _Getters.MaxCapacity = value; }
		}

		public new int Count
		{
			get { return _Getters.Count; }
		}

		/// <exception cref="ArgumentOutOfRangeException"><c>memberName</c> is out of range.</exception>
		public Func<T, object> TryGetGetter(string memberName)
		{
			Func<T, object> func;
			if (!_Getters.TryGetValue(memberName, out func))
			{
				func = buildGetterExpressionForName(memberName).Compile();
				_Getters[memberName] = func;
			}
			return func;
		}

		/// <exception cref="ArgumentOutOfRangeException"><c>memberName</c> is out of range.</exception>
		private Expression<Func<T, object >> buildGetterExpressionForName(string memberName)
		{
			var param = Expression.Parameter(_Type, "x");
			var key = (MemberInfo)_Type.GetProperty(memberName) ?? _Type.GetField(memberName);
			if (key == null)
				throw new ArgumentOutOfRangeException("memberName",
					string.Format("The type {0} has no public property or field named {1}", _TypeName, memberName));
			var body = Expression.MakeMemberAccess(param, key);
			var convert = Expression.Convert(body, typeof(object));
			return Expression.Lambda<Func<T, object>>(convert, param);
		}

		/// <summary>
		/// Gets the name of the member (property) from <paramref name="propertySelector"/> expression.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <typeparam name="TPropetyType">The propety type.</typeparam>
		/// <param name="propertySelector">The property selector expression.</param>
		/// <returns>The property name for the specified property accessor expression or empty string if failed.</returns>
		/// <exception cref="ArgumentException">member does not represent a field or property</exception>
		public string GetMemberName<TModel, TPropetyType>(Expression<Func<TModel, TPropetyType>> propertySelector)
		{
			if (propertySelector == null)
				return string.Empty;

			MemberExpression memberExpression;
			switch (propertySelector.Body.NodeType)
			{
				case ExpressionType.Convert:
					memberExpression = ((UnaryExpression)propertySelector.Body).Operand as MemberExpression;
					break;
				case ExpressionType.MemberAccess:
					memberExpression = propertySelector.Body as MemberExpression;
					break;
				default:
					throw new ArgumentException("member does not represent a field or property", "propertySelector");
			}


			return memberExpression != null ? memberExpression.Member.Name : string.Empty;
		}

		public Func<T, object> TryGetGetter(Expression<Func<T, object>> member)
		{
			Func<T, object> func;
			string memberName = GetMemberName(member);
			if (!_Getters.TryGetValue(memberName, out func))
			{
				func = member.Compile();
				_Getters[memberName] = func;
			}
			return func;
		}

		public new bool ContainsKey(string memberName)
		{
			return _Getters.ContainsKey(memberName);
		}
	}
}