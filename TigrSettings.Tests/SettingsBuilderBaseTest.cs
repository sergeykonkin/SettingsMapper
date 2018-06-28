using System;
using System.Globalization;
using Moq;
using NUnit.Framework;

namespace TigrSettings.Tests
{
    [TestFixture]
    public class SettingsBuilderBaseTest
    {
        private class SettingsBuilderBaseTestWrapper : SettingsBuilderBase
        {
            public SettingsBuilderBaseTestWrapper(ISettingsProvider settingsProvider, params ISettingConverter[] converters)
                : this(settingsProvider, CultureInfo.InvariantCulture, converters)
            {
            }

            public SettingsBuilderBaseTestWrapper(ISettingsProvider settingsProvider, IFormatProvider formatProvider, params ISettingConverter[] converters)
                : base(settingsProvider, formatProvider, converters)
            {
            }

            internal override IBinder Binder { get; } = new PocoBinder();
        }

        private const string ShouldBeAbleToConvertByDefault = "Should be able to convert {0} by default";
        [Test(TestOf = typeof(SettingsBuilderBase))]
        [TestCase(typeof(int), TestName = ShouldBeAbleToConvertByDefault)]
        [TestCase(typeof(Guid), TestName = ShouldBeAbleToConvertByDefault)]
        [TestCase(typeof(DateTime), TestName = ShouldBeAbleToConvertByDefault)]
        [TestCase(typeof(TimeSpan), TestName = ShouldBeAbleToConvertByDefault)]
        [TestCase(typeof(float[]), TestName = ShouldBeAbleToConvertByDefault)]
        [TestCase(typeof(Model.Enum), TestName = ShouldBeAbleToConvertByDefault)]
        [TestCase(typeof(byte[]), TestName = ShouldBeAbleToConvertByDefault)]
        [TestCase(typeof(char?), TestName = ShouldBeAbleToConvertByDefault)]
        [TestCase(typeof(double?[]), TestName = ShouldBeAbleToConvertByDefault)]
        public void SettingsBuilderShouldBeAbleToConvertSomeTypesByDefault(Type type)
        {
            var settingsProvider = new Mock<ISettingsProvider>();
            var settingsBuilder = new StaticSettingsBuilder(settingsProvider.Object);

            var canConvert = settingsBuilder.CanConvert(type);

            Assert.IsTrue(canConvert);
        }

        [Test(TestOf = typeof(SettingsBuilderBase))]
        [TestCase(TestName = "Should throw if null provided for value type setting")]
        public void SettingsBuilderShouldThrowOnNullValue()
        {
            var settingsProvider = new Mock<ISettingsProvider>();
            settingsProvider.Setup(sp => sp.Get("Int")).Returns(null as string);
            var settingsBuilder = new SettingsBuilderBaseTestWrapper(settingsProvider.Object);

            Assert.That(() => settingsBuilder.Build(typeof(Model.Poco)),
                Throws.TypeOf<InvalidOperationException>()
                    .With.Message.AtLeast("Null value encountered for value type setting")
            );
        }

        [Test(TestOf = typeof(SettingsBuilderBase))]
        [TestCase(TestName = "Should throw if cannot convert inner type and no custom converter specified")]
        public void SettingsBuilderShouldThrowIfNoSettingTypeConverterSpecified()
        {
            var settingsProvider = new Mock<ISettingsProvider>();
            var settingsBuilder = new SettingsBuilderBaseTestWrapper(settingsProvider.Object);

            Assert.That(
                () => settingsBuilder.Build(typeof(Model.PocoWithUnknownType)),
                Throws.TypeOf<InvalidOperationException>()
                    .With.Message.AtLeast($"Cannot map settings to {typeof(Model.UnknownType).Name} type")
            );
        }

        [Test(TestOf = typeof(SettingsBuilderBase))]
        [TestCase(TestName = "Should throw if null arguments are provided")]
        public void SettingsBuilderShouldThrowIfNullArgumentsAreProvided()
        {
            var settingsProvider = new Mock<ISettingsProvider>();

            Assert.That(() => new SettingsBuilderBaseTestWrapper(null),
                Throws.TypeOf<ArgumentNullException>()
            );

            Assert.That(() => new SettingsBuilderBaseTestWrapper(settingsProvider.Object, formatProvider: null),
                Throws.TypeOf<ArgumentNullException>()
            );

            Assert.That(() => new SettingsBuilderBaseTestWrapper(settingsProvider.Object, CultureInfo.CurrentCulture, null),
                Throws.TypeOf<ArgumentNullException>()
            );
        }
    }
}
