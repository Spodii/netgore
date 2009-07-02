using System;
using System.Diagnostics;

namespace DemoGame.Server
{
    /// <summary>
    /// Represents an item, amount of the item, and chance of the item to drop. Used for checking
    /// what items a NPC drop will upon death.
    /// </summary>
    public class NPCDrop
    {
        /// <summary>
        /// The numerator component of calculating the drop chance. The corresponding denominator is
        /// the NPCDrop.Chance.
        /// </summary>
        public const ushort DropChanceNumerator = 10;

        /// <summary>
        /// Random number generator for the NPCDrop.
        /// </summary>
        static readonly Random _rand = new Random();

        readonly ItemTemplate _itemTemplate;
        readonly NPCDropChance _dropChance;

        public ItemTemplate ItemTemplate { get { return _itemTemplate; } }

        public NPCDropChance DropChance { get { return _dropChance; } }

        public NPCDrop(ItemTemplate itemTemplate, NPCDropChance dropChance)
        {
            _itemTemplate = itemTemplate;
            _dropChance = dropChance;
        }

        /// <summary>
        /// Applies a random number to the NPCDrop's Chance, Min and Max to see how much of the item will drop.
        /// </summary>
        /// <returns>0 if the item failed to drop, else a random number between the NPCDrop's Min and Max (inclusive)
        /// will be returned.</returns>
        public byte GetDropAmount()
        {
            // Test if the item will drop
            if (!WillDrop())
                return 0;

            // Item will drop, so return a random amount
            return RandomDropAmount();
        }

        /// <summary>
        /// Applies a random number to the NPCDrop's Min and Max to see how much of an item will drop.
        /// </summary>
        /// <returns>A random number between Min and Max (inclusive).</returns>
        protected byte RandomDropAmount()
        {
            // Check for a constant drop amount (min == max)
            if (DropChance.Min == DropChance.Max)
                return DropChance.Min;

            // Return a random value between Min and Max
            return (byte)_rand.Next(DropChance.Min, DropChance.Max + 1);
        }

        /// <summary>
        /// Applies a random number to the NPCDrop's Chance to see if an item will drop.
        /// </summary>
        /// <returns>True if the random number generator resulted in a drop, else false.</returns>
        protected bool WillDrop()
        {
            // Check for always dropping (numerator >= denominator)
            if (DropChanceNumerator >= DropChance.Chance)
                return true;

            // Get the random number between 0 and the Chance
            int seed = _rand.Next(1, DropChance.Chance + 1);

            // If the random number is less than the DropChanceNumerator, the drop was successful
            return seed < DropChanceNumerator;
        }
    }
}