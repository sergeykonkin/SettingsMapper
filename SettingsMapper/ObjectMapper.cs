using System;
using System.Collections.Generic;
using System.Linq;

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
                .GetWritableMembers()
                .Select(BindingProp.FromMemberInfo);

        /// <inheritdoc />
        public void Map(object target, Type targetType, string name, object value)
            => targetType.GetWritableMember(name).SetValue(target, value);
    }
}
