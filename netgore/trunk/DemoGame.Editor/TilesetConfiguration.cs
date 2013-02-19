using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// Represents a configuration for a given tileset.
    /// </summary>
    public class TilesetConfiguration
    {

        /// <summary>
        /// The width of this given set
        /// </summary>
        public int Width { get; set; }

        public List<GrhData> GrhDatas { get; set; }


        /// <summary>
        /// Initialize a new instance of a <see cref="TilesetConfiguration"/>
        /// </summary>
        /// <param name="grhDatas"The GRHData that can be used by this tileset.></param>
        /// <param name="width"The width of this tileset. This is used to render the display properly.></param>
        public TilesetConfiguration(List<GrhData> grhDatas, int width )
        {
            Width = width;
            GrhDatas = grhDatas;
        }

    }
}
