using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class Label : TextControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="parent">Parent Control of this Control. Cannot be null.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        public Label(Control parent, Vector2 position) : base(parent, position, Vector2.One)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="gui">The <see cref="GUIManagerBase"/> this Control will be part of. Cannot be null.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        public Label(GUIManagerBase gui, Vector2 position) : base(gui, position, Vector2.One)
        {
        }

        /// <summary>
        /// Gets or sets if the <see cref="Label"/> will automatically resize when the text or font changes.
        /// </summary>
        public bool AutoResize { get; set; }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.OnChangeFont"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.OnChangeFont"/> when possible.
        /// </summary>
        protected override void ChangeFont()
        {
            base.ChangeFont();

            ResizeToFitText();
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Text"/> has changed.
        /// This is called immediately before <see cref="TextControl.OnChangeText"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.OnChangeText"/> when possible.
        /// </summary>
        protected override void ChangeText()
        {
            base.ChangeText();

            ResizeToFitText();
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            DrawText(spriteBatch, new Vector2(Border.LeftWidth, Border.TopHeight));
        }

        /// <summary>
        /// Resizes the <see cref="Label"/> so it is large enough to fit the <see cref="TextControl.Text"/>.
        /// </summary>
        void ResizeToFitText()
        {
            Size = Border.Size + Font.MeasureString(Text);
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Border = GUIManager.LabelSettings.Border;
            CanDrag = false;
            CanFocus = false;
            AutoResize = true;
        }
    }
}