using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Manager for <see cref="ChatBubble"/>s.
    /// </summary>
    public sealed class ChatBubbleManager : ChatBubbleManagerBase
    {
        /// <summary>
        /// Name to use for grabbing the <see cref="ControlBorder"/> in the <see cref="ISkinManager"/>.
        /// </summary>
        const string _controlSkinName = "Chat Bubble";

        readonly ISkinManager _skinManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatBubbleManagerBase"/> class.
        /// </summary>
        /// <param name="skinManager">The skin manager.</param>
        /// <param name="font">The <see cref="Font"/> to use to write the bubble's text.</param>
        public ChatBubbleManager(ISkinManager skinManager, Font font) : base(skinManager.GetBorder(_controlSkinName), font)
        {
            _skinManager = skinManager;
            _skinManager.SkinChanged += skinManager_SkinChanged;
        }

        /// <summary>
        /// When overridden in the derived class, gets the
        /// </summary>
        /// <param name="target">The <see cref="Entity"/> that the <see cref="ChatBubble"/> will be draw at.</param>
        /// <param name="bubbleSize">The size of the bubble.</param>
        /// <returns>The coordinates of the top-left corner to draw the <see cref="ChatBubble"/> at.</returns>
        public override Vector2 GetDrawOffset(Entity target, Vector2 bubbleSize)
        {
            // Get the top-left corner
            var p = GetTopLeftDrawCorner(target);

            // Move to the center of the Character
            p.X += target.Size.X / 2f;
            p -= bubbleSize / 2f;

            // Move up a little to avoid covering their name
            p.Y -= 20f;

            return p.Floor();
        }

        /// <summary>
        /// Gets the top-left corner to use for drawing for the given <paramref name="target"/>.
        /// </summary>
        /// <param name="target">The <see cref="ISpatial"/> to attach the bubble to.</param>
        /// <returns>The coordinate of the top-left corner of the <paramref name="target"/> to use for drawing.</returns>
        static Vector2 GetTopLeftDrawCorner(ISpatial target)
        {
            Character asCharacter;

            // Make use of the Character's DrawPosition, otherwise it will look like the bubble is moving all over
            // the place since Characters like to interpolate all over the place
            if ((asCharacter = target as Character) != null)
                return asCharacter.DrawPosition;
            else
                return target.Position;
        }

        /// <summary>
        /// Handles updating the <see cref="ChatBubbleManagerBase.Border"/> when the skin changes.
        /// </summary>
        /// <param name="newSkinName">New name of the skin.</param>
        /// <param name="oldSkinName">Old name of the skin.</param>
        void skinManager_SkinChanged(string newSkinName, string oldSkinName)
        {
            Border = _skinManager.GetBorder(_controlSkinName);
        }
    }
}