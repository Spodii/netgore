using System;
using System.IO;
using System.Reflection;

namespace InstallationValidator
{
    class Program
    {
        delegate void TestHandler();

        static void CheckForXNA()
        {
            const string xnaAssembly =
                "Microsoft.Xna.Framework, Version=3.1.0.0, Culture=neutral, PublicKeyToken=6d5c3888ef60e27d, processorArchitecture=x86";
            const string testName = "XNA 3.1 installed";
            const string failInfo = "Please install XNA 3.1";

            Test(testName, IsAssemblyInstalled(xnaAssembly), failInfo);
        }

        static bool IsAssemblyInstalled(string assemblyName)
        {
            try
            {
                var asm = Assembly.ReflectionOnlyLoad(assemblyName);
                return asm != null;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            TestHandler[] tests = new TestHandler[] { CheckForXNA };

            foreach (var test in tests)
                test();

            Console.ReadLine();
        }

        static void Write(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
        }

        static void Test(string testName, bool passed, string failMessage)
        {
            const ConsoleColor normalColor = ConsoleColor.White;
            const ConsoleColor passColor = ConsoleColor.Green;
            const ConsoleColor failColor = ConsoleColor.Red;
            const ConsoleColor testColor = ConsoleColor.Yellow;
            const ConsoleColor msgColor = ConsoleColor.Red;

            Write("[", normalColor);
            Write(passed ? "PASS" : "FAIL", passed ? passColor : failColor);
            Write("] ", normalColor);

            Write(testName, testColor);

            if (!passed && !string.IsNullOrEmpty(failMessage))
            {
                Write(" - ", normalColor);
                Write(failMessage, msgColor);
            }

            Console.WriteLine();
        }
    }
}