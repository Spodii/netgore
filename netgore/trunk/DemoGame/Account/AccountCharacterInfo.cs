using System.Linq;
using NetGore.IO;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountCharacterInfo"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="name">The name.</param>
        /// <param name="bodyIndex">Index of the body.</param>
        public AccountCharacterInfo(byte index, string name, BodyIndex bodyIndex)
        {
            Index = index;
            Name = name;
            BodyIndex = bodyIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountCharacterInfo"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> used to read the object data from..</param>
        public AccountCharacterInfo(IValueReader r)
        {
            Index = r.ReadByte("Index");
            Name = r.ReadString("Name");
            BodyIndex = r.ReadBodyIndex("BodyIndex");
        }

        public BodyIndex BodyIndex { get; protected set; }
        public byte Index { get; protected set; }
        public string Name { get; protected set; }

        public void Write(IValueWriter w)
        {
            w.Write(_valueKeyIndex, Index);
            w.Write(_valueKeyName, Name);
            w.Write(_valueKeyBodyIndex, BodyIndex);
        }
    }
}