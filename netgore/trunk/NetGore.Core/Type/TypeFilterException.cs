using System;
using System.Linq;

namespace NetGore
{
    public class TypeFilterException : Exception
    {
        public TypeFilterException(string message) : base(message)
        {
        }
    }
}