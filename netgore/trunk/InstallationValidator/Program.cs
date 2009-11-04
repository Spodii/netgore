using System;
using InstallationValidator.Tests;
using NetGore;

namespace InstallationValidator
{
    public class Program
    {
        static void Main()
        {
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