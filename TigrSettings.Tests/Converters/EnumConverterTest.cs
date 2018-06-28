using System;
using NUnit.Framework;
using TigrSettings.Converters;

namespace TigrSettings.Tests
{
    [TestFixture]
    public class EnumConverterTest
    {
        private readonly ISettingConverter _converter = new EnumConverter();

        private const string ShouldBeAbleToConvertEnumTypeName = "Should be able to convert {0}";
        [Test(TestOf = typeof(EnumConverter))]
        [TestCase(typeof(Model.Enum), TestName = ShouldBeAbleToConvertEnumTypeName)]
        public void ShouldBeAbleToConvertEnumType(Type enumType)
        {
            var canConvert = _converter.CanConvert(enumType);
            Assert.IsTrue(canConvert);
        }

        private const string ShouldConvertToExpectedValueName = "Should convert string {0} to corresponding {1} value";
        [Test(TestOf = typeof(EnumConverter))]
        [TestCase("Two", typeof(Model.Enum), ExpectedResult = Model.Enum.Two, TestName = ShouldConvertToExpectedValueName)]
        [TestCase("One|Four", typeof(Model.Enum), ExpectedResult = Model.Enum.One | Model.Enum.Four, TestName = ShouldConvertToExpectedValueName)]
        public object ShouldConvertToExpectedValue(string value, Type type)
        {
            return _converter.Convert(value, type);
        }
    }
}
