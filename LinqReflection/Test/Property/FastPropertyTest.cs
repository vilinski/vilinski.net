using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqReflection.Property;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqReflectionTest.Property
{
	[TestClass]
	public class PropertyTest
	{
		private class Employee
		{
			public Employee()
			{
				this.Name = "Mike";
				this._ID = Guid.NewGuid();
				this.Age = null;
			}

			private Guid _ID;

			public string Name { get; set; }

			public Guid ID
			{
				get
				{
					return _ID;
				}
			}

			private int? _Age;

			public int? Age
			{
				set
				{
					_Age = value;
				}
			}
		}

		/// <summary>
		/// Verify <see cref="FastProperty{T,P}.Get"/>
		/// </summary>
		[TestMethod]
		public void Verify_Get()
		{
			var nameProperty = typeof(Employee).GetProperty("Name").ToFastProperty();
			var idProperty = typeof(Employee).GetProperty("ID").ToFastProperty();

			Assert.IsTrue(nameProperty.CanWrite);
			Assert.IsFalse(idProperty.CanWrite);
		}

		/// <summary>
		/// Verify <see cref="FastProperty{T,P}.Set"/>
		/// </summary>
		[TestMethod]
		public void Verify_Set()
		{
			var nameProperty = typeof(Employee).GetProperty("Name").ToFastProperty();
			var ageProperty = typeof(Employee).GetProperty("Age").ToFastProperty();

			Assert.IsTrue(nameProperty.CanRead);
			Assert.IsFalse(ageProperty.CanRead);
		}

		/// <summary>
		/// Run basis test for <see cref="FastProperty{T,P}.Set"/>
		/// </summary>
		[TestMethod]
		public void Run_Set_Basic()
		{
			var e = new Employee();
			var nameProperty = typeof(Employee).GetProperty("Name").ToFastProperty();
			var ageProperty = typeof(Employee).GetProperty("Age").ToFastProperty();

			Assert.IsTrue((nameProperty.Get(e) as string) == "Mike");
			nameProperty.Set(e, "Jake");
			Assert.IsTrue((nameProperty.Get(e) as string) == "Jake");

			try
			{
				ageProperty.Set(e, 4);
			}
			catch (Exception)
			{
				Assert.Fail("FastProperty.Set should throw no exception");
			}
		}
	}
}
