using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class Form : TextControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="parent">Parent Control of this Control. Cannot be null.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="size">Size of the Control.</param>
        public Form(Control parent, Vector2 position, Vector2 size) : base(parent, position, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="gui">The <see cref="GUIManagerBase"/> this Control will be part of. Cannot be null.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="size">Size of the Control.</param>
        public Form(GUIManagerBase gui, Vector2 position, Vector2 size) : base(gui, position, size)
        {
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Border = GUIManager.FormSettings.Border;
            CanDrag = true;
            ForeColor = GUIManager.FormSettings.ForeColor;
        }

        /// <summary>
        /// Draws the Control.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            DrawText(spriteBatch, new Vector2(3, 3));
        }
    }
}