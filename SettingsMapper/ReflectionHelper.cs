using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SettingsMapper
{
    internal static class ReflectionHelper
    {
        private static readonly IDictionary<Type, MemberInfo[]> _writableMembersCache = new ConcurrentDictionary<Type, MemberInfo[]>();

        public static MemberInfo[] GetWritableMembers(this Type type)
        {
            MemberInfo[] GetWritableMembersImpl()
            {
                var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
                if (type.IsStatic())
                {
                    bindingFlags |= BindingFlags.Static;
                }

                return type
                    .GetProperties(bindingFlags)
                    .Where(prop => prop.CanWrite)
                    .Cast<MemberInfo>()
                    .Union(type.GetFields(bindingFlags))
                    .ToArray();
            }

            if (_writableMembersCache.ContainsKey(type))
            {
                return _writableMembersCache[type];
            }

            var members = GetWritableMembersImpl();
            _writableMembersCache[type] = members;
            return members;
        }

        public static MemberInfo GetWritableMember(this Type type, string name)
        {
            if (!_writableMembersCache.ContainsKey(type))
            {
                GetWritableMembers(type);
            }

            return _writableMembersCache[type].FirstOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static object GetValue(this MemberInfo memberInfo, object obj)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(obj);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(obj, null);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static void SetValue(this MemberInfo memberInfo, object obj, object value)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo)memberInfo).SetValue(obj, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo)memberInfo).SetValue(obj, value, null);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Type GetMemberType(this MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
