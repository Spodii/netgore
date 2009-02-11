using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    public class UserQueryValues
    {
        readonly ushort _bodyIndex;
        readonly ushort _guid;
        readonly ushort _mapIndex;
        readonly string _name;
        readonly UserStats _stats;
        readonly int _x;
        readonly int _y;

        public ushort BodyIndex
        {
            get { return _bodyIndex; }
        }

        public ushort Guid
        {
            get { return _guid; }
        }

        public ushort MapIndex
        {
            get { return _mapIndex; }
        }

        public string Name
        {
            get { return _name; }
        }

        public UserStats Stats
        {
            get { return _stats; }
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public UserQueryValues(User user)
        {
            _guid = user.Guid;
            _mapIndex = user.Map.Index;
            _x = (int)user.Position.X;
            _y = (int)user.Position.Y;
            _bodyIndex = user.BodyInfo.Index;
            _name = user.Name;

            _stats = new UserStats();
            _stats.CopyStatValuesFrom(user.Stats, false);
        }
    }
}