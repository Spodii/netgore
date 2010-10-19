using System;
using System.Linq;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Contains the information about a Character from the account-level view.
    /// </summary>
    public class AccountCharacterInfo : IPersistable
    {
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
            ReadState(r);
        }

        [SyncValue]
        public BodyID BodyID { get; protected set; }

        [SyncValue]
        public byte Index { get; protected set; }

        [SyncValue]
        public string Name { get; protected set; }

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }
    }
}