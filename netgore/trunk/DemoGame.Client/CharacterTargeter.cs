using System;
using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using IDrawable=NetGore.Graphics.IDrawable;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages targeting a <see cref="Character"/>.
    /// </summary>
    public class CharacterTargeter
    {
        readonly IDrawableDrawEventHandler _mouseOverAfterDrawHandler;
        readonly IDrawableDrawEventHandler _mouseOverBeforeDrawHandler;
        readonly Color _mouseOverColor = new Color(150, 255, 150, 255);
        readonly IDrawableDrawEventHandler _targetAfterDrawHandler;
        readonly IDrawableDrawEventHandler _targetBeforeDrawHandler;
        readonly Color _targetColor = new Color(0, 255, 0, 255);
        readonly World _world;

        Character _mouseOverChar;
        Color _oldMouseOverColor;
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

        void MouseOverCharacter_AfterDraw(IDrawable sender, ISpriteBatch sb)
        {
            if (sender == TargetCharacter)
                return;

            sender.Color = _oldMouseOverColor;
        }

        void MouseOverCharacter_BeforeDraw(IDrawable sender, ISpriteBatch sb)
        {
            if (sender == TargetCharacter)
                return;

            _oldMouseOverColor = sender.Color;
            sender.Color = _mouseOverColor;
        }

        void TargetCharacter_AfterDraw(IDrawable sender, ISpriteBatch sb)
        {
            sender.Color = _oldTargetColor;
        }

        void TargetCharacter_BeforeDraw(IDrawable sender, ISpriteBatch sb)
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

            MouseOverCharacter = World.Map.Spatial.Get<Character>(World.Camera.Min + cursorPos, x => x != World.UserChar);

            if (MouseOverCharacter != null && gui.IsMouseButtonDown(SFML.Window.MouseButton.Left))
                TargetCharacter = MouseOverCharacter;

            if (MouseOverCharacter != null && MouseOverCharacter.IsDisposed)
                MouseOverCharacter = null;

            if (TargetCharacter != null && TargetCharacter.IsDisposed)
                TargetCharacter = null;
        }

        void World_MapChanged(World world, Map oldMap, Map newMap)
        {
            MouseOverCharacter = null;
            TargetCharacter = null;
        }
    }
}