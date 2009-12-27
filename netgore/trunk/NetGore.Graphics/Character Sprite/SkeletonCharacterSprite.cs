using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A Character sprite that uses the skeleton system.
    /// </summary>
    public class SkeletonCharacterSprite : ICharacterSprite
    {
        readonly Entity _character;
        readonly SkeletonManager _skelManager;
        readonly float _speedModifier;
        Vector2 _bodySize;
        string _currSkelSet = string.Empty;
        SkeletonAnimation _skelAnim;

        public SkeletonCharacterSprite(Entity character, SkeletonManager skeletonManager, float speedModifier)
        {
            _character = character;
            _skelManager = skeletonManager;
            _speedModifier = speedModifier;
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
        /// Sets the sprite's paper doll layers.
        /// </summary>
        /// <param name="layers">The name of the paper doll layers.</param>
        public void SetPaperDollLayers(IEnumerable<string> layers)
        {
            _skelAnim.BodyLayers.Clear();
            foreach (var layer in layers)
            {
                var bodyInfo = _skelManager.LoadBodyInfo(layer, ContentPaths.Build);
                if (bodyInfo == null)
                    continue;

                _skelAnim.BodyLayers.Add(new SkeletonBody(bodyInfo, _skelAnim.Skeleton));
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

            SkeletonSet newSet = _skelManager.LoadSet(setName, ContentPaths.Build);

            if (_skelAnim == null)
                _skelAnim = new SkeletonAnimation(0, newSet); // TODO: Get correct time

            _skelAnim.ChangeSet(newSet);
            _currSkelSet = setName;
        }

        /// <summary>
        /// Sets the sprite's body.
        /// </summary>
        /// <param name="bodyName">The name of the sprite body.</param>
        public void SetBody(string bodyName)
        {
            SkeletonBodyInfo bodyInfo = _skelManager.LoadBodyInfo(bodyName, ContentPaths.Build);
            _skelAnim.SkeletonBody = new SkeletonBody(bodyInfo, _skelAnim.Skeleton);
        }

        /// <summary>
        /// Adds a sprite body modifier that alters some, but not all, of the body. <see cref="ICharacterSprite"/>s
        /// that do not support dynamic sprites treat this the same as <see cref="ICharacterSprite.SetBody"/>.
        /// </summary>
        /// <param name="bodyModifierName">The name of the sprite body modifier.</param>
        public void AddBodyModifier(string bodyModifierName)
        {
            SkeletonSet set = _skelManager.LoadSet(bodyModifierName, ContentPaths.Build);
            set = SkeletonAnimation.CreateSmoothedSet(set, _skelAnim.Skeleton);
            SkeletonAnimation mod = new SkeletonAnimation(0, set); // TODO: Get real time
            _skelAnim.AddModifier(mod);
        }

        /// <summary>
        /// Updates the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public void Update(int currentTime)
        {
            _skelAnim.Update(currentTime);

            // Update the animation's speed
            if (Character.Velocity.X != 0)
                _skelAnim.Speed = Math.Abs(Character.Velocity.X) / _speedModifier;
            else
                _skelAnim.Speed = 1.0f;
        }

        /// <summary>
        /// Draws the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw with.</param>
        /// <param name="position">The position to draw the sprite.</param>
        /// <param name="heading">The character's heading.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Direction heading)
        {
            SpriteEffects se = (heading == Direction.East ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            Vector2 p = position + new Vector2(_bodySize.X / 2f, _bodySize.Y);
            _skelAnim.Draw(spriteBatch, p, se);
        }

        #endregion
    }
}