using System;
using System.Linq.Expressions;
using System.Reflection;
using RecentList;

namespace LinqReflection
{
	public static class Reflect<T>
	{
		private static MruDictionary<string, Func<T, object>> _getters;
		private static readonly Type _type = typeof(T);
		private static readonly string _typeName = typeof(T).FullName;

		static Reflect()
		{
			var initialMaxCapacity = 128;
			_getters = new MruDictionary<string, Func<T, object>>(initialMaxCapacity);
		}

		public static int MaxCapacity
		{
			get { return _getters.MaxCapacity; }
			set { _getters.MaxCapacity = value; }
		}

		public static int Count
		{
			get { return _getters.Count; }
		}

		/// <exception cref="ArgumentOutOfRangeException"><c>memberName</c> is out of range.</exception>
		public static Func<T, object> TryGetGetter(string memberName)
		{
			Func<T, object> func;
			if (!_getters.TryGetValue(memberName, out func))
			{
				func = buildGetterExpressionForName(memberName).Compile();
				_getters[memberName] = func;
			}
			return func;
		}

		/// <exception cref="ArgumentOutOfRangeException"><c>memberName</c> is out of range.</exception>
		private static Expression<Func<T, object >> buildGetterExpressionForName(string memberName)
		{
			var param = Expression.Parameter(_type, "x");
			var key = (MemberInfo)_type.GetProperty(memberName) ?? _type.GetField(memberName);
			if (key == null)
				throw new ArgumentOutOfRangeException("memberName",
					string.Format("The type {0} has no public property or field named {1}", _typeName, memberName));
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
		public static string GetMemberName<TModel, TPropetyType>(Expression<Func<TModel, TPropetyType>> propertySelector)
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


		public static Func<T, object> TryGetGetter(Expression<Func<T, object>> member)
		{
			Func<T, object> func;
			string memberName = GetMemberName(member);
			if (!_getters.TryGetValue(memberName, out func))
			{
				func = member.Compile();
				_getters[memberName] = func;
			}
			return func;
		}

		public static bool ContainsKey(string memberName)
		{
			return _getters.ContainsKey(memberName);
		}
	}
}