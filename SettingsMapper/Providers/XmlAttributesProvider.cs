using System;
using System.Linq;
using System.Xml;

namespace SettingsMapper
{
    /// <summary>
    /// Provides raw settings' values from XML node's attributes.
    /// </summary>
    public class XmlAttributesProvider : ISettingsProvider
    {
        private readonly XmlNode _node;

        /// <summary>
        /// Initializes new instance of <see cref="XmlAttributesProvider"/>.
        /// </summary>
        /// <param name="node">Node to read attributes from.</param>
        public XmlAttributesProvider(XmlNode node)
        {
            _node = node;
        }

        /// <inheritdoc />
        public string Get(string name)
        {
            return _node.Attributes?
                .Cast<XmlAttribute>()
                .FirstOrDefault(attr => string.Equals(attr.Name, name, StringComparison.InvariantCultureIgnoreCase))?
                .Value;
        }
    }
}
