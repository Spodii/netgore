using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.AI
{
    /*TODO: Expand to "remember" strategies in some form - TODO: design how this part of memory will exist.
     *TODO: Remember NPC's AND Player Characters who have recently posed a threat i.e. Attacked so we can identify who we need to either target
     *      or evade or call for help (get alliances)?
     *TODO: Interact with "EMOTIVE CONTROLLER" by directly providing information about certain past events for initial "post processin" of input data
     *      TODO: design how to interact with "EMOTIVE CONTROLLER"
     *TODO: Optimize so as little physical memory is used as possible.
     *TODO: PreLoad a memoryMap from a file?
     * 
     *              PLEASE NOTE:    These ideas and theories are very much in a pre design stage and i just want to outline what to expect in the future
     *                              note not all of these ideas will be implemented, some will probably be implemented differently.  I've researched different methods extensively for a while
     *                              and these ideas are an amalgamation of different ideas i've read and studied as well as my own ideas.
     *                              It's also just to give me an idea of what i need to include in the future.
     *                                                                                                                                 aPhRo_
     */





    public class MemoryMap
    {
        //2 dimensional List<T> holding data for each MemoryCell.
        List<List<MemoryCell>> _memoryCells = new List<List<MemoryCell>>();

        //Holds some information about the list.
        int _cellsX;
        int _cellsY;
        int _totalCells;

        //Dimensions of a MemoryCell.
        double _cellSize;

        int _maxX;
        int _minY;

        /// <summary>
        /// Constructor for a MemoryMap, uses default value of 64 for MemoryCell dimensions.
        /// </summary>
        public MemoryMap()
        {
            _cellSize = 64;
        }

        /// <summary>
        /// Constructor for a MemoryMap
        /// </summary>
        /// <param name="cellSize">Defines the dimensions of a MemoryCell.</param>
        public MemoryMap(int cellSize)
        {
            _cellSize = (double)cellSize;
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

            _cellsX = (int)(maxX / _cellSize) + 1;
            _cellsY = (int)(minY / _cellSize) + 1;

            for (int X = 0; X < _cellsX; ++X)
            {
                List<MemoryCell> _temp = new List<MemoryCell>();

                for (int Y = 0; Y < _cellsY; ++Y)
                {
                    _temp.Add(new MemoryCell((int)(X * _cellSize), 
                                             (int)((X + 1) * _cellSize), 
                                             (int)(Y * _cellSize), 
                                             (int)((Y + 1) * _cellSize))); 
                }
                _memoryCells.Add(_temp);
            }

            _totalCells = _cellsX * _cellsY;
                 
        }

        /// <summary>
        /// Updates the MemoryMap.
        /// </summary>
        /// <param name="xPos">X position of the Entity</param>
        /// <param name="yPos">Y position of the Entity</param>
        public void Update(double xPos, double yPos)
        {
            if (((xPos < 0) || (xPos > _maxX)) || ((yPos < 0) || (yPos > _minY)))
            {
                return;
            }

            int cellX = (int)(xPos / _cellSize) + 1;
            int cellY = (int)(yPos / _cellSize) + 1;

            _memoryCells[cellX][cellY].Update();

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

            return _memoryCells[cellX][cellY].TickCount;
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
                    if (_memoryCells[X][Y].TickCount > 0)
                    {
                        ++total;
                    }
                }
            }
            return total;
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

            if (_memoryCells[cellX][cellY].TickCount > 0)
            { return true; }
            else
            { return false; }

        }

        public int NumCells
        {
            get { return _totalCells; }
        }


    }
}
