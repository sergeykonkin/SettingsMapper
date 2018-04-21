using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;
using TigrSettings.Dynamic;

namespace TigrSettings.Tests
{
	[TestFixture]
	public class BindersTest
	{
		private const string PocoBinderTestName = "Should set public property {0} to value {1}";
		[Test(TestOf = typeof(PocoBinder))]
		[TestCase("Int", 5, TestName = PocoBinderTestName)]
		[TestCase("String", "foobar", TestName = PocoBinderTestName)]
		[TestCase("NullableDouble", null, TestName = PocoBinderTestName)]
		public void PocoBinderTest(string propName, object value)
		{
			var poco = new Poco();
			var pocoBinder = new PocoBinder(poco);

			pocoBinder.Bind(propName, value);

			var prop = poco.GetType().GetProperty(propName);
			var result = prop.GetValue(poco);

			Assert.AreEqual(value, result);
		}

		private const string StaticBinderTestName = "Should set public static property {0} to value {1}";
		[Test(TestOf = typeof(StaticBinder))]
		[TestCase("Int", 5, TestName = StaticBinderTestName)]
		[TestCase("String", "foobar", TestName = StaticBinderTestName)]
		[TestCase("NullableDouble", null, TestName = StaticBinderTestName)]
		public void StaticBinderTest(string propName, object value)
		{
			var staticBinder = new StaticBinder(typeof(Static));

			staticBinder.Bind(propName, value);

			var prop = typeof(Static).GetProperty(propName);
			var result = prop.GetValue(null);

			Assert.AreEqual(value, result);
		}

		private const string DynamicBinderTestName = "Should set public property {0} to value {1}";
		[Test(TestOf = typeof(DynamicBinder))]
		[TestCase("Int", 5, TestName = DynamicBinderTestName)]
		[TestCase("String", "foobar", TestName = DynamicBinderTestName)]
		[TestCase("NullableDouble", null, TestName = DynamicBinderTestName)]
		public void DynamicBinderTest(string propName, object value)
		{
			var target = new ExpandoObject();
			var dynamicBinder = new DynamicBinder(target, typeof(IDynamic));

			dynamicBinder.Bind(propName, value);

			var dict = (IDictionary<string, object>) target;
			var result = dict[propName];

			Assert.AreEqual(value, result);
		}
	}
}
