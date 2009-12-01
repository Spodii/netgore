using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Creates a graphical body over a <see cref="Skeleton"/> using a <see cref="SkeletonBodyInfo"/> to define
    /// the body layout information. A new instance of this class is created for each <see cref="Skeleton"/>.
    /// </summary>
    public class SkeletonBody
    {
        /// <summary>
        /// SkeletonBodyItems that make up the SkeletonBody
        /// </summary>
        public SkeletonBodyItem[] BodyItems;

        /// <summary>
        /// SkeletonBodyInfo used to make up the body information
        /// </summary>
        readonly SkeletonBodyInfo _bodyInfo = null;

        /// <summary>
        /// Skeleton used to track the SkeletonBody
        /// </summary>
        Skeleton _skeleton = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonBody"/> class.
        /// </summary>
        /// <param name="bodyInfo"><see cref="SkeletonBodyInfo"/> to create the <see cref="SkeletonBody"/> from.</param>
        public SkeletonBody(SkeletonBodyInfo bodyInfo)
        {
            if (bodyInfo == null)
                throw new ArgumentNullException("bodyInfo");

            // Store the BodyInfo object reference
            _bodyInfo = bodyInfo;

            // Create the BodyItems array
            BodyItems = new SkeletonBodyItem[bodyInfo.Items.Length];

            // Create the individual BodyItems
            for (int i = 0; i < BodyItems.Length; i++)
            {
                BodyItems[i] = new SkeletonBodyItem(bodyInfo.Items[i]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonBody"/> class.
        /// </summary>
        /// <param name="bodyInfo"><see cref="SkeletonBodyInfo"/> to create the <see cref="SkeletonBody"/> from.</param>
        /// <param name="skeleton"><see cref="Skeleton"/> to attach to.</param>
        public SkeletonBody(SkeletonBodyInfo bodyInfo, Skeleton skeleton) : this(bodyInfo)
        {
            Attach(skeleton);
        }

        /// <summary>
        /// Gets the <see cref="SkeletonBodyInfo"/> that this <see cref="SkeletonBody"/> is created from.
        /// </summary>
        public SkeletonBodyInfo BodyInfo
        {
            get { return _bodyInfo; }
        }

        /// <summary>
        /// Gets the <see cref="Skeleton"/> that this <see cref="SkeletonBody"/> is attached to.
        /// </summary>
        public Skeleton Skeleton
        {
            get { return _skeleton; }
        }

        /// <summary>
        /// Attaches the body to a <see cref="Skeleton"/>.
        /// </summary>
        /// <param name="skeleton"><see cref="Skeleton"/> to attach to.</param>
        public void Attach(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                Debug.Fail("skeleton is null.");
                return;
            }

            _skeleton = skeleton;

            foreach (var bodyItem in BodyItems)
                bodyItem.Attach(skeleton);
        }

        /// <summary>
        /// Draws the SkeletonBody
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        /// <param name="position">Position to draw at</param>
        public void Draw(SpriteBatch sb, Vector2 position)
        {
            Draw(sb, position, 1.0f, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the SkeletonBody
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        /// <param name="position">Position to draw at</param>
        /// <param name="scale">Scale in percent (1.0f for normal scale)</param>
        public void Draw(SpriteBatch sb, Vector2 position, float scale)
        {
            Draw(sb, position, scale, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the SkeletonBody
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        /// <param name="position">Position to draw at</param>
        /// <param name="scale">Scale in percent (1.0f for normal scale)</param>
        /// <param name="effect">SpriteEffects to use when drawing</param>
        public void Draw(SpriteBatch sb, Vector2 position, float scale, SpriteEffects effect)
        {
            foreach (var bodyItem in BodyItems)
                bodyItem.Draw(sb, position, scale, effect);
        }

        /// <summary>
        /// Updates the SkeletonBody
        /// </summary>
        /// <param name="currentTime">Current time</param>
        public void Update(int currentTime)
        {
            foreach (var bodyItem in BodyItems)
                bodyItem.Update(currentTime);
        }
    }
}