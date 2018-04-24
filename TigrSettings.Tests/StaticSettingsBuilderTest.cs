using Moq;
using NUnit.Framework;

namespace TigrSettings.Tests
{
	public class StaticSettingsBuilderTest
	{
		[Test(TestOf = typeof(StaticSettingsBuilder))]
		[TestCase(TestName = "Should map settings to static class")]
		public void StaticSettingsBuilderShouldMapSettingsToStaticClass()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			settingsProvider.Setup(sp => sp.Get("Int")).Returns("5");
			settingsProvider.Setup(sp => sp.Get("String")).Returns("foo");
			settingsProvider.Setup(sp => sp.Get("NullableDouble")).Returns(null as string);
			var staticSettingsBuilder = new StaticSettingsBuilder(settingsProvider.Object);

			staticSettingsBuilder.MapTo(typeof(Model.Static));

			Assert.AreEqual(5, Model.Static.Int);
			Assert.AreEqual("foo", Model.Static.String);
			Assert.AreEqual(null, Model.Static.NullableDouble);
			settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
		}

		[Test(TestOf = typeof(StaticSettingsBuilder))]
		[TestCase(TestName = "Should map settings to inner POCO object")]
		public void StaticSettingsBuilderShouldMapSettingsInnerPocoObject()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			var staticSettingsBuilder = new StaticSettingsBuilder(settingsProvider.Object);
			settingsProvider.Setup(sp => sp.Get("Inner.Int")).Returns("5");

			staticSettingsBuilder.MapTo(typeof(Model.StaticWithInner));

			Assert.NotNull(Model.StaticWithInner.Inner);
			Assert.AreEqual(5, Model.StaticWithInner.Inner.Int);
			settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(1));
		}

		[Test(TestOf = typeof(StaticSettingsBuilder))]
		[TestCase(TestName = "Should map settings to nested static class")]
		public void StaticSettingsBuilderShouldMapNestedClasses()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			settingsProvider.Setup(sp => sp.Get("Nested.Int")).Returns("5");
			var staticSettingsBuilder = new StaticSettingsBuilder(settingsProvider.Object);

			staticSettingsBuilder.MapTo(typeof(Model.StaticWithNested));

			Assert.AreEqual(5, Model.StaticWithNested.Nested.Int);
			settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(1));
		}

		[Test(TestOf = typeof(StaticSettingsBuilder))]
		[TestCase(TestName = "Should throw if non-static class type passed")]
		public void StaticSettingsBuilderShouldThrowIfNonStaticClassTypePassed()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			var staticSettingsBuilder = new StaticSettingsBuilder(settingsProvider.Object);

			Assert.That(
				() => staticSettingsBuilder.MapTo(typeof(Model.Poco)),
				Throws.ArgumentException.With.Message.AtLeast("Provided type must be a static class.")
			);
		}
	}
}
