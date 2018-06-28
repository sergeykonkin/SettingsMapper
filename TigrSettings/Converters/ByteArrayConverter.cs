using System;
using System.Linq;
using static System.Convert;

namespace TigrSettings.Converters
{
    /// <summary>
    /// Converts Base64 encoded raw string setting values to Byte Array type.
    /// </summary>
    public class ByteArrayConverter : SettingConverterBase<byte[]>
    {
        /// <inheritdoc />
        public override byte[] Convert(string value)
        {
            try
            {
                return value.StartsWith("0x")
                    ? FromHexString(value.Substring(2, value.Length - 2))
                    : FromBase64String(value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not parse input string as either Base64 nor HEX string.",
                    ex);
            }
        }

        private static byte[] FromHexString(string hex)
        {
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Input string must be of even length.");

            return Enumerable.Range(0, hex.Length)
                .Where(index => index % 2 == 0)
                .Select(index => ToByte(hex.Substring(index, 2), 16))
                .ToArray();
        }
    }
}
