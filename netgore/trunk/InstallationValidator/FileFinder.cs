using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstallationValidator
{
    public static class FileFinder
    {
        public static string Find(string fileName, string root)
        {
            if (!Directory.Exists(root))
                return null;

            foreach (var file in Directory.GetFiles(root, "*", SearchOption.AllDirectories))
            {
                if (Path.GetFileName(file).ToLower().EndsWith(fileName))
                    return file;
            }

            return null;
        }
    }
}
