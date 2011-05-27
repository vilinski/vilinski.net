using System;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using LinqReflection.Property;

namespace LinqReflection.PerformanceTest
{
	/// <summary>
	/// Performance test form
	/// </summary>
	public partial class Form1 : Form
	{
		/// <summary>
		/// Class under test
		/// </summary>
		public class Employee
		{
			/// <summary>
			/// Gets or sets the name.
			/// </summary>
			/// <value>
			/// The name.
			/// </value>
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets the hire date.
			/// </summary>
			/// <value>
			/// The hire date.
			/// </value>
			public DateTime HireDate { get; set; }

			/// <summary>
			/// Gets or sets the age.
			/// </summary>
			/// <value>
			/// The age.
			/// </value>
			public int? Age { get; set; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Form1"/> class.
		/// </summary>
		public Form1()
		{
			InitializeComponent();
		}

		private void FPTPRunButton_Click(object sender, EventArgs e)
		{
			var emp = new Employee { Name = "Mike", Age = null, HireDate = DateTime.Now.AddYears(-3) };

			ExecuteFastPropertyGet(FastProperty<Employee, string>.Make(typeof(Employee).GetProperty("Name")), emp, FPTPGetResults);
			ExecuteFastPropertySet(FastProperty<Employee, string>.Make(typeof(Employee).GetProperty("Name")), emp, "James", FPTPSetResults);
		}

		/// <summary>
		/// Executes the fast property get.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="P"></typeparam>
		/// <param name="property">The property.</param>
		/// <param name="item">The item.</param>
		/// <param name="results">The results.</param>
		public static void ExecuteFastPropertyGet<T, P>(FastProperty<T, P> property, T item, Control results)
		{
			DateTime start = DateTime.Now;
			for (int i = 0; i < 10000000; i++)
			{
				P value = property.Get(item);
			}
			DateTime end = DateTime.Now;

			results.Text = (end - start).TotalMilliseconds + "ms";
		}

		/// <summary>
		/// Executes the fast property set.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="property">The property.</param>
		/// <param name="item">The item.</param>
		/// <param name="value">The value.</param>
		/// <param name="results">The results.</param>
		public static void ExecuteFastPropertySet<T, TProperty>(FastProperty<T, TProperty> property, T item, TProperty value, Control results)
		{
			DateTime start = DateTime.Now;
			for (int i = 0; i < 10000000; i++)
				property.Set(item, value);
			DateTime end = DateTime.Now;

			results.Text = (end - start).TotalMilliseconds + "ms";
		}

		private void FPRunButton_Click(object sender, EventArgs e)
		{
			var emp = new Employee { Name = "Mike", Age = null, HireDate = DateTime.Now.AddYears(-3) };

			ExecuteFastPropertyGet(FastProperty<object, object>.Make(typeof(Employee).GetProperty("Name")), emp, FPGetResults);
			ExecuteFastPropertySet(FastProperty<object, object>.Make(typeof(Employee).GetProperty("Name")), emp, "James", FPSetResults);
		}

		/// <summary>
		/// Executes the standard get over <see cref="PropertyInfo.GetValue(object,object[])"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="property">The property.</param>
		/// <param name="instance">The instance.</param>
		/// <param name="results">The results.</param>
		public static void ExecuteStandardGet<T>(PropertyInfo property, T instance, Control results)
		{
			DateTime start = DateTime.Now;
			for (int i = 0; i < 10000000; i++)
				property.GetValue(instance, null);
			DateTime end = DateTime.Now;

			results.Text = (end - start).TotalMilliseconds + "ms";
		}

		/// <summary>
		/// Executes the standard set.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="property">The property.</param>
		/// <param name="instance">The instance.</param>
		/// <param name="value">The value.</param>
		/// <param name="results">The results.</param>
		public static void ExecuteStandardSet<T, TProperty>(PropertyInfo property, T instance, TProperty value, Control results)
		{
			DateTime start = DateTime.Now;
			for (int i = 0; i < 10000000; i++)
			{
				property.SetValue(instance, value, null);
			}
			DateTime end = DateTime.Now;

			results.Text = (end - start).TotalMilliseconds + "ms";
		}

		private void STDRunButton_Click(object sender, EventArgs e)
		{
			var emp = new Employee { Name = "Mike", Age = null, HireDate = DateTime.Now.AddYears(-3) };
			var property = typeof(Employee).GetProperty("Name");
			ExecuteStandardGet(property, emp, STGetResults);
			ExecuteStandardSet(property, emp, "James", STSetResults);
		}

		/// <summary>
		/// Natives the get.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="results">The results.</param>
		private static void nativeGet(Employee instance, Control results)
		{
			DateTime start = DateTime.Now;
			for (int i = 0; i < 10000000; i++)
			{
				string value = instance.Name;
			}
			DateTime end = DateTime.Now;

			results.Text = (end - start).TotalMilliseconds + "ms";
		}

		private static void nativeSet(Employee instance, string value, Control results)
		{
			DateTime start = DateTime.Now;
			for (int i = 0; i < 10000000; i++)
			{
				instance.Name = value;
			}
			DateTime end = DateTime.Now;

			results.Text = (end - start).TotalMilliseconds + "ms";
		}

		private void NRunButton_Click(object sender, EventArgs e)
		{
			var emp = new Employee { Name = "Mike", Age = null, HireDate = DateTime.Now.AddYears(-3) };
			nativeGet(emp, NGetResults);
			nativeSet(emp, "Jason", NSetResults);
		}

		/// <summary>
		/// Makes the fast properties.
		/// </summary>
		/// <param name="results">The results.</param>
		public static void MakeFastProperties(Control results)
		{
			DateTime start = DateTime.Now;
			var types =
				(from type in Assembly.GetAssembly(typeof(Guid)).GetTypes()
				where !type.IsGenericType || !type.IsGenericTypeDefinition
				select type).ToList();
			var typeCount = types.Count;

			var properties =
				(from type in types
				from p in type.GetProperties()
				where p.GetIndexParameters().Length == 0
				 // exclude indexers
				 select p).ToList();
			var propertyCount = properties.Count;

			var fastProperties =
				(from p in properties
				let f = p.ToFastProperty()
				where f.Get != null || f.Set != null
				select f).ToList();
			var fastPropertyCount = fastProperties.Count;
			//foreach (var type in Assembly.GetAssembly(typeof(Guid)).GetTypes().Where(p => (!p.IsGenericType || !p.IsGenericTypeDefinition)).ToList())
			//{
			//    propertyCount = type.GetProperties().Where(p => p.GetIndexParameters().Length == 0).ToList().Select(property => property.ToFastProperty()).Count();

			//    typeCount++;
			//}
			DateTime end = DateTime.Now;

			results.Text = (end - start).TotalMilliseconds + "ms" + " - " + typeCount + " types - " + fastPropertyCount + "/" + propertyCount + " properties.";
		}

		private void MakeRunButton_Click(object sender, EventArgs e)
		{
			MakeFastProperties(MakeTextBox);
		}
	}
}
