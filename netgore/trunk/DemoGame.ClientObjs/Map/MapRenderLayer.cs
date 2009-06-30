using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Client
{
    /// <summary>
    /// Enum consisting of the layer at which map entities are rendered
    /// </summary>
    public enum MapRenderLayer
    {
        /// <summary>
        /// Back-most layer
        /// </summary>
        Background,

        /// <summary>
        /// Character layer
        /// </summary>
        Chararacter,

        /// <summary>
        /// Item layer
        /// </summary>
        Item,

        /// <summary>
        /// Foreground layer, in front of characters and items
        /// </summary>
        Foreground
    }
}