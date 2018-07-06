using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using ImpromptuInterface;

namespace SettingsMapper
{
    /// <summary>
    /// Provides functionality to bind raw settings to Dynamic objects that implements specified interface.
    /// </summary>
    public class DynamicSettingsBuilder<TSettings> : SettingsBuilderBase
        where TSettings : class
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DynamicSettingsBuilder{TSettings}"/>.
        /// </summary>
        /// <param name="settingsProvider">Raw string settings provider.</param>
        /// <param name="converters">Set of additional converters.</param>
        public DynamicSettingsBuilder(
            ISettingsProvider settingsProvider,
            params ISettingConverter[] converters)
            : this(settingsProvider, CultureInfo.InvariantCulture, converters)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DynamicSettingsBuilder{TSettings}"/>.
        /// </summary>
        /// <param name="settingsProvider">Raw string settings provider.</param>
        /// <param name="formatProvider">Format provider for numbers and/or dates.</param>
        /// <param name="converters">Set of additional converters.</param>
        public DynamicSettingsBuilder(
            ISettingsProvider settingsProvider,
            IFormatProvider formatProvider,
            params ISettingConverter[] converters)
            : base(settingsProvider, formatProvider, converters)
        {
            if (!typeof(TSettings).IsInterface)
                throw new ArgumentException($"{nameof(TSettings)} typeparam must be an interface type.",
                    nameof(TSettings));
        }

        /// <summary>
        /// Creates new instance of dynamic object that implements <typeparamref name="TSettings"/> with properties filled with converted settings.
        /// </summary>
        /// <returns>Dynamic object instance.</returns>
        public TSettings Create()
        {
            object expando = Build(typeof(TSettings)) ?? new ExpandoObject();
            return expando.ActLike<TSettings>();
        }

        /// <inheritdoc />
        internal override object Build(Type targetType, string prefix = null)
        {
            object result = base.Build(targetType, prefix);
            if (!(result is ExpandoObject))
                return result;

            IDictionary<string, Type> props = Binder
                .GetProps(targetType)
                .ToDictionary(prop => prop.Name, prop => prop.Type);

            return result.ActLikeProperties(props);
        }

        /// <inheritdoc />
        internal override IBinder Binder { get; } = new DynamicBinder();

        /// <inheritdoc />
        protected override string TypeDesc => base.TypeDesc + " or interface";
    }
}
