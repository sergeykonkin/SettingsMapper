using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TigrSettings.Converters
{
    /// <summary>
    /// Converts raw string setting values to specified Array type.
    /// </summary>
    public class ArrayConverter : ISettingConverter
    {
        private readonly IEnumerable<ISettingConverter> _singleValueConverters;

        /// <summary>
        /// Initializes new instance of <see cref="ArrayConverter"/>.
        /// </summary>
        /// <param name="singleValueConverters">Set of converters for array elements.</param>
        public ArrayConverter(IEnumerable<ISettingConverter> singleValueConverters)
        {
            _singleValueConverters = singleValueConverters;
        }

        /// <inheritdoc />
        public virtual bool CanConvert(Type type)
        {
            if (!type.IsArray)
                return false;

            Type elementType = type.GetElementType();
            return _singleValueConverters.Any(c => c.CanConvert(elementType));
        }

        /// <inheritdoc />
        public virtual object Convert(string value, Type type)
        {
            Type elementType = type.GetElementType();
            var converter = _singleValueConverters.First(c => c.CanConvert(elementType));

            var elements = value.Split(',').Select(s => s.Trim()).ToArray();

            var array = Activator.CreateInstance(type, elements.Length);
            var setValue = type.GetMethod("SetValue", new[] {typeof(object), typeof(int)});

            for (int i = 0; i < elements.Length; i++)
            {
                string element = elements[i];
                var converted = converter.Convert(element, elementType);
                Debug.Assert(setValue != null, nameof(setValue) + " != null");
                setValue.Invoke(array, new[] {converted, i});
            }

            return array;
        }
    }
}
