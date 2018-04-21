using System;
using System.Reflection;

namespace TigrSettings
{
	/// <summary>
	/// Binder for Poco objects.
	/// </summary>
	internal class PocoBinder : IBinder
	{
		private readonly object _target;
		private readonly Type _targetType;

		/// <summary>
		/// Initializes a new instance of <see cref="PocoBinder"/>.
		/// </summary>
		/// <param name="target">Target object settings must be bound to.</param>
		public PocoBinder(object target)
		{
			_target = target;
			_targetType = _target.GetType();
		}

		/// <inheritdoc />
		public virtual PropertyInfo[] GetProps()
		{
			return _targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		}

		/// <inheritdoc />
		public virtual void Bind(string name, object value)
		{
			var prop = _targetType.GetProperty(name);
			prop.SetValue(_target, value);
		}
	}
}
