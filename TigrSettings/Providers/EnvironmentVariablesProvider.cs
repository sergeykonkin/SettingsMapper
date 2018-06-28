using System;
using System.ComponentModel;
using System.Text;

namespace TigrSettings
{
    /// <summary>
    /// Provides raw settings' values from environment variables.
    /// </summary>
    public class EnvironmentVariablesProvider : ISettingsProvider
    {
        private readonly EnvironmentVariableTarget _target;

        /// <summary>
        /// Initializes new instance of <see cref="EnvironmentVariablesProvider"/>.
        /// </summary>
        /// <param name="target">Environment varables target. <see cref="EnvironmentVariableTarget.Process"/> by default.</param>
        public EnvironmentVariablesProvider(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            if (!Enum.IsDefined(typeof(EnvironmentVariableTarget), target))
                throw new InvalidEnumArgumentException(nameof(target), (int) target, typeof(EnvironmentVariableTarget));

            _target = target;
        }

        /// <inheritdoc />
        public virtual string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name, _target) ??
                   Environment.GetEnvironmentVariable(TransformToSnakeCase(name), _target);
        }

        private string TransformToSnakeCase(string str)
        {
            var sb = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsUpper(c))
                    sb.Append('_');

                sb.Append(Char.ToLower(c));
            }

            return sb.ToString().TrimStart('_');
        }
    }
}
