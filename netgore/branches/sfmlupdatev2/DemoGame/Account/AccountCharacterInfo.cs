using System.Collections.Generic;
using System.Diagnostics;
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
        readonly List<string> _equippedBodies = new List<string>();

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

        public IEnumerable<string> EquippedBodies
        {
            get { return _equippedBodies; }
        }

        [SyncValue]
        public byte Index { get; protected set; }

        [SyncValue]
        public string Name { get; protected set; }

        public void SetEquippedBodies(IEnumerable<string> values)
        {
            _equippedBodies.Clear();
            _equippedBodies.AddRange(values);
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);

            // Equipped bodies

            int count = reader.ReadByte("EquippedBodiesCount");
            _equippedBodies.Clear();

            for (var i = 0; i < count; i++)
            {
                var s = reader.ReadString("EquippedBody_" + i);
                _equippedBodies.Add(s);
            }
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);

            // Equipped bodies

            Debug.Assert(_equippedBodies.Count <= byte.MaxValue);

            writer.Write("EquippedBodiesCount", (byte)_equippedBodies.Count);

            for (var i = 0; i < _equippedBodies.Count; i++)
            {
                writer.Write("EquippedBody_" + i, _equippedBodies[i]);
            }
        }

        #endregion
    }
}