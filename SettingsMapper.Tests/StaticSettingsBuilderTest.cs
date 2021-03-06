﻿using Moq;
using NUnit.Framework;

namespace SettingsMapper.Tests
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
            settingsProvider.Setup(sp => sp.Get("Field")).Returns("baz");

            var staticSettingsBuilder = new StaticSettingsBuilder(settingsProvider.Object);

            staticSettingsBuilder.MapTo(typeof(Model.Static));

            Assert.AreEqual(5, Model.Static.Int);
            Assert.AreEqual("foo", Model.Static.String);
            Assert.AreEqual(null, Model.Static.NullableDouble);
            Assert.AreEqual("baz", Model.Static.Field);
            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(4));
        }

        [Test(TestOf = typeof(StaticSettingsBuilder))]
        [TestCase(TestName = "Should map settings to inner POCO object")]
        public void StaticSettingsBuilderShouldMapSettingsInnerPocoObject()
        {
            var settingsProvider = new Mock<ISettingsProvider>();
            var staticSettingsBuilder = new StaticSettingsBuilder(settingsProvider.Object);
            settingsProvider.Setup(sp => sp.Get("Inner:Int")).Returns("5");
            settingsProvider.Setup(sp => sp.Get("foo:Int")).Returns("10");

            staticSettingsBuilder.MapTo(typeof(Model.StaticWithInner));

            Assert.NotNull(Model.StaticWithInner.Inner);
            Assert.NotNull(Model.StaticWithInner.InnerPrefixed);
            Assert.AreEqual(5, Model.StaticWithInner.Inner.Int);
            Assert.AreEqual(10, Model.StaticWithInner.InnerPrefixed.Int);
            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test(TestOf = typeof(StaticSettingsBuilder))]
        [TestCase(TestName = "Should map settings to nested static class")]
        public void StaticSettingsBuilderShouldMapNestedClasses()
        {
            var settingsProvider = new Mock<ISettingsProvider>();
            settingsProvider.Setup(sp => sp.Get("Nested:Int")).Returns("5");
            settingsProvider.Setup(sp => sp.Get("foo:Int")).Returns("10");

            var staticSettingsBuilder = new StaticSettingsBuilder(settingsProvider.Object);

            staticSettingsBuilder.MapTo(typeof(Model.StaticWithNested));

            Assert.AreEqual(5, Model.StaticWithNested.Nested.Int);
            Assert.AreEqual(10, Model.StaticWithNested.NestedPrefixed.Int);
            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test(TestOf = typeof(StaticSettingsBuilder))]
        [TestCase(TestName = "Should respect attributes")]
        public void StaticSettingsBuilderShouldRespectAttributes()
        {
            var settingsType = typeof(Model.StaticWithAttributes);
            var settingsProvider = new Mock<ISettingsProvider>();
            var staticSettingsBuilder = new StaticSettingsBuilder(settingsProvider.Object);
            settingsProvider.Setup(sp => sp.Get("foo:Int")).Returns("10");
            settingsProvider.Setup(sp => sp.Get("Bar")).Returns("1.1");
            settingsProvider.Setup(sp => sp.Get("String")).Returns("Value");

            staticSettingsBuilder.MapTo(settingsType);

            Assert.AreEqual(5, Model.StaticWithAttributes.Int);
            Assert.IsNull(Model.StaticWithAttributes.String);
            Assert.NotNull(Model.StaticWithAttributes.InnerPrefixed);
            Assert.AreEqual(10, Model.StaticWithAttributes.InnerPrefixed.Int);
            Assert.AreEqual(1.1d, Model.StaticWithAttributes.NullableDouble);
            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
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
