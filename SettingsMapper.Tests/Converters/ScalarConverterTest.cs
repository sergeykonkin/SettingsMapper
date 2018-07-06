using System;
using NUnit.Framework;
using SettingsMapper.Converters;

namespace SettingsMapper.Tests
{
    [TestFixture]
    public class ScalarConverterTest
    {
        private readonly ISettingConverter _converter = new ScalarConverter();

        private const string ShouldBeAbleToConvertAllPrimitiveTypesName = "Should be able to convert {0}";
        [Test(TestOf = typeof(ScalarConverter))]
        [TestCase(typeof(bool), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(byte), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(sbyte), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(short), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(ushort), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(int), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(uint), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(long), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(ulong), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(char), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(float), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        [TestCase(typeof(double), TestName = ShouldBeAbleToConvertAllPrimitiveTypesName)]
        public void ShouldBeAbleToConvertAllPrimitiveTypes(Type primitiveType)
        {
            var canConvert = _converter.CanConvert(primitiveType);
            Assert.IsTrue(canConvert);
        }

        private const string ShouldNotConvertNonScalarPrimitiveTypesName = "Should not be able to convert {0}";
        [Test(TestOf = typeof(ScalarConverter))]
        [TestCase(typeof(IntPtr), TestName = ShouldNotConvertNonScalarPrimitiveTypesName)]
        [TestCase(typeof(UIntPtr), TestName = ShouldNotConvertNonScalarPrimitiveTypesName)]
        [TestCase(typeof(string), TestName = ShouldNotConvertNonScalarPrimitiveTypesName)]
        [TestCase(typeof(object), TestName = ShouldNotConvertNonScalarPrimitiveTypesName)]
        public void ShouldNotConvertNonScalarPrimitiveTypes(Type primitiveType)
        {
            var canConvert = _converter.CanConvert(primitiveType);
            Assert.IsFalse(canConvert);
        }

        private const string ShouldConvertToExpectedValueName = "Should convert string {0} to corresponding {1} value";
        [Test(TestOf = typeof(ScalarConverter))]
        [TestCase("false", typeof(bool), ExpectedResult = false, TestName = ShouldConvertToExpectedValueName)]
        [TestCase("true", typeof(bool), ExpectedResult = true, TestName = ShouldConvertToExpectedValueName)]
        [TestCase("0", typeof(int), ExpectedResult = 0, TestName = ShouldConvertToExpectedValueName)]
        [TestCase("-1", typeof(int), ExpectedResult = -1, TestName = ShouldConvertToExpectedValueName)]
        [TestCase("1", typeof(int), ExpectedResult = 1, TestName = ShouldConvertToExpectedValueName)]
        [TestCase("14.88", typeof(double), ExpectedResult = 14.88d, TestName = ShouldConvertToExpectedValueName)]
        [TestCase("-13.37", typeof(float), ExpectedResult = -13.37f, TestName = ShouldConvertToExpectedValueName)]
        [TestCase("255", typeof(byte), ExpectedResult = 255, TestName = ShouldConvertToExpectedValueName)]
        [TestCase("F", typeof(char), ExpectedResult = 'F', TestName = ShouldConvertToExpectedValueName)]
        public object ShouldConvertToExpectedValue(string value, Type type)
        {
            return _converter.Convert(value, type);
        }

        private const string ShouldThrowOnNegativeUnsignedName = "Should throw if negative value provided for {1} type";
        [Test(TestOf = typeof(ScalarConverter))]
        [TestCase("-1", typeof(ushort), TestName = ShouldThrowOnNegativeUnsignedName)]
        [TestCase("-1", typeof(uint), TestName = ShouldThrowOnNegativeUnsignedName)]
        [TestCase("-1", typeof(ulong), TestName = ShouldThrowOnNegativeUnsignedName)]
        [TestCase("-1", typeof(byte), TestName = ShouldThrowOnNegativeUnsignedName)]
        public void ShouldThrowOnNegativeUnsigned(string value, Type type)
        {
            Assert.That(() => _converter.Convert(value, type),
                Throws.TypeOf<OverflowException>()
            );
        }
    }
}
