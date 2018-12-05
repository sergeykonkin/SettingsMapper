using System;
using System.Collections.Generic;
using System.Linq;

namespace SettingsMapper
{
    /// <inheritdoc />
    /// <summary>
    /// Mapper for Static classes.
    /// </summary>
    internal class StaticMapper : IMapper
    {
        /// <inheritdoc />
        public object CreateTarget(Type targetType) =>
            targetType.IsStatic()
                ? null
                : Activator.CreateInstance(targetType);

        /// <inheritdoc />
        public IEnumerable<BindingProp> GetProps(Type targetType) =>
            targetType
                .GetWritableMembers()
                .Select(BindingProp.FromMemberInfo)
                .Union(
                    targetType
                        .GetNestedTypes()
                        .Where(TypeHelper.IsStatic)
                        .Select(BindingProp.FromType));

        /// <inheritdoc />
        public void Map(object target, Type targetType, string name, object value)
            => targetType.GetWritableMember(name)?.SetValue(target, value);
    }
}
