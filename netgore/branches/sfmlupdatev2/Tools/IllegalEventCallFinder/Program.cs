using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication7
{
    class Program
    {
        const string _eventCallFinder = @"(?<IfCheck>if \({0} != null\))?\s+{0}(?<UseRaise>\.Raise)?\((?<Params>.*)\);";
        const string _eventNameFinder = @"\sevent (?<Type>[\w\<\> ,]+)? (?<Name>\w+);";

        const int _maxFilesOpenInVSPerBatch = 500;
        const bool _openInVS = true;

        const bool _printReason = true;

        static readonly EventIgnore[] _ignoreEvents = new EventIgnore[]
        {
            new EventIgnore("ChatForm.cs", "Say"), new EventIgnore("GrhTreeView.cs", "GrhMouseDoubleClick"),
            new EventIgnore("GrhTreeView.cs", "GrhMouseClick"), new EventIgnore("GrhTreeView.cs", "GrhBeforeSelect"),
            new EventIgnore("GrhTreeView.cs", "GrhAfterSelect"), new EventIgnore("SkillsForm.cs", "RequestUseSkill"),
            new EventIgnore("WaterRefractionEffect.cs", "Moved"), new EventIgnore("ExplosionRefractionEffect.cs", "Moved"),
        };

        static readonly string[] _ignores = new string[]
        {
            "\\bin\\", "\\_dependencies\\", "\\\\.svn\\", "\\_ReSharper\\.\\", "\\DevContent\\", "\\Externals\\",
            "\\NetGore.Tests\\", "\\Settings\\", "\\Tools\\", "\\NetGore.Editor\\Docking\\"
        };

        static bool IsIllegalEventCall(Match m)
        {
            if (!m.Success)
                return false;

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
                    Debug.Fail("...");

                if (paramsSplit[1].Trim() != "EventArgs.Empty")
                {
                    if (_printReason)
                        Console.WriteLine("-IfCheck = False");

                    return true;
                }
            }

            return false;
        }

        static void Main()
        {
            var eventFinder = new Regex(_eventNameFinder, RegexOptions.Compiled | RegexOptions.Multiline);

            var openCount = 0;

            foreach (var fileTemp in Directory.GetFiles(@"E:\NetGore", "*.cs", SearchOption.AllDirectories))
            {
                var file = fileTemp;

                if (_ignores.Any(file.Contains))
                    continue;

                var text = File.ReadAllText(file);
                var em = eventFinder.Match(text);

                var wroteFileName = false;

                while (em.Success)
                {
                    var name = em.Groups["Name"].Value;

                    if (
                        _ignoreEvents.Any(
                            x =>
                            StringComparer.Ordinal.Equals(x.FileName, Path.GetFileName(file)) &&
                            StringComparer.Ordinal.Equals(x.EventName, name)))
                        break;

                    var mPattern = string.Format(_eventCallFinder, name);
                    var m = Regex.Match(text, mPattern);

                    if (IsIllegalEventCall(m))
                    {
                        if (!wroteFileName)
                        {
                            Console.WriteLine(file);
                            wroteFileName = true;

                            if (_openInVS)
                            {
                                Process.Start(file);

                                openCount++;
                                if (openCount >= _maxFilesOpenInVSPerBatch)
                                    return;
                            }
                        }

                        Console.WriteLine(" * " + name);
                    }

                    em = em.NextMatch();
                }
            }

            Console.WriteLine("Done");
        }

        struct EventIgnore
        {
            readonly string _fileName;
            readonly string _eventName;

            public EventIgnore(string fileName, string eventName)
            {
                _fileName = fileName;
                _eventName = eventName;
            }

            public string FileName
            {
                get { return _fileName; }
            }

            public string EventName
            {
                get { return _eventName; }
            }
        }
    }
}