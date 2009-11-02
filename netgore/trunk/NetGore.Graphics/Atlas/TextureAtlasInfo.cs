using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;

namespace NetGore.Graphics
{
    /// <summary>
    /// Contains information about a texture atlas.
    /// </summary>
    class TextureAtlasInfo
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly int _height;
        readonly IEnumerable<AtlasNode> _nodes;
        readonly int _width;

        /// <summary>
        /// Gets the height of the atlas texture.
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Gets the <see cref="AtlasNode"/>s that are in this atlas texture.
        /// </summary>
        public IEnumerable<AtlasNode> Nodes
        {
            get { return _nodes; }
        }

        /// <summary>
        /// Gets the width of the atlas texture.
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlasInfo"/> class.
        /// </summary>
        /// <param name="nodes">IEnumerable of each of the nodes in the atlas.</param>
        /// <param name="width">Width of the atlas texture.</param>
        /// <param name="height">Height of the atlas texture.</param>
        public TextureAtlasInfo(IEnumerable<AtlasNode> nodes, int width, int height)
        {
            if (nodes == null)
            {
                if (log.IsFatalEnabled)
                    log.Fatal("argument a is null.");
                throw new ArgumentNullException("nodes");
            }

            _nodes = nodes;
            _width = width;
            _height = height;
        }
    }
}