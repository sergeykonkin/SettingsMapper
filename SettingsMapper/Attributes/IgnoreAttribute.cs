using System;

namespace SettingsMapper.Attributes
{
    /// <summary>
    /// Indicates that this property should be ignored.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}
