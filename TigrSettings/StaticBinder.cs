using System;
using System.Reflection;

namespace TigrSettings
{
	/// <summary>
	/// Binder for Static classes.
	/// </summary>
	internal class StaticBinder : IBinder
	{
		private readonly Type _staticType;

		/// <summary>
		/// Initializes a new instance of <see cref="StaticBinder"/>.
		/// </summary>
		/// <param name="staticType">Target class type.</param>
		public StaticBinder(Type staticType)
		{
			_staticType = staticType;
		}

		/// <inheritdoc />
		public PropertyInfo[] GetProps()
		{
			return _staticType.GetProperties(BindingFlags.Public | BindingFlags.Static);
		}

		/// <inheritdoc />
		public void Bind(string name, object value)
		{
			var prop = _staticType.GetProperty(name);
			prop.SetValue(null, value);
		}
	}
}
