using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TigrSettings
{
    /// <summary>
    /// Binder for Poco objects.
    /// </summary>
    internal class PocoBinder : IBinder
    {
        /// <inheritdoc />
        public object CreateTarget(Type targetType) => Activator.CreateInstance(targetType);

        /// <inheritdoc />
        public IEnumerable<BindingProp> GetProps(Type targetType) =>
            targetType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(BindingProp.FromProperty);

        /// <inheritdoc />
        public void Bind(object target, Type targetType, string name, object value)
            => targetType.GetProperty(name).SetValue(target, value, null);
    }
}
