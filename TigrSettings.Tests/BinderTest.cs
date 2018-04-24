using System;
using System.Collections.Generic;
using NUnit.Framework;

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
			var pocoBinder = new PocoBinder();
			Type targetType = typeof(Model.Poco);
			object target = pocoBinder.CreateTarget(targetType);

			pocoBinder.Bind(target, targetType, propName, value);

			var prop = target.GetType().GetProperty(propName);
			var result = prop.GetValue(target);

			Assert.AreEqual(value, result);
		}

		private const string StaticBinderTestName = "Should set public static property {0} to value {1}";
		[Test(TestOf = typeof(StaticBinder))]
		[TestCase("Int", 5, TestName = StaticBinderTestName)]
		[TestCase("String", "foobar", TestName = StaticBinderTestName)]
		[TestCase("NullableDouble", null, TestName = StaticBinderTestName)]
		public void StaticBinderTest(string propName, object value)
		{
			var staticBinder = new StaticBinder();
			var targetType = typeof(Model.Static);
			var target = staticBinder.CreateTarget(targetType);

			staticBinder.Bind(target, targetType, propName, value);

			var prop = typeof(Model.Static).GetProperty(propName);
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
			var dynamicBinder = new DynamicBinder();
			Type targetType = typeof(Model.IDynamic);
			object target = dynamicBinder.CreateTarget(targetType);

			dynamicBinder.Bind(target, targetType, propName, value);

			var dict = (IDictionary<string, object>) target;
			var result = dict[propName];

			Assert.AreEqual(value, result);
		}
	}
}
