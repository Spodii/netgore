using System;
using System.Linq;
using System.Text;
using NetGore;

namespace NetGore.Network
{
    /// <summary>
    /// Contains helper methods for working with IP addresses.
    /// </summary>
    public static class IPAddressHelper
    {
        /// <summary>
        /// Converts an IPv4 address to an unsigned int.
        /// </summary>
        /// <param name="dottedAddress">The dotted IP address string, formatted as "xxx.xxx.xxx.xxx".</param>
        /// <returns>The <paramref name="dottedAddress"/> as an unsigned int.</returns>
        public static uint IPv4AddressToUInt(string dottedAddress)
        {
            var split = dottedAddress.Split('.');
            if (split.Length != 4)
                throw new ArgumentException("Expected a dotted address with 4 segments.", "dottedAddress");

            var bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                byte parsedByte = byte.Parse(split[i]);
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
            StringBuilder sb = new StringBuilder(16);
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