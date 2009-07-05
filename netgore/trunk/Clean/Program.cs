using System;
using System.IO;
using System.Reflection;

namespace Clean
{
    class Program
    {
        /// <summary>
        /// Recursively checks a directory and all directories under it
        /// </summary>
        /// <param name="dir">Root directory to check</param>
        static void CheckDirectory(string dir)
        {
            // Check through all sub-directories
            foreach (string subDir in Directory.GetDirectories(dir))
            {
                // Check for an empty directory
                if (Directory.GetFiles(subDir).Length == 0 && Directory.GetDirectories(subDir).Length == 0)
                {
                    Console.WriteLine("Empty directory: " + subDir);
                    DeleteDir(subDir);
                    continue;
                }

                // Check the sub-directories
                CheckDirectory(subDir);

                // Clean build folders
                CleanBuild(subDir);

                // Clean ReSharper cache
                CleanReSharper(subDir);

                // Clean StyleCop caches
                CleanStyleCop(subDir);
            }
        }

        /// <summary>
        /// Cleans a directory of all build-related sub-directories
        /// </summary>
        /// <param name="dir">Directory to clean</param>
        static void CleanBuild(string dir)
        {
            // Check for a project file
            if (!ContainsProjectFile(dir))
                return;

            // Check for a bin and obj directories
            string binDir = string.Format("{0}{1}bin", dir, Path.DirectorySeparatorChar);
            string objDir = string.Format("{0}{1}obj", dir, Path.DirectorySeparatorChar);
            if (!Directory.Exists(binDir) || !Directory.Exists(objDir))
                return;

            // Delete the directories
            Console.WriteLine("Project bin dir: " + binDir);
            DeleteDir(binDir);

            Console.WriteLine("Project obj dir: " + objDir);
            DeleteDir(objDir);
        }

        /// <summary>
        /// Cleans a directory of all resharper-related files and sub-directories
        /// </summary>
        /// <param name="dir">Directory to clean</param>
        static void CleanReSharper(string dir)
        {
            // Check for .resharper and .resharper.user files
            foreach (string filePath in Directory.GetFiles(dir))
            {
                if (filePath.EndsWith(".resharper") || filePath.EndsWith(".resharper.user") ||
                    filePath.ToLower().EndsWith(string.Format("{0}{1}", Path.DirectorySeparatorChar, "sourceanalysis.cache")))
                {
                    // Delete the file
                    Console.WriteLine("ReSharper file: " + filePath);
                    DeleteFile(filePath);
                }
            }

            // Split the individual directories
            var dirs = dir.Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });

            if (dirs.Length < 1)
                return;

            // Get the last directory
            string endDir = dirs[dirs.Length - 1];
            if (string.IsNullOrEmpty(endDir))
                return;

            // Check for a ReSharper cache
            if (!endDir.ToLower().StartsWith("_resharper."))
                return;

            // Kill the ReSharper cache
            Console.WriteLine("ReSharper cache directory: " + dir);
            DeleteDir(dir);
        }

        /// <summary>
        /// Cleans the StyleCop cache files
        /// </summary>
        /// <param name="dir">Directory to clean</param>
        static void CleanStyleCop(string dir)
        {
            if (!Directory.Exists(dir))
                return;

            foreach (string file in Directory.GetFiles(dir))
            {
                if (!file.ToLower().EndsWith("stylecop.cache"))
                    continue;

                Console.WriteLine("StyleCop cache: " + file);
                DeleteFile(file);
            }
        }

        /// <summary>
        /// Checks if a directory contains a project file
        /// </summary>
        /// <param name="dir">Directory to check</param>
        /// <returns>True if the directory contains a project file, else false</returns>
        static bool ContainsProjectFile(string dir)
        {
            // Check if there are any .csproj files in the directory
            return (Directory.GetFiles(dir, "*.csproj").Length > 0);
        }

        /// <summary>
        /// Deletes a directory and all directories/files under it
        /// </summary>
        /// <param name="dir">Directory to delete</param>
        static void DeleteDir(string dir)
        {
            if (!Directory.Exists(dir))
                return;

            // Check all subdirectories
            foreach (string subDir in Directory.GetDirectories(dir))
            {
                DeleteDir(subDir);
            }

            // Delete all files
            foreach (string filePath in Directory.GetFiles(dir))
            {
                DeleteFile(filePath);
            }

            // Delete the directory
            try
            {
                Directory.Delete(dir);
                Console.WriteLine("Deleted directory: {0}", dir);
            }
            catch (UnauthorizedAccessException)
            {
                // Don't need to be verbose about this one
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to delete directory `{0}`. Reason: {1}", dir, ex);
            }
        }

        /// <summary>
        /// Wrapper for deleting a file, with more verbosity
        /// </summary>
        /// <param name="file">File to delete</param>
        static void DeleteFile(string file)
        {
            // We don't care about non-existant files
            if (!File.Exists(file))
                return;

            // Try to delete
            try
            {
                File.Delete(file);
                Console.WriteLine("Deleted file: {0}", file);
            }
            catch (UnauthorizedAccessException)
            {
                // Don't need to be verbose about this one, it is probably just in use
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to delete file `{0}`. Reason: {1}", file, ex);
            }
        }

        static void Main()
        {
            // Get the directory for the application
            string appPath = Assembly.GetExecutingAssembly().Location;
            appPath = Path.GetDirectoryName(appPath);

            // Validate
            if (appPath == null)
            {
                Console.WriteLine(string.Format("Invalid application path: {0}", appPath));
                return;
            }

            // Recursively check all directories
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    CheckDirectory(appPath);
                }
            }
            catch (IOException)
            {
            }

            // Done
            Console.WriteLine("Complete!");
        }
    }
}