using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace zlib
{
//	[Serializable]
    class ZStreamException : IOException
    {
        public ZStreamException()
        {
        }

        public ZStreamException(String s) : base(s)
        {
        }
    }
}