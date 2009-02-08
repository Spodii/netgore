using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Platyform;
using Platyform.Extensions;

namespace DemoGame.Client
{
    /// <summary>
    /// A wall with the ability to draw its CollisionBox
    /// </summary>
    public class Wall : WallEntity
    {
        /// <summary>
        /// Wall constructor
        /// </summary>
        public Wall(Vector2 position, Vector2 size) : base(position, size)
        {
            Weight = 0.0f; // Walls have no weight
        }

        /// <summary>
        /// Wall constructor
        /// </summary>
        public Wall()
        {
        }
    }
}