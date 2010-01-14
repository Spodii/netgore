using System;
using System.IO;
using InstallationValidator.Tests;
using NetGore;

namespace InstallationValidator
{
    public class Program
    {
        static void Main()
        {
            // Make sure the current directory is always the root, whether we run it using the .bat file or
            // through the IDE or whatever
            var currentDir = Directory.GetCurrentDirectory();
            if (currentDir.EndsWith("bin", StringComparison.OrdinalIgnoreCase))
            {
                // ReSharper disable PossibleNullReferenceException
                // Move two directories down
                var parent1 = Directory.GetParent(currentDir);
                var parent2 = Directory.GetParent(parent1.FullName);

                // Set the new current directory
                Directory.SetCurrentDirectory(parent2.FullName);
                // ReSharper restore PossibleNullReferenceException
            }

            if (!MySqlHelper.ValidateFilePathsLoaded())
                return;

            ITestable[] tests = new ITestable[]
            {
                new LocateMySqlExe(), new CheckForXNA(), new LoadXNA(), new CheckForMySqlConnector(), new LoadMySqlData(),
                new DatabaseFileExists(), new LoadDbConnectionSettings(), new ConnectToDatabase(), new DatabaseExists(),
                new LoadSchemaFile(), new DatabasePopulated()
            };

            foreach (var test in tests)
            {
                test.Test();
            }

            Console.WriteLine();

            if (Tester.HasErrors)
            {
                Tester.Write("One or more errors were found. Resolve them for NetGore to work properly.\n", ConsoleColor.Red);
                Tester.Write("If you need help resolving these errors, please ask on the NetGore forums at www.netgore.com.",
                             ConsoleColor.Yellow);
            }
            else
                Tester.Write("Congratulations, no errors! :)", ConsoleColor.Green);

            Console.ReadLine();
        }
    }
}