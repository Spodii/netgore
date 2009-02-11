using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Platyform.Extensions;

/*** UNUSED ***/

namespace DemoGame.Server
{
    class InventoryUpdateTracker
    {
        /// <summary>
        /// Number of slots being tracked
        /// </summary>
        public const int Size = Inventory.MaxInventorySize;

        /// <summary>
        /// Number of bytes in the buffer
        static readonly int _bufferSize = (int)Math.Ceiling(Inventory.MaxInventorySize / 8.0f);

        /// <summary>
        /// Buffer used to store the bits
        /// </summary>
        readonly byte[] _buffer = new byte[_bufferSize];

        /// <summary>
        /// Gets or sets if a given inventory slot needs to be updated
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <returns>True if the slot needs to be updated, else false</returns>
        public bool this[int index]
        {
            get
            {
                Debug.Assert(index < Size && index >= 0, "Index out of range.");

                return (_buffer[index / 8] & (1 << (index % 8))) != 0;
            }
            set
            {
                Debug.Assert(index < Size && index >= 0, "Index out of range.");

                int sub = index / 8;
                int bit = 1 << (index % 8);

                if (value)
                    _buffer[sub] |= (byte)bit;
                else
                    _buffer[sub] &= (byte)(~bit);
            }
        }

        /// <summary>
        /// Gets or sets if a given inventory slot needs to be updated
        /// </summary>
        /// <param name="index">Index of the slot</param>
        /// <returns>True if the slot needs to be updated, else false</returns>
        public bool this[byte index]
        {
            get
            {
                Debug.Assert(index < Size && index >= 0, "Index out of range.");

                return (_buffer[index / 8] & (1 << (index % 8))) != 0;
            }
            set
            {
                Debug.Assert(index < Size && index >= 0, "Index out of range.");

                int sub = index / 8;
                int bit = 1 << (index % 8);

                if (value)
                    _buffer[sub] |= (byte)bit;
                else
                    _buffer[sub] &= (byte)(~bit);
            }
        }

        /// <summary>
        /// Sets all slots to not needing to be updated
        /// </summary>
        public void SetAllFalse()
        {
            for (int i = 0; i < _bufferSize; i++)
            {
                _buffer[i] = 0;
            }
        }

        /// <summary>
        /// Sets all slots to needing to be updated
        /// </summary>
        public void SetAllTrue()
        {
            for (int i = 0; i < _bufferSize; i++)
            {
                _buffer[i] = 255;
            }
        }
    }
}