using System;
using System.Reflection;

namespace LinqReflection.Property
{
	/// <summary>
	/// Interface containing the property information;
	/// </summary>
	public interface IFastProperty
	{
		/// <summary>
		/// Gets the <see cref="PropertyInfo"/> the fast property is associated with
		/// </summary>
		/// <value>The property.</value>
		PropertyInfo Property { get; }

		/// <summary>
		/// Gets the type of the object being passed in as the instance when getting/setting the property
		/// </summary>
		/// <value>The type of the instance.</value>
		Type InstanceType { get; }

		/// <summary>
		/// Gets the type of the object being passed in as the value when setting the property.
		/// </summary>
		/// <value>The type of the return.</value>
		Type ReturnType { get; }

		/// <summary>
		/// Gets a value indicating whether this instance can read.
		/// </summary>
		/// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
		bool CanRead { get; }

		/// <summary>
		/// Gets a value indicating whether this instance can write.
		/// </summary>
		/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
		bool CanWrite { get; }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">Declaring type</typeparam>
	/// <typeparam name="TProperty">Property type</typeparam>
	public interface IFastProperty<in T, TProperty> : IFastProperty
	{
		/// <summary>
		/// Gets the <see cref="System.Func{T,P}"/> delegate used to access the getter for the property.
		/// </summary>
		/// <value>The get.</value>
		Func<T, TProperty> Get { get; }

		/// <summary>
		/// Gets the <see cref="System.Action"/> delegate used to access the setter for the property.
		/// </summary>
		/// <value>The set.</value>
		Action<T, TProperty> Set { get; }
	}
}