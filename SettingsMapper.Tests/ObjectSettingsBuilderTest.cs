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
            var objectSettingsBuilder = new ObjectSettingsBuilder(settingsType, settingsProvider.Object);

            var result = (Model.Poco)objectSettingsBuilder.Create();

            Assert.AreEqual(5, result.Int);
            Assert.AreEqual("foobar", result.String);
            Assert.AreEqual(null, result.NullableDouble);

            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
        }

        [Test(TestOf = typeof(ObjectSettingsBuilder))]
        [TestCase(TestName = "Should build inner settings object")]
        public void ObjectSettingsBuilderShouldBuildInnerPocoObject()
        {
            var settingsType = typeof(Model.PocoWithInner);
            var settingsProvider = new Mock<ISettingsProvider>();
            var objectSettingsBuilder = new ObjectSettingsBuilder(settingsType, settingsProvider.Object);
            settingsProvider.Setup(sp => sp.Get("Inner:Int")).Returns("5");

            var result = (Model.PocoWithInner)objectSettingsBuilder.Create();

            Assert.NotNull(result.Inner);
            Assert.AreEqual(5, result.Inner.Int);
            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Once);
        }

        [Test(TestOf = typeof(ObjectSettingsBuilder))]
        [TestCase(TestName = "Should respect attributes")]
        public void ObjectSettingsBuilderShouldRespectAttributes()
        {
            var settingsType = typeof(Model.PocoWithAttributes);
            var settingsProvider = new Mock<ISettingsProvider>();
            var objectSettingsBuilder = new ObjectSettingsBuilder(settingsType, settingsProvider.Object);
            settingsProvider.Setup(sp => sp.Get("foo:Int")).Returns("10");
            settingsProvider.Setup(sp => sp.Get("Bar")).Returns("1.1");
            settingsProvider.Setup(sp => sp.Get("String")).Returns("Value");

            var result = (Model.PocoWithAttributes)objectSettingsBuilder.Create();

            Assert.AreEqual(5, result.Int);
            Assert.IsNull(result.String);
            Assert.NotNull(result.InnerPrefixed);
            Assert.AreEqual(10, result.InnerPrefixed.Int);
            Assert.AreEqual(1.1d, result.NullableDouble);
            settingsProvider.Verify(sp => sp.Get(It.IsAny<string>()), Times.Exactly(3));
        }
    }
}
