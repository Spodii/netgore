using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// The different ways that a <see cref="ControlBorder"/> can be drawn.
    /// </summary>
    public enum ControlBorderDrawStyle : byte
    {
        /// <summary>
        /// The sprite is stretched to fit the target, resulting in only one draw call.
        /// </summary>
        Stretch,

        /// <summary>
        /// The sprite is drawn repeatedly at the original size to fit the target. The sprite is never stretched.
        /// Can result in multiple draw calls.
        /// </summary>
        Tile
    }
}