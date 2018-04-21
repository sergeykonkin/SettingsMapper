using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace TigrSettings.Dynamic
{
	/// <summary>
	/// Binder for Dynamic objects.
	/// </summary>
	internal class DynamicBinder : IBinder
	{
		private readonly ExpandoObject _target;
		private readonly Type _targetType;

		/// <summary>
		/// Initializes a new instance of <see cref="DynamicBinder"/>.
		/// </summary>
		/// <param name="target">Target object settings must be bound to.</param>
		/// <param name="targetType">Target object type.</param>
		public DynamicBinder(ExpandoObject target, Type targetType)
		{
			_target = target;
			_targetType = targetType;
		}

		/// <inheritdoc />
		public PropertyInfo[] GetProps()
		{
			return _targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		}

		/// <inheritdoc />
		public void Bind(string name, object value)
		{
			((IDictionary<string, object>)_target).Add(name, value);
		}
	}
}
