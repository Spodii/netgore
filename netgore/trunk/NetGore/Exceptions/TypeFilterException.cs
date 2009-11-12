using System;
using System.Linq;
using NetGore;

namespace NetGore
{
    public class TypeFilterException : Exception
    {
        public TypeFilterException(string message) : base(message)
        {
        }
    }
}