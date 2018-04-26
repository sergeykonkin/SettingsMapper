using System;
using NUnit.Framework;
using TigrSettings.Converters;

namespace TigrSettings.Tests
{
	[TestFixture]
	public class GuidConverterTest
	{
		private readonly ISettingConverter _converter = new GuidConverter();

		private const string ShouldBeAbleToConvertGuidTypeName = "Should be able to convert {0}";
		[Test(TestOf = typeof(GuidConverter))]
		[TestCase(typeof(Guid), TestName = ShouldBeAbleToConvertGuidTypeName)]
		public void ShouldBeAbleToConvertGuidType(Type guidType)
		{
			var canConvert = _converter.CanConvert(guidType);

			Assert.IsTrue(canConvert);
		}

		private const string ShouldConvertToExpectedValueName = "Should convert string {0} to corresponding {1} value";
		[Test(TestOf = typeof(GuidConverter))]
		[TestCase("5ad47b4cd9bb4929bcd608a0809ffc8f", typeof(Guid), TestName = ShouldConvertToExpectedValueName)]
		[TestCase("5AD47B4CD9BB4929BCD608A0809FFC8F", typeof(Guid), TestName = ShouldConvertToExpectedValueName)]
		[TestCase("5ad47b4c-d9bb-4929-bcd6-08a0809ffc8f", typeof(Guid), TestName = ShouldConvertToExpectedValueName)]
		[TestCase("{5ad47b4c-d9bb-4929-bcd6-08a0809ffc8f}", typeof(Guid), TestName = ShouldConvertToExpectedValueName)]
		[TestCase("(5ad47b4c-d9bb-4929-bcd6-08a0809ffc8f)", typeof(Guid), TestName = ShouldConvertToExpectedValueName)]
		public void ShouldConvertToExpectedValue(string value, Type type)
		{
			var result = _converter.Convert(value, type);

			Assert.IsTrue(result is Guid);

			var resultGuid = (Guid)result;
			var expectedResult = new Guid("5ad47b4c-d9bb-4929-bcd6-08a0809ffc8f");
			Assert.AreEqual(expectedResult, resultGuid);
		}
	}
}
