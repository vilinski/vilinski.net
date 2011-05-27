using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LinqReflection.Property
{
	/// <summary>
	/// Helpers to convert <see cref="PropertyInfo"/> to <see cref="FastProperty{T,P}"/>
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Returns the fast property instance associated with the property
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public static FastProperty<object, object> ToFastProperty(this PropertyInfo property)
		{
			return FastProperty<object, object>.Make(property);
		}

		/// <summary>
		/// Returns the fast property instance associated with the property
		/// </summary>
		/// <typeparam name="T">The type of the instance variable</typeparam>
		/// <typeparam name="P">The type of the return value</typeparam>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public static FastProperty<T, P> ToFastProperty<T, P>(this PropertyInfo property)
		{
			return FastProperty<T, P>.Make(property);
		}
	}
}
