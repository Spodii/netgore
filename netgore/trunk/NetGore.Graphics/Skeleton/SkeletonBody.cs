using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Creates a body over a skeleton
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
        /// Gets the SkeletonBodyInfo that this SkeletonBody is created from
        /// </summary>
        public SkeletonBodyInfo BodyInfo
        {
            get { return _bodyInfo; }
        }

        /// <summary>
        /// Gets the Skeleton that this SkeletonBody is attached to
        /// </summary>
        public Skeleton Skeleton
        {
            get { return _skeleton; }
        }

        /// <summary>
        /// SkeletonBody constructor
        /// </summary>
        /// <param name="bodyInfo">SkeletonBodyInfo to create the SkeletonBody from</param>
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
        /// SkeletonBody constructor
        /// </summary>
        /// <param name="bodyInfo">SkeletonBodyInfo to create the SkeletonBody from</param>
        /// <param name="skeleton">Skeleton to attach to</param>
        public SkeletonBody(SkeletonBodyInfo bodyInfo, Skeleton skeleton) : this(bodyInfo)
        {
            Attach(skeleton);
        }

        /// <summary>
        /// Attaches the body to a skeleton
        /// </summary>
        /// <param name="skeleton">Skeleton to attach to</param>
        public void Attach(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                Debug.Fail("skeleton is null.");
                return;
            }

            _skeleton = skeleton;
            foreach (SkeletonBodyItem sbi in BodyItems)
            {
                sbi.Attach(skeleton);
            }
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
            if (sb == null)
            {
                Debug.Fail("sb is null.");
                return;
            }
            if (sb.IsDisposed)
            {
                Debug.Fail("sb is disposed.");
                return;
            }

            for (int i = BodyItems.Length - 1; i >= 0; i--)
            {
                BodyItems[i].Draw(sb, position, scale, effect);
            }
        }

        /// <summary>
        /// Updates the SkeletonBody
        /// </summary>
        /// <param name="currentTime">Current time</param>
        public void Update(int currentTime)
        {
            for (int i = BodyItems.Length - 1; i >= 0; i--)
            {
                BodyItems[i].Update(currentTime);
            }
        }
    }
}