using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    /// <summary>
    /// Helper methods for file operations.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Tries to open a file from a file path with preferably Notepad, or the default program
        /// if Notepad failed.
        /// </summary>
        /// <param name="filePath">The path to the file to open.</param>
        /// <returns>If the file failed to open, contains an <see cref="Exception"/> for why the operation
        /// failed. If null, the file opened successfully.</returns>
        public static Exception TryOpenWithNotepad(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return new FileNotFoundException("The specified file could not be found.", filePath);

            try
            {
                Process.Start("notepad.exe", filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail("Failed to open file in notepad. Attempting to open with default program for xml files... " + ex);

                try
                {
                    Process.Start(filePath);
                }
                catch (Exception ex2)
                {
                    return ex2;
                }
            }

            return null;
        }
    }
}
