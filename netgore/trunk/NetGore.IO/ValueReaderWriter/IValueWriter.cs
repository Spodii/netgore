using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    public interface IValueWriter
    {
        void Write(int value);
        void Write(uint value);
        void Write(short value);
        void Write(ushort value);
        void Write(byte value);
        void Write(sbyte value);
        void Write(float value);
        void Write(string value);
    }
}
