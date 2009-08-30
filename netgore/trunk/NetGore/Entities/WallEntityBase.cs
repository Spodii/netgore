using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Base of all entities in the world which are not passable by other solid entities. WallEntities also do
    /// not ever change or move, allowing them to be stored in the map files. Despite the name,
    /// this refers to more than just walls such as a floor, platform, or even just an invisible
    /// plane of nothingness that you are just not allowed to go inside of for absolutely no reason.
    /// If you want a dynamic wall, you will want to use a DynamicEntity.
    /// </summary>
    public abstract class WallEntityBase : Entity
    {
        /// <summary>
        /// Gets or sets the weight of the Entity (used in gravity calculations).
        /// </summary>
        /// <value></value>
        [Browsable(false)]
        public override float Weight
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WallEntityBase"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        protected WallEntityBase(Vector2 position, Vector2 size) : this(position, size, CollisionType.Full)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WallEntityBase"/> class.
        /// </summary>
        /// <param name="r">The r.</param>
        protected WallEntityBase(IValueReader r)
        {
            Read(r);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WallEntityBase"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="collisionType">Type of the collision.</param>
        protected WallEntityBase(Vector2 position, Vector2 size, CollisionType collisionType) : base(position, size)
        {
            CollisionType = collisionType;
        }

        void Read(IValueReader r)
        {
            Vector2 position = r.ReadVector2("Position");
            Vector2 size = r.ReadVector2("Size");
            CollisionType ct = r.ReadCollisionType("CollisionType");

            SetPositionRaw(position);
            SetSizeRaw(size);
            SetCollisionTypeRaw(ct);
        }

        public void Write(IValueWriter w)
        {
            w.Write("Position", Position);
            w.Write("Size", Size);
            w.Write("CollisionType", CollisionType);
        }
    }
}