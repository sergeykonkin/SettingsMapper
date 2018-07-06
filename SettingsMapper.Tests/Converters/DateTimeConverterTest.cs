using System;
using NUnit.Framework;
using SettingsMapper.Converters;

namespace SettingsMapper.Tests
{
    [TestFixture]
    public class DateTimeConverterTest
    {
        private readonly ISettingConverter _converter = new DateTimeConverter();

        private const string ShouldBeAbleToConvertDateTimeTypeName = "Should be able to convert {0}";
        [Test(TestOf = typeof(DateTimeConverter))]
        [TestCase(typeof(DateTime), TestName = ShouldBeAbleToConvertDateTimeTypeName)]
        public void ShouldBeAbleToConvertDateTypeType(Type dateTimeType)
        {
            var canConvert = _converter.CanConvert(dateTimeType);

            Assert.IsTrue(canConvert);
        }

        private const string ShouldConvertToExpectedValueName = "Should convert string {0} to corresponding {1} value";
        [Test(TestOf = typeof(DateTimeConverter))]
        [TestCase("2018-04-21T12:34:56.789Z", typeof(DateTime), TestName = ShouldConvertToExpectedValueName)]
        [TestCase("Sat, 21 Apr 2018 15:34:56.789 +0300", typeof(DateTime), TestName = ShouldConvertToExpectedValueName)]
        public void ShouldConvertToExpectedValue(string value, Type type)
        {
            var result = _converter.Convert(value, type);

            Assert.IsTrue(result is DateTime);

            var dateTime = ((DateTime)result).ToUniversalTime();
            Assert.AreEqual(2018, dateTime.Year);
            Assert.AreEqual(4, dateTime.Month);
            Assert.AreEqual(21, dateTime.Day);
            Assert.AreEqual(12, dateTime.Hour);
            Assert.AreEqual(34, dateTime.Minute);
            Assert.AreEqual(56, dateTime.Second);
            Assert.AreEqual(789, dateTime.Millisecond);
        }
    }
}
