﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Handles when an <see cref="ISpatial"/> has moved.
    /// </summary>
    /// <param name="sender">The <see cref="ISpatial"/> that moved.</param>
    /// <param name="oldPosition">The position of the <see cref="ISpatial"/> before it moved.</param>
    public delegate void SpatialMoveEventHandler(ISpatial sender, Vector2 oldPosition);

    /// <summary>
    /// Handles when an <see cref="ISpatial"/> has been resized.
    /// </summary>
    /// <param name="sender">The <see cref="ISpatial"/> that was resized.</param>
    /// <param name="oldSize">The size of the <see cref="ISpatial"/> before it was resized.</param>
    public delegate void SpatialResizeEventHandler(ISpatial sender, Vector2 oldSize);

    /// <summary>
    /// Interface for an object that occupies space in the world.
    /// </summary>
    public interface ISpatial
    {
        /// <summary>
        /// Gets the <see cref="CollisionBox"/> used to determine the location of the object in the world.
        /// </summary>
        CollisionBox CB { get; }

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        event SpatialMoveEventHandler OnMove;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        event SpatialResizeEventHandler OnResize;
    }
}
