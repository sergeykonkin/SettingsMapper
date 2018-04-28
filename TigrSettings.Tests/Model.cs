using System;

namespace TigrSettings.Tests
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

			[SettingPrefix("foo")]
			public Inner InnerPrefixed { get; set; }
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

			[SettingPrefix("foo")]
			public static Inner InnerPrefixed { get; set; }
		}

		public static class StaticWithNested
		{
			public static class Nested
			{
				public static int Int { get; set; }
			}

			[SettingPrefix("foo")]
			public static class NestedPrefixed
			{
				public static int Int { get; set; }
			}
		}

		// Dynamic:
		public interface IDynamic
		{
			int Int { get; }
			string String { get; }
			double? NullableDouble { get; }
		}

		public interface IDynamicWithInner
		{
			Inner Inner { get; }

			[SettingPrefix("foo")]
			Inner InnerPrefixed { get;}
		}

		public interface IDynamicWithIInner
		{
			IInner Inner { get; }

			[SettingPrefix("foo")]
			IInner InnerPrefixed { get; }
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
			private UnknownType(string _){}
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
