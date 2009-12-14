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
        const string _valueKeyCollisionType = "CollisionType";
        const string _valueKeyPosition = "Position";
        const string _valueKeySize = "Size";
        static readonly CollisionTypeHelper _collisionTypeHelper = CollisionTypeHelper.Instance;

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

        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
        /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
        /// </summary>
        public override bool CollidesAgainstWalls
        {
            get { return false; }
        }

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

        public static void HandleCollideInto(Entity other, Vector2 displacement)
        {
            // Move the other entity away from the wall
            other.Move(displacement);

            // Check for vertical collision
            if (displacement.Y != 0)
            {
                if (other.Velocity.Y >= 0 && displacement.Y < 0)
                {
                    // Collision from falling (land on feet)
                    other.SetVelocity(new Vector2(other.Velocity.X, 0.0f));
                    other.OnGround = true;
                }
                else if (other.Velocity.Y < 0 && displacement.Y > 0)
                {
                    // Collision from rising (hit head)
                    other.SetVelocity(new Vector2(other.Velocity.X, 0.0f));
                }
            }
        }

        void Read(IValueReader r)
        {
            Vector2 position = r.ReadVector2(_valueKeyPosition);
            Vector2 size = r.ReadVector2(_valueKeySize);
            CollisionType ct = r.ReadEnum(_collisionTypeHelper, _valueKeyCollisionType);

            SetPositionRaw(position);
            SetSizeRaw(size);
            SetCollisionTypeRaw(ct);
        }

        public void Write(IValueWriter w)
        {
            w.Write(_valueKeyPosition, Position);
            w.Write(_valueKeySize, Size);
            w.WriteEnum(_collisionTypeHelper, _valueKeyCollisionType, CollisionType);
        }
    }
}