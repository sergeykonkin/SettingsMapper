﻿using Moq;
using NUnit.Framework;

namespace SettingsMapper.Tests
{
    public class PocoSettingsBuilderTTest
    {
        [Test(TestOf = typeof(PocoSettingsBuilder<Model.Poco>))]
        [TestCase(TestName = "Should build POCO settings object")]
        public void PocoSettingsBuilderShouldBuildPocoSettingsObject()
        {
            var settingsProvider = new Mock<ISettingsProvider>();
            settingsProvider.Setup(sp => sp.Get("Int")).Returns("5");
            settingsProvider.Setup(sp => sp.Get("String")).Returns("foobar");
            settingsProvider.Setup(sp => sp.Get("NullableDouble")).Returns(null as string);
            settingsProvider.Setup(sp => sp.Get("Field")).Returns("baz");

            var pocoSettingsBuilder = new PocoSettingsBuilder<Model.Poco>(settingsProvider.Object);

            var result = pocoSettingsBuilder.Create();

            Assert.AreEqual(5, result.Int);
            Assert.AreEqual("foobar", result.String);
            Assert.AreEqual(null, result.NullableDouble);
            Assert.AreEqual("baz", result.Field);

            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(4));
        }

        [Test(TestOf = typeof(PocoSettingsBuilder<Model.PocoWithInner>))]
        [TestCase(TestName = "Should build inner POCO settings object")]
        public void PocoSettingsBuilderShouldBuildInnerPocoObject()
        {
            var settingsProvider = new Mock<ISettingsProvider>();
            var pocoSettingsBuilder = new PocoSettingsBuilder<Model.PocoWithInner>(settingsProvider.Object);
            settingsProvider.Setup(sp => sp.Get("Inner:Int")).Returns("5");

            var result = pocoSettingsBuilder.Create();

            Assert.NotNull(result.Inner);
            Assert.AreEqual(5, result.Inner.Int);
            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Once);
        }

        [Test(TestOf = typeof(PocoSettingsBuilder<Model.PocoWithAttributes>))]
        [TestCase(TestName = "Should respect attributes")]
        public void ObjectSettingsBuilderShouldRespectAttributes()
        {
            var settingsProvider = new Mock<ISettingsProvider>();
            var pocoSettingsBuilder = new PocoSettingsBuilder<Model.PocoWithAttributes>(settingsProvider.Object);
            settingsProvider.Setup(sp => sp.Get("foo:Int")).Returns("10");
            settingsProvider.Setup(sp => sp.Get("Bar")).Returns("1.1");
            settingsProvider.Setup(sp => sp.Get("String")).Returns("Value");

            var result = pocoSettingsBuilder.Create();

            Assert.AreEqual(5, result.Int);
            Assert.IsNull(result.String);
            Assert.NotNull(result.InnerPrefixed);
            Assert.AreEqual(10, result.InnerPrefixed.Int);
            Assert.AreEqual(1.1d, result.NullableDouble);
            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
        }
    }
}
