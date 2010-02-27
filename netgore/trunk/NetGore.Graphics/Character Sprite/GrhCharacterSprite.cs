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

        /// <summary>
        /// The <see cref="Direction"/> the character was facing when the body modifier was set.
        /// </summary>
        Direction _bodyModifierDirection;

        /// <summary>
        /// The current body. This joins with the root category to create the sprite category to grab the sprites from.
        /// </summary>
        string _bodyName = string.Empty;

        /// <summary>
        /// The current modifier, which will animate once then revert back to the <see cref="_currentSet"/>.
        /// </summary>
        string _currentBodyModifier;

        /// <summary>
        /// The current direction of the character.
        /// </summary>
        Direction _currentHeading;

        /// <summary>
        /// The current set. This joins with the body name as the sprite title.
        /// </summary>
        string _currentSet = string.Empty;

        int _currentTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicGrhCharacterSprite"/> class.
        /// </summary>
        /// <param name="character">The character this <see cref="BasicGrhCharacterSprite"/> is for.</param>
        /// <param name="rootCategory">The root category for the character sprites.</param>
        public BasicGrhCharacterSprite(Entity character, SpriteCategory rootCategory)
        {
            _character = character;
            _rootCategory = rootCategory;
        }

        /// <summary>
        /// Gets the name used for the set for a given <paramref name="direction"/>.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns>The name used for the set for a given <paramref name="direction"/>.</returns>
        static string GetDirectionSetName(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                case Direction.NorthWest:
                case Direction.NorthEast:
                    return "Up";

                case Direction.South:
                case Direction.SouthWest:
                case Direction.SouthEast:
                    return "Down";

                case Direction.East:
                    return "Right";

                case Direction.West:
                    return "Left";
            }

            throw new ArgumentOutOfRangeException("direction");
        }

        /// <summary>
        /// Gets the <see cref="GrhData"/> for a set.
        /// </summary>
        /// <param name="bodyName">The name of the body (determines the category to use).</param>
        /// <param name="setName">The name of the set (the sprite categorization title).</param>
        /// <returns>The <see cref="GrhData"/> for the given body and set, or null if not found.</returns>
        GrhData GetSetGrhData(string bodyName, string setName)
        {
            return GrhInfo.GetData(_rootCategory + SpriteCategorization.Delimiter + bodyName, setName);
        }

        /// <summary>
        /// Sets the primary set, which in this case is just the sprite's title.
        /// </summary>
        /// <param name="setName">The name of the set.</param>
        void InternalSetSet(string setName)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(_currentSet, setName))
                return;

            _currentSet = setName;

            var grhData = GetSetGrhData(_bodyName, setName);
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
            if (StringComparer.OrdinalIgnoreCase.Equals(_currentBodyModifier, bodyModifierName))
                return;

            // Update the sprite
            var grhData = GetSetGrhData(_bodyName, bodyModifierName + " " + GetDirectionSetName(_currentHeading));
            if (grhData == null)
                return;

            // Set the new modifier
            _currentBodyModifier = bodyModifierName;
            _bodyModifierDirection = _currentHeading;

            // Set the animation to loop once
            _grh.SetGrh(grhData, AnimType.LoopOnce, _currentTime);
        }

        /// <summary>
        /// Draws the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="position">The position to draw the sprite.</param>
        /// <param name="heading">The character's heading.</param>
        /// <param name="color">The color of the sprite.</param>
        public void Draw(ISpriteBatch spriteBatch, Vector2 position, Direction heading, Color color)
        {
            // If we have a body modifier being used, invalidate it if:
            // 1. The heading has changed.
            // 2. The animation has ended.
            //
            // If we don't have a body modifier being used, just ensure we have the correct Set being used.
            //
            // If we are moving, always use the walking animation.

            _currentHeading = heading;

            // If the body modifier is set, check if it needs to be unset
            if (_currentBodyModifier != null)
            {
                if (_grh.AnimType == AnimType.None || _bodyModifierDirection != heading)
                    _currentBodyModifier = null;
            }

            // If we are moving, the body modifier is not set, or the sprite is invalid, use the non-modifier set
            if (Character.Velocity != Vector2.Zero || _currentBodyModifier == null || _grh.GrhData == null)
            {
                string prefix = (Character.Velocity == Vector2.Zero ? string.Empty : "Walk ");
                string directionSuffix = GetDirectionSetName(heading);

                _currentBodyModifier = null;
                InternalSetSet(prefix + directionSuffix);
            }

            // Ensure the sprite is valid before trying to update and draw it
            if (_grh.GrhData == null)
                return;

            // Update and draw the sprite
            _grh.Update(_currentTime);
            _grh.Draw(spriteBatch, position, color);
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
        /// Sets the sprite's paper doll layers. This will set all of the layers at once. Layers that are not in the
        /// <paramref name="layers"/> collection should be treated as they are not used and be removed, not be treated
        /// as they are just not updating.
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