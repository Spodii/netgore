using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore.AI
{
    struct MemoryCell
    {

        //The number of ticks that the unit spends at a particular MemoryCell
        int _ticks;

        //The rectangle that holds the position of the MemoryCell
        Rectangle _cell;

        /// <summary>
        /// MemoryCell constuctor
        /// </summary>
        /// <param name="XMinimum">X position of the left side of MemoryCell</param>
        /// <param name="XMaximum">X position of the right side of MemoryCell</param>
        /// <param name="YMinimum">Y position of the top of MemoryCell</param>
        /// <param name="YMaximum">Y position of the bottom of MemoryCell</param>
        public MemoryCell(int XMinimum, int XMaximum, int YMinimum, int YMaximum)
        {
            _cell = new Rectangle(XMinimum, YMinimum, XMaximum - XMinimum, YMaximum - YMinimum);
            _ticks = 0;
        }

        /// <summary>
        /// Updates the MemoryCell
        /// </summary>
        public void Update()
        {
            ++_ticks;
        }

        /// <summary>
        /// Resets the MemoryCell
        /// </summary>
        public void Reset()
        {
            _ticks = 0;
        }

        /// <summary>
        /// A rectangle that holds positional data for this MemoryCell.
        /// </summary>
        public Rectangle Cell
        {
            get { return _cell; }
        }

        /// <summary>
        /// Number of ticks this MemoryCell has been occupied.
        /// </summary>
        public int TickCount
        {
            get { return _ticks; }
        }

    }
}
