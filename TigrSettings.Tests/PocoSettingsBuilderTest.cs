using System;
using Moq;
using NUnit.Framework;

namespace TigrSettings.Tests
{
	public class PocoSettingsBuilderTest
	{
		[Test(TestOf = typeof(PocoSettingsBuilder<Model.Poco>))]
		[TestCase(TestName = "Should build POCO settings object")]
		public void PocoSettingsBuilderShouldBuildPocoSettingsObject()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			settingsProvider.Setup(sp => sp.Get("Int")).Returns("5");
			settingsProvider.Setup(sp => sp.Get("String")).Returns("foobar");
			settingsProvider.Setup(sp => sp.Get("NullableDouble")).Returns(null as string);
			var pocoSettingsBuilder = new PocoSettingsBuilder<Model.Poco>(settingsProvider.Object);

			var result = pocoSettingsBuilder.Create();

			Assert.AreEqual(5, result.Int);
			Assert.AreEqual("foobar", result.String);
			Assert.AreEqual(null, result.NullableDouble);

			settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
		}

		[Test(TestOf = typeof(PocoSettingsBuilder<Model.Poco>))]
		[TestCase(TestName = "Should build inner POCO settings object")]
		public void PocoSettingsBuilderShouldBuildInnerPocoObject()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			var pocoSettingsBuilder = new PocoSettingsBuilder<Model.PocoWithInner>(settingsProvider.Object);
			settingsProvider.Setup(sp => sp.Get("Inner.Int")).Returns("5");

			var result = pocoSettingsBuilder.Create();

			Assert.NotNull(result.Inner);
			Assert.AreEqual(5, result.Inner.Int);
			settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(1));
		}

		[Test(TestOf = typeof(DynamicSettingsBuilder<Model.Poco>))]
		[TestCase(TestName = "Should throw if non-interface type passed")]
		public void DynamicSettingsBuilderShouldThrowIfNonPocoTypePassed()
		{
			var settingsProvider = new Mock<ISettingsProvider>();

			Assert.That(
				() => new DynamicSettingsBuilder<Model.Poco>(settingsProvider.Object),
				Throws.ArgumentException.With.Message.AtLeast("TSettings typeparam must be an interface type.")
			);
		}
	}
}
