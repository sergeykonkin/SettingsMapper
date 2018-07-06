using System;

namespace SettingsMapper
{
    /// <summary>
    /// Nested settings custom prefix.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class PrefixAttribute : Attribute
    {
        /// <summary>
        /// Gets the value of custom prefix.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes new instance of <see cref="PrefixAttribute" />
        /// </summary>
        /// <param name="value">Value of custom prefix.</param>
        public PrefixAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            Value = value;
        }
    }
}
