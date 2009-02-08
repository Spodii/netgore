using System;
using System.Bits;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

namespace DemoGame
{
    public interface IStat
    {
        event StatChangeHandler OnChange;
        bool CanWrite { get; }
        StatType StatType { get; }
        int Value { get; set; }

        IStat DeepCopy();

        IStatValueType DeepCopyValueType();

        void Read(BitStream bitStream);

        void Read(IDataRecord dataReader, int ordinal);

        void Write(BitStream bitStream);
    }
}