using System;
using System.Linq;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
    static class PackUtils
    {
        // Methods
        static double ClampAndRound(float value, float min, float max)
        {
            if (float.IsNaN(value))
                return 0.0;
            if (float.IsInfinity(value))
                return (float.IsNegativeInfinity(value) ? (min) : (max));
            if (value < min)
                return min;
            if (value > max)
                return max;
            return Math.Round(value);
        }

        public static uint PackSigned(uint bitmask, float value)
        {
            float max = bitmask >> 1;
            float min = -max - 1f;
            return (((uint)((int)ClampAndRound(value, min, max))) & bitmask);
        }

        public static uint PackSNorm(uint bitmask, float value)
        {
            float max = bitmask >> 1;
            value *= max;
            return (((uint)((int)ClampAndRound(value, -max, max))) & bitmask);
        }

        public static uint PackUNorm(float bitmask, float value)
        {
            value *= bitmask;
            return (uint)ClampAndRound(value, 0f, bitmask);
        }

        public static uint PackUnsigned(float bitmask, float value)
        {
            return (uint)ClampAndRound(value, 0f, bitmask);
        }

        public static float UnpackSNorm(uint bitmask, uint value)
        {
            uint num = ((bitmask + 1) >> 1);
            if ((value & num) != 0)
            {
                if ((value & bitmask) == num)
                    return -1f;
                value |= ~bitmask;
            }
            else
                value &= bitmask;
            float num2 = bitmask >> 1;
            return ((value) / num2);
        }

        public static float UnpackUNorm(uint bitmask, uint value)
        {
            value &= bitmask;
            return ((value) / ((float)bitmask));
        }
    }
}