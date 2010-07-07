using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;
using SFML.Graphics;

namespace NetGore.AI
{
    public class PathFinder : IPathFinder
    {
        // TODO: Documentation

        readonly List<AINode> _close;

#if TOPDOWN
        const bool topDown = true;
#else
        const bool topDown = false;
#endif

        readonly sbyte[,] _direction = new sbyte[8,2]
        { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };

        readonly AIGrid _grid;
        readonly AINode[] _nodeGrid;
        readonly PriorityQueue<int> _open;
        int _closeNodeCounter;
        byte _closeNodeValue = 2;
        int _endLocation;
        bool _found;
        int _h;
        int _heuristicEstimate = 2;
        Heuristics _heuristicFormula;
        int _location;

        ushort _locationX;
        ushort _locationY;
        int _newG;
        int _newLocation;
        ushort _newLocationX;
        ushort _newLocationY;
        byte _openNodeValue = 1;

        int _searchLimit;
        bool _stop;
        bool _stopped;

        public PathFinder(AIGrid Grid)
        {
            _grid = Grid;
            _nodeGrid = new AINode[_grid.GridX * _grid.GridY];

            _open = new PriorityQueue<int>(new CompareNodes(_nodeGrid));
            _close = new List<AINode>();
        }

        public int SearchLimit
        {
            get { return _searchLimit; }
            set { _searchLimit = value; }
        }

        public bool Stopped
        {
            get { return _stopped; }
        }

        #region IPathFinder Members

        public Heuristics HeuristicFormula
        {
            get { return _heuristicFormula; }
            set { _heuristicFormula = value; }
        }

        public IEnumerable<AINode> FindPath(Vector2 Start, Vector2 End)
        {
            lock (this)
            {
                _closeNodeCounter = 0;
                _openNodeValue += 2;
                _closeNodeValue += 2;

                _open.Clear();
                _close.Clear();

                _location = ((int)Start.Y << (int)_grid.Log2GridY) + (int)Start.X;
                _endLocation = ((int)End.Y << (int)_grid.Log2GridY) + (int)End.X;

                _nodeGrid[_location].G = 0;
                _nodeGrid[_location].F = _heuristicEstimate;
                _nodeGrid[_location].PX = (ushort)Start.X;
                _nodeGrid[_location].PY = (ushort)Start.Y;
                _nodeGrid[_location].Status = _openNodeValue;

                _open.Push(_location);

                while (_open.Count > 0 && !_stop)
                {
                    _location = _open.Pop();

                    if (_nodeGrid[_location].Status == _closeNodeValue)
                        continue;

                    _locationX = (ushort)(_location & (_grid.GridX - 1));
                    _locationY = (ushort)(_location >> (int)_grid.Log2GridY);

                    if (_location == _endLocation)
                    {
                        _nodeGrid[_location].Status = _closeNodeValue;
                        _found = true;
                        break;
                    }

                    if (_closeNodeCounter > _searchLimit)
                    {
                        _stopped = true;
                        return null;
                    }

                    for (var i = 0; i < (topDown ? 8 : 4); i++)
                    {
                        _newLocationX = (ushort)(_locationX + _direction[i, 0]);
                        _newLocationY = (ushort)(_locationY + _direction[i, 1]);
                        _newLocation = (_newLocationY << (int)_grid.Log2GridY) + _newLocationX;

                        if (_newLocationX >= _grid.GridX || _newLocationY >= _grid.GridY)
                            continue;

                        if (_grid._grid[_newLocationX, _newLocationY] == 0)
                            continue;

                        _newG = _nodeGrid[_location].G + _grid._grid[_newLocationX, _newLocationY];

                        if (_nodeGrid[_newLocation].Status == _openNodeValue || _nodeGrid[_newLocation].Status == _closeNodeValue)
                        {
                            if (_nodeGrid[_newLocation].G <= _newG)
                                continue;
                        }

                        _nodeGrid[_newLocation].PX = _locationX;
                        _nodeGrid[_newLocation].PY = _locationY;
                        _nodeGrid[_newLocation].G = _newG;

                        switch (_heuristicFormula)
                        {
                            case Heuristics.DiagonalShortCut:
                                var _hDiag = (int)Math.Min(Math.Abs(_newLocationX - End.X), Math.Abs(_newLocationY - End.Y));
                                var _hStra = (int)(Math.Abs(_newLocationX - End.X) + Math.Abs(_newLocationY - End.Y));

                                _h = (_heuristicEstimate * 2) * _hDiag + _heuristicEstimate * (_hStra - 2 * _hDiag);
                                break;

                            case Heuristics.Euclidean:
                                _h =
                                    (int)
                                    (_heuristicEstimate *
                                     Math.Sqrt(Math.Pow(_newLocationY - End.X, 2) + Math.Pow(_newLocationY - End.Y, 2)));
                                break;

                            case Heuristics.DXDY:
                                _h =
                                    (int)
                                    (_heuristicEstimate *
                                     (Math.Max(Math.Abs(_newLocationX - End.X), Math.Abs(_newLocationY - End.Y))));
                                break;

                            default:
                                _h =
                                    (int)
                                    (_heuristicEstimate * (Math.Abs(_newLocationX - End.X) + Math.Abs(_newLocationY - End.Y)));
                                break;
                        }

                        _nodeGrid[_newLocation].F = _newG + _h;

                        _open.Push(_newLocation);

                        _nodeGrid[_newLocation].Status = _openNodeValue;
                    }

                    _closeNodeCounter++;
                    _nodeGrid[_location].Status = _closeNodeValue;
                }

                if (_found)
                {
                    _close.Clear();

                    var _tmpNode = _nodeGrid[((int)End.Y << (int)_grid.Log2GridY) + (int)End.X];
                    var _node = new AINode
                    { F = _tmpNode.F, G = _tmpNode.G, PX = _tmpNode.PX, PY = _tmpNode.PY, X = (int)End.X, Y = (int)End.Y, H = 0 };

                    while (_node.X != _node.PX || _node.Y != _node.PY)
                    {
                        _close.Add(_node);

                        var posX = _node.PX;
                        var posY = _node.PY;
                        _tmpNode = _nodeGrid[(posY << (int)_grid.Log2GridY) + posX];
                        _node.F = _tmpNode.F;
                        _node.G = _tmpNode.G;
                        _node.H = 0;
                        _node.PX = _tmpNode.PX;
                        _node.PY = _tmpNode.PY;
                        _node.X = posX;
                        _node.Y = posY;
                    }

                    _close.Add(_node);
                    _stopped = true;
                    return _close;
                }

                _stopped = true;
                return null;
            }
        }

        #endregion

        internal class CompareNodes : IComparer<int>
        {
            readonly AINode[] _nodeGrid;

            public CompareNodes(AINode[] Nodes)
            {
                _nodeGrid = Nodes;
            }

            #region IComparer<int> Members

            public int Compare(int X, int Y)
            {
                if (_nodeGrid[X].F > _nodeGrid[Y].F)
                    return 1;
                else if (_nodeGrid[X].F < _nodeGrid[Y].F)
                    return -1;
                else
                    return 0;
            }

            #endregion
        }
    }
}