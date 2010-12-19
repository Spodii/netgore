using System.Linq;

namespace DoxyPacker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
                args = new string[] { @"E:\NetGore\Tools\DocGen\docs\html" };
            var p = new Packer(args[0]);
            p.Run();
        }
    }
}