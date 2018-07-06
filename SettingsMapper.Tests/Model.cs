using System;
using SettingsMapper.Attributes;

namespace SettingsMapper.Tests
{
    public class Model
    {
        // POCO:
        public class Poco
        {
            public int Int { get; set; }
            public string String { get; set; }
            public double? NullableDouble { get; set; }
        }

        public class PocoWithInner
        {
            public Inner Inner { get; set; }
        }

        public class PocoWithAttributes
        {
            [Default(5)]
            public int Int { get; set; }

            [Ignore]
            public string String { get; set; }

            [Prefix("foo")]
            public Inner InnerPrefixed { get; set; }

            [Name("Bar")]
            public double? NullableDouble { get; set; }
        }

        // Static:
        public static class Static
        {
            public static int Int { get; set; }
            public static string String { get; set; }
            public static double? NullableDouble { get; set; }
        }

        public static class StaticWithInner
        {
            public static Inner Inner { get; set; }

            [Prefix("foo")]
            public static Inner InnerPrefixed { get; set; }
        }

        public static class StaticWithAttributes
        {
            [Default(5)]
            public static int Int { get; set; }

            [Ignore]
            public static string String { get; set; }

            [Prefix("foo")]
            public static Inner InnerPrefixed { get; set; }

            [Name("Bar")]
            public static double? NullableDouble { get; set; }
        }

        public static class StaticWithNested
        {
            public static class Nested
            {
                public static int Int { get; set; }
            }

            [Prefix("foo")]
            public static class NestedPrefixed
            {
                public static int Int { get; set; }
            }
        }

        // Service:

        public class Inner
        {
            public int Int { get; set; }
        }

        public interface IInner
        {
            int Int { get; }
        }

        public class PocoWithUnknownType
        {
            public UnknownType Unknown { get; set; }
        }

        public class UnknownType
        {
            private UnknownType(string _) { }
        }

        [Flags]
        public enum Enum
        {
            One = 1,
            Two = 2,
            Four = 4
        }
    }
}
