using System;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server;
using DemoGame.Server.DbObjs;

namespace DemoGame.Editor
{
    /// <summary>
    /// A struct that describes the creation of an equipped item for a <see cref="CharacterTemplate"/>.
    /// </summary>
    public struct CharacterTemplateEquippedItem : IEquatable<CharacterTemplateEquippedItem>
    {
        ItemTemplateID _id;
        ItemChance _chance;

        /// <summary>
        /// Gets a <see cref="ICharacterTemplateEquippedTable"/> from this struct.
        /// </summary>
        /// <param name="charID">The <see cref="CharacterTemplateID"/>.</param>
        /// <param name="rowID">The database table row ID.</param>
        /// <returns>The <see cref="ICharacterTemplateEquippedTable"/></returns>
        public ICharacterTemplateEquippedTable ToTableRow(CharacterTemplateID charID, int rowID)
        {
            return new CharacterTemplateEquippedTable(iD: rowID, chance: Chance, characterTemplateID: charID, itemTemplateID: ID);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">The left side argument.</param>
        /// <param name="b">The right side argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(CharacterTemplateEquippedItem a, CharacterTemplateEquippedItem b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">The left side argument.</param>
        /// <param name="b">The right side argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(CharacterTemplateEquippedItem a, CharacterTemplateEquippedItem b)
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
        /// Gets or sets the chance of the item being equipped.
        /// </summary>
        public ItemChance Chance
        {
            get { return _chance; }
            set { _chance = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateEquippedItem"/> struct.
        /// </summary>
        /// <param name="id">The <see cref="ItemTemplateID"/>.</param>
        /// <param name="chance">The chance of the item being equipped.</param>
        public CharacterTemplateEquippedItem(ItemTemplateID id, ItemChance chance)
        {
            _id = id;
            _chance = chance;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(CharacterTemplateEquippedItem other)
        {
            return other._id.Equals(_id) && other._chance.Equals(_chance);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is CharacterTemplateEquippedItem && this == (CharacterTemplateEquippedItem)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_id.GetHashCode() * 397) ^ _chance.GetHashCode();
            }
        }
    }
}