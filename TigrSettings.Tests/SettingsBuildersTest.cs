using System;
using System.Globalization;
using Moq;
using NUnit.Framework;
using TigrSettings.Dynamic;

namespace TigrSettings.Tests
{
	[TestFixture]
	public class SettingsBuildersTest
	{
		private Mock<ISettingsProvider> _settingsProvider;
		private Mock<ISettingValueConverter> _converter;

		[SetUp]
		public void Setup()
		{
			_settingsProvider = new Mock<ISettingsProvider>();
			_settingsProvider.Setup(sp => sp.Get("Int")).Returns("5");
			_settingsProvider.Setup(sp => sp.Get("String")).Returns("foobar");
			_settingsProvider.Setup(sp => sp.Get("NullableDouble")).Returns(null as string);
			_settingsProvider.Setup(sp => sp.Get("InvalidValue")).Returns(null as string);
			_settingsProvider.Setup(sp => sp.Get("NoConverter")).Returns(null as string);

			_converter = new Mock<ISettingValueConverter>();
			_converter.Setup(c => c.CanConvert(typeof(int))).Returns(true);
			_converter.Setup(c => c.CanConvert(typeof(double?))).Returns(true);
			_converter.Setup(c => c.Convert("5", typeof(int))).Returns(5);
			_converter.Setup(c => c.Convert(null, typeof(double?))).Returns(null);
		}

		private const string PocoSettingsBuilderTestName = "Should build Poco settings object";
		[Test(TestOf = typeof(PocoSettingsBuilder<Poco>))]
		[TestCase(TestName = PocoSettingsBuilderTestName)]
		public void PocoSettingsBuilderShouldBuildPocoSettingsObject()
		{
			var pocoSettingsBuilder = new PocoSettingsBuilder<Poco>(_settingsProvider.Object, _converter.Object);

			Poco poco = pocoSettingsBuilder.Create();

			Assert.AreEqual(5, poco.Int);
			Assert.AreEqual("foobar", poco.String);
			Assert.AreEqual(null, poco.NullableDouble);

			_settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
			_converter.Verify(sp => sp.CanConvert(It.IsAny<Type>()), Times.Exactly(2));
			_converter.Verify(sp => sp.Convert(It.IsAny<string>(), It.IsAny<Type>()), Times.Exactly(2));
		}

		private const string StaticSettingsBuilderTestName = "Should map settings to static class";
		[Test(TestOf = typeof(StaticSettingsBuilder))]
		[TestCase(TestName = StaticSettingsBuilderTestName)]
		public void StaticSettingsBuilderShouldMapSettingsToStaticClass()
		{
			var staticSettingsBuilder = new StaticSettingsBuilder(_settingsProvider.Object, _converter.Object);

			staticSettingsBuilder.MapTo(typeof(Static));

			Assert.AreEqual(5, Static.Int);
			Assert.AreEqual("foobar", Static.String);
			Assert.AreEqual(null, Static.NullableDouble);

			_settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
			_converter.Verify(sp => sp.CanConvert(It.IsAny<Type>()), Times.Exactly(2));
			_converter.Verify(sp => sp.Convert(It.IsAny<string>(), It.IsAny<Type>()), Times.Exactly(2));
		}

		private const string DynamicSettingsBuilderTestName = "Should build dynamic settings object";
		[Test(TestOf = typeof(DynamicSettingsBuilder<IDynamic>))]
		[TestCase(TestName = DynamicSettingsBuilderTestName)]
		public void DynamicSettingsBuilderShouldBuildDynamicSettingsObject ()
		{
			var dynamicSettingsBuilder = new DynamicSettingsBuilder<IDynamic>(_settingsProvider.Object, _converter.Object);

			IDynamic dynamic = dynamicSettingsBuilder.Create();

			Assert.AreEqual(5, dynamic.Int);
			Assert.AreEqual("foobar", dynamic.String);
			Assert.AreEqual(null, dynamic.NullableDouble);

			_settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
			_converter.Verify(sp => sp.CanConvert(It.IsAny<Type>()), Times.Exactly(2));
			_converter.Verify(sp => sp.Convert(It.IsAny<string>(), It.IsAny<Type>()), Times.Exactly(2));
		}

		[Test(TestOf = typeof(SettingsBuilderBase))]
		[TestCase(TestName = "Should throw if null provided for value type setting")]
		public void SettingsBuilderShouldThrowOnNullValue()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			settingsProvider.Setup(sp => sp.Get("Int")).Returns(null as string);
			var settingsBuilder = new PocoSettingsBuilder<Poco>(settingsProvider.Object, _converter.Object);

			Assert.That(() => settingsBuilder.Create(),
				Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Null value encountered for value type setting.")
			);
		}

		[Test(TestOf = typeof(SettingsBuilderBase))]
		[TestCase(TestName = "Should throw if no setting type converter specified")]
		public void SettingsBuilderShouldThrowIfNoSettingTypeConverterSpecified()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			settingsProvider.Setup(sp => sp.Get("Unknown")).Returns(null as string);
			var settingsBuilder = new PocoSettingsBuilder<PocoWithUnknownType>(settingsProvider.Object, _converter.Object);

			Assert.That(() => settingsBuilder.Create(),
				Throws.TypeOf<NotSupportedException>().With.Message.EqualTo($"No setting value converter found for '{typeof(UnknownType).Name}' type.")
			);
		}

		[Test(TestOf = typeof(SettingsBuilderBase))]
		[TestCase(TestName = "Should throw if null arguments are provided")]
		public void SettingsBuilderShouldThrowIfNullArgumentsAreProvided()
		{
			Assert.That(() => new StaticSettingsBuilder(null),
				Throws.TypeOf<ArgumentNullException>()
			);

			Assert.That(() => new StaticSettingsBuilder(_settingsProvider.Object, formatProvider: null),
				Throws.TypeOf<ArgumentNullException>()
			);

			Assert.That(() => new StaticSettingsBuilder(_settingsProvider.Object, CultureInfo.CurrentCulture, null),
				Throws.TypeOf<ArgumentNullException>()
			);
		}

		private const string ShouldBeAbleToConvertByDefault = "Should be able to convert {0} by default";
		[Test(TestOf = typeof(SettingsBuilderBase))]
		[TestCase(typeof(int), TestName = ShouldBeAbleToConvertByDefault)]
		[TestCase(typeof(DateTime), TestName = ShouldBeAbleToConvertByDefault)]
		[TestCase(typeof(TimeSpan), TestName = ShouldBeAbleToConvertByDefault)]
		[TestCase(typeof(float[]), TestName = ShouldBeAbleToConvertByDefault)]
		[TestCase(typeof(TestEnum), TestName = ShouldBeAbleToConvertByDefault)]
		[TestCase(typeof(char?), TestName = ShouldBeAbleToConvertByDefault)]
		[TestCase(typeof(double?[]), TestName = ShouldBeAbleToConvertByDefault)]
		public void SettingsBuilderShouldBeAbleToConvertSomeTypesByDefault(Type type)
		{
			var settingsBuilder = new StaticSettingsBuilder(_settingsProvider.Object);
			var canConvert = settingsBuilder.CanConvert(type);

			Assert.IsTrue(canConvert);
		}
	}
}
