using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    public class TypeFilterException : Exception
    {
        public TypeFilterException(string message)
            : base(message)
        {
        }
    }
}
