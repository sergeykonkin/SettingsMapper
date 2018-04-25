﻿using Moq;
using NUnit.Framework;

namespace TigrSettings.Tests
{
	public class DynamicSettingsBuilderTest
	{
		[Test(TestOf = typeof(DynamicSettingsBuilder<Model.IDynamic>))]
		[TestCase(TestName = "Should build dynamic settings object")]
		public void DynamicSettingsBuilderShouldBuildDynamicSettingsObject ()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			settingsProvider.Setup(sp => sp.Get("Int")).Returns("5");
			settingsProvider.Setup(sp => sp.Get("String")).Returns("foobar");
			settingsProvider.Setup(sp => sp.Get("NullableDouble")).Returns(null as string);

			var dynamicSettingsBuilder = new DynamicSettingsBuilder<Model.IDynamic>(settingsProvider.Object);

			var result = dynamicSettingsBuilder.Create();

			Assert.AreEqual(5, result.Int);
			Assert.AreEqual("foobar", result.String);
			Assert.AreEqual(null, result.NullableDouble);

			settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
		}

		[Test(TestOf = typeof(DynamicSettingsBuilder<Model.IDynamic>))]
		[TestCase(TestName = "Should build inner POCO settings object")]
		public void DynamicSettingsBuilderShouldBuildInnerPocoObject()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			var dynamicSettingsBuilder = new DynamicSettingsBuilder<Model.IDynamicWithInner>(settingsProvider.Object);
			settingsProvider.Setup(sp => sp.Get("Inner.Int")).Returns("5");

			var result = dynamicSettingsBuilder.Create();

			Assert.NotNull(result.Inner);
			Assert.AreEqual(5, result.Inner.Int);

			settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(1));
		}

		[Test(TestOf = typeof(DynamicSettingsBuilder<Model.IDynamic>))]
		[TestCase(TestName = "Should build dynamic inner object which implements interface")]
		public void DynamicSettingsBuilderShouldBuildInnerDynamicObject()
		{
			var settingsProvider = new Mock<ISettingsProvider>();
			var dynamicSettingsBuilder = new DynamicSettingsBuilder<Model.IDynamicWithIInner>(settingsProvider.Object);
			settingsProvider.Setup(sp => sp.Get("Inner.Int")).Returns("5");

			var result = dynamicSettingsBuilder.Create();

			Assert.NotNull(result.Inner);
			Assert.IsInstanceOf<Model.IInner>(result.Inner);
			Assert.AreEqual(5, result.Inner.Int);

			settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(1));
		}
	}
}