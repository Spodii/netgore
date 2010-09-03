using System;
using System.IO;
using System.Linq;
using NetGore.IO;

namespace NetGore.AI
{
    public class MemoryMap
    {

        //Holds some information about the list.
        const string _rootNodeName = "MemoryMap";
        ushort _cellSize;
        ushort _cellsX;
        ushort _cellsY;

        ushort _maxX;
        MemoryCell[,] _memoryCells;
        ushort _minY;

        bool _isInitialized = false;


        /// <summary>
        /// Constructor for a MemoryMap, uses default value of 32 for MemoryCell dimensions. This constructor should be only used in an AI Editor environment.
        /// </summary>
        public MemoryMap()
        {
            EncodingFormat = GenericValueIOFormat.Binary;
            _cellSize = 32;
        }

        /// <summary>
        /// Constructor for a MemoryMap with a set initial size. This constructor should be only used in an AI Editor environment.
        /// </summary>
        /// <param name="cellSize">Defines the dimensions of a MemoryCell.</param>
        public MemoryMap(ushort cellSize)
        {
            EncodingFormat = GenericValueIOFormat.Binary;
            _cellSize = cellSize;
        }

        /// <summary>
        /// Constructor that loads an already saved memory map into it's memory.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="ID"></param>
        public MemoryMap(ContentPaths content, int ID)
        {
            EncodingFormat = GenericValueIOFormat.Binary;
            LoadMemoryMap(content, ID);
        }

        /// <summary>
        /// Returns the default cell size of the <see cref="MemoryMap"/>.
        /// </summary>
        public ushort CellSize
        {
            get { return _cellSize; }
        }

        /// <summary>
        /// Returns the number of columns in the <see cref="MemoryMap"/>
        /// </summary>
        public ushort CellsX
        {
            get { return _cellsX; }
        }

        /// <summary>
        /// Returns the number of rows in the <see cref="MemoryMap"/>.
        /// </summary>
        public ushort CellsY
        {
            get { return _cellsY; }
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is <see cref="GenericValueIOFormat.Binary"/>.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

        /// <summary>
        /// Returns the underlying detailed information about the <see cref="MemoryMap"/>.
        /// </summary>
        public MemoryCell[,] MemoryCells
        {
            get { return _memoryCells; }
        }

        /// <summary>
        /// Static class to retrieve the complete file path of a <see cref="MemoryMap"/> using a given <see cref="ContentPaths"/> and AIMap ID.
        /// </summary>
        /// <param name="contentPath">Contentpath to find the AIMap.</param>
        /// <param name="ID">The ID of the map to find.</param>
        /// <returns></returns>
        static PathString GetFilePath(ContentPaths contentPath, int ID)
        {
            return contentPath.Maps.Join("AIMap" + ID + EngineSettings.DataFileSuffix);
        }

        /// <summary>
        /// Initializes the MemoryMap by setting up the List of MemoryCells.  Before a MemoryMap can be used it must be Initialized.
        /// </summary>
        /// <param name="maxX">X size of map</param>
        /// <param name="minY">Y size of map</param>
        public void Initialize(ushort maxX, ushort minY)
        {

            _memoryCells = null;
            
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
        
        /// <summary>
        /// Loads a <see cref="MemoryMap"/> into the current class.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load this new MemoryMap from.</param>
        /// <param name="ID">The ID of the MemoryMap to load, this should be kept the same as it's corresponding World Map.</param>
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
        /// Saves the current <see cref="MemoryMap"/> to a binary file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to save this MemoryMap to.</param>
        /// <param name="ID">The ID of the MemoryMap, this should be kept the same as it's corresponding World Map.</param>
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
        /// Gets the weight specified for a particular cell in a <see cref="MemoryMap"/>.  Use only for Debug information. 
        /// </summary>
        /// <param name="xPos">X position on map.</param>
        /// <param name="yPos">Y position on map.</param>
        /// <returns>Integer indicating the weight of a particular cell.</returns>
        public int WeightOfCell(double xPos, double yPos)
        {
            var cellX = (ushort)(xPos / _cellSize);
            var cellY = (ushort)(yPos / _cellSize);

            return _memoryCells[cellX, cellY].Weight;
        }


        /// <summary>
        /// Converts the more detailed information held in a <see cref="MemoryMap"/> to a raw byte array.
        /// </summary>
        /// <returns>The raw data of this <see cref="MemoryMap"/> in a two dimensional byte array.</returns>
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