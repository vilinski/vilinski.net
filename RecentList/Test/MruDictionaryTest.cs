using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RecentList;

namespace LinqReflectionTest
{
	[TestClass]
	public class MruDictionaryTest
	{
		[TestMethod]
		public void AddItem()
		{
			// ARRANGE
			var dict = new MruDictionary<string, string>();

			// ACT
			dict.Add("aa", "aa");
			dict.Add("bb", "bb");
			dict.Add("cc", "cc");
			dict.Add("dd", "dd");

			// ACCERT
			Assert.AreEqual(4, dict.Count);
			Assert.IsTrue(dict.ContainsKey("aa"));
			Assert.IsTrue(dict.ContainsKey("bb"));
			Assert.IsTrue(dict.ContainsKey("cc"));
			Assert.IsTrue(dict.ContainsKey("dd"));
		}

		[TestMethod]
		public void ThrowsExceptionIfAddDuplicateKeys()
		{
			// ARRANGE
			var dict = new MruDictionary<string, string>();

			// ACT
			try
			{
				dict.Add("aa", "aa");
				dict.Add("aa", "aa");

			}
			catch (ArgumentException)
			{
				return;
			}

			// ACCERT
			Assert.Fail("Exception is not thrown");
		}
		[TestMethod]
		public void DoNotThrowsExceptionIfSetIndexerValue()
		{
			// ARRANGE
			var dict = new MruDictionary<string, string>();

			try
			{
				// ACT
				dict["aa"] = "aa";
				dict["aa"] = "bb";

				// ACCERT
				Assert.AreEqual("bb", dict["aa"], "Value is not set");
			}
			catch (ArgumentException e)
			{
				Assert.Fail(e.Message + "Exception should not be thrown by setting the value by the same key.");
			}

		}

		[TestMethod]
		public void DecreaseMaxCapacityRemovesLruItems()
		{
			var dict = new MruDictionary<string, string>();
		}

		[TestMethod]
		public void KeepOrderOfItemsFromCtorParameter()
		{
			// ARRANGE
			
			var items = new Dictionary<string,string>
			{
				{"aa", "aa"}, {"bb", "bb"}, {"cc", "cc"}
			};

			// ACT
			var dict = new MruDictionary<string, string>(items);
			dict.MaxCapacity = 2;

			// ACCERT
			Assert.IsTrue(dict.ContainsKey("aa"));
			Assert.IsTrue(dict.ContainsKey("bb"));
		}
	}
}
