using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;
using NetGore.Graphics;
using NetGore.World;

namespace DemoGame.Client
{
    /// <summary>
    /// The <see cref="IDrawableTarget"/> defines an interface for an entity that is Targetable using a <see cref="Targeter"/>.
    /// </summary>
    public interface IDrawableTarget : IDrawable
    {
        /// <summary>
        /// The <see cref="MapEntityIndex"/> of this object.
        /// </summary>
        MapEntityIndex MapEntityIndex { get; }

        /// <summary>
        /// gets if this object is disposing.
        /// </summary>
        bool IsDisposed {get;}
    }
}
