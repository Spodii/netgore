using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages the GUI
    /// </summary>
    public class GUIManager : GUIManagerBase
    {
        /// <summary>
        /// Static instance of the System.Blank GrhData used as the default blank sprite
        /// </summary>
        static Grh _blankGrh = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GUIManager"/> class.
        /// </summary>
        /// <param name="font">The default <see cref="SpriteFont"/> to use on added <see cref="Control"/>s.</param>
        public GUIManager(SpriteFont font) : base(font, "Default")
        {
        }
    }
}