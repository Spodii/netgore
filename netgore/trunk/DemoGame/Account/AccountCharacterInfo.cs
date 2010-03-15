using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Contains the information about a Character from the account-level view.
    /// </summary>
    public class AccountCharacterInfo
    {
        const string _valueKeyBodyID = "BodyID";
        const string _valueKeyIndex = "Index";
        const string _valueKeyName = "Name";

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountCharacterInfo"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="name">The name.</param>
        /// <param name="bodyID">The ID of the body.</param>
        public AccountCharacterInfo(byte index, string name, BodyID bodyID)
        {
            Index = index;
            Name = name;
            BodyID = bodyID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountCharacterInfo"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> used to read the object data from..</param>
        public AccountCharacterInfo(IValueReader r)
        {
            Index = r.ReadByte(_valueKeyIndex);
            Name = r.ReadString(_valueKeyName);
            BodyID = r.ReadBodyID(_valueKeyBodyID);
        }

        public BodyID BodyID { get; protected set; }
        public byte Index { get; protected set; }
        public string Name { get; protected set; }

        public void Write(IValueWriter w)
        {
            w.Write(_valueKeyIndex, Index);
            w.Write(_valueKeyName, Name);
            w.Write(_valueKeyBodyID, BodyID);
        }
    }
}