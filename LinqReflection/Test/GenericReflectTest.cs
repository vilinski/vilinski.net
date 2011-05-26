using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqReflection
{
	[TestClass]
	public class GenericReflectTest
	{
		[TestMethod]
		public void ReflectionTryGetGetterByName()
		{
			Func<string,object> getter = Reflect.ByStrongType<string>().TryGetGetter("Length");
			Assert.IsNotNull(getter, "Could not create property getter");
			var result = (int)getter("abc");
			Assert.AreEqual(3, result, "Getter does not work");
		}

		[TestMethod]
		public void ReflectionTryGetGetterByExpression()
		{
			Func<string, object> getter = Reflect.ByStrongType<string>().TryGetGetter(s => s.Length);
			Assert.IsNotNull(getter, "Could not create property getter");
			var result = (int)getter("abc");
			Assert.AreEqual(3, result, "Getter does not work");
		}

		[TestMethod]
		public void MultipleAccessByNameDoesNotImpactCountOfGetters()
		{
			Reflect.ByStrongType<string>().TryGetGetter("Length");
			Reflect.ByStrongType<string>().TryGetGetter("Length");
			Reflect.ByStrongType<string>().TryGetGetter("Length");
			Assert.AreNotEqual(0, Reflect.ByStrongType<string>().Count, "Access by name is not cached");
			Assert.AreEqual(1, Reflect.ByStrongType<string>().Count, "Multiple getters for the same property");
			Reflect.ByStrongType<string>().TryGetGetter(s => s.Length);
			Assert.AreEqual(1, Reflect.ByStrongType<string>().Count, "Multiple getters for the same property");
		}

		[TestMethod]
		public void MultipleAccessByExpressionDoesNotImpactCountOfGetters()
		{
			Reflect.ByStrongType<string>().TryGetGetter(s => s.Length);
			Reflect.ByStrongType<string>().TryGetGetter(s => s.Length);
			Reflect.ByStrongType<string>().TryGetGetter(s => s.Length);
			Assert.AreNotEqual(0, Reflect.ByStrongType<string>().Count, "Access by expression is not cached");
			Assert.AreEqual(1, Reflect.ByStrongType<string>().Count, "Multiple getters for the same property");
			Reflect.ByStrongType<string>().TryGetGetter("Length");
			Assert.AreEqual(1, Reflect.ByStrongType<string>().Count, "Multiple getters for the same property");
		}

		[TestMethod]
		public void DecreaseMaxCapacityRemovesLruItems()
		{
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.Attributes);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.CanRead);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.CanWrite);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.CanWrite);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.DeclaringType);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.DeclaringType);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.IsSpecialName);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.IsSpecialName);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.MemberType);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.MemberType);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.PropertyType);
			Reflect.ByStrongType<PropertyInfo>().TryGetGetter(p => p.PropertyType);
			Assert.AreEqual(7, Reflect.ByStrongType<PropertyInfo>().Count);
			Reflect.ByStrongType<PropertyInfo>().MaxCapacity = 5;
			Assert.AreEqual(5, Reflect.ByStrongType<PropertyInfo>().Count);
			Assert.IsFalse(Reflect.ByStrongType<PropertyInfo>().ContainsKey("Attributes"), "LRU is not removed by decreasing max capacity");
			Assert.IsFalse(Reflect.ByStrongType<PropertyInfo>().ContainsKey("CanRead"), "LRU is not removed by decreasing max capacity");
		}
	}
}
