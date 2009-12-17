using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;

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
        public GUIManager(SpriteFont font) : base(font, BlankGrh, "Default")
        {
        }

        /// <summary>
        /// Gets the blank Grh, ensuring it is loaded
        /// </summary>
        static Grh BlankGrh
        {
            get
            {
                if (_blankGrh == null)
                {
                    GrhData gd = GrhInfo.GetData("System", "Blank");
                    if (gd == null)
                        throw new GrhDataNotFoundException("System", "Blank");
                    _blankGrh = new Grh(gd);
                }

                return _blankGrh;
            }
        }
    }
}