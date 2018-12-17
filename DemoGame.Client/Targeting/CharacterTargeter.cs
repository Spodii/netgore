using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Graphics;
using SFML.Graphics;
using NetGore;

namespace DemoGame.Client{

    /// <summary>
    /// A <see cref="CharacterTargeter"/> used for targeting characters on a <see cref="Map"/>.
    /// </summary>
    public class CharacterTargeter : Targeter
    {

        readonly Color _targetColor = new Color(0, 255, 0, 255);

        Color _oldTargetColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTargeter"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <exception cref="ArgumentNullException"><paramref name="world" /> is <c>null</c>.</exception>
        public CharacterTargeter(World world) : base(world)
        {
        }

        /// <summary>
        /// Occurs immediately after the Target is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        protected override void Target_AfterDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
            sender.Color = _oldTargetColor;
        }

        /// <summary>
        /// Occurs immediately before the Target is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        protected override void Target_BeforeDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
            _oldTargetColor = sender.Color;
            sender.Color = _targetColor;
        }
    }
}
