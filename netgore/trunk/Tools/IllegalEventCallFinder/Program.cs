using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApplication7
{
    class Program
    {
        const string _eventNameFinder = @"\sevent (?<Type>[\w\<\> ,]+)? (?<Name>\w+);";
        const string _eventCallFinder = @"(?<IfCheck>if \({0} != null\))?\s+{0}(?<UseRaise>\.Raise)?\((?<Params>.*)\);";

        static readonly string[] _ignores = new string[] { "\\bin\\","\\_dependencies\\", "\\\\.svn\\","\\_ReSharper\\.\\",
            "\\DevContent\\","\\Externals\\","\\NetGore.Tests\\", "\\Settings\\", "\\Tools\\"};

        static void Main(string[] args)
        {
            Regex eventFinder = new Regex(_eventNameFinder, RegexOptions.Compiled | RegexOptions.Multiline);

            foreach (var file in Directory.GetFiles(@"E:\NetGore", "*.cs", SearchOption.AllDirectories))
            {
                if (_ignores.Any(x => file.Contains(x)))
                    continue;

                var text = File.ReadAllText(file);
                var em = eventFinder.Match(text);

                bool wroteFileName = false;

                while (em.Success)
                {
                    var name = em.Groups["Name"].Value;
                    var mPattern = string.Format(_eventCallFinder, name);
                    var m = Regex.Match(text, mPattern);

                    if (IsIllegalEventCall(m))
                    {
                        if (!wroteFileName)
                        {
                            Console.WriteLine(file);
                            wroteFileName = true;
                        }

                        Console.WriteLine(" * " + name);
                    }

                    em = em.NextMatch();
                }
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        const bool _printReason = true;

        static bool IsIllegalEventCall(Match m)
        {
            if (!m.Groups["UseRaise"].Success)
            {
                if (_printReason)
                    Console.WriteLine("-UseRaise = False");

                return true;
            }

            if (!m.Groups["IfCheck"].Success)
            {
                var paramsSplit = m.Groups["Params"].Value.Split(new string[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (paramsSplit.Length != 2)
                {
                    Debug.Fail("...");
                }

                if (paramsSplit[1].Trim() != "EventArgs.Empty")
                {
                    if (_printReason)
                        Console.WriteLine("-IfCheck = False");

                    return true;
                }
            }

            return false;
        }
    }
}
