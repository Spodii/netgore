using System;

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

        readonly ushort _chance;
        readonly ushort _guid;
        readonly ItemTemplate _itemTemplate;
        readonly byte _max;
        readonly byte _min;

        /// <summary>
        /// Gets the chance for the item to drop. The actual chance of a drop is DropChanceNumerator / Chance.
        /// </summary>
        public ushort Chance
        {
            get { return _chance; }
        }

        /// <summary>
        /// Gets the unique ID of this NPCDrop.
        /// </summary>
        public ushort Guid
        {
            get { return _guid; }
        }

        /// <summary>
        /// Gets the ItemTemplate for the item that this NPCDrop will drop..
        /// </summary>
        public ItemTemplate ItemTemplate
        {
            get { return _itemTemplate; }
        }

        /// <summary>
        /// Gets the maximum amount of the item that will drop. Must be greater than 0 and less than or
        /// equal to the minimum drop amount.
        /// </summary>
        public byte Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Gets the minimum amount of the item that will drop. Can be equal to 0, but must be less than
        /// or equal to the maximum drop amount.
        /// </summary>
        public byte Min
        {
            get { return _min; }
        }

        /// <summary>
        /// NPCDrop constructor.
        /// </summary>
        /// <param name="guid">The unique ID of this NPCDrop.</param>
        /// <param name="itemTemplate">The ItemTemplate for the item that this NPCDrop will drop.</param>
        /// <param name="min">The minimum amount of the item that will drop. Can be equal to 0, but must be less than
        /// or equal to the maximum drop amount.</param>
        /// <param name="max">The maximum amount of the item that will drop. Must be greater than 0 and less than or
        /// equal to the minimum drop amount.</param>
        /// <param name="chance">The chance for the item to drop. The actual chance of a drop is
        /// DropChanceNumerator / Chance.</param>
        public NPCDrop(ushort guid, ItemTemplate itemTemplate, byte min, byte max, ushort chance)
        {
            if (chance == 0)
                throw new ArgumentOutOfRangeException("chance", "The drop chance must be greater than 0.");
            if (max == 0)
                throw new ArgumentOutOfRangeException("max", "The max drop amount must be greater than 0.");
            if (min > max)
                throw new ArgumentOutOfRangeException("min", "The min drop amount must be less than or equal to the max.");

            _guid = guid;
            _itemTemplate = itemTemplate;
            _min = min;
            _max = max;
            _chance = chance;
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
            if (Min == Max)
                return Min;

            // Return a random value between Min and Max
            return (byte)_rand.Next(Min, Max + 1);
        }

        /// <summary>
        /// Applies a random number to the NPCDrop's Chance to see if an item will drop.
        /// </summary>
        /// <returns>True if the random number generator resulted in a drop, else false.</returns>
        protected bool WillDrop()
        {
            // Check for always dropping (numerator >= denominator)
            if (DropChanceNumerator >= Chance)
                return true;

            // Get the random number between 0 and the Chance
            int seed = _rand.Next(1, Chance + 1);

            // If the random number is less than the DropChanceNumerator, the drop was successful
            return seed < DropChanceNumerator;
        }
    }
}