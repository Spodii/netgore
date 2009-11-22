using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.GUI
{
    public class PictureBox : SpriteControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBox"/> class.
        /// </summary>
        /// <param name="gui">The GUI.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="sprite">The sprite.</param>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="parent">The parent.</param>
        public PictureBox(GUIManagerBase gui, PictureBoxSettings settings, ISprite sprite, Vector2 position, Vector2 size,
                          Control parent) : base(gui, settings, position, sprite, size, parent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBox"/> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="parent">The parent.</param>
        public PictureBox(ISprite sprite, Vector2 position, Vector2 size, Control parent)
            : base(parent.GUIManager, parent.GUIManager.PictureBoxSettings, position, sprite, size, parent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBox"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="sprite">The sprite.</param>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="parent">The parent.</param>
        public PictureBox(PictureBoxSettings settings, ISprite sprite, Vector2 position, Vector2 size, Control parent)
            : base(parent.GUIManager, settings, position, sprite, size, parent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBox"/> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public PictureBox(ISprite sprite, Vector2 position, Control parent)
            : base(parent.GUIManager.PictureBoxSettings, position, sprite, parent)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureBox"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="position">The position.</param>
        /// <param name="sprite">The sprite.</param>
        /// <param name="parent">The parent.</param>
        public PictureBox(PictureBoxSettings settings, Vector2 position, ISprite sprite, Control parent)
            : base(settings, position, sprite, parent)
        {
        }
    }
}