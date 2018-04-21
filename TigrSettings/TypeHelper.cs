using System;

namespace TigrSettings
{
	/// <summary>
	/// Helper methods for <see cref="Type"/>.
	/// </summary>
	internal static class TypeHelper
	{
		/// <summary>
		/// Checks if <see cref="Type"/> is non-nullable value type.
		/// </summary>
		/// <param name="type">Type to check.</param>
		/// <returns>true - if specified type is non-nullable value type; false - otherwise.</returns>
		public static bool IsNonNullableValueType(this Type type) =>
			type.IsValueType
			&& !(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
	}
}
