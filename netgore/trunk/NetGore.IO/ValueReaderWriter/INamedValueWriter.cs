using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    public interface INamedValueWriter
    {
        void Write(string name, int value);
        void Write(string name, uint value);
        void Write(string name, short value);
        void Write(string name, ushort value);
        void Write(string name, byte value);
        void Write(string name, sbyte value);
        void Write(string name, float value);
        void Write(string name, string value);
    }
}
