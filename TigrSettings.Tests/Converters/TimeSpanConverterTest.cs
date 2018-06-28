using System;
using NUnit.Framework;
using TigrSettings.Converters;

namespace TigrSettings.Tests
{
    [TestFixture]
    public class TimeSpanConverterTest
    {
        private readonly ISettingConverter _converter = new TimeSpanConverter();

        private const string ShouldBeAbleToConvertTimeSpanTypeName = "Should be able to convert {0}";
        [Test(TestOf = typeof(TimeSpanConverter))]
        [TestCase(typeof(TimeSpan), TestName = ShouldBeAbleToConvertTimeSpanTypeName)]
        public void ShouldBeAbleToConvertTimeSpanType(Type timeSpanType)
        {
            var canConvert = _converter.CanConvert(timeSpanType);

            Assert.IsTrue(canConvert);
        }

        private const string ShouldConvertToExpectedValueName = "Should convert string {0} to corresponding {1} value";
        [Test(TestOf = typeof(TimeSpanConverter))]
        [TestCase("12:34:56.789", typeof(TimeSpan), TestName = ShouldConvertToExpectedValueName)]
        public void ShouldConvertToExpectedValue(string value, Type type)
        {
            var result = _converter.Convert(value, type);

            Assert.IsTrue(result is TimeSpan);

            var timeSpan = (TimeSpan)result;
            Assert.AreEqual(12, timeSpan.Hours);
            Assert.AreEqual(34, timeSpan.Minutes);
            Assert.AreEqual(56, timeSpan.Seconds);
            Assert.AreEqual(789, timeSpan.Milliseconds);
        }
    }
}
