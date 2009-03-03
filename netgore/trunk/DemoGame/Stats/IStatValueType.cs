using System;
using NetGore.IO.Bits;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace DemoGame
{
    public interface IStatValueType
    {
        IStatValueType DeepCopy();

        int GetValue();

        void Read(BitStream bitStream);

        void Read(IDataRecord dataRecord, int ordinal);

        void SetValue(int value);

        void Write(BitStream bitStream);
    }

    public class StatValueByte : IStatValueType
    {
        byte _value;

        public StatValueByte()
        {
        }

        public StatValueByte(byte value)
        {
            _value = value;
        }

        #region IStatValueType Members

        public int GetValue()
        {
            return _value;
        }

        public void SetValue(int value)
        {
            _value = (byte)value;
        }

        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        public void Read(BitStream bitStream)
        {
            _value = bitStream.ReadByte();
        }

        public void Read(IDataRecord dataRecord, int ordinal)
        {
            _value = dataRecord.GetByte(ordinal);
        }

        public IStatValueType DeepCopy()
        {
            return new StatValueByte(_value);
        }

        #endregion
    }

    public class StatValueSByte : IStatValueType
    {
        sbyte _value;

        public StatValueSByte()
        {
        }

        public StatValueSByte(sbyte value)
        {
            _value = value;
        }

        #region IStatValueType Members

        public int GetValue()
        {
            return _value;
        }

        public void SetValue(int value)
        {
            _value = (sbyte)value;
        }

        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        public void Read(BitStream bitStream)
        {
            _value = bitStream.ReadSByte();
        }

        public void Read(IDataRecord dataRecord, int ordinal)
        {
            _value = (sbyte)dataRecord.GetValue(ordinal);
        }

        public IStatValueType DeepCopy()
        {
            return new StatValueSByte(_value);
        }

        #endregion
    }

    public class StatValueInt : IStatValueType
    {
        int _value;

        public StatValueInt()
        {
        }

        public StatValueInt(int value)
        {
            _value = value;
        }

        #region IStatValueType Members

        public int GetValue()
        {
            return _value;
        }

        public void SetValue(int value)
        {
            _value = value;
        }

        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        public void Read(BitStream bitStream)
        {
            _value = bitStream.ReadInt();
        }

        public void Read(IDataRecord dataRecord, int ordinal)
        {
            _value = dataRecord.GetInt32(ordinal);
        }

        public IStatValueType DeepCopy()
        {
            return new StatValueInt(_value);
        }

        #endregion
    }

    public class StatValueShort : IStatValueType
    {
        short _value;

        public StatValueShort()
        {
        }

        public StatValueShort(short value)
        {
            _value = value;
        }

        #region IStatValueType Members

        public int GetValue()
        {
            return _value;
        }

        public void SetValue(int value)
        {
            _value = (short)value;
        }

        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        public void Read(BitStream bitStream)
        {
            _value = bitStream.ReadShort();
        }

        public void Read(IDataRecord dataRecord, int ordinal)
        {
            _value = dataRecord.GetInt16(ordinal);
        }

        public IStatValueType DeepCopy()
        {
            return new StatValueShort(_value);
        }

        #endregion
    }

    public class StatValueUShort : IStatValueType
    {
        ushort _value;

        public StatValueUShort()
        {
        }

        public StatValueUShort(ushort value)
        {
            _value = value;
        }

        #region IStatValueType Members

        public int GetValue()
        {
            return _value;
        }

        public void SetValue(int value)
        {
            _value = (ushort)value;
        }

        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        public void Read(BitStream bitStream)
        {
            _value = bitStream.ReadUShort();
        }

        public void Read(IDataRecord dataRecord, int ordinal)
        {
            _value = (ushort)dataRecord.GetValue(ordinal);
        }

        public IStatValueType DeepCopy()
        {
            return new StatValueUShort(_value);
        }

        #endregion
    }

    [Obsolete]
    public class StatValueUInt : IStatValueType
    {
#pragma warning disable 168
        public StatValueUInt()
        {
        }

        public StatValueUInt(uint value)
        {
        }

        #region IStatValueType Members

        public int GetValue()
        {
            throw new Exception("Do not use StatValueUInt.");
        }

        public void SetValue(int value)
        {
            throw new Exception("Do not use StatValueUInt.");
        }

        public void Write(BitStream bitStream)
        {
            throw new Exception("Do not use StatValueUInt.");
        }

        public void Read(BitStream bitStream)
        {
            throw new Exception("Do not use StatValueUInt.");
        }

        public IStatValueType DeepCopy()
        {
            throw new Exception("Do not use StatValueUInt.");
        }

        public void Read(IDataRecord dataRecord, int ordinal)
        {
            throw new Exception("Do not use StatValueUInt.");
        }

        #endregion

#pragma warning restore 168
    }
}