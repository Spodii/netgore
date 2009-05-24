using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    public interface IValueReader
    {
        int ReadInt();
        uint ReadUInt();
        short ReadShort();
        ushort ReadUShort();
        byte ReadByte();
        sbyte ReadSByte();
        float ReadFloat();
        string ReadString();
    }
}
