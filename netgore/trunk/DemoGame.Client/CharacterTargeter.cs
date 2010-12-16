using System;
using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.World;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages targeting a <see cref="Character"/>.
    /// </summary>
    public class CharacterTargeter
    {
        readonly TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> _mouseOverAfterDrawHandler;
        readonly TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> _mouseOverBeforeDrawHandler;
        readonly TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> _targetAfterDrawHandler;
        readonly TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> _targetBeforeDrawHandler;
        readonly Color _targetColor = new Color(0, 255, 0, 255);
        readonly World _world;

        Character _mouseOverChar;
        Color _oldTargetColor;
        Character _targetChar;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTargeter"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        public CharacterTargeter(World world)
        {
            if (world == null)
                throw new ArgumentNullException("world");

            _world = world;

            _mouseOverBeforeDrawHandler = MouseOverCharacter_BeforeDraw;
            _mouseOverAfterDrawHandler = MouseOverCharacter_AfterDraw;
            _targetBeforeDrawHandler = TargetCharacter_BeforeDraw;
            _targetAfterDrawHandler = TargetCharacter_AfterDraw;

            World.MapChanged += World_MapChanged;
        }

        /// <summary>
        /// Gets the <see cref="Character"/> the cursor is currently over.
        /// </summary>
        public Character MouseOverCharacter
        {
            get { return _mouseOverChar; }
            private set
            {
                if (_mouseOverChar == value)
                    return;

                if (_mouseOverChar != null)
                {
                    _mouseOverChar.BeforeDraw -= _mouseOverBeforeDrawHandler;
                    _mouseOverChar.AfterDraw -= _mouseOverAfterDrawHandler;
                }

                _mouseOverChar = value;

                if (_mouseOverChar != null)
                {
                    _mouseOverChar.BeforeDraw += _mouseOverBeforeDrawHandler;
                    _mouseOverChar.AfterDraw += _mouseOverAfterDrawHandler;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="MapEntityIndex"/> of the <see cref="CharacterTargeter.MouseOverCharacter"/>.
        /// </summary>
        public MapEntityIndex? MouseOverCharacterIndex
        {
            get
            {
                if (MouseOverCharacter == null)
                    return null;

                return MouseOverCharacter.MapEntityIndex;
            }
        }

        /// <summary>
        /// Gets the <see cref="Character"/> that is currently being targeted.
        /// </summary>
        public Character TargetCharacter
        {
            get { return _targetChar; }
            private set
            {
                if (_targetChar == value)
                    return;

                if (_targetChar != null)
                {
                    _targetChar.BeforeDraw -= _targetBeforeDrawHandler;
                    _targetChar.AfterDraw -= _targetAfterDrawHandler;
                }

                _targetChar = value;

                if (_targetChar != null)
                {
                    _targetChar.BeforeDraw += _targetBeforeDrawHandler;
                    _targetChar.AfterDraw += _targetAfterDrawHandler;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="MapEntityIndex"/> of the <see cref="CharacterTargeter.TargetCharacter"/>.
        /// </summary>
        public MapEntityIndex? TargetCharacterIndex
        {
            get
            {
                if (TargetCharacter == null)
                    return null;

                return TargetCharacter.MapEntityIndex;
            }
        }

        /// <summary>
        /// Gets the <see cref="World"/>.
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        /// <summary>
        /// Occurs immediately after the <see cref="MouseOverCharacter"/> is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        void MouseOverCharacter_AfterDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
        }

        /// <summary>
        /// Occurs immediately before the <see cref="MouseOverCharacter"/> is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        void MouseOverCharacter_BeforeDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
        }

        /// <summary>
        /// Occurs immediately after the <see cref="TargetCharacter"/> is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        void TargetCharacter_AfterDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
            sender.Color = _oldTargetColor;
        }

        /// <summary>
        /// Occurs immediately before the <see cref="TargetCharacter"/> is drawn.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISpriteBatch}"/> instance containing the event data.</param>
        void TargetCharacter_BeforeDraw(IDrawable sender, EventArgs<ISpriteBatch> e)
        {
            _oldTargetColor = sender.Color;
            sender.Color = _targetColor;
        }

        /// <summary>
        /// Updates the targeting.
        /// </summary>
        /// <param name="gui">The <see cref="IGUIManager"/>.</param>
        public void Update(IGUIManager gui)
        {
            var cursorPos = gui.CursorPosition;

            // Get the character under the cursor
            MouseOverCharacter = World.Map.Spatial.Get<Character>(World.Camera.ToWorld(cursorPos));

            // Update the target character when the left mouse button is down
            if (gui.IsMouseButtonDown(MouseButton.Left))
            {
                if (MouseOverCharacter != null)
                {
                    if (MouseOverCharacter == World.UserChar)
                    {
                        // If the target character is the user's character, remove the targeting
                        TargetCharacter = null;
                    }
                    else
                    {
                        // Otherwise, set the target character
                        TargetCharacter = MouseOverCharacter;
                    }
                }
                else
                {
                    // No target
                    TargetCharacter = null;
                }
            }

            // If the MouseOverCharacter or TargetCharacter have been disposed, unset them
            if (MouseOverCharacter != null && MouseOverCharacter.IsDisposed)
                MouseOverCharacter = null;

            if (TargetCharacter != null && TargetCharacter.IsDisposed)
                TargetCharacter = null;
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs{Map}"/> instance containing the event data.</param>
        void World_MapChanged(World sender, ValueChangedEventArgs<Map> e)
        {
            MouseOverCharacter = null;
            TargetCharacter = null;
        }
    }
}