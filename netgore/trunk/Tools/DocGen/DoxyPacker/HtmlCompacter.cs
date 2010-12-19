using System.IO;
using System.Linq;
using DoxyPacker.Properties;

namespace DoxyPacker
{
    public class HtmlCompacter
    {
        public HtmlCompacter(string root)
        {
            EnsureFileExists(root, Resources.jscriptToggleVisibility_FileName, Resources.jscriptToggleVisibility_FileData);
        }

        public string Compact(string input)
        {
            var ret = ReduceJScript(input);
            return ret;
        }

        static void EnsureFileExists(string root, string fileName, string fileData)
        {
            var f = Path.Combine(root, fileName);
            if (File.Exists(f))
                File.Delete(f);

            File.WriteAllText(f, fileData);
        }

        static string ReduceJScript(string input)
        {
            // toggleVisibility
            var ret = input.Replace(Resources.jscriptToggleVisiblity_Find, Resources.jscriptToggleVisiblity_Rep);

            return ret;
        }
    }
}