using System;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Extension methods for the <see cref="DbConnectionSettings"/>.
    /// </summary>
    public static class DbConnectionSettingsExtensions
    {
        /// <summary>
        /// Attempts to create a <see cref="IDbController"/> from a <see cref="DbConnectionSettings"/>. If the
        /// <see cref="IDbController"/> failed to be created, a prompt will be presented to edit the values and retry.
        /// </summary>
        /// <param name="s">The <see cref="DbConnectionSettings"/>.</param>
        /// <param name="createController">The <see cref="Func{T,TResult}"/> describing how to create the
        /// <see cref="IDbController"/> instance.</param>
        /// <param name="createPrompt">The <see cref="Func{T,U}"/> to create the prompt asking to change the values.</param>
        /// <returns>
        /// The <see cref="IDbController"/> instance, or null if the user aborted before making
        /// a successful connection.
        /// </returns>
        public static IDbController CreateDbControllerPromptEditWhenInvalid(this DbConnectionSettings s,
            Func<DbConnectionSettings, IDbController> createController, Func<string, bool> createPrompt)
        {
            IDbController ret = null;

            while (true)
            {
                string msg = null;

                try
                {
                    ret = createController(s);
                }
                catch (DatabaseConnectionException ex)
                {
                    msg = ex.Message;
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }

                if (ret != null)
                    break;

                if (!createPrompt(msg))
                    return null;
            }

            return ret;
        }
    }
}