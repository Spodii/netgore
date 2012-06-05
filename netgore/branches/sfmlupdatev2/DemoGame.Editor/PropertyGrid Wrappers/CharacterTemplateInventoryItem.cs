using System;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server;
using DemoGame.Server.DbObjs;

namespace DemoGame.Editor
{
    /// <summary>
    /// A struct that describes the creation of an inventory item for a <see cref="CharacterTemplate"/>.
    /// </summary>
    public struct CharacterTemplateInventoryItem : IEquatable<CharacterTemplateInventoryItem>
    {
        ItemTemplateID _id;
        ItemChance _chance;
        ushort _min;
        ushort _max;

        /// <summary>
        /// Gets a <see cref="ICharacterTemplateInventoryTable"/> from this struct.
        /// </summary>
        /// <param name="charID">The <see cref="CharacterTemplateID"/>.</param>
        /// <param name="rowID">The database table row ID.</param>
        /// <returns>The <see cref="ICharacterTemplateInventoryTable"/></returns>
        public ICharacterTemplateInventoryTable ToTableRow(CharacterTemplateID charID, int rowID)
        {
            return new CharacterTemplateInventoryTable(iD: rowID, chance: Chance, characterTemplateID: charID, itemTemplateID: ID,
                min: (byte)Min, max: (byte)Max);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">The left side argument.</param>
        /// <param name="b">The right side argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(CharacterTemplateInventoryItem a, CharacterTemplateInventoryItem b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">The left side argument.</param>
        /// <param name="b">The right side argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(CharacterTemplateInventoryItem a, CharacterTemplateInventoryItem b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Gets or sets the <see cref="ItemTemplateID"/>.
        /// </summary>
        public ItemTemplateID ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets the chance of the item being in the inventory.
        /// </summary>
        public ItemChance Chance
        {
            get { return _chance; }
            set { _chance = value; }
        }

        /// <summary>
        /// Gets or sets the minimum amount of items to be created.
        /// </summary>
        public ushort Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// Gets or sets the maximum amount of items to be created.
        /// </summary>
        public ushort Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateInventoryItem"/> struct.
        /// </summary>
        /// <param name="id">The <see cref="ItemTemplateID"/>.</param>
        /// <param name="chance">The chance of the item being in the inventory.</param>
        /// <param name="min">The minimum amount of items to be created.</param>
        /// <param name="max">The maximum amount of items to be created.</param>
        public CharacterTemplateInventoryItem(ItemTemplateID id, ItemChance chance, ushort min, ushort max)
        {
            _id = id;
            _chance = chance;
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(CharacterTemplateInventoryItem other)
        {
            return other._id.Equals(_id) && other._chance.Equals(_chance) && other._min == _min && other._max == _max;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is CharacterTemplateInventoryItem && this == (CharacterTemplateInventoryItem)obj;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = _id.GetHashCode();
                result = (result * 397) ^ _chance.GetHashCode();
                result = (result * 397) ^ _min.GetHashCode();
                result = (result * 397) ^ _max.GetHashCode();
                return result;
            }
        }
    }
}