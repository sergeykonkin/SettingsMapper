using System;
using System.Diagnostics;
using System.Reflection;
using SettingsMapper.Attributes;

namespace SettingsMapper
{
    [DebuggerDisplay("{Name}:{Type}")]
    internal class BindingProp
    {
        public string PropName { get; private set; }

        public string SettingName { get; private set; }

        public Type Type { get; private set; }

        public string CustomPrefix { get; private set; }

        public object DefaultValue { get; private set; }

        public bool HasDefaultValue => DefaultValue != null;

        public bool IsIgnored { get; private set; }

        private BindingProp()
        {
        }

        public static BindingProp FromType(Type type) =>
            new BindingProp
            {
                PropName = type.Name,
                SettingName = GetNameAttr(type),
                Type = type,
                CustomPrefix = GetPrefixAttr(type),
                DefaultValue = GetDefaultAttr(type),
                IsIgnored = GetIgnoredAttr(type)
            };

        public static BindingProp FromProperty(PropertyInfo prop) =>
            new BindingProp
            {
                PropName = prop.Name,
                SettingName = GetNameAttr(prop),
                Type = prop.PropertyType,
                CustomPrefix = GetPrefixAttr(prop),
                DefaultValue = GetDefaultAttr(prop),
                IsIgnored = GetIgnoredAttr(prop)
            };

        private static string GetNameAttr(MemberInfo memberInfo) =>
            memberInfo.GetSingleCustomAttribute<NameAttribute>()?.Value ?? memberInfo.Name;

        private static string GetPrefixAttr(MemberInfo memberInfo) =>
            memberInfo.GetSingleCustomAttribute<PrefixAttribute>()?.Value;

        private static object GetDefaultAttr(MemberInfo memberInfo) =>
            memberInfo.GetSingleCustomAttribute<DefaultAttribute>()?.Value;

        private static bool GetIgnoredAttr(MemberInfo memberInfo) =>
            memberInfo.GetSingleCustomAttribute<IgnoreAttribute>() != null;
    }
}
