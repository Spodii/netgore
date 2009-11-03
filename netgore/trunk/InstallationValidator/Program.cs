using System;
using InstallationValidator.Tests;
using NetGore;

namespace InstallationValidator
{
    public class Program
    {
        static void Main()
        {
            Tester.Write("Warning: ", ConsoleColor.Yellow);
            Tester.Write(
                "This program will most likely NOT be able to detect if your current database" +
                " is from an older release of NetGore. If you are upgrading, please make sure you" +
                "use the new `db.sql` file. If you delete your old database, this program will" +
                "import the new database for you.\n\n" +
                "If you want to try and upgrade your database, you can attempt to do so with a program" + " such as mysqldiff:\n" +
                "http://www.adamspiers.org/computing/mysqldiff/\n\n", ConsoleColor.White);

            if (!MySqlHelper.ValidateFilePathsLoaded())
                return;

            ITestable[] tests = new ITestable[]
            {
                new LocateMySqlExe(), new CheckForXNA(), new LoadXNA(), new CheckForMySqlConnector(), new LoadMySqlData(),
                new DatabaseFileExists(), new LoadDbConnectionSettings(), new ConnectToDatabase(), new DatabaseExists(),
                new DatabasePopulated()
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