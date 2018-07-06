using System;
using System.Diagnostics;
using System.Reflection;

namespace SettingsMapper
{
    [DebuggerDisplay("{Name}:{Type}")]
    internal class BindingProp
    {
        public string Name { get; }

        public Type Type { get; }

        public string CustomPrefix { get; }

        private BindingProp(string name, Type type, string prefix)
        {
            Name = name;
            Type = type;
            CustomPrefix = prefix;
        }

        public static BindingProp FromType(Type type) =>
            new BindingProp(type.Name, type, GetPrefixAttributeValue(type));

        public static BindingProp FromProperty(PropertyInfo prop) =>
            new BindingProp(prop.Name, prop.PropertyType, GetPrefixAttributeValue(prop));

        private static string GetPrefixAttributeValue(MemberInfo memberInfo) =>
            memberInfo.GetSingleCustomAttribute<SettingPrefixAttribute>()?.Value;
    }
}
