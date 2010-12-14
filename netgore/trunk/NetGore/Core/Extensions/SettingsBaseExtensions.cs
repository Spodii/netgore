using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="SettingsBase"/> class.
    /// </summary>
    public static class SettingsBaseExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// In debug builds, asserts that a setting with the given name exists in a <see cref="SettingsBase"/>.
        /// </summary>
        /// <param name="settings">The <see cref="SettingsBase"/>.</param>
        /// <param name="propertyName">The name of the property to check for.</param>
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "dummy")]
        [Conditional("DEBUG")]
        public static void AssertPropertyExists(this SettingsBase settings, string propertyName)
        {
            try
            {
#pragma warning disable 168
                // We need to assign the property to some value, even if we don't use the local, so suppress the warning
                var dummy = settings[propertyName];
#pragma warning restore 168
            }
            catch (SettingsPropertyNotFoundException)
            {
                const string errmsg = "No setting named `{0}` exists in `{1}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, propertyName, settings);
                Debug.Fail(string.Format(errmsg, propertyName, settings));
            }
        }
    }
}