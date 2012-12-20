using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;

namespace NetGore.IO
{
    public static class PathHelper
    {
        /// <summary>
        /// Finds the first parent directory of the given starting path that matches the specified condition.
        /// </summary>
        /// <param name="startDirectory">The starting path.</param>
        /// <param name="condition">A func containing a directory to check. When true, returns the directory.</param>
        /// <returns>The DirectoryInfo of the first parent directory from the <paramref name="startDirectory"/> that matches the <paramref name="condition"/>,
        /// or null if none could be found.</returns>
        public static DirectoryInfo FindParentDirectoryWhere(string startDirectory, Func<DirectoryInfo, bool> condition)
        {
            DirectoryInfo curr = Directory.Exists(startDirectory) ? new DirectoryInfo(startDirectory) : Directory.GetParent(startDirectory);

            try
            {
                while ((curr != null) && !condition(curr))
                {
                    curr = curr.Parent;
                }
            }
            catch (IOException)
            {
                return null;
            }
            catch (SecurityException)
            {
                return null;
            }

            return curr;
        }
    }

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