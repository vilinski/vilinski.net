using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqReflection
{
	[TestClass]
	public class NonGenericReflectTest
	{
		[TestMethod]
		public void ReflectByInstance()
		{
			//Arrange
			var instance = "string";

			//Act
			Func<object, object> getter;
			var ret = Reflect.ByInstance(instance).TryGetGetter("Length", out getter);

			//Assert
			Assert.IsTrue(ret);
			Assert.AreEqual(6, getter(instance));
		}

		[TestMethod]
		public void ReflectionTryGetGetterByName()
		{
			Func<object, object> getter;
			var ret = Reflect.ByType<string>().TryGetGetter("Length", out getter);
			Assert.IsTrue(ret, "Could not create property getter");
			Assert.IsNotNull(getter, "Could not create property getter");
			var result = (int)getter("abc");
			Assert.AreEqual(3, result, "Getter does not work");
		}

		[TestMethod]
		public void MultipleAccessByNameDoesNotImpactCountOfGetters()
		{
			Func<object, object> getter;
			var r1 = Reflect.ByType<string>();
			r1.TryGetGetter("Length", out getter);
			Func<object, object> getter2;
			var r2 = Reflect.ByType<string>();
			r2.TryGetGetter("Length", out getter2);
			Func<object, object> getter3;
			var r3 = Reflect.ByType<string>();
			r3.TryGetGetter("Length", out getter3);
			Assert.AreNotEqual(0, r1.Count, "Access by name is not cached");
			Assert.AreEqual(1, r2.Count, "Multiple getters for the same property");
			Assert.AreEqual(1, r3.Count, "Multiple getters for the same property");
		}

		[TestMethod]
		public void DecreaseMaxCapacityRemovesLruItems()
		{
			Func<object, object> getter;
			Reflect.ByType<PropertyInfo>().TryGetGetter("Attributes", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("CanRead", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("CanWrite", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("CanWrite", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("DeclaringType", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("DeclaringType", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("IsSpecialName", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("IsSpecialName", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("MemberType", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("MemberType", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("PropertyType", out getter);
			Reflect.ByType<PropertyInfo>().TryGetGetter("PropertyType", out getter);
			Assert.AreEqual(7, Reflect.ByType<PropertyInfo>().Count, "Getters are not correctly cached.");
			Reflect.ByType<PropertyInfo>().MaxCapacity = 5;
			Assert.AreEqual(5, Reflect.ByType<PropertyInfo>().Count, "Getters Count should decrease to max capacity if the latter was decreased");
			Assert.IsFalse(Reflect.ByType<PropertyInfo>().ContainsKey("Attributes"), "LRU is not removed by decreasing max capacity");
			Assert.IsFalse(Reflect.ByType<PropertyInfo>().ContainsKey("CanRead"), "LRU is not removed by decreasing max capacity");
		}
	}
}