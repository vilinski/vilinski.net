using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqReflection
{
	[TestClass]
	public class ReflectTest
	{
		[TestMethod]
		public void ReflectionTryGetGetterByName()
		{
			Func<string,object> getter = Reflect<string>.TryGetGetter("Length");
			Assert.IsNotNull(getter, "Could not create property getter");
			var result = (int)getter("abc");
			Assert.AreEqual(3, result, "Getter does not work");
		}

		[TestMethod]
		public void ReflectionTryGetGetterByExpression()
		{
			Func<string, object> getter = Reflect<string>.TryGetGetter(s => s.Length);
			Assert.IsNotNull(getter, "Could not create property getter");
			var result = (int)getter("abc");
			Assert.AreEqual(3, result, "Getter does not work");
		}

		[TestMethod]
		public void MultipleAccessByNameDoesNotImpactCountOfGetters()
		{
			Reflect<string>.TryGetGetter("Length");
			Reflect<string>.TryGetGetter("Length");
			Reflect<string>.TryGetGetter("Length");
			Assert.AreNotEqual(0, Reflect<string>.Count, "Access by name is not cached");
			Assert.AreEqual(1, Reflect<string>.Count, "Multiple getters for the same property");
			Reflect<string>.TryGetGetter(s => s.Length);
			Assert.AreEqual(1, Reflect<string>.Count, "Multiple getters for the same property");
		}

		[TestMethod]
		public void MultipleAccessByExpressionDoesNotImpactCountOfGetters()
		{
			Reflect<string>.TryGetGetter(s => s.Length);
			Reflect<string>.TryGetGetter(s => s.Length);
			Reflect<string>.TryGetGetter(s => s.Length);
			Assert.AreNotEqual(0, Reflect<string>.Count, "Access by expression is not cached");
			Assert.AreEqual(1, Reflect<string>.Count, "Multiple getters for the same property");
			Reflect<string>.TryGetGetter("Length");
			Assert.AreEqual(1, Reflect<string>.Count, "Multiple getters for the same property");
		}

		[TestMethod]
		public void DecreaseMaxCapacityRemovesLruItems()
		{
			Reflect<PropertyInfo>.TryGetGetter(p => p.Attributes);
			Reflect<PropertyInfo>.TryGetGetter(p => p.CanRead);
			Reflect<PropertyInfo>.TryGetGetter(p => p.CanWrite);
			Reflect<PropertyInfo>.TryGetGetter(p => p.CanWrite);
			Reflect<PropertyInfo>.TryGetGetter(p => p.DeclaringType);
			Reflect<PropertyInfo>.TryGetGetter(p => p.DeclaringType);
			Reflect<PropertyInfo>.TryGetGetter(p => p.IsSpecialName);
			Reflect<PropertyInfo>.TryGetGetter(p => p.IsSpecialName);
			Reflect<PropertyInfo>.TryGetGetter(p => p.MemberType);
			Reflect<PropertyInfo>.TryGetGetter(p => p.MemberType);
			Reflect<PropertyInfo>.TryGetGetter(p => p.PropertyType);
			Reflect<PropertyInfo>.TryGetGetter(p => p.PropertyType);
			Assert.AreEqual(7, Reflect<PropertyInfo>.Count);
			Reflect<PropertyInfo>.MaxCapacity = 5;
			Assert.AreEqual(5, Reflect<PropertyInfo>.Count);
			Assert.IsFalse(Reflect<PropertyInfo>.ContainsKey("Attributes"), "LRU is not removed by decreasing max capacity");
			Assert.IsFalse(Reflect<PropertyInfo>.ContainsKey("CanRead"), "LRU is not removed by decreasing max capacity");
		}
	}
}
