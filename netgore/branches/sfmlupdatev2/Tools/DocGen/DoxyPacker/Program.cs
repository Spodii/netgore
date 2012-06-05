using System;
using System.Linq;

namespace DoxyPacker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Usage: DoxyPacker <path>");
                return;
            }

            var p = new Packer(args[0]);
            p.Run();
        }
    }
}