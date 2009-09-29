using System.Linq;
using DemoGame;
using NetGore;
using NetGore.IO;
using NetGore.RPGComponents;

namespace DemoGame
{
    /// <summary>
    /// Contains the information about a Character from the account-level view.
    /// </summary>
    public class AccountCharacterInfo
    {
        const string _valueKeyBodyIndex = "BodyIndex";
        const string _valueKeyIndex = "Index";
        const string _valueKeyName = "Name";

        public BodyIndex BodyIndex { get; protected set; }
        public byte Index { get; protected set; }
        public string Name { get; protected set; }

        public AccountCharacterInfo(byte index, string name, BodyIndex bodyIndex)
        {
            Index = index;
            Name = name;
            BodyIndex = bodyIndex;
        }

        public AccountCharacterInfo(IValueReader r)
        {
            Index = r.ReadByte("Index");
            Name = r.ReadString("Name");
            BodyIndex = r.ReadBodyIndex("BodyIndex");
        }

        public void Write(IValueWriter w)
        {
            w.Write(_valueKeyIndex, Index);
            w.Write(_valueKeyName, Name);
            w.Write(_valueKeyBodyIndex, BodyIndex);
        }
    }
}