using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NetGore.Network
{
    /// <summary>
    /// Contains helper methods for working with IP addresses.
    /// </summary>
    public static class IPAddressHelper
    {
        static readonly IEnumerable<ExternalIPParser> _parsers;

        /// <summary>
        /// Initializes the <see cref="IPAddressHelper"/> class.
        /// </summary>
        static IPAddressHelper()
        {
            const string ipMatch = "(?<ip>\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3})";
            const RegexOptions regOpts = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            var rStraightCheck = new Regex(ipMatch, regOpts);
            var rCheckMyIP = new Regex("Your local IP address is&nbsp;" + ipMatch, regOpts);
            var rFaqs = new Regex("Your IP address is: " + ipMatch, regOpts);

            _parsers = new ExternalIPParser[]
            {
                new ExternalIPParser(new Uri("http://checkip.dyndns.org/"), rStraightCheck),
                new ExternalIPParser(new Uri("http://www.faqs.org/ip.php"), rFaqs),
                new ExternalIPParser(new Uri("http://www.checkmyip.com/"), rCheckMyIP),
                new ExternalIPParser(new Uri("http://icanhazip.com/"), null)
            };
        }

        /// <summary>
        /// Gets the default <see cref="ExternalIPParser"/>s.
        /// </summary>
        public static IEnumerable<ExternalIPParser> DefaultExternalIPParsers
        {
            get { return _parsers; }
        }

        /// <summary>
        /// Gets the external (public) IP address for this machine using the default <see cref="ExternalIPParser"/>s..
        /// </summary>
        /// <returns>The string representation of the external IP, or null if all of the parsers
        /// failed.</returns>
        public static string GetExternalIP()
        {
            return GetExternalIP(_parsers);
        }

        /// <summary>
        /// Gets the external (public) IP address for this machine.
        /// </summary>
        /// <param name="parsers">The <see cref="ExternalIPParser"/>s to use to try to get the external IP.</param>
        /// <returns>The string representation of the external IP, or null if all of the <paramref name="parsers"/>
        /// failed.</returns>
        public static string GetExternalIP(IEnumerable<ExternalIPParser> parsers)
        {
            using (var wc = new WebClient())
            {
                foreach (var p in parsers)
                {
                    try
                    {
                        var data = wc.DownloadString(p.Uri);
                        try
                        {
                            if (p.Parser == null)
                                return data;

                            var match = p.Parser.Match(data);
                            if (match.Success)
                            {
                                var g = match.Groups["ip"];
                                if (g.Success)
                                    return g.Value;
                            }
                        }
                        catch (ArgumentException)
                        {
                            continue;
                        }
                    }
                    catch (WebException)
                    {
                        continue;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Converts an IPv4 address to an unsigned int.
        /// </summary>
        /// <param name="dottedAddress">The dotted IP address string, formatted as "xxx.xxx.xxx.xxx".</param>
        /// <returns>The <paramref name="dottedAddress"/> as an unsigned int.</returns>
        /// <exception cref="ArgumentException"><paramref name="dottedAddress"/> was not in the expected format.</exception>
        public static uint IPv4AddressToUInt(string dottedAddress)
        {
            var split = dottedAddress.Split('.');
            if (split.Length != 4)
                throw new ArgumentException("Expected a dotted address with 4 segments.", "dottedAddress");

            var bytes = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                var parsedByte = byte.Parse(split[i]);
                bytes[i] = parsedByte;
            }

            return IPv4AddressToUInt(bytes, 0);
        }

        /// <summary>
        /// Converts an IPv4 address to an unsigned int.
        /// </summary>
        /// <param name="address">An array of the segments of an IPv4 address.</param>
        /// <param name="startIndex">The starting index to use in the <paramref name="address"/> array.</param>
        /// <returns>The <paramref name="address"/> as an unsigned int.</returns>
        public static uint IPv4AddressToUInt(byte[] address, int startIndex)
        {
            return BitConverter.ToUInt32(address, startIndex);
        }

        /// <summary>
        /// Converts an IPv4 address to a dotted IP address string.
        /// </summary>
        /// <param name="address">An array of the segments of an IPv4 address.</param>
        /// <param name="startIndex">The starting index to use in the <paramref name="address"/> array.</param>
        /// <returns>The dotted IP address string for the given <paramref name="address"/>.</returns>
        public static string ToIPv4Address(byte[] address, int startIndex)
        {
            var sb = new StringBuilder(16);
            sb.Append(address[startIndex + 0]);
            sb.Append(".");
            sb.Append(address[startIndex + 1]);
            sb.Append(".");
            sb.Append(address[startIndex + 2]);
            sb.Append(".");
            sb.Append(address[startIndex + 3]);

            return sb.ToString();
        }

        /// <summary>
        /// Converts an IPv4 address to a dotted IP address string.
        /// </summary>
        /// <param name="address">The unsigned 32-bit int for the address.</param>
        /// <returns>The dotted IP address string for the given <paramref name="address"/>.</returns>
        public static string ToIPv4Address(uint address)
        {
            var bytes = BitConverter.GetBytes(address);
            return ToIPv4Address(bytes, 0);
        }
    }
}