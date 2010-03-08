using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using NetGore.Collections;

namespace NetGore.AI
{
    public struct Node
    {
        public int F;
        public int G;
        public ushort PX;
        public ushort PY;
        public byte Status;
        public int X;
        public int Y;
        public int H;
    }
    
    
    public class PathFinder : IPathFinder
    {
        

        internal class CompareNodes : IComparer<int>
        {
            Node[] _nodeGrid;

            public CompareNodes(Node[] Nodes)
            {
                _nodeGrid = Nodes;
            }

            public int Compare(int X, int Y)
            {
                if (_nodeGrid[X].F > _nodeGrid[Y].F)
                {
                    return 1;
                }
                else if (_nodeGrid[X].F < _nodeGrid[Y].F)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        readonly AIGrid _grid;
        private Node[] _nodeGrid;
        PriorityQueue<int> _open;
        List<Node> _close;


        private bool _found;
        private bool _stop;
        private bool _stopped;
        private int _location;
        private int _endLocation;
        private int _closeNodeCounter;
        private byte _openNodeValue;
        private byte _closeNodeValue;
        private int _heuristicEstimate = 2;

        private ushort _locationX;
        private ushort _locationY;
        private ushort _newLocationX;
        private ushort _newLocationY;
        private int _newLocation;
        private int _newG;
        private Heuristics _heuristicFormula;
        private int _h;
        private sbyte[,] _direction = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
        private int _searchLimit;

        public PathFinder(AIGrid Grid)
        {
            _grid = Grid;
            _nodeGrid = new Node[_grid.TotalNumberofCells];

            _open = new PriorityQueue<int>(new CompareNodes(_nodeGrid));

        }

        public List<Node> FindPath(Vector2 Start, Vector2 End)
        {
            lock (this)
            {
                
                _closeNodeCounter = 0;
                _openNodeValue += 2;
                _closeNodeCounter += 2;

                _open.Clear();
                _close.Clear();

                _location = ((int)Start.X << (int)_grid.Log2GridY) + (int)Start.X;
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

                    _locationX = (ushort) (_location & (_grid.GridX - 1));
                    _locationY = (ushort)(_location >> (int)(_grid.Log2GridY));

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

                    for (int i = 0; i < 8; i++)
                    {
                        _newLocationX = (ushort)(_locationX + _direction[i, 0]);
                        _newLocationY = (ushort)(_locationY + _direction[i, 1]);
                        _newLocation = (_newLocationY << (ushort)_grid.Log2GridY) + _newLocationX;
                        

                        if (_newLocationX >= _grid.GridX || _newLocationY >= _grid.GridY)
                        {
                            continue;
                        }

                        _newG = _nodeGrid[_location].G + _grid._grid[_newLocationX, _newLocationY];

                        if (_nodeGrid[_newLocation].Status == _openNodeValue || _nodeGrid[_newLocation].Status == _closeNodeValue)
                        {
                            if (_nodeGrid[_newLocation].G <= _newG)
                            {
                                continue;
                            }
                        }

                        _nodeGrid[_newLocation].PX = _locationX;
                        _nodeGrid[_newLocation].PY = _locationY;
                        _nodeGrid[_newLocation].G = _newG;

                        switch (_heuristicFormula)
                        {
                            case Heuristics.DiagonalShortCut:
                                int _hDiag = (int)Math.Min(Math.Abs(_newLocationX - End.X), Math.Abs(_newLocationY - End.Y));
                                int _hStra = (int)(Math.Abs(_newLocationX - End.X) + Math.Abs(_newLocationY - End.Y));
                                
                                _h = (_heuristicEstimate * 2) * _hDiag + _heuristicEstimate * (_hStra - 2 * _hDiag);
                                break;

                            default:
                            case Heuristics.Manhattan:
                                _h = (int)(_heuristicEstimate * (Math.Abs(_newLocationX - End.X) + Math.Abs(_newLocationY - End.Y)));
                                break;
                            
                            case Heuristics.Euclidean:
                                _h = (int) (_heuristicEstimate * Math.Sqrt(Math.Pow(_newLocationY - End.X,2) + Math.Pow(_newLocationY - End.Y, 2)));
                                break;
                            
                            case Heuristics.DXDY:
                                _h = (int)(_heuristicEstimate * (Math.Max(Math.Abs(_newLocationX - End.X), Math.Abs(_newLocationY - End.Y))));
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
                    int posX = (int)End.X;
                    int posY = (int)End.Y;

                    Node _tmpNode = _nodeGrid[((int)End.Y << (int)_grid.Log2GridY) + (int)End.X];
                    Node _node;
                    _node.F = _tmpNode.F;
                    _node.G = _tmpNode.G;
                    _node.PX = _tmpNode.PX;
                    _node.PY = _tmpNode.PY;
                    _node.X = (int)End.X;
                    _node.Y = (int)End.Y;
                    _node.H = 0;

                    while (_node.X != _node.PX || _node.Y != _node.PY)
                    {
                        _close.Add(_node);

                        posX = _node.PX;
                        posY = _node.PY;
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

        public bool Stopped
        {
            get { return _stopped; }
        }

        public Heuristics HeuristicFormula
        {
            get { return _heuristicFormula; }
            set { _heuristicFormula = value; }
        }

        public int SearchLimit
        {
            get { return _searchLimit; }
            set { _searchLimit = value; }
        }

    }




}
