using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DemoGame.Client.Properties;
using NetGore.IO;

namespace DemoGame.Client.Say
{
    /// <summary>
    /// A helper class responsible for replacing all profane language in a given incoming string
    /// </summary>
    public class ProfanityHandler
    {
        /// <summary>
        /// A list of profane words that have been defined
        /// </summary>
        private List<string> _profaneWords;

        private Regex _regularExpression;

        public ProfanityHandler()
        {

            try
            {
                string path = ContentPaths.Build.Data.Join("lang_filter.txt");

                // Load up our profane words
                _profaneWords = File.ReadAllLines(path).ToList();
                string expString = string.Format("({0})\\b", string.Join("|", _profaneWords));
                _regularExpression = new Regex(expString, RegexOptions.IgnoreCase);



            }
            catch (FileNotFoundException exception)
            {
                Debug.Fail("Language filter is missing; failing...");
            }

        }

        /// <summary>
        /// Proccesses a given message and applies the profanity filter to it.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A clean, filtered message by the rules of the profanity filter</returns>
        public string ProcessMessage(string message)
        {
            string filtered = message;

            // Filter if neccessary
            if (ClientSettings.Default.UseProfanityFilter)
                filtered = _regularExpression.Replace(message, new MatchEvaluator(StarText));
            
            return filtered;
        }

        static string StarText(Match m)
        {
            return new StringBuilder().Insert(0, "*", m.Length).ToString();

        }

    }
}
