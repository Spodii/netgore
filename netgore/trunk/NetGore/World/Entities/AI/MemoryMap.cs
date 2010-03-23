using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.AI
{
    public class MemoryMap
    {
        //2 dimensional List<T> holding data for each MemoryCell.
        readonly double _cellSize;
        readonly List<List<MemoryCell>> _memoryCells = new List<List<MemoryCell>>();

        //Holds some information about the list.
        int _cellsX;
        int _cellsY;

        int _maxX;
        int _minY;
        int _totalCells;

        /// <summary>
        /// Constructor for a MemoryMap, uses default value of 16 for MemoryCell dimensions.
        /// </summary>
        public MemoryMap()
        {
            _cellSize = 16;
        }

        /// <summary>
        /// Constructor for a MemoryMap
        /// </summary>
        /// <param name="cellSize">Defines the dimensions of a MemoryCell.</param>
        public MemoryMap(int cellSize)
        {
            _cellSize = cellSize;
        }

        public int NumCells
        {
            get { return _totalCells; }
        }

        /// <summary>
        /// Initializes the MemoryMap by setting up the List of MemoryCells.
        /// </summary>
        /// <param name="maxX">X size of map</param>
        /// <param name="minY">Y size of map</param>
        public void Initialize(int maxX, int minY)
        {
            //Store map size values for use by MemoryMap
            _maxX = maxX;
            _minY = minY;

            int next_powY = 0;
            int next_powX = 0;
            bool arrayInc = false;

            _cellsX = (int)(maxX / _cellSize) + 1;
            _cellsY = (int)(minY / _cellSize) + 1;

            if (Math.Log(_cellsX, 2) != (int)Math.Log(_cellsX, 2) || Math.Log(_cellsY, 2) != (int)Math.Log(_cellsY, 2))
            {
                arrayInc = true;
                next_powX = BitOps.NextPowerOf2(_cellsX);
                next_powY = BitOps.NextPowerOf2(_cellsY);
            }

            for (int X = 0; X < (arrayInc ? next_powX : _cellsX); ++X)
            {
                List<MemoryCell> _temp = new List<MemoryCell>();

                for (int Y = 0; Y < (arrayInc ? next_powY : _cellsY); ++Y)
                {
                    _temp.Add(new MemoryCell((int)(X * _cellSize), (int)((X + 1) * _cellSize), (int)(Y * _cellSize),
                                             (int)((Y + 1) * _cellSize)));
                }
                _memoryCells.Add(_temp);
            }
            // Double check, (remove after debugging complete)
            if (Math.Log(_memoryCells.Count, 2) != (int)Math.Log(_memoryCells.Count, 2) ||
                Math.Log(_memoryCells[1].Count, 2) != (int)Math.Log(_memoryCells[1].Count, 2))
                throw new Exception("Grid not 2^n. _cellsX = " + _cellsX + ". _cellsY = " + _cellsY + ".");

            _totalCells = _cellsX * _cellsY;
        }

        public void LoadMemoryMap(ContentPaths contentPath, int ID)
        {
            var path = contentPath.Maps.Join("\\AIMap" + ID + ".xml");

            XmlValueReader read = new XmlValueReader(path, "MemoryMap");

            for (int X = 0; X < _cellsX; X++)
            {
                for (int Y = 0; Y < _cellsY; Y++)
                {
                    _memoryCells[X][Y].Weight = read.ReadInt("_cell" + X + Y);
                }
            }
        }

        /// <summary>
        /// Gets the total number of cells visited.
        /// </summary>
        /// <returns>Returns an integer greater than or equal to 0</returns>
        public int NumberofCellsVisited()
        {
            int total = 0;

            for (int X = 0; X < _cellsX; ++X)
            {
                for (int Y = 0; Y < _cellsY; ++Y)
                {
                    if (_memoryCells[X][Y].Weight > 0)
                        ++total;
                }
            }
            return total;
        }

        public void SaveMemoryMap(ContentPaths contentPath, int ID)
        {
            var path = contentPath.Maps.Join("\\AIMap" + ID + ".xml");
            using (var writer = new XmlValueWriter(path, "MemoryMap"))
            {
                for (int X = 0; X < _cellsX; X++)
                {
                    for (int Y = 0; Y < _cellsY; Y++)
                    {
                        writer.Write("_cell" + X + Y, _memoryCells[X][Y].Weight);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of Ticks that the instantiating Entity has spent at a particular cell containing a given point. 
        /// </summary>
        /// <param name="xPos">X position.</param>
        /// <param name="yPos">Y position.</param>
        /// <returns>Integer indicating the amount of ticks spent at this particular cell.</returns>
        public int TicksLingered(double xPos, double yPos)
        {
            int cellX = (int)(xPos / _cellSize);
            int cellY = (int)(yPos / _cellSize);

            return _memoryCells[cellX][cellY].Weight;
        }

        public sbyte[,] ToByteArray()
        {
            sbyte[,] temp = new sbyte[_cellsX,_cellsY];
            for (int X = 0; X < _cellsX; X++)
            {
                for (int Y = 0; Y < _cellsY; Y++)
                {
                    temp[X, Y] = (sbyte)_memoryCells[X][Y].Weight;
                }
            }

            return temp;
        }

        /// <summary>
        /// Updates the MemoryMap.
        /// </summary>
        /// <param name="xPos">X position of the Entity</param>
        /// <param name="yPos">Y position of the Entity</param>
        public void Update(double xPos, double yPos)
        {
            if (((xPos < 0) || (xPos > _maxX)) || ((yPos < 0) || (yPos > _minY)))
                return;

            int cellX = (int)(xPos / _cellSize) + 1;
            int cellY = (int)(yPos / _cellSize) + 1;

            _memoryCells[cellX][cellY].Update();
        }

        /// <summary>
        /// Gets whether a particular cell containing a given point has been visited yet.
        /// </summary>
        /// <param name="xPos">X position</param>
        /// <param name="yPos">Y position</param>
        /// <returns>True if it has been visited, false if it has not been visited.</returns>
        public bool Visited(double xPos, double yPos)
        {
            int cellX = (int)(xPos / _cellSize);
            int cellY = (int)(yPos / _cellSize);

            if (_memoryCells[cellX][cellY].Weight > 0)
                return true;
            else
                return false;
        }

        public List<List<MemoryCell>> MemoryCells
        {
            get
            {return _memoryCells;}
        }
    }
}


