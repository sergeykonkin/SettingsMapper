using System;
using System.Linq;
using System.Reflection;

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
            &&
            !(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));

        /// <summary>
        /// Checks if <see cref="Type"/> is static type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true - if specified type is static type; false - otherwise.</returns>
        public static bool IsStatic(this Type type)
            => type.IsAbstract && type.IsSealed;

        /// <summary>
        /// Gets an single instance of member's attribute.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="memberInfo">Member.</param>
        /// <returns>Instance of member's attribute</returns>
        public static TAttribute GetSingleCustomAttribute<TAttribute>(this MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            return memberInfo
                    .GetCustomAttributes(typeof(TAttribute), false)
                    .SingleOrDefault()
                as TAttribute;
        }
    }
}
