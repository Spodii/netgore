using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// Container for a WallEntity placed in a Listbox.
    /// </summary>
    class ListboxWall
    {
        /// <summary>
        /// Wall used in the Listbox entry.
        /// </summary>
        readonly WallEntityBase _wall;

        /// <summary>
        /// ListboxWall constructor.
        /// </summary>
        /// <param name="wall">Wall used in the Listbox entry.</param>
        public ListboxWall(WallEntityBase wall)
        {
            _wall = wall;
        }

        /// <summary>
        /// Converts the WallEntity to a Listbox-friendly string.
        /// </summary>
        /// <returns>Listbox-friendly string for the WallEntity.</returns>
        public override string ToString()
        {
            return string.Format("({0},{1})-({2},{3})", _wall.CB.Min.X, _wall.CB.Min.Y, _wall.CB.Max.X, _wall.CB.Max.Y);
        }

        /// <summary>
        /// Implicitly converts from a ListboxWall to a WallEntity.
        /// </summary>
        /// <param name="lbw">ListboxWall to convert from.</param>
        /// <returns>WallEntity for the given ListboxWall.</returns>
        public static implicit operator WallEntityBase(ListboxWall lbw)
        {
            return lbw._wall;
        }
    }
}