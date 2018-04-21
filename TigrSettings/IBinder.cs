using System.Reflection;

namespace TigrSettings
{
	/// <summary>
	/// Provides functionality to bind provided values to target's properties.
	/// </summary>
	public interface IBinder
	{
		/// <summary>
		/// Gets target object's properties to bind.
		/// </summary>
		/// <returns>Properties bind to.</returns>
		PropertyInfo[] GetProps();

		/// <summary>
		/// Binds propvided value to specified property.
		/// </summary>
		/// <param name="name">Property name.</param>
		/// <param name="value">Value to set.</param>
		void Bind(string name, object value);
	}
}
