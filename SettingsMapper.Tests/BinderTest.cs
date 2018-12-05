using System;
using NUnit.Framework;

namespace SettingsMapper.Tests
{
    [TestFixture]
    public class MapersTest
    {
        private const string PocoMapperTestName = "Should set public property {0} to value {1}";
        [Test(TestOf = typeof(ObjectMapper))]
        [TestCase("Int", 5, TestName = PocoMapperTestName)]
        [TestCase("String", "foobar", TestName = PocoMapperTestName)]
        [TestCase("NullableDouble", null, TestName = PocoMapperTestName)]
        [TestCase("Field", "baz", TestName = PocoMapperTestName)]
        public void PocoMapperTest(string propName, object value)
        {
            var pocoMapper = new ObjectMapper();
            Type targetType = typeof(Model.Poco);
            object target = pocoMapper.CreateTarget(targetType);

            pocoMapper.Map(target, targetType, propName, value);

            var member = target.GetType().GetWritableMember(propName);
            var result = member.GetValue(target);

            Assert.AreEqual(value, result);
        }

        private const string StaticMapperTestName = "Should set public static property {0} to value {1}";
        [Test(TestOf = typeof(StaticMapper))]
        [TestCase("Int", 5, TestName = StaticMapperTestName)]
        [TestCase("String", "foobar", TestName = StaticMapperTestName)]
        [TestCase("NullableDouble", null, TestName = StaticMapperTestName)]
        [TestCase("Field", "baz", TestName = StaticMapperTestName)]
        public void StaticMapperTest(string propName, object value)
        {
            var staticMapper = new StaticMapper();
            var targetType = typeof(Model.Static);
            var target = staticMapper.CreateTarget(targetType);

            staticMapper.Map(target, targetType, propName, value);

            var member = typeof(Model.Static).GetWritableMember(propName);
            var result = member.GetValue(null);

            Assert.AreEqual(value, result);
        }
    }
}
