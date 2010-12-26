using System;
using System.Diagnostics;
using System.Linq;
using SFML.Graphics;

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
        /// <exception cref="ArgumentNullException"><paramref name="bodyInfo" /> is <c>null</c>.</exception>
        public SkeletonBody(SkeletonBodyInfo bodyInfo)
        {
            if (bodyInfo == null)
                throw new ArgumentNullException("bodyInfo");

            // Store the BodyInfo object reference
            _bodyInfo = bodyInfo;

            // Create the BodyItems array
            BodyItems = new SkeletonBodyItem[bodyInfo.Items.Length];

            // Create the individual BodyItems
            for (var i = 0; i < BodyItems.Length; i++)
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
            {
                bodyItem.Attach(skeleton);
            }
        }

        /// <summary>
        /// Draws the <see cref="SkeletonBody"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="position">Position to draw at.</param>
        public void Draw(ISpriteBatch sb, Vector2 position)
        {
            Draw(sb, position, Color.White, 1.0f, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the <see cref="SkeletonBody"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="position">Position to draw at.</param>
        /// <param name="color">The color.</param>
        /// <param name="scale">Scale in percent (1.0f for normal scale).</param>
        /// <param name="effect"><see cref="SpriteEffects"/> to use when drawing.</param>
        public void Draw(ISpriteBatch sb, Vector2 position, Color color, float scale, SpriteEffects effect)
        {
            foreach (var bodyItem in BodyItems)
            {
                bodyItem.Draw(sb, position, scale, color, effect);
            }
        }

        /// <summary>
        /// Updates the <see cref="SkeletonBody"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public void Update(TickCount currentTime)
        {
            foreach (var bodyItem in BodyItems)
            {
                bodyItem.Update(currentTime);
            }
        }
    }
}