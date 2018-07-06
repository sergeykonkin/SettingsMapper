using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SettingsMapper
{
    /// <inheritdoc />
    /// <summary>
    /// Mapper for objects.
    /// </summary>
    internal class ObjectMapper : IMapper
    {
        /// <inheritdoc />
        public object CreateTarget(Type targetType) => Activator.CreateInstance(targetType);

        /// <inheritdoc />
        public IEnumerable<BindingProp> GetProps(Type targetType) =>
            targetType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.CanWrite)
                .Select(BindingProp.FromProperty);

        /// <inheritdoc />
        public void Map(object target, Type targetType, string name, object value)
            => targetType.GetProperty(name).SetValue(target, value, null);
    }
}
