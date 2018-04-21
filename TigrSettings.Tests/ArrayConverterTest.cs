using System;
using Moq;
using NUnit.Framework;
using TigrSettings.Converters;

namespace TigrSettings.Tests
{
	[TestFixture]
	public class ArrayConverterTest
	{
		private static Mock<ISettingValueConverter> _elementConverter;
		private ISettingValueConverter _converter;

		[SetUp]
		public void Setup()
		{
			_elementConverter = new Mock<ISettingValueConverter>();
			_elementConverter.Setup(c => c.CanConvert(typeof(int))).Returns(true);
			_elementConverter.Setup(c => c.CanConvert(typeof(TimeSpan))).Returns(true);
			_elementConverter.Setup(c => c.Convert("3", typeof(int))).Returns(3);
			_elementConverter.Setup(c => c.Convert("4", typeof(int))).Returns(4);
			_elementConverter.Setup(c => c.Convert("5", typeof(int))).Returns(5);
			_elementConverter.Setup(c => c.Convert("00:05:00", typeof(TimeSpan))).Returns(TimeSpan.FromMinutes(5));
			_elementConverter.Setup(c => c.Convert("00:15:00", typeof(TimeSpan))).Returns(TimeSpan.FromMinutes(15));

			_converter = new ArrayConverter(new[] {_elementConverter.Object});
		}

		private const string ShouldBeAbleToConvertArrayTypeName = "Should be able to convert {0}";
		[Test(TestOf = typeof(ArrayConverter))]
		[TestCase(typeof(int[]), TestName = ShouldBeAbleToConvertArrayTypeName)]
		[TestCase(typeof(TimeSpan[]), TestName = ShouldBeAbleToConvertArrayTypeName)]
		public void ShouldBeAbleToConvertArrayType(Type arrayType)
		{
			var elementType = arrayType.GetElementType();

			var canConvert = _converter.CanConvert(arrayType);

			Assert.IsTrue(canConvert);
			_elementConverter.Verify(c => c.CanConvert(elementType), Times.Once);
		}

		private const string ShouldNotBeAbleToConvertIfNoElementConverterProvidedName = "Should be able to convert if no array element type converter provided";
		[Test(TestOf = typeof(ArrayConverter))]
		[TestCase(typeof(DateTime[]), TestName = ShouldNotBeAbleToConvertIfNoElementConverterProvidedName)]
		public void ShouldNotBeAbleToConvertIfNoElementConverterProvided(Type arrayType)
		{
			var elementType = arrayType.GetElementType();

			var canConvert = _converter.CanConvert(arrayType);

			Assert.IsFalse(canConvert);
			_elementConverter.Verify(c => c.CanConvert(elementType), Times.Once);
		}

		private const string ShouldConvertToExpectedValueName = "Should convert string {0} to corresponding {1} value";
		[Test(TestOf = typeof(ArrayConverter))]
		[TestCase("3,4,5", typeof(int[]), TestName = ShouldConvertToExpectedValueName)]
		[TestCase("00:05:00,00:15:00", typeof(TimeSpan[]), TestName = ShouldConvertToExpectedValueName)]
		public void ShouldConvertToExpectedValue(string value, Type type)
		{
			var expectedLength = value.Split(',').Length;
			var elementType = type.GetElementType();

			var array = _converter.Convert(value, type);

			Assert.IsTrue(array is Array);
			Assert.AreEqual(expectedLength, (array as Array).Length);
			_elementConverter.Verify(c => c.Convert(It.IsAny<string>(), elementType), Times.Exactly(expectedLength));
		}
	}
}
