using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    interface IMapControlledEntity
    {
        /// <summary>
        /// Sets the new map for the Entity. It is strongly recommended that every implementation
        /// of this method is explicitly defined to ensure only the Map is calling it.
        /// </summary>
        /// <param name="newMap">New map for for the Entity.</param>
        void SetMap(Map newMap);
    }
}