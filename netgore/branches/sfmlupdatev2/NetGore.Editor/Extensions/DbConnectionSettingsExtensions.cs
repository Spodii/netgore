using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Db;

namespace NetGore.Editor
{
    /// <summary>
    /// Extension methods for the <see cref="DbConnectionSettings"/>.
    /// </summary>
    public static class DbConnectionSettingsExtensions
    {
        /// <summary>
        /// Creates a <see cref="MessageBox"/> prompt to edit the file.
        /// </summary>
        /// <param name="s">The <see cref="DbConnectionSettings"/>.</param>
        /// <param name="msg">The message to display.</param>
        /// <returns>
        /// True if the file was edited, reloaded, and the new settings should be used. False if the user
        /// aborted entering the new values or the file failed to be opened for editing.
        /// </returns>
        public static bool PromptEditFileMessageBox(this DbConnectionSettings s, string msg)
        {
            if (!s.OpenFileForEdit())
                return false;

            const string instructions =
                "Please edit the database settings with the appropriate values. Press Retry when done editing, or Cancel to abort.";

            if (msg == null)
                msg = instructions;
            else
                msg += Environment.NewLine + Environment.NewLine + instructions;

            if (MessageBox.Show(msg, "Edit database settings", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                return false;

            s.Reload();

            return true;
        }
    }
}