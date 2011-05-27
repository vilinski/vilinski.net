using System;
using System.Linq.Expressions;
using System.Reflection;
using RecentList;

namespace LinqReflection.Property
{
	/// <summary>
	/// Implements fast property access.
	/// </summary>
	/// <typeparam name="T">Declaring type.</typeparam>
	/// <typeparam name="TProperty">Property type</typeparam>
	public class FastProperty<T, TProperty> : IFastProperty<T, TProperty>
	{
		/// <summary>
		/// Gets or sets the property.
		/// </summary>
		/// <value>The property.</value>
		public PropertyInfo Property { get; protected set; }

		/// <summary>
		/// Gets or sets the type of the parameter being passed in as the instance.
		/// </summary>
		/// <value>The type of the instance.</value>
		public Type InstanceType { get; private set; }

		/// <summary>
		/// Gets or sets the type of value being returned by the property.
		/// </summary>
		/// <value>The type of the return.</value>
		public Type ReturnType { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance can read.
		/// </summary>
		/// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
		public bool CanRead
		{
			get
			{
				return Property.CanRead;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance can write.
		/// </summary>
		/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
		public bool CanWrite
		{
			get
			{
				return Property.CanWrite;
			}
		}

		/// <summary>
		/// Makes the fast property for the specified <see cref="System.Reflection.PropertyInfo">Property</see>.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		/// <exception cref="Exception">The type cannot be assigned to values of type.</exception>
		public static FastProperty<T, TProperty> Make(PropertyInfo property)
		{
			FastProperty<T, TProperty> fastProperty;
			if (s_cache.TryGetValue(property, out fastProperty))
				return fastProperty;
			
			var instanceType = typeof(T);
			var returnType = typeof(TProperty);

			// make sure that (T)instance is valid
			if (!instanceType.IsAssignableFrom(property.DeclaringType))
			{
				throw new Exception(string.Format("The type {0} cannot be assigned to values of type {1}",
					property.DeclaringType.Name, typeof(T).Name));
			}

			// make sure that (TProperty)value is valid
			if (!returnType.IsAssignableFrom(property.PropertyType))
			{
				throw new Exception(string.Format("The type {0} cannot be assigned to values of type {1}",
					property.PropertyType.Name, typeof(TProperty).Name));
			}

			// create our fast property instance
			fastProperty = new FastProperty<T, TProperty>(property, instanceType, returnType);

			// add it to the cache
			s_cache.Add(property, fastProperty);

			return fastProperty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FastProperty{T, TProperty}"/> class.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <param name="instanceType">Type of the instance.</param>
		/// <param name="returnType">The return type.</param>
		private FastProperty(PropertyInfo property, Type instanceType, Type returnType)
		{
			Property = property;
			InstanceType = instanceType;
			ReturnType = returnType;

			makeDelegates();
		}

		/// <summary>
		/// Makes the  get/set delegates.
		/// </summary>
		private void makeDelegates()
		{
			var instance = Expression.Parameter(InstanceType, "instance");
			var value = Expression.Parameter(ReturnType, "value");

			Expression getExpression = null;
			Expression instanceExpression;
			Expression valueExpression;

			// handle instance conversion
			if (InstanceType == Property.DeclaringType)
			{
				instanceExpression = instance;
			}
			else
			{
				instanceExpression = (Property.DeclaringType.IsValueType) ?
					Expression.Convert(instance, Property.DeclaringType) :
					Expression.TypeAs(instance, Property.DeclaringType);
			}

			// create the call to the get method
			if (Property.CanRead)
			{
				var getMethod = Property.GetGetMethod();
				if (getMethod.IsStatic)
					getExpression = Expression.Call(getMethod);
				else
					getExpression = Expression.Call(instanceExpression, getMethod);
			}

			// handle value conversion
			if (ReturnType == Property.PropertyType)
			{
				valueExpression = value;
			}
			else
			{
				if (Property.PropertyType.IsValueType)
				{
					valueExpression = Expression.Convert(value, Property.PropertyType);

					// convert the return value of the get method
					if (getExpression != null)
						getExpression = Expression.Convert(getExpression, ReturnType);
				}
				else
				{
					valueExpression = Expression.TypeAs(value, Property.PropertyType);

					// convert the return value of the get method
					if (getExpression != null) 
						getExpression = Expression.TypeAs(getExpression, ReturnType);
				}
			}

			if (Property.CanRead && getExpression != null)
				Get = Expression.Lambda<Func<T, TProperty>>(getExpression, instance).Compile();

			if (Property.CanWrite)
			{
				// create the call to the set method
				var setMethod = Property.GetSetMethod(true);
				//if (setMethod != null)
				{
					if (!setMethod.IsStatic)
					{
						Expression setExpression = Expression.Call(instanceExpression, setMethod, valueExpression);
						Set = Expression.Lambda<Action<T, TProperty>>(setExpression, instance, value).Compile();
					}
					else
					{
						Expression setExpression = Expression.Call(setMethod, valueExpression);
						Set = Expression.Lambda<Action<T, TProperty>>(setExpression, instance, value).Compile();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the get.
		/// </summary>
		/// <value>The get.</value>
		public Func<T, TProperty> Get { get; private set; }

		/// <summary>
		/// Gets or sets the set.
		/// </summary>
		/// <value>The set.</value>
		public Action<T, TProperty> Set { get; private set; }

		/// <summary>
		/// A cache to store properties that have already been parsed
		/// </summary>
		private static readonly MruDictionary<PropertyInfo, FastProperty<T, TProperty>> s_cache = new MruDictionary<PropertyInfo, FastProperty<T, TProperty>>(500);

		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to compare with the current <see cref="object"/>.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="object"/>; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="obj" /> is <c>null</c>.</exception>
		public override bool Equals(object obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			// check to see if we are comparing fast properties
			if (obj is IFastProperty)
			{
				var instance = obj as IFastProperty;

				return (
					Property == instance.Property &&
					ReturnType == instance.ReturnType &&
					InstanceType == instance.InstanceType
				);
			}

			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			return Property.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return string.Format("FastProperty<{0},{1}> - {2}.{3}", InstanceType.Name, ReturnType.Name, Property.DeclaringType.Name, Property.Name);
		}

		/// <summary>
		/// Gets the <see cref="Func{T,TProperty}"/> delegate used to access the getter for the property.
		/// </summary>
		/// <value>The get.</value>
		Func<T, TProperty> IFastProperty<T, TProperty>.Get
		{
			get { return Get; }
		}

		/// <summary>
		/// Gets the <see cref="System.Action{T,TProperty}"/> delegate used to access the setter for the property.
		/// </summary>
		/// <value>The set.</value>
		Action<T, TProperty> IFastProperty<T, TProperty>.Set
		{
			get { return Set; }
		}

		/// <summary>
		/// Gets the <see cref="PropertyInfo"/> the fast property is associated with
		/// </summary>
		/// <value>The property.</value>
		PropertyInfo IFastProperty.Property
		{
			get { return Property; }
		}

		/// <summary>
		/// Gets the type of the object being passed in as the instance when getting/setting the property
		/// </summary>
		/// <value>The type of the instance.</value>
		Type IFastProperty.InstanceType
		{
			get { return InstanceType; }
		}

		/// <summary>
		/// Gets the type of the object being passed in as the value when setting the property.
		/// </summary>
		/// <value>The type of the return.</value>
		Type IFastProperty.ReturnType
		{
			get { return ReturnType; }
		}


		/// <summary>
		/// Gets a value indicating whether this instance can read.
		/// </summary>
		/// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
		bool IFastProperty.CanRead
		{
			get { return CanRead; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance can write.
		/// </summary>
		/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
		bool IFastProperty.CanWrite
		{
			get { return CanWrite; }
		}
	}
}
