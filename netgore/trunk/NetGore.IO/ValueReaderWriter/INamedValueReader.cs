using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    public interface INamedValueReader
    {
        int ReadInt(string name);
        uint ReadUInt(string name);
        short ReadShort(string name);
        ushort ReadUShort(string name);
        byte ReadByte(string name);
        sbyte ReadSByte(string name);
        float ReadFloat(string name);
        string ReadString(string name);
    }
}
