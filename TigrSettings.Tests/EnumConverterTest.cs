using System;
using NUnit.Framework;
using TigrSettings.Converters;

namespace TigrSettings.Tests
{
	[TestFixture]
	public class EnumConverterTest
	{
		private readonly ISettingValueConverter _converter = new EnumConverter();

		private const string ShouldBeAbleToConvertEnumTypeName = "Should be able to convert {0}";
		[Test(TestOf = typeof(EnumConverter))]
		[TestCase(typeof(TestEnum), TestName = ShouldBeAbleToConvertEnumTypeName)]
		public void ShouldBeAbleToConvertEnumType(Type enumType)
		{
			var canConvert = _converter.CanConvert(enumType);
			Assert.IsTrue(canConvert);
		}

		private const string ShouldConvertToExpectedValueName = "Should convert string {0} to corresponding {1} value";
		[Test(TestOf = typeof(EnumConverter))]
		[TestCase("Two", typeof(TestEnum), ExpectedResult = TestEnum.Two, TestName = ShouldConvertToExpectedValueName)]
		[TestCase("One|Four", typeof(TestEnum), ExpectedResult = TestEnum.One|TestEnum.Four, TestName = ShouldConvertToExpectedValueName)]
		public object ShouldConvertToExpectedValue(string value, Type type)
		{
			return _converter.Convert(value, type);
		}
	}
}
