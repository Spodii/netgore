using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Contains helper methods for file opening and saving dialogs for differnet editor file types. All file dialogs
    /// should be created from this class if possible.
    /// </summary>
    public static class FileDialogs
    {
        /// <summary>
        /// Gets the results from a generic open file dialog.
        /// </summary>
        /// <typeparam name="T">The Type of content to load.</typeparam>
        /// <param name="contentType">The name of the content type being handled.</param>
        /// <param name="fileFilterSuffix">The suffix of the files to filter.</param>
        /// <param name="initialDirectory">The initial directory.</param>
        /// <param name="loadHandler">Delegate describing how to load the type.</param>
        /// <param name="loadedContent">When the method returns a non-null value, and the <paramref name="loadHandler"/>
        /// is not null, contains the loaded content.</param>
        /// <returns>The path to the selected file, or null if invalid or canceled.</returns>
        static string GenericOpenFile<T>(string contentType, string fileFilterSuffix, string initialDirectory,
                                         Func<string, T> loadHandler, out T loadedContent) where T : class
        {
            string filePath;
            loadedContent = null;

            try
            {
                // Create the dialog and get the result
                using (var ofd = new OpenFileDialog())
                {
                    ofd.CheckPathExists = true;
                    ofd.CheckFileExists = true;
                    ofd.AutoUpgradeEnabled = true;
                    ofd.AddExtension = true;
                    ofd.Multiselect = false;
                    ofd.RestoreDirectory = true;
                    ofd.Title = "Open " + contentType;
                    ofd.Filter = string.Format("{0} (*.{1})|*.{1}", contentType, fileFilterSuffix);
                    ofd.InitialDirectory = initialDirectory;
                    var ofdResult = ofd.ShowDialog();
                    if (ofdResult != DialogResult.OK)
                        return null;

                    filePath = ofd.FileName;
                }

                if (filePath == null)
                    return null;

                // Ensure the file game from the initial directory
                if (filePath.Length <= initialDirectory.Length ||
                    !initialDirectory.StartsWith(filePath, StringComparison.OrdinalIgnoreCase))
                {
                    HandleError(
                        "The selected file was from an invalid directory. Files must be selected from the initial directory.",
                        true);
                    return null;
                }

                // Can only check if the content loaded correctly if we are told how to load it
                if (loadHandler != null)
                {
                    // Ensure the content is valid
                    try
                    {
                        loadedContent = loadHandler(filePath);
                    }
                    catch (Exception ex)
                    {
                        HandleError(string.Format("Unable to load the {0} from the selected file.", fileFilterSuffix), true, ex);
                        return null;
                    }

                    if (loadedContent == null)
                    {
                        HandleError(string.Format("Unable to load the {0} from the selected file.", fileFilterSuffix), true);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleUnhandledException(contentType, ex);
                return null;
            }

            return filePath;
        }

        /// <summary>
        /// Handles an error made from one of this class's methods.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="wasLoadError">True if the error was from loading; false if from saving.</param>
        static void HandleError(string message, bool wasLoadError)
        {
            HandleError(message, wasLoadError, null);
        }

        /// <summary>
        /// Handles an error made from one of this class's methods.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="wasLoadError">True if the error was from loading; false if from saving.</param>
        /// <param name="innerException">The inner exception, or null if none.</param>
        static void HandleError(string message, bool wasLoadError, Exception innerException)
        {
            string caption = string.Format("File {0} error", wasLoadError ? "load" : "save");

            if (wasLoadError)
                message = "Failed to load file - " + message;
            else
                message = "Failed to save file - " + message;

            if (innerException != null)
                message = string.Format("{0}:{1}{1}{2}", message, Environment.NewLine, innerException);

            MessageBox.Show(message, caption, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Handles an unhandled <see cref="Exception"/> thrown from one of this class's methods.
        /// </summary>
        /// <param name="contentType">The name of the content type being handled.</param>
        /// <param name="innerException">The inner exception.</param>
        static void HandleUnhandledException(string contentType, Exception innerException)
        {
            string msg = string.Format("Failed to load {0}:{1}{1}{2}", contentType, Environment.NewLine, innerException);

            MessageBox.Show(msg, "Unhandled I/O error", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Creats a dialog to open a particle effect file.
        /// </summary>
        /// <param name="filePath">When the method returns true, contains the path to the file that was open.</param>
        /// <param name="emitter">When the method returns true, contains the <see cref="ParticleEmitter"/>
        /// that was loaded from the path.</param>
        /// <returns>True if the path was valid and the <see cref="ParticleEmitter"/> was successfully loaded
        /// from it; otherwise false.</returns>
        public static bool TryOpenParticleEffect(out string filePath, out ParticleEmitter emitter)
        {
            filePath = null;
            emitter = null;

            try
            {
                filePath = GenericOpenFile("Particle Effect", ParticleEmitterFactory.EmitterFileSuffix,
                                           ContentPaths.Dev.ParticleEffects,
                                           x =>
                                           ParticleEmitterFactory.LoadEmitter(ContentPaths.Dev,
                                                                              ParticleEffectHelper.GetEffectDisplayNameFromFile(x)),
                                           out emitter);

                if (filePath == null)
                    return false;
            }
            catch (Exception ex)
            {
                HandleUnhandledException("Particle Effect", ex);
                return false;
            }

            return true;
        }
    }
}