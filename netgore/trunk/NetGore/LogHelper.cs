using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    public static class LogHelper
    {
        public static string GetBufferDump(byte[] buffer)
        {
            return GetBufferDump(buffer, 0, buffer.Length);
        }

        public static string GetBufferDump(byte[] buffer, int start, int length)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            int count = 0;
            int end = start + length;
            for (int i = start; i < end; i++)
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
                    {
                        sb.Append(", ");
                    }
                }
            }
            sb.AppendLine();
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
