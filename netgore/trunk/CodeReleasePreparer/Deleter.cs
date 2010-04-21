using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CodeReleasePreparer
{
    public static class Deleter
    {
        public static void DeleteFile(string path)
        {
            Thread.Sleep(1);

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
            Thread.Sleep(5);

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

        public static void RecursiveDelete(string path, Func<string, bool> folderFilter, Func<string, bool> fileFilter)
        {
            Thread.Sleep(5);

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
    }
}