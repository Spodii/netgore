using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetGore
{
    /// <summary>
    /// Provides some ways to parse a duration of time from a string. Duration strings are formatted as
    /// a positive numeric value followed by a unit. The possible types of units, and their possible
    /// unit names, are:
    ///     second  - s, sec, secs, second, seconds
    ///     minute  - m, min, mins, minute, minutes
    ///     hour    - h, hr, hrs, hour, hours
    ///     day     - d, day, days
    ///     week    - w, wk, week, weeks
    ///     month   - mon, month, months
    ///     year    - y, yr, year, years
    /// </summary>
    /// <example>
    /// 5m10s       - 5 minutes and 10 seconds
    /// 3s          - 3 seconds
    /// 10s5y3mon    - 5 years, 3 months, and 10 seconds
    /// </example>
    public static class DurationParser
    {
        static readonly Regex _regex = new Regex("(?<Value>[\\-0-9]+)\\s*(?<Unit>[a-z]+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);

        /// <summary>
        /// Parses a duration of time from a string.
        /// </summary>
        /// <param name="str">The string representation of the time duration. See <see cref="DurationParser"/> for information
        /// about the formatting.</param>
        /// <returns>The parsed duration.</returns>
        /// <exception cref="ArgumentException">The <paramref name="str"/> failed to be parsed.</exception>
        public static TimeSpan Parse(string str)
        {
            TimeSpan ret;
            string failReason;
            if (!TryParse(str, out ret, out failReason))
                throw new ArgumentException(failReason, "str");

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

                var unit = match.Groups["Unit"].Value.Trim().ToUpperInvariant();
                switch (unit)
                {
                    case "S":
                    case "SEC":
                    case "SECS":
                    case "SECOND":
                    case "SECONDS":
                        ts += TimeSpan.FromSeconds(value);
                        break;

                    case "M":
                    case "MIN":
                    case "MINS":
                    case "MINUTE":
                    case "MINUTES":
                        ts += TimeSpan.FromMinutes(value);
                        break;

                    case "H":
                    case "HR":
                    case "HRS":
                    case "HOUR":
                    case "HOURS":
                        ts += TimeSpan.FromHours(value);
                        break;

                    case "D":
                    case "DAY":
                    case "DAYS":
                        ts += TimeSpan.FromDays(value);
                        break;

                    case "W":
                    case "WK":
                    case "WEEK":
                    case "WEEKS":
                        ts += TimeSpan.FromDays(value * 7);
                        break;

                    case "MON":
                    case "MONTH":
                    case "MONTHS":
                        var now = DateTime.Now;
                        ts += now.AddMonths(value) - now;
                        break;

                    case "Y":
                    case "YR":
                    case "YEAR":
                    case "YEARS":
                        var now2 = DateTime.Now;
                        ts += now2.AddYears(value) - now2;
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