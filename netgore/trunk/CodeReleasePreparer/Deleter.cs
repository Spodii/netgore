using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeReleasePreparer
{
    public static class Deleter
    {
        public static void RecursiveDelete(string path, Func<string, bool> folderFilter, Func<string, bool> fileFilter)
        {
            var files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                if (fileFilter(f))
                    DeleteFile(f);
            }

            var folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in folders)
            {
                if (folderFilter(f))
                    DeleteFolder(f);
                else
                    RecursiveDelete(f, folderFilter, fileFilter);
            }
        }

        public static void DeleteFile(string path)
        {
            if (path.EndsWith("dbdump.bat"))
                return;

            Console.WriteLine("Delete: " + path);

            var attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) != 0)
                File.SetAttributes(path, FileAttributes.Normal);

            try
            {
                File.Delete(path);
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        public static void DeleteFolder(string path)
        {
            if (path.EndsWith(string.Format(@"CodeReleasePreparer{0}bin", Path.DirectorySeparatorChar),
                              StringComparison.InvariantCultureIgnoreCase))
                return;

            Console.WriteLine("Delete: " + path);

            foreach (var file in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
            {
                DeleteFile(file);
            }

            foreach (var subDirectory in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
            {
                DeleteFolder(subDirectory);
            }

            Directory.Delete(path);
        }
    }
}
