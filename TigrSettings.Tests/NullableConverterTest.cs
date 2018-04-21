using System;
using Moq;
using NUnit.Framework;
using TigrSettings.Converters;

namespace TigrSettings.Tests
{
	[TestFixture]
	public class NullableConverterTest
	{
		private static Mock<ISettingValueConverter> _underlyingConverter;
		private ISettingValueConverter _converter;

		[SetUp]
		public void Setup()
		{
			_underlyingConverter = new Mock<ISettingValueConverter>();
			_underlyingConverter.Setup(c => c.CanConvert(typeof(int))).Returns(true);
			_underlyingConverter.Setup(c => c.CanConvert(typeof(double))).Returns(true);
			_underlyingConverter.Setup(c => c.CanConvert(typeof(TimeSpan))).Returns(true);
			_underlyingConverter.Setup(c => c.CanConvert(typeof(DateTime))).Returns(true);
			_underlyingConverter.Setup(c => c.Convert("5", typeof(int))).Returns(5);
			_underlyingConverter.Setup(c => c.Convert("-5", typeof(int))).Returns(-5);

			_converter = new NullableConverter(new[] {_underlyingConverter.Object});
		}

		private const string ShouldBeAbleToConvertNullableTypeName = "Should be able to convert {0}";
		[Test(TestOf = typeof(NullableConverter))]
		[TestCase(typeof(int?), TestName = ShouldBeAbleToConvertNullableTypeName)]
		[TestCase(typeof(double?), TestName = ShouldBeAbleToConvertNullableTypeName)]
		[TestCase(typeof(TimeSpan?), TestName = ShouldBeAbleToConvertNullableTypeName)]
		[TestCase(typeof(DateTime?), TestName = ShouldBeAbleToConvertNullableTypeName)]
		public void ShouldBeAbleToConvertNullableType(Type nullableType)
		{
			var underlyingType = nullableType.GetGenericArguments()[0];

			var canConvert = _converter.CanConvert(nullableType);

			Assert.IsTrue(canConvert);
			_underlyingConverter.Verify(c => c.CanConvert(underlyingType), Times.Once);
		}

		private const string ShouldNotBeAbleToConvertNonNullableTypeName = "Should be able to convert {0}";
		[Test(TestOf = typeof(NullableConverter))]
		[TestCase(typeof(int), TestName = ShouldNotBeAbleToConvertNonNullableTypeName)]
		[TestCase(typeof(double), TestName = ShouldNotBeAbleToConvertNonNullableTypeName)]
		[TestCase(typeof(TimeSpan), TestName = ShouldNotBeAbleToConvertNonNullableTypeName)]
		[TestCase(typeof(DateTime), TestName = ShouldNotBeAbleToConvertNonNullableTypeName)]
		public void ShouldNotBeAbleToConvertNonNullableType(Type nonNullableType)
		{
			var canConvert = _converter.CanConvert(nonNullableType);

			Assert.IsFalse(canConvert);
			_underlyingConverter.Verify(c => c.CanConvert(It.IsAny<Type>()), Times.Never);
		}

		private const string ShouldNotBeAbleToConvertIfNoUnderlyingTypeConverterProvidedName = "Should be able to convert if no underlying type converter provided";
		[Test(TestOf = typeof(NullableConverter))]
		[TestCase(typeof(byte?), TestName = ShouldNotBeAbleToConvertIfNoUnderlyingTypeConverterProvidedName)]
		public void ShouldNotBeAbleToConvertIfNoUnderlyingTypeConverterProvided(Type nullableType)
		{
			var underlyingType = nullableType.GetGenericArguments()[0];

			var canConvert = _converter.CanConvert(nullableType);

			Assert.IsFalse(canConvert);
			_underlyingConverter.Verify(c => c.CanConvert(underlyingType), Times.Once);
		}

		private const string ShouldConvertToExpectedValueName = "Should convert string {0} to corresponding {1} value";
		[Test(TestOf = typeof(NullableConverter))]
		[TestCase("5", typeof(int?), ExpectedResult = 5, TestName = ShouldConvertToExpectedValueName)]
		[TestCase("-5", typeof(int?), ExpectedResult = -5, TestName = ShouldConvertToExpectedValueName)]
		[TestCase("null", typeof(int?), ExpectedResult = null, TestName = ShouldConvertToExpectedValueName)]
		[TestCase("", typeof(int?), ExpectedResult = null, TestName = ShouldConvertToExpectedValueName)]
		[TestCase(" ", typeof(int?), ExpectedResult = null, TestName = ShouldConvertToExpectedValueName)]
		[TestCase(null, typeof(int?), ExpectedResult = null, TestName = ShouldConvertToExpectedValueName)]
		public object ShouldConvertToExpectedValue(string value, Type type)
		{
			var underlyingType = type.GetGenericArguments()[0];

			var result = _converter.Convert(value, type);

			_underlyingConverter.Verify(c => c.Convert(It.IsAny<string>(), underlyingType), Times.AtMostOnce);
			return result;
		}
	}
}
