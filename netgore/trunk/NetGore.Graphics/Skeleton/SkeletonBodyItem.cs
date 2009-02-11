using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
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
        /// SkeletonBodyItem constructor
        /// </summary>
        /// <param name="itemInfo">SkeletonBodyItemInfo to create the SkeletonBodyItem from</param>
        public SkeletonBodyItem(SkeletonBodyItemInfo itemInfo)
        {
            _itemInfo = itemInfo;
            Grh = new Grh(itemInfo.GrhIndex, AnimType.Loop, 0);
        }

        /// <summary>
        /// SkeletonBodyItem constructor
        /// </summary>
        /// <param name="itemInfo">SkeletonBodyItemInfo to create the SkeletonBodyItem from</param>
        /// <param name="skeleton">Skeleton to attach to</param>
        public SkeletonBodyItem(SkeletonBodyItemInfo itemInfo, Skeleton skeleton) : this(itemInfo)
        {
            Attach(skeleton);
        }

        /// <summary>
        /// Attaches the SkeletonBodyItem to a skeleton using the name of the joints
        /// </summary>
        /// <param name="skeleton">Skeleton to attach to</param>
        public void Attach(Skeleton skeleton)
        {
            // Source node
            if (skeleton == null)
                Source = null;
            else
                Source = skeleton.FindNode(ItemInfo.SourceName);

            // Destination node
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

    /// <summary>
    /// Describes a SkeletonBodyItem
    /// </summary>
    public class SkeletonBodyItemInfo
    {
        readonly string _destName;
        readonly ushort _grhIndex;
        readonly string _sourceName;

        /// <summary>
        /// Gets the name of the destination node
        /// </summary>
        public string DestName
        {
            get { return _destName; }
        }

        /// <summary>
        /// Gets the GrhIndex to use
        /// </summary>
        public ushort GrhIndex
        {
            get { return _grhIndex; }
        }

        /// <summary>
        /// Gets or sets the Grh drawing offset
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Gets or sets the Grh drawing origin
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Gets the name of the source node
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
        }

        /// <summary>
        /// SkeletonBodyItemInfo constructor
        /// </summary>
        /// <param name="grhIndex">GrhIndex of the body item</param>
        /// <param name="sourceName">Name of the source node</param>
        /// <param name="destName">Name of the destination node (String.Empty for no destination)</param>
        /// <param name="offset">Grh drawing offset</param>
        /// <param name="origin">Grh drawing origin</param>
        public SkeletonBodyItemInfo(ushort grhIndex, string sourceName, string destName, Vector2 offset, Vector2 origin)
        {
            _grhIndex = grhIndex;
            _sourceName = sourceName;
            _destName = destName;
            Offset = offset;
            Origin = origin;
        }

        /// <summary>
        /// Loads a SkeletonBodyItemInfo from a XmlReader
        /// </summary>
        /// <param name="r">XmlReader to read from</param>
        /// <returns>New SkeletonBodyItemInfo</returns>
        public static SkeletonBodyItemInfo Load(XmlReader r)
        {
            ushort grhIndex = 0;
            string srcName = string.Empty;
            string destName = string.Empty;
            Vector2 offset = Vector2.Zero;
            Vector2 origin = Vector2.Zero;

            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                {
                    if (r.Name == "GrhIndex")
                        grhIndex = (ushort)r.ReadElementContentAsInt();
                    else if (r.Name == "Source")
                        srcName = r.ReadElementContentAsString();
                    else if (r.Name == "Dest")
                        destName = r.ReadElementContentAsString();
                    else if (r.Name == "OffsetX")
                        offset.X = r.ReadElementContentAsFloat();
                    else if (r.Name == "OffsetY")
                        offset.Y = r.ReadElementContentAsFloat();
                    else if (r.Name == "OriginX")
                        origin.X = r.ReadElementContentAsFloat();
                    else if (r.Name == "OriginY")
                        origin.Y = r.ReadElementContentAsFloat();
                }
            }

            return new SkeletonBodyItemInfo(grhIndex, srcName, destName, offset, origin);
        }

        /// <summary>
        /// Writes the SkeletonBodyItemInfo to a XmlWriter
        /// </summary>
        /// <param name="w">XmlWriter to write to</param>
        public void Save(XmlWriter w)
        {
            w.WriteStartElement("BodyItem");
            w.WriteElementString("GrhIndex", GrhIndex.ToString());
            w.WriteElementString("Source", SourceName);
            w.WriteElementString("Dest", DestName);
            w.WriteElementString("OffsetX", Offset.X.ToString());
            w.WriteElementString("OffsetY", Offset.Y.ToString());
            w.WriteElementString("OriginX", Origin.X.ToString());
            w.WriteElementString("OriginY", Origin.Y.ToString());
            w.WriteEndElement();
        }
    }
}