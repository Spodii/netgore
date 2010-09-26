using System.Linq;
using NetGore.Graphics.GUI;
using NetGore.World;

namespace DemoGame.Client
{
    /// <summary>
    /// A custom implementation of the <see cref="ChatBubble"/>.
    /// </summary>
    public class GameChatBubble : ChatBubble
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameChatBubble"/> class.
        /// </summary>
        /// <param name="parent">The parent <see cref="Control"/>.</param>
        /// <param name="owner">The <see cref="Entity"/> that this <see cref="ChatBubble"/> is for.</param>
        /// <param name="text">The text to display.</param>
        public GameChatBubble(Control parent, Entity owner, string text) : base(parent, owner, text)
        {
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Font = GameScreenHelper.DefaultChatFont;
        }
    }
}