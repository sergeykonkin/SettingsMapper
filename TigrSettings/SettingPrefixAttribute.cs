using System;

namespace TigrSettings
{
	/// <summary>
	/// Nested settings custom prefix.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SettingPrefixAttribute : Attribute
	{
		/// <summary>
		/// Gets the value of custom prefix.
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// Initializes new instance of <see cref="SettingPrefixAttribute" />
		/// </summary>
		/// <param name="value">Value of custom prefix.</param>
		public SettingPrefixAttribute(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

			Value = value;
		}
	}
}
