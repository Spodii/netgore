using System;
using System.Linq;

namespace NetGore
{
    [Serializable]
    public class TypeFilterException : Exception
    {
        public TypeFilterException(string message) : base(message)
        {
        }
    }
}