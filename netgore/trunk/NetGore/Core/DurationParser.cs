using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetGore
{
    /// <summary>
    /// Provides some ways to parse a duration of time from a string.
    /// </summary>
    public static class DurationParser
    {
        static readonly Regex _regex = new Regex("(?<Value>[0-9]+)(?<Unit>[a-z]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);

        /// <summary>
        /// Parses a duration of time from a string.
        /// </summary>
        /// <param name="str">The string representation of the time duration. See <see cref="DurationParser"/> for information
        /// about the formatting.</param>
        /// <returns>The parsed duration.</returns>
        public static TimeSpan Parse(string str)
        {
            TimeSpan ret;
            string failReason;
            if (!TryParse(str, out ret, out failReason))
                throw new ArgumentException("str", failReason);

            return ret;
        }

        /// <summary>
        /// Tries to parse the duration from a string.
        /// </summary>
        /// <param name="str">The duration string.</param>
        /// <param name="ts">When this method returns true, contains the duration as a <see cref="TimeSpan"/>.</param>
        /// <returns>True if the parsing was successful; otherwise false.</returns>
        public static bool TryParse(string str, out TimeSpan ts)
        {
            string failReason;
            return TryParse(str, out ts, out failReason);
        }

        /// <summary>
        /// Tries to parse the duration from a string.
        /// </summary>
        /// <param name="str">The duration string.</param>
        /// <param name="ts">When this method returns true, contains the duration as a <see cref="TimeSpan"/>.</param>
        /// <param name="failReason">When this method returns false, contains the reason why it failed.</param>
        /// <returns>True if the parsing was successful; otherwise false.</returns>
        public static bool TryParse(string str, out TimeSpan ts, out string failReason)
        {
            var matches = _regex.Matches(str);

            ts = TimeSpan.Zero;
      
            // Make sure we have a match
            if (matches.Count == 0)
            {
                const string errmsg = "Invalid input format - unable to make any matches.";
                failReason = errmsg;
                return false;
            }

            // Handle each match
            foreach (var match in matches.Cast<Match>())
            {
                Debug.Assert(match.Success, "Why did _regex.Matches() give us unsuccessful matches?");

                int value;
                if (!int.TryParse(match.Groups["Value"].Value, out value))
                {
                    const string errmsg = "Invalid number value given near `{0}`.";
                    failReason = string.Format(errmsg, match.Value);
                    return false;
                }

                var unit = match.Groups["Unit"].Value.ToLowerInvariant();
                switch (unit)
                {
                    case "s":
                    case "sec":
                    case "secs":
                    case "second":
                    case "seconds":
                        ts.Add(TimeSpan.FromSeconds(value));
                        break;

                    case "m":
                    case "min":
                    case "mins":
                    case "minute":
                    case "minutes":
                        ts.Add(TimeSpan.FromMinutes(value));
                        break;

                    case "h":
                    case "hr":
                    case "hour":
                    case "hours":
                        ts.Add(TimeSpan.FromHours(value));
                        break;

                    case "d":
                    case "day":
                    case "days":
                        ts.Add(TimeSpan.FromDays(value));
                        break;

                    case "w":
                    case "wk":
                    case "week":
                    case "weeks":
                        ts.Add(TimeSpan.FromDays(value * 7));
                        break;

                    case "mon":
                    case "month":
                    case "months":
                        var now = DateTime.Now;
                        ts.Add(now.AddMonths(value) - now);
                        break;

                    case "y":
                    case "yr":
                    case "year":
                    case "years":
                        var now2 = DateTime.Now;
                        ts.Add(now2.AddYears(value) - now2);
                        break;

                    default:
                        const string errmsg = "Invalid unit `{0}` near `{1}`.";
                        failReason = string.Format(errmsg, unit, match.Value);
                        return false;
                }
            }

            failReason = null;
            return true;
        }
    }
}
