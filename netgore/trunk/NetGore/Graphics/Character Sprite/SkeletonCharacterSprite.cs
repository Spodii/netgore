using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Creates and manages a sprite for a Character that uses the skeletonal animation system.
    /// </summary>
    public class SkeletonCharacterSprite : ICharacterSprite, IGetTime
    {
        readonly Entity _character;
        readonly IGetTime _getTime;
        readonly SkeletonManager _skelManager;
        readonly float _speedModifier;

        Vector2 _bodySize;
        string _currSkelSet = string.Empty;
        SkeletonAnimation _skelAnim;
        string[] _paperdollLayers;
        bool _paperdoll;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonCharacterSprite"/> class.
        /// </summary>
        /// <param name="getTime">The interface used to get the current time.</param>
        /// <param name="character">The character that this sprite is for.</param>
        /// <param name="skeletonManager">The skeleton manager.</param>
        /// <param name="speedModifier">The animation speed modifier.</param>
        public SkeletonCharacterSprite(IGetTime getTime, Entity character, SkeletonManager skeletonManager, float speedModifier)
        {
            _character = character;
            _skelManager = skeletonManager;
            _speedModifier = speedModifier;
            _getTime = getTime;
        }

        #region ICharacterSprite Members

        /// <summary>
        /// Gets or sets if paperdolling is enabled.
        /// </summary>
        public bool Paperdoll
        {
            get { return _paperdoll; }
            set
            {
                if (_paperdoll == value)
                    return;

                _paperdoll = value;

                // Update paperdoll layers
                SetPaperDollLayers(_paperdollLayers);
            }
        }

        /// <summary>
        /// Gets the character this <see cref="ICharacterSprite"/> is drawing the sprite for.
        /// </summary>
        public Entity Character
        {
            get { return _character; }
        }

        public Vector2 SpriteSize
        {
            get { return _bodySize;  }
        }

        /// <summary>
        /// Adds a sprite body modifier that alters some, but not all, of the body. <see cref="ICharacterSprite"/>s
        /// that do not support dynamic sprites treat this the same as <see cref="ICharacterSprite.SetBody"/>.
        /// </summary>
        /// <param name="bodyModifierName">The name of the sprite body modifier.</param>
        public void AddBodyModifier(string bodyModifierName)
        {
            var set = _skelManager.GetSet(bodyModifierName);
            set = SkeletonAnimation.CreateSmoothedSet(set, _skelAnim.Skeleton);
            var mod = new SkeletonAnimation(GetTime(), set);
            _skelAnim.AddModifier(mod);
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
            if (_skelAnim == null)
                return;

            var se = (heading == Direction.East ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            var p = position + new Vector2(_bodySize.X / 2f, _bodySize.Y);

            _skelAnim.Draw(spriteBatch, p, color, se);
        }

        /// <summary>
        /// Sets the sprite's body.
        /// </summary>
        /// <param name="bodyName">The name of the sprite body.</param>
        public void SetBody(string bodyName)
        {
            var bodyInfo = _skelManager.GetBodyInfo(bodyName);
            _skelAnim.SkeletonBody = new SkeletonBody(bodyInfo, _skelAnim.Skeleton);
        }

        /// <summary>
        /// Sets the sprite's paper doll layers. This will set all of the layers at once. Layers that are not in the
        /// <paramref name="layers"/> collection should be treated as they are not used and be removed, not be treated
        /// as they are just not updating.
        /// </summary>
        /// <param name="layers">The name of the paper doll layers.</param>
        public void SetPaperDollLayers(IEnumerable<string> layers)
        {
            _paperdollLayers = layers.ToArray();

            _skelAnim.BodyLayers.Clear();

            if (layers == null)
                return;

            if (Paperdoll)
            {
                foreach (var layer in layers)
                {
                    var bodyInfo = _skelManager.GetBodyInfo(layer);
                    if (bodyInfo == null)
                        continue;

                    _skelAnim.BodyLayers.Add(new SkeletonBody(bodyInfo, _skelAnim.Skeleton));
                }
            }
        }

        /// <summary>
        /// Sets the Set that describes how the sprite is laid out.
        /// </summary>
        /// <param name="setName">The name of the Set.</param>
        /// <param name="bodySize">The size of the body.</param>
        public void SetSet(string setName, Vector2 bodySize)
        {
            // Check that the set has changed
            if (setName == _currSkelSet)
                return;

            _bodySize = bodySize;

            var newSet = _skelManager.GetSet(setName);

            if (_skelAnim == null)
                _skelAnim = new SkeletonAnimation(GetTime(), newSet);

            _skelAnim.ChangeSet(newSet);
            _currSkelSet = setName;
        }

        /// <summary>
        /// Updates the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public void Update(TickCount currentTime)
        {
            if (_skelAnim == null)
                return;

            _skelAnim.Update(currentTime);

            // Update the animation's speed
            if (Character.Velocity.X != 0)
                _skelAnim.Speed = Math.Abs(Character.Velocity.X) / _speedModifier;
            else
                _skelAnim.Speed = 1.0f;
        }

        #endregion

        #region IGetTime Members

        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public TickCount GetTime()
        {
            return _getTime.GetTime();
        }

        #endregion
    }
}