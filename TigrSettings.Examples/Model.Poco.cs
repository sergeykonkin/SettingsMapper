using System;
using System.Linq;

namespace Examples
{
	public class AppSettings
	{
		public int Int { get; set; }
		public string String { get; set; }
		public Guid Guid { get; set; }
		public EnumSetting Enum { get; set; }
		public DateTime DateTime { get; set; }
		public TimeSpan[] ArrayOfTimeSpans { get; set; }
		public bool? NullableBoolean { get; set; }
		public Inner Inner { get; set; }
		public Empty Empty { get; set; }
	}

	public class Empty
	{
	}

	public class Inner
	{
		public string InnerProp1 { get; set; }
		public int?[] InnerProp2 { get; set; }
		public Inner2 Deeper { get; set; }

		public class Inner2
		{
			public EnumSetting InnerProp3 { get; set; }
		}
	}
}