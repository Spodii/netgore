using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// An individual body item for a SkeletonBody
    /// </summary>
    public class SkeletonBodyItem
    {
        readonly SkeletonBodyItemInfo _itemInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonBodyItem"/> class.
        /// </summary>
        /// <param name="itemInfo">SkeletonBodyItemInfo to create the SkeletonBodyItem from</param>
        public SkeletonBodyItem(SkeletonBodyItemInfo itemInfo)
        {
            _itemInfo = itemInfo;
            Grh = new Grh(itemInfo.GrhIndex, AnimType.Loop, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonBodyItem"/> class.
        /// </summary>
        /// <param name="itemInfo">SkeletonBodyItemInfo to create the SkeletonBodyItem from</param>
        /// <param name="skeleton">Skeleton to attach to</param>
        public SkeletonBodyItem(SkeletonBodyItemInfo itemInfo, Skeleton skeleton) : this(itemInfo)
        {
            Attach(skeleton);
        }

        /// <summary>
        /// Gets or sets the destination SkeletonNode to draw to
        /// </summary>
        public SkeletonNode Dest { get; set; }

        /// <summary>
        /// Gets or sets the Grh used by this SkeletonBodyItem. Setting the Grh will not affect the ItemInfo.
        /// </summary>
        public Grh Grh { get; set; }

        /// <summary>
        /// Gets the SkeletonBodyItemInfo used by this SkeletonBodyItem
        /// </summary>
        public SkeletonBodyItemInfo ItemInfo
        {
            get { return _itemInfo; }
        }

        /// <summary>
        /// Gets or sets the source SkeletonNode to draw from
        /// </summary>
        public SkeletonNode Source { get; set; }

        /// <summary>
        /// Attaches the SkeletonBodyItem to a skeleton using the name of the joints
        /// </summary>
        /// <param name="skeleton">Skeleton to attach to</param>
        public void Attach(Skeleton skeleton)
        {
            // Source n
            if (skeleton == null)
                Source = null;
            else
                Source = skeleton.FindNode(ItemInfo.SourceName);

            // Destination n
            if (skeleton == null || ItemInfo.DestName.Length == 0)
                Dest = null;
            else
                Dest = skeleton.FindNode(ItemInfo.DestName);
        }

        /// <summary>
        /// Draws the SkeletonBodyItem
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="position">Position to draw at</param>
        public void Draw(SpriteBatch sb, Vector2 position)
        {
            Draw(sb, position, 1.0f, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the SkeletonBodyItem
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="position">Position to draw at</param>
        /// <param name="scale">Amount to scale the Grh in percent (1.0f for no scaling)</param>
        public void Draw(SpriteBatch sb, Vector2 position, float scale)
        {
            Draw(sb, position, scale, SpriteEffects.None);
        }

        /// <summary>
        /// Draws the SkeletonBodyItem
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="position">Position to draw at</param>
        /// <param name="scale">Amount to scale the Grh in percent (1.0f for no scaling)</param>
        /// <param name="effect">SpriteEffects to use when drawing</param>
        public void Draw(SpriteBatch sb, Vector2 position, float scale, SpriteEffects effect)
        {
            // Validate
            if (Source == null)
                return;

            // Find the effect
            Vector2 m;
            switch (effect)
            {
                case SpriteEffects.FlipHorizontally:
                    m = new Vector2(-1, 1);
                    break;

                case SpriteEffects.FlipVertically:
                    m = new Vector2(1, -1);
                    break;

                default:
                    m = new Vector2(1, 1);
                    break;
            }

            // Calculate the angle
            float angle;
            if (Dest == null)
                angle = 0.0f;
            else
                angle = SkeletonNode.GetAngle(Source.Position * m, Dest.Position * m) - MathHelper.PiOver2;

            // Draw
            Vector2 v = Source.Position + ItemInfo.Offset;
            Grh.Draw(sb, (v * m) + position, Color.White, effect, angle, ItemInfo.Origin, scale);
        }

        /// <summary>
        /// Updates the Grh used by the SkeletonBodyItem
        /// </summary>
        /// <param name="currentTime">Current time</param>
        public void Update(int currentTime)
        {
            Grh.Update(currentTime);
        }
    }
}