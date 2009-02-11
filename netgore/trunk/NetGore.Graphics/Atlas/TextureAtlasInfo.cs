using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Graphics
{
    /// <summary>
    /// Information about a texture atlas
    /// </summary>
    public class TextureAtlasInfo
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly int _height;
        readonly Stack<AtlasNode> _nodes;
        readonly int _width;

        /// <summary>
        /// Gets the height of the texture atlas
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Gets a stack of each of the nodes in the atlas
        /// </summary>
        public Stack<AtlasNode> Nodes
        {
            get { return _nodes; }
        }

        /// <summary>
        /// Gets the width of the texture atlas
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// TextureAtlasInfo constructor
        /// </summary>
        /// <param name="nodes">Stack of each of the nodes in the atlas</param>
        /// <param name="width">Width of the texture atlas</param>
        /// <param name="height">Height of the texture atlas</param>
        public TextureAtlasInfo(Stack<AtlasNode> nodes, int width, int height)
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