using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace System.Text
{
    public static class StringEncoding
    {
        /// <summary>
        /// Characters a-z, A-Z
        /// </summary>
        public static readonly char[] _charsAlpha = new[]
                                                    {
                                                        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
                                                        'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
                                                        'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
                                                        'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
                                                    };

        /// <summary>
        /// Characters a-z
        /// </summary>
        public static readonly char[] _charsAlphaLower = new[]
                                                         {
                                                             'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                                             'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
                                                         };

        /// <summary>
        /// Characters a-z, 0-9
        /// </summary>
        public static readonly char[] _charsAlphaLowerNumeric = new[]
                                                                {
                                                                    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c',
                                                                    'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                                                                    'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
                                                                };

        /// <summary>
        /// Characters a-z, A-Z, 0-9
        /// </summary>
        public static readonly char[] _charsAlphaNumeric = new[]
                                                           {
                                                               '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd',
                                                               'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                                                               's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F',
                                                               'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                                                               'U', 'V', 'W', 'X', 'Y', 'Z'
                                                           };

        /// <summary>
        /// Characters A-Z
        /// </summary>
        public static readonly char[] _charsAlphaUpper = new[]
                                                         {
                                                             'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                                                             'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
                                                         };

        /// <summary>
        /// Characters A-Z, 0-9
        /// </summary>
        public static readonly char[] _charsAlphaUpperNumeric = new[]
                                                                {
                                                                    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C',
                                                                    'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                                                                    'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
                                                                };

        /// <summary>
        /// Characters 0-9
        /// </summary>
        public static readonly char[] _charsNumeric = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Builds the values for Truncated Binary Encoding
        /// </summary>
        /// <param name="numValues">Number of unique values</param>
        /// <param name="lastSmallValue">Returned index of the last small-bit value</param>
        /// <param name="smallValueBits">Number of bits used for the small-bit values</param>
        /// <returns>Array containing values of given indices</returns>
        static int[] BuildTBEValues(int numValues, out int lastSmallValue, out int smallValueBits)
        {
            var values = new int[numValues];

            if (BitOps.IsPowerOf2(numValues))
            {
                // Is already a power of 2 - requires nothing special
                lastSmallValue = -1;
                smallValueBits = BitOps.RequiredBits((uint)numValues) - 2;

                for (int i = 0; i < numValues; i++)
                {
                    values[i] = i;
                }
            }
            else
            {
                // Not power of 2 - requires TBE to make optimal use of all values
                int maxVal = BitOps.NextPowerOf2(numValues) - 1;
                int unused = maxVal - (numValues - 1);

                lastSmallValue = unused - 1;
                smallValueBits = BitOps.RequiredBits((uint)numValues) - 1;

                for (int i = 0; i < unused; i++)
                {
                    values[i] = i;
                }

                for (int i = 0; i < numValues - unused; i++)
                {
                    values[unused + i] = maxVal - i;
                }
            }

            return values;
        }

        /// <summary>
        /// Decodes an encoded string
        /// </summary>
        /// <param name="value">String to be decoded</param>
        /// <param name="chars">Possible characters to be used in the string (order and consistancy between encoder and decoder are vital!)</param>
        /// <returns>Decoded version of the encoded string</returns>
        public static string Decode(byte[] value, char[] chars)
        {
            BitStream input = new BitStream(value);
            string output = string.Empty;

            // Grab the TBE values
            int lastSmallValue;
            int smallValueBits;
            var tmptbeCodes = BuildTBEValues(chars.Length + 1, out lastSmallValue, out smallValueBits);

            // Swap the TBE indices with their values for faster referencing later (since we're reversing)
            var tbeCodes = new int[1 << (smallValueBits + 1)];
            for (int i = 0; i < tmptbeCodes.Length; i++)
            {
                tbeCodes[tmptbeCodes[i]] = i;
            }

            // Grab all the values
            while (true)
            {
                // Read the minimalist value first
                uint index = input.ReadUInt(smallValueBits);

                // If needed, grab the next bit and append it to the least significant bit
                if (index > lastSmallValue)
                    index = (index << 1) + input.ReadUInt(1);

                // If we hit the terminating character, break
                if (tbeCodes[index] == chars.Length)
                    break;

                // Append to the output string
                output += chars[tbeCodes[index]];
            }

            return output;
        }

        /// <summary>
        /// Encodes a string using a Truncated Binary Encoding for minimal wasted values
        /// </summary>
        /// <param name="value">String to be encoded</param>
        /// <param name="chars">Possible characters to be used in the string (order and consistancy between encoder and decoder are vital!)</param>
        /// <returns>Byte array representing the encoded string</returns>
        public static byte[] Encode(string value, char[] chars)
        {
            BitStream output = new BitStream(BitStreamMode.Write, value.Length);
            var valueChars = value.ToCharArray();

            // Grab the TBE values
            int lastSmallValue;
            int smallValueBits;
            var tbeCodes = BuildTBEValues(chars.Length + 1, out lastSmallValue, out smallValueBits);

            // Write out the TBE values
            for (int i = 0; i < valueChars.Length; i++)
            {
                // Find the index of the current character in the chars array
                uint c = 0;
                while (valueChars[i] != chars[c])
                {
                    c++;
                    if (c >= chars.Length)
                        throw new Exception("String contained unspecified characters");
                }

                // Find the number of bits needed for the value
                int reqBits = smallValueBits;
                if (c > lastSmallValue)
                    reqBits++;

                // Write out the bits
                output.Write(tbeCodes[c], reqBits);
            }

            // Write out the terminating value
            output.Write(tbeCodes[tbeCodes.Length - 1], smallValueBits + 1);

            // Trim down the buffer and return it
            output.TrimExcess();
            return output.GetBuffer();
        }
    }
}