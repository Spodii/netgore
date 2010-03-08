using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.AI
{
    public struct AIGrid
    {
        public byte[,] _grid;

        public int GridX
        {
            get { return _grid.GetUpperBound(0) + 1; }
        }

        public int GridY
        {
            get { return _grid.GetUpperBound(1) + 1; }
        }

        public AIGrid(byte[,] Grid)
        {
            _grid = Grid;
        }

        public int TotalNumberofCells
        {
            get { return (_grid.GetUpperBound(0) + 1) * (_grid.GetUpperBound(1) + 1); }
        }

        public double Log2GridY
        {
            get { return Math.Log((_grid.GetUpperBound(1) + 1), 2); }
        }

    }
}
