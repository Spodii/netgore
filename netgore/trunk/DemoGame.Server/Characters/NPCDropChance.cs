using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public struct NPCDropChance
    {
        /// <summary>
        /// The minimum amount of the item that will drop. Can be equal to 0, but must be less than
        /// or equal to the maximum drop amount.
        /// </summary>
        public readonly byte Min;

        /// <summary>
        /// The maximum amount of the item that will drop. Must be greater than 0 and less than or
        /// equal to the minimum drop amount.
        /// </summary>
        public readonly byte Max;
        public readonly ushort Chance;

        public NPCDropChance(byte min, byte max, ushort chance)
        {
            if (chance == 0)
                throw new ArgumentOutOfRangeException("chance", "The drop chance must be greater than 0.");
            if (max == 0)
                throw new ArgumentOutOfRangeException("max", "The max drop amount must be greater than 0.");
            if (min > max)
                throw new ArgumentOutOfRangeException("min", "The min drop amount must be less than or equal to the max.");

            Min = min;
            Max = max;
            Chance = chance;
        }
    }
}
