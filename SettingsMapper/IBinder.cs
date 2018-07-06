using System;
using System.Collections.Generic;

namespace SettingsMapper
{
    /// <summary>
    /// Provides functionality to bind provided values to target's properties.
    /// </summary>
    internal interface IBinder
    {
        /// <summary>
        /// Creates new target properties must bind to.
        /// </summary>
        /// <param name="targetType">Target's type.</param>
        /// <returns>Target object.</returns>
        object CreateTarget(Type targetType);

        /// <summary>
        /// Gets target object's properties to bind.
        /// </summary>
        /// <param name="targetType">Target's type.</param>
        /// <returns>Properties bind to.</returns>
        IEnumerable<BindingProp> GetProps(Type targetType);

        /// <summary>
        /// Binds propvided value to specified property.
        /// </summary>
        /// <param name="target">Target bind to.</param>
        /// <param name="targetType">Target's type.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Value to set.</param>
        void Bind(object target, Type targetType, string name, object value);
    }
}
