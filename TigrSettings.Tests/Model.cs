using System;

namespace TigrSettings.Tests
{
	public class Poco
	{
		public int Int { get; set; }
		public string String { get; set; }
		public double? NullableDouble { get; set; }
	}

	public static class Static
	{
		public static int Int { get; set; }
		public static string String { get; set; }
		public static double? NullableDouble { get; set; }
	}

	public interface IDynamic
	{
		int Int { get; }
		string String { get; }
		double? NullableDouble { get; }
	}

	public class PocoWithUnknownType
	{
		public UnknownType Unknown { get; set; }
	}

	public class UnknownType
	{
	}

	[Flags]
	public enum TestEnum
	{
		One = 1,
		Two = 2,
		Four = 4
	}
}
