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
        /// Recursively deletes everything from a path.
        /// </summary>
        /// <param name="path">The path containing the items to delete.</param>
        /// <param name="fileFilter">The filter used to determine what files to delete.</param>
        public static void RecursiveDelete(string path, Func<string, bool> fileFilter)
        {
            // Delete all the child directories recursively
            // If the folder isn't one to be deleted, still recurse to see if any child items are to be deleted
            var folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in folders)
            {
                var fu = f.ToUpperInvariant();
                if (fu.Contains("\\_RESHARPER") || fu.Contains("\\.svn"))
                    Directory.Delete(f, true);
                else
                    RecursiveDelete(f, fileFilter);
            }

            // Delete all files in the directory that match the filter
            var files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in files.Where(fileFilter))
            {
                DeleteFile(f);
            }

            Thread.Sleep(5);

            // Delete empty directories
            if (Directory.GetFiles(path).Count() == 0 && Directory.GetDirectories(path).Count() == 0)
                Directory.Delete(path);
        }
    }
}