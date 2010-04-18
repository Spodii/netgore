using System;
using System.Linq;

namespace NetGore.AI
{
    public struct AIGrid
    {
        public readonly byte[,] _grid;

        public AIGrid(byte[,] Grid)
        {
            _grid = Grid;
        }

        public byte GridX
        {
            get { return (byte)(_grid.GetUpperBound(0) + 1); }
        }

        public byte GridY
        {
            get { return (byte)(_grid.GetUpperBound(1) + 1); }
        }

        public double Log2GridY
        {
            get { return Math.Log((_grid.GetUpperBound(1) + 1), 2); }
        }
    }
}