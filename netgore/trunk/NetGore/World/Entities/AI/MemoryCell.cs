using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.AI
{
    public struct MemoryCell
    {
        ushort _minX;
        ushort _minY;
        byte _debugStatus;
        byte _weight;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCell"/> struct.
        /// </summary>
        /// <param name="minX">X position of the left side of cell's area.</param>
        /// <param name="minY">Y position of the top side of cell's area.</param>
        internal MemoryCell(ushort minX, ushort minY)
        {
            _minX = minX;
            _minY = minY;
            _weight = 0;
            _debugStatus = 0;
        }

        public Point Location { get { return new Point(MinX, MinY); } }

        public ushort MinX { get { return _minX; } set { _minX = value; } }

        public ushort MinY { get { return _minY; } set { _minY = value; } }

        public Rectangle GetArea(int cellSize)
        {
            return new Rectangle(MinX, MinY, cellSize, cellSize);
        }

        public byte DebugStatus
        {
            get { return _debugStatus; }
            set { _debugStatus = value; }
        }

        /// <summary>
        /// The weighting of this MemoryCell.
        /// </summary>
        public byte Weight
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
    }
}