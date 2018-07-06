using Moq;
using NUnit.Framework;

namespace SettingsMapper.Tests
{
    public class ObjectSettingsBuilderTest
    {
        [Test(TestOf = typeof(ObjectSettingsBuilder))]
        [TestCase(TestName = "Should build settings object")]
        public void ObjectSettingsBuilderShouldBuildPocoSettingsObject()
        {
            var settingsType = typeof(Model.Poco);
            var settingsProvider = new Mock<ISettingsProvider>();
            settingsProvider.Setup(sp => sp.Get("Int")).Returns("5");
            settingsProvider.Setup(sp => sp.Get("String")).Returns("foobar");
            settingsProvider.Setup(sp => sp.Get("NullableDouble")).Returns(null as string);
            var pocoSettingsBuilder = new ObjectSettingsBuilder(settingsType, settingsProvider.Object);

            var result = (Model.Poco)pocoSettingsBuilder.Create();

            Assert.AreEqual(5, result.Int);
            Assert.AreEqual("foobar", result.String);
            Assert.AreEqual(null, result.NullableDouble);

            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
        }

        [Test(TestOf = typeof(PocoSettingsBuilder<Model.Poco>))]
        [TestCase(TestName = "Should build inner settings object")]
        public void ObjectSettingsBuilderShouldBuildInnerPocoObject()
        {
            var settingsType = typeof(Model.PocoWithInner);
            var settingsProvider = new Mock<ISettingsProvider>();
            var pocoSettingsBuilder = new ObjectSettingsBuilder(settingsType, settingsProvider.Object);
            settingsProvider.Setup(sp => sp.Get("Inner.Int")).Returns("5");
            settingsProvider.Setup(sp => sp.Get("foo.Int")).Returns("10");

            var result = (Model.PocoWithInner)pocoSettingsBuilder.Create();

            Assert.NotNull(result.Inner);
            Assert.NotNull(result.InnerPrefixed);
            Assert.AreEqual(5, result.Inner.Int);
            Assert.AreEqual(10, result.InnerPrefixed.Int);
            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
