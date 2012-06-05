using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// Helper methods for logging.
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Gets the formatted dump of a byte array.
        /// </summary>
        /// <param name="buffer">The byte array to get the dump for.</param>
        /// <returns>A string containing the contents of the given <paramref name="buffer"/>.</returns>
        /// <example>
        /// The default output dump is formatted in the following way:
        /// 
        /// {
        /// 001, 002, 003, 004, 005, 006, 007, 008, 009, 010
        /// 011, 012, 013, 014, 015, 016, 017, 018, 019, 020
        /// 021, 022, 023, 024, 025, 026, 027, 028, 029, 030
        /// 031, 032, 033, 034, 035, 036, 037, 038, 039, 040
        /// }
        /// 
        /// The first line and last lines will be an opening and closing curly bracket respectively. Each line will
        /// have 10 elements on it, and be formatted to show each number using 3 digits.
        /// </example>
        public static string GetBufferDump(byte[] buffer)
        {
            return GetBufferDump(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Gets the formatted dump of a byte array.
        /// </summary>
        /// <param name="buffer">The byte array to get the dump for.</param>
        /// <param name="start">The index to start the dump at.</param>
        /// <param name="length">The number of elements for the dump to contain.</param>
        /// <returns>
        /// A string containing the contents of the given <paramref name="buffer"/>.
        /// </returns>
        /// <example>
        /// The default output dump is formatted in the following way:
        /// 
        /// {
        /// 001, 002, 003, 004, 005, 006, 007, 008, 009, 010
        /// 011, 012, 013, 014, 015, 016, 017, 018, 019, 020
        /// 021, 022, 023, 024, 025, 026, 027, 028, 029, 030
        /// 031, 032, 033, 034, 035, 036, 037, 038, 039, 040
        /// }
        /// 
        /// The first line and last lines will be an opening and closing curly bracket respectively. Each line will
        /// have 10 elements on it, and be formatted to show each number using 3 digits.
        /// </example>
        public static string GetBufferDump(byte[] buffer, int start, int length)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            var count = 0;
            var end = start + length;

            for (var i = start; i < end; i++)
            {
                sb.Append(buffer[i].ToString("000"));
                if (i < end - 1)
                {
                    if (++count == 16)
                    {
                        sb.AppendLine();
                        count = 0;
                    }
                    else
                        sb.Append(", ");
                }
            }

            sb.AppendLine();
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}