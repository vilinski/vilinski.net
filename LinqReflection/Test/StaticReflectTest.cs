using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqReflection
{
	/// <summary>
	///  Unit tests for <see cref="Reflect"/>
	/// </summary>
	[TestClass]
	public class StaticReflectTest
	{
		//#region Initialize/Cleanup

		///// <summary>
		///// Initialize before running the first test in the class.
		///// </summary>
		///// <param name="testContext">The test context.</param>
		//[ClassInitialize]
		//public static void ClassInitialize(TestContext testContext)
		//{
		//}

		///// <summary>
		///// Cleanup after all tests in a class have run.
		///// </summary>
		//[ClassCleanup]
		//public static void ClassCleanup()
		//{	
		//}

		///// <summary>
		///// Initialize before running each test
		///// </summary>
		//[TestInitialize]
		//public void TestInitialize()
		//{
		//}

		///// <summary>
		///// Cleanup after each test has run.
		///// </summary>
		//[TestCleanup]
		//public void TestCleanup()
		//{
		//}

		//#endregion

	    [TestMethod]
	    public void GetProperty()
	    {
	        Getter getter = Reflect.TryGetGetter(typeof(string), "Length");
            Assert.IsNotNull(getter, "Couldn't create getter");
	        var ret = getter.Get("string");
            Assert.AreEqual(6, ret, "Getter does not work.");
	    }
		[TestMethod]
		public void CacheByTypeTest()
		{
			//Arrange

			//Act
			var r1 = Reflect.ByType<string>();
			Func<object, object> g1;
			var ret1 = r1.TryGetGetter("Length", out g1);
			var r2 = Reflect.ByType<string>();
			Func<object, object> g2;
			var ret2 = r2.TryGetGetter("Length", out g2);
			var r3 = Reflect.ByType<string>();
			Func<object, object> g3;
			var ret3 = r3.TryGetGetter("Length", out g3);

			//Assert
			Assert.IsTrue(ret1, "TryGetGetter should return true for existing property");
			Assert.IsTrue(ret2);
			Assert.IsTrue(ret3);
			Assert.AreSame(r1, r2, "Not the same reflection instance of cached reflection");
            Assert.AreSame(r1, r2, "Not the same reflection instance of cached reflection");
            Assert.AreSame(r1, r2, "Not the same reflection instance of cached reflection");

            Assert.AreSame(r1, r2, "Not the same getter instance of cached getter");
            Assert.AreSame(r1, r2, "Not the same getter instance of cached getter");
            Assert.AreSame(r1, r2, "Not the same getter instance of cached getter");

			Assert.AreEqual(1, r1.Count, "More than one getter after multiple access");
		}
		[TestMethod]
		public void CacheByStrongTypeTest()
		{
			//Arrange

			//Act
			var r1 = Reflect.ByStrongType<string>();
			var g1 = r1.TryGetGetter("Length");
			var r2 = Reflect.ByStrongType<string>();
			var g2 = r2.TryGetGetter("Length");
			var r3 = Reflect.ByStrongType<string>();
			var g3 = r3.TryGetGetter("Length");

			//Assert
			Assert.IsTrue(ReferenceEquals(r1, r2), "Not the same reflection instance of cached reflection");
			Assert.IsTrue(ReferenceEquals(r2, r3), "Not the same reflection instance of cached reflection");
			Assert.IsTrue(ReferenceEquals(r3, r1), "Not the same reflection instance of cached reflection");

			Assert.IsTrue(ReferenceEquals(g1, g2), "Not the same getter instance of cached reflection");
			Assert.IsTrue(ReferenceEquals(g2, g3), "Not the same getter instance of cached reflection");
			Assert.IsTrue(ReferenceEquals(g3, g1), "Not the same getter instance of cached reflection");

			Assert.AreEqual(1, r1.Count, "More than one getter after multiple access");
		}
	}
}