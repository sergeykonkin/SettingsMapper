using System;

namespace TigrSettings.Converters
{
	/// <summary>
	/// Converts raw string setting values to <see cref="Guid"/> type.
	/// </summary>
	public class GuidConverter : SettingValueConverterBase<Guid>
	{
		/// <inheritdoc />
		public override Guid Convert(string value)
		{
			return Guid.Parse(value);
		}
	}
}
