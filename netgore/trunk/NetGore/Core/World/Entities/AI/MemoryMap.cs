using System;
using System.IO;
using System.Linq;
using NetGore.IO;

namespace NetGore.AI
{
    public class MemoryMap
    {
        // Array holding data for each MemoryCell.

        //Holds some information about the list.
        ushort _cellSize;
        ushort _cellsX;
        ushort _cellsY;

        ushort _maxX;
        MemoryCell[,] _memoryCells;
        ushort _minY;

        /// <summary>
        /// Constructor for a MemoryMap, uses default value of 32 for MemoryCell dimensions.
        /// </summary>
        public MemoryMap()
        {
            _cellSize = 32;
        }

        /// <summary>
        /// Constructor for a MemoryMap
        /// </summary>
        /// <param name="cellSize">Defines the dimensions of a MemoryCell.</param>
        public MemoryMap(ushort cellSize)
        {
            _cellSize = cellSize;
        }

        public ushort CellSize
        {
            get { return _cellSize; }
        }

        public ushort CellsX
        {
            get { return _cellsX; }
        }

        public ushort CellsY
        {
            get { return _cellsY; }
        }

        public MemoryCell[,] MemoryCells
        {
            get { return _memoryCells; }
        }

        /// <summary>
        /// Initializes the MemoryMap by setting up the List of MemoryCells.
        /// </summary>
        /// <param name="maxX">X size of map</param>
        /// <param name="minY">Y size of map</param>
        public void Initialize(ushort maxX, ushort minY)
        {
            //Store map size values for use by MemoryMap
            _maxX = maxX;
            _minY = minY;

            _cellsX = (ushort)((_maxX / _cellSize) + 1);
            _cellsY = (ushort)((_minY / _cellSize) + 1);

            if (Math.Log(_cellsX, 2) != (int)Math.Log(_cellsX, 2) || Math.Log(_cellsY, 2) != (int)Math.Log(_cellsY, 2))
            {
                _cellsX = (ushort)BitOps.NextPowerOf2(_cellsX);
                _cellsY = (ushort)BitOps.NextPowerOf2(_cellsY);
            }

            _memoryCells = new MemoryCell[_cellsX,_cellsY];

            for (var X = 0; X < _cellsX; X++)
            {
                for (var Y = 0; Y < _cellsY; Y++)
                {
                    _memoryCells[X, Y] = new MemoryCell((ushort)(X * _cellSize), (ushort)(Y * _cellSize));
                }
            }
        }

        static PathString GetFilePath(ContentPaths contentPath, int ID)
        {
            return contentPath.Maps.Join("AIMap" + ID + EngineSettings.DataFileSuffix);
        }

        public void LoadMemoryMap(ContentPaths contentPath, int ID)
        {
            var path = GetFilePath(contentPath, ID);
            if (!File.Exists(path))
            {
                Initialize(1024, 1024);
                return;
            }

            var read = new GenericValueReader(path, _rootNodeName);

            Initialize(1024, 1024);

            _cellSize = read.ReadUShort("CellSize");

            for (var X = 0; X < _cellsX; X++)
            {
                var xReader = read.ReadNode("CellX" + X);
                for (var Y = 0; Y < _cellsY; Y++)
                {
                    _memoryCells[X, Y].Weight = xReader.ReadByte("CellY" + Y);
                }
            }
        }

        /// <summary>
        /// Gets the total number of cells visited.
        /// </summary>
        /// <returns>Returns an integer greater than or equal to 0</returns>
        public int NumberofCellsVisited()
        {
            var total = 0;

            for (var X = 0; X < _cellsX; X++)
            {
                for (var Y = 0; Y < _cellsY; Y++)
                {
                    if (_memoryCells[X, Y].Weight > 0)
                        ++total;
                }
            }
            return total;
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is <see cref="GenericValueIOFormat.Binary"/>.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

        const string _rootNodeName = "MemoryMap";

        /// <summary>
        /// Initializes the <see cref="MemoryMap"/> class.
        /// </summary>
        static MemoryMap()
        {
            EncodingFormat = GenericValueIOFormat.Binary;
        }

        public void SaveMemoryMap(ContentPaths contentPath, int ID)
        {
            var path = GetFilePath(contentPath, ID);

            using (var writer = new GenericValueWriter(path, _rootNodeName, EncodingFormat))
            {
                writer.Write("CellSize", _cellSize);

                for (var X = 0; X < _cellsX; X++)
                {
                    writer.WriteStartNode("CellX" + X);

                    for (var Y = 0; Y < _cellsY; Y++)
                    {
                        writer.Write("CellY" + Y, _memoryCells[X, Y].Weight);
                    }

                    writer.WriteEndNode("CellX" + X);
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
            var cellX = (ushort)(xPos / _cellSize);
            var cellY = (ushort)(yPos / _cellSize);

            return _memoryCells[cellX, cellY].Weight;
        }

        public byte[,] ToByteArray()
        {
            var temp = new byte[_cellsX,_cellsY];
            for (var X = 0; X < _cellsX; X++)
            {
                for (var Y = 0; Y < _cellsY; Y++)
                {
                    temp[X, Y] = _memoryCells[X, Y].Weight;
                }
            }

            return temp;
        }
    }
}