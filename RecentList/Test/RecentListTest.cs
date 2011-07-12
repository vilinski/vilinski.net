using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RecentList;

namespace RecentListTest
{
	/// <summary>
	///  Unit tests for <see cref="RecentList"/>
	/// </summary>
	[TestClass]
	public class RecentListTest
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
		public void RecentListAddTest()
		{
			// ARRANGE
			var mock = new Mock<IRecentList<Guid>>();
			mock.Setup(foo => foo.Contains(It.IsAny<Guid>())).Returns(true);
			mock.Setup(foo => foo.Count).Returns(1);
			IRecentList<Guid> collection = mock.Object;

			// ACT
			collection.Add(Guid.NewGuid());

			// ACCERT
			Assert.IsTrue(collection.Contains(Guid.NewGuid()));
			Assert.AreEqual(1, collection.Count, "1 != 2");
		}
	}
}
