using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace TigrSettings
{
    /// <summary>
    /// Binder for Dynamic objects.
    /// </summary>
    internal class DynamicBinder : IBinder
    {
        /// <inheritdoc />
        public object CreateTarget(Type targetType) =>
            targetType.IsInterface
                ? new ExpandoObject()
                : Activator.CreateInstance(targetType);

        /// <inheritdoc />
        public IEnumerable<BindingProp> GetProps(Type targetType) =>
            targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(BindingProp.FromProperty);

        /// <inheritdoc />
        public void Bind(object target, Type targetType, string name, object value)
        {
            if (target is ExpandoObject)
                ((IDictionary<string, object>) target).Add(name, value);
            else
                targetType.GetProperty(name).SetValue(target, value, null);
        }
    }
}
