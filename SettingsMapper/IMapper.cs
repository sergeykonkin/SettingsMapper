using System;
using System.Collections.Generic;

namespace SettingsMapper
{
    /// <summary>
    /// Provides functionality to map provided values to target's properties.
    /// </summary>
    internal interface IMapper
    {
        /// <summary>
        /// Creates new target.
        /// </summary>
        /// <param name="targetType">Target's type.</param>
        /// <returns>Target object.</returns>
        object CreateTarget(Type targetType);

        /// <summary>
        /// Gets target object's properties to map.
        /// </summary>
        /// <param name="targetType">Target's type.</param>
        /// <returns>Properties for mapping.</returns>
        IEnumerable<BindingProp> GetProps(Type targetType);

        /// <summary>
        /// Maps propvided value to specified property.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <param name="targetType">Target's type.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">Value to set.</param>
        void Map(object target, Type targetType, string name, object value);
    }
}
