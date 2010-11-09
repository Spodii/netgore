using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CodeReleasePreparer
{
    /// <summary>
    /// Handles the deletion of files and folder.s
    /// </summary>
    public static class Deleter
    {
        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="path">The path to the file to delete.</param>
        static void DeleteFile(string path)
        {
            // Skip the dbdump.bat file, always
            if (path.EndsWith("dbdump.bat"))
                return;

            try
            {
                // Remove read-only flag if needed
                var attributes = File.GetAttributes(path);
                if ((attributes & FileAttributes.ReadOnly) != 0)
                    File.SetAttributes(path, FileAttributes.Normal);

                // Try to delete
                File.Delete(path);
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        /// <summary>
        /// Deletes a folder and all the files in it.
        /// </summary>
        /// <param name="path">The path to the folder to delete.</param>
        static void DeleteFolder(string path)
        {
            // Skip the CodeReleasePreparer's bin folder
            if (path.EndsWith(string.Format(@"CodeReleasePreparer{0}bin", Path.DirectorySeparatorChar),
                              StringComparison.InvariantCultureIgnoreCase))
                return;

            // Delete the files in the folder
            foreach (var file in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
            {
                DeleteFile(file);
            }

            // Delete the subdirectories
            foreach (var subDirectory in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
            {
                DeleteFolder(subDirectory);
            }

            // Delete the now-empty directory
            Directory.Delete(path, true);
        }

        /// <summary>
        /// Recursively deletes everything from a path.
        /// </summary>
        /// <param name="path">The path containing the items to delete.</param>
        /// <param name="folderFilter">The filter used to determine what folders to delete.</param>
        /// <param name="fileFilter">The filter used to determine what files to delete.</param>
        public static void RecursiveDelete(string path, Func<string, bool> folderFilter, Func<string, bool> fileFilter)
        {
            Thread.Sleep(5);

            // Delete all files in the directory that match the filter
            var files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in files.Where(fileFilter))
            {
                DeleteFile(f);
            }

            // Delete all the child directories recursively
            // If the folder isn't one to be deleted, still recurse to see if any child items are to be deleted
            var folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in folders)
            {
                if (folderFilter(f))
                    DeleteFolder(f);
                else
                    RecursiveDelete(f, folderFilter, fileFilter);
            }
        }
    }
}