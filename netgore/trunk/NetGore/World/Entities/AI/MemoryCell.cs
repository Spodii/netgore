using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.AI
{
    public class MemoryCell
    {
        //The rectangle that holds the position of the MemoryCell
        Rectangle _cell;
        int _weight;

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
            _weight = 0;
        }

        /// <summary>
        /// A rectangle that holds positional data for this MemoryCell.
        /// </summary>
        public Rectangle Cell
        {
            get { return _cell; }
            set { _cell = value; }
        }

        /// <summary>
        /// The weighting of this MemoryCell.
        /// </summary>
        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        /// <summary>
        /// Resets the MemoryCell
        /// </summary>
        public void Reset()
        {
            _weight = 0;
        }

        /// <summary>
        /// Updates the MemoryCell
        /// </summary>
        public void Update()
        {
            ++_weight;
        }
    }
}