using System;
using NUnit.Framework;
using TigrSettings.Converters;

namespace TigrSettings.Tests
{
	[TestFixture]
	public class ByteArrayConverterTest
	{
		private readonly ISettingConverter _converter = new ByteArrayConverter();

		private const string ShouldBeAbleToConvertByteArrayTypeName = "Should be able to convert {0}";
		[Test(TestOf = typeof(DateTimeConverter))]
		[TestCase(typeof(byte[]), TestName = ShouldBeAbleToConvertByteArrayTypeName)]
		public void ShouldBeAbleToConvertByteArrayType(Type byteArrayType)
		{
			var canConvert = _converter.CanConvert(byteArrayType);

			Assert.IsTrue(canConvert);
		}

		private const string ShouldBeAbleToConvertBase64AndHexStringsName = "Should be able to convert {0} string";
		[Test(TestOf = typeof(ByteArrayConverter))]
		[TestCase("BAgPEBcq", TestName = ShouldBeAbleToConvertBase64AndHexStringsName)]
		[TestCase("0x04080f10172a", TestName = ShouldBeAbleToConvertBase64AndHexStringsName)]
		public void ShouldBeAbleToConvertBase64AndHexStrings(string value)
		{
			var result = _converter.Convert(value, typeof(byte[]));

			var expected = new byte[]
			{
				4, 8, 15, 16, 23, 42
			};

			Assert.AreEqual(expected, result);
		}

		[Test(TestOf = typeof(ByteArrayConverter))]
		[TestCase(TestName = "Should throw if string neither Base64 nor HEX string.")]
		public void ShouldThrowOnInvalidString()
		{
			Assert.That(
				() => _converter.Convert("invalid string", typeof(byte[])),
				Throws.InvalidOperationException.With.Message.EqualTo("Could not parse input string as either Base64 nor HEX string.")
			);
		}

		[Test(TestOf = typeof(ByteArrayConverter))]
		[TestCase(TestName = "Should throw if HEX string of odd length provided.")]
		public void ShouldThrowOnInvalidStringLength()
		{
			Assert.That(
				() => _converter.Convert("0x04080f10172a1", typeof(byte[])),
				Throws.InvalidOperationException.With.InnerException.With.Message.EqualTo("Input string must be of even length.")
			);
		}
	}
}
