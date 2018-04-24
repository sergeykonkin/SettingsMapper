using System;
using System.Diagnostics;

namespace TigrSettings
{
	[DebuggerDisplay("{Name}:{Type}")]
	internal class BindingProp
	{
		public string Name { get; }

		public Type Type { get; }

		private BindingProp(string name, Type type)
		{
			Name = name;
			Type = type;
		}

		public static BindingProp FromType(Type type) =>
			new BindingProp(type.Name, type);

		public static BindingProp FromProperty(System.Reflection.PropertyInfo sysProp) =>
			new BindingProp(sysProp.Name, sysProp.PropertyType);
	}
}
