using System;

namespace InstallationValidator
{
    public static class Tester
    {
        static bool _hasErrors = false;

        /// <summary>
        /// Gets if there has been an error in any of the tests.
        /// </summary>
        public static bool HasErrors
        {
            get { return _hasErrors; }
        }

        public static void Test(string testName, bool passed, string failMessage)
        {
            if (!passed)
                _hasErrors = true;

            const ConsoleColor normalColor = ConsoleColor.White;
            const ConsoleColor passColor = ConsoleColor.Green;
            const ConsoleColor failColor = ConsoleColor.Red;
            const ConsoleColor testColor = ConsoleColor.Yellow;
            const ConsoleColor msgColor = ConsoleColor.Red;

            if (Console.CursorLeft > 0)
                Write("\n", normalColor);

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

        public static void Write(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
        }
    }
}