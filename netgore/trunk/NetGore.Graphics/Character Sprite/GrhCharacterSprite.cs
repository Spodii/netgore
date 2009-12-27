using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A very basic, primitive, and restrictive implementation of <see cref="ICharacterSprite"/> that draws the
    /// character's sprite using the Grh system. This is only provided for demonstration purposes for top-down
    /// view, and is recommended you implement your own more powerful <see cref="ICharacterSprite"/> if you are
    /// not using skeleton sprites.
    /// </summary>
    public class BasicGrhCharacterSprite : ICharacterSprite
    {
        readonly Entity _character;
        readonly Grh _grh = new Grh(null);
        readonly SpriteCategory _rootCategory;

        string _bodyName = string.Empty;
        string _currentSet = string.Empty;
        int _currentTime;

        public BasicGrhCharacterSprite(Entity character, SpriteCategory rootCategory)
        {
            _character = character;
            _rootCategory = rootCategory;
        }

        void InternalSetSet(string setName)
        {
            if (_currentSet.Equals(setName, StringComparison.OrdinalIgnoreCase))
                return;

            _currentSet = setName;

            var grhData = GrhInfo.GetData(_rootCategory + SpriteCategorization.Delimiter + _bodyName, setName);
            if (grhData == null)
                return;

            _grh.SetGrh(grhData, AnimType.Loop, _currentTime);
        }

        #region ICharacterSprite Members

        /// <summary>
        /// Gets the character this <see cref="ICharacterSprite"/> is drawing the sprite for.
        /// </summary>
        public Entity Character
        {
            get { return _character; }
        }

        /// <summary>
        /// Adds a sprite body modifier that alters some, but not all, of the body. <see cref="ICharacterSprite"/>s
        /// that do not support dynamic sprites treat this the same as <see cref="ICharacterSprite.SetBody"/>.
        /// </summary>
        /// <param name="bodyModifierName">The name of the sprite body modifier.</param>
        public void AddBodyModifier(string bodyModifierName)
        {
            SetBody(bodyModifierName);
        }

        /// <summary>
        /// Draws the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw with.</param>
        /// <param name="position">The position to draw the sprite.</param>
        /// <param name="heading">The character's heading.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction heading)
        {
            string prefix = (Character.Velocity == Vector2.Zero ? string.Empty : "Walk ");

            switch (heading)
            {
                case Direction.North:
                case Direction.NorthWest:
                case Direction.NorthEast:
                    InternalSetSet(prefix + "Up");
                    break;

                case Direction.South:
                case Direction.SouthWest:
                case Direction.SouthEast:
                    InternalSetSet(prefix + "Down");
                    break;

                case Direction.East:
                    InternalSetSet(prefix + "Right");
                    break;

                case Direction.West:
                    InternalSetSet(prefix + "Left");
                    break;
            }

            if (_grh.GrhData == null)
                return;

            _grh.Update(_currentTime);
            _grh.Draw(spriteBatch, position);
        }

        /// <summary>
        /// Sets the sprite's body, which describes the components to use to draw a Set.
        /// </summary>
        /// <param name="bodyName">The name of the sprite body.</param>
        public void SetBody(string bodyName)
        {
            if (_bodyName.Equals(bodyName, StringComparison.OrdinalIgnoreCase))
                return;

            _bodyName = bodyName;
            _currentSet = string.Empty;
        }

        /// <summary>
        /// Sets the sprite's paper doll layers.
        /// </summary>
        /// <param name="layers">The name of the paper doll layers.</param>
        public void SetPaperDollLayers(IEnumerable<string> layers)
        {
        }

        /// <summary>
        /// Sets the Set that describes how the sprite is laid out.
        /// </summary>
        /// <param name="setName">The name of the Set.</param>
        /// <param name="bodySize">The size of the body.</param>
        public void SetSet(string setName, Vector2 bodySize)
        {
        }

        /// <summary>
        /// Updates the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public void Update(int currentTime)
        {
            _currentTime = currentTime;
        }

        #endregion
    }
}