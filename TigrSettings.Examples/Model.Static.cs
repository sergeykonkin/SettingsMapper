using System;

namespace Examples
{
	public static class StaticAppSettings
	{
		public static int Int { get; set; }
		public static string String { get; set; }
		public static Guid Guid { get; set; }
		public static EnumSetting Enum { get; set; }
		public static DateTime DateTime { get; set; }
		public static TimeSpan[] ArrayOfTimeSpans { get; set; }
		public static bool? NullableBoolean { get; set; }
		public static Inner Inner { get; set; }
		public static Empty Empty { get; set; }

		public static class NestedStaticClass
		{
			public static string NestedStaticProp { get; set; }
		}
	}
}