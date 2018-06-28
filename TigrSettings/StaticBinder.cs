using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TigrSettings
{
    /// <summary>
    /// Binder for Static classes.
    /// </summary>
    internal class StaticBinder : IBinder
    {
        /// <inheritdoc />
        public object CreateTarget(Type targetType) =>
            targetType.IsStatic()
                ? null
                : Activator.CreateInstance(targetType);

        /// <inheritdoc />
        public IEnumerable<BindingProp> GetProps(Type targetType) =>
            targetType
                .GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .Select(BindingProp.FromProperty)
                .Union(
                    targetType
                        .GetNestedTypes()
                        .Where(TypeHelper.IsStatic)
                        .Select(BindingProp.FromType));

        /// <inheritdoc />
        public void Bind(object target, Type targetType, string name, object value)
            => targetType.GetProperty(name)?.SetValue(target, value, null);
    }
}
