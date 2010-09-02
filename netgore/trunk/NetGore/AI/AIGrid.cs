using System;
using System.Linq;

namespace NetGore.AI
{
    public struct AIGrid
    {
        // TODO: Documentation

        public readonly byte[,] _grid;

        byte _gridX;
        byte _gridY;
        double _log2gridY;

        public AIGrid(byte[,] Grid)
        {
            _grid = Grid;
            _gridX = (byte)(_grid.GetUpperBound(0) + 1);
            _gridY = (byte)(_grid.GetUpperBound(1) + 1);
            _log2gridY = Math.Log((_grid.GetUpperBound(1) + 1), 2);
        }

        public byte GridX
        {
            get { return _gridX; }
        }

        public byte GridY
        {
            get { return _gridY; }
        }

        public double Log2GridY
        {
            get { return _log2gridY; }
        }
    }
}