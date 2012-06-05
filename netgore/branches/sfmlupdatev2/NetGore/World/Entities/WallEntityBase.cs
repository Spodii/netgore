using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.World
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
        const string _isPlatformValueKey = "IsPlatform";

        /// <summary>
        /// Platforms will only prevent someone from falling down through a platform if they started above the platform.
        /// This value determines how much leniency is given for determining if they were above the platform. That is,
        /// instead of requiring them to actually be above the platform, this value determines how many pixels they
        /// could be below the platform to still be caught on the platform. If this value wasn't used (or is 0), then
        /// entities may end up falling through a platform randomly since their position may place them a fraction
        /// of a pixel below the top of the platform, which we don't want. A recommended value is around 2-4.
        /// </summary>
        const float _platformYPositionLeniency = 3;

        const string _positionValueKey = "Position";
        const string _sizeValueKey = "Size";

        /// <summary>
        /// Local cache of the <see cref="EngineSettings.MaxWallStepUpHeight"/> value.
        /// </summary>
        static readonly int _maxWallStepUpHeight = EngineSettings.Instance.MaxWallStepUpHeight;

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
        protected WallEntityBase(Vector2 position, Vector2 size) : base(position, size)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
        /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
        /// </summary>
        [Browsable(false)]
        public override bool CollidesAgainstWalls
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets if this <see cref="WallEntityBase"/> will be treated as a platform. Platforms are like walls,
        /// but instead of completely blocking other entities from occupying the same space as them, they will just "catch"
        /// entities that are falling down on them.
        /// </summary>
        [Browsable(true)]
        [Description("If this wall will be treated as a platform instead of an actual solid wall.")]
        [DefaultValue(false)]
        [SyncValue]
        public bool IsPlatform { get; set; }

        /// <summary>
        /// Gets or sets the weight of the Entity (used in gravity calculations).
        /// Always 0 in a <see cref="WallEntityBase"/>.
        /// </summary>
        [Browsable(false)]
        public override float Weight
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets if the values of this <see cref="WallEntityBase"/> equal the values of another, and if they are of the
        /// same type. Not all values need to be taken into consideration, just relevant ones to seeing if two walls are
        /// functionally the same.
        /// </summary>
        /// <param name="other">The <see cref="WallEntityBase"/> to compare to. Can be null.</param>
        /// <returns>True if this <see cref="WallEntityBase"/> has the same values and type as the <paramref name="other"/>;
        /// otherwise false. Also returns false when <paramref name="other"/> is null.</returns>
        public virtual bool AreValuesEqual(WallEntityBase other)
        {
            if (other == null)
                return false;

            // Check if the same object instance
            if (this == other)
                return true;

            // Check the values
            if (Position != other.Position || Size != other.Size || IsPlatform != other.IsPlatform || Weight != other.Weight ||
                Velocity != other.Velocity)
                return false;

            // Check for the same type
            if (GetType() != other.GetType())
                return false;

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, creates a deep copy of this object.
        /// </summary>
        /// <returns>The deep copy of this object.</returns>
        public abstract WallEntityBase DeepCopy();

        /// <summary>
        /// Performs the collision handling for an <see cref="Entity"/> colliding into this <see cref="WallEntityBase"/>.
        /// </summary>
        /// <param name="map">The map that the <see cref="WallEntityBase"/> is on.</param>
        /// <param name="other">The <see cref="Entity"/> that collided into this <see cref="WallEntityBase"/>.</param>
        /// <param name="displacement">The minimum transitional displacement to move the <paramref name="other"/>
        /// to make it no longer overlap this wall.</param>
        public void HandleCollideInto(IMap map, Entity other, Vector2 displacement)
        {
            Debug.Assert(map != null);
            Debug.Assert(other != null);

            if (IsPlatform)
                HandleCollideIntoPlatform(other, displacement);
            else
                HandleCollideIntoWall(map, other, displacement);
        }

        /// <summary>
        /// Handles collision for when this wall is a platform.
        /// </summary>
        /// <param name="other">The <see cref="Entity"/> that collided into this <see cref="WallEntityBase"/>.</param>
        /// <param name="displacement">The minimum transitional displacement to move the <paramref name="other"/>
        /// to make it no longer overlap this wall.</param>
        void HandleCollideIntoPlatform(Entity other, Vector2 displacement)
        {
            if (other.Velocity.Y <= 0 || displacement.Y >= 0 ||
                ((other.LastPosition.Y + other.Size.Y) > Position.Y + _platformYPositionLeniency))
                return;

            // Move the other entity away from the wall
            displacement.X = 0;
            other.Move(displacement);

            // Collision from falling (land on feet)
            other.SetVelocity(new Vector2(other.Velocity.X, 0.0f));
            other.StandingOn = this;
        }

        /// <summary>
        /// Handles collision for when this wall is a wall.
        /// </summary>
        /// <param name="map">The map that the <see cref="WallEntityBase"/> is on.</param>
        /// <param name="other">The <see cref="Entity"/> that collided into this <see cref="WallEntityBase"/>.</param>
        /// <param name="displacement">The minimum transitional displacement to move the <paramref name="other"/>
        /// to make it no longer overlap this wall.</param>
        void HandleCollideIntoWall(IMap map, Entity other, Vector2 displacement)
        {
            var displaced = false;

#if !TOPDOWN
            // Allow entities to walk up very small inclines. Makes no sense in top-down, so its used in sidescroller only.

            // Check if we have a displacement just on the X axis
            if (displacement.Y == 0)
            {
                // Check how far the "other" is from being on top of "this"
                var distFromThis = other.Max.Y - Position.Y;

                // If they are not that far away from being on top of "this", then instead of displacing them horizontally away
                // from us, pop them up on top of us, effectively "stepping" up
                if (distFromThis > 0 && distFromThis < _maxWallStepUpHeight)
                {
                    var horizontalOffset = (int)distFromThis + 1;
                    var otherRect = other.ToRectangle();
                    otherRect.Y -= horizontalOffset;

                    // Make sure that if we move them up on top of us, that they will not be colliding with any walls. If they will
                    // be colliding with any walls, do NOT let them up here
                    if (!map.Spatial.Contains<WallEntityBase>(otherRect, x => !x.IsPlatform))
                    {
                        other.Move(new Vector2(0, -horizontalOffset));
                        displaced = true;
                    }
                }
            }
#endif

            // Move the other entity away from the wall using the MTD
            if (!displaced)
                other.Move(displacement);

            // Check for vertical collision
            if (displacement.Y != 0)
            {
                if (other.Velocity.Y >= 0 && displacement.Y < 0)
                {
                    // Collision from falling (land on feet)
                    other.SetVelocity(new Vector2(other.Velocity.X, 0.0f));
                    other.StandingOn = this;
                }
                else if (other.Velocity.Y < 0 && displacement.Y > 0)
                {
                    // Collision from rising (hit head)
                    other.SetVelocity(new Vector2(other.Velocity.X, 0.0f));
                }
            }
        }

        /// <summary>
        /// Checks if the <paramref name="spatial"/> is standing on this <see cref="WallEntityBase"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to check if standing on this <see cref="WallEntityBase"/>.</param>
        /// <returns>True if the <paramref name="spatial"/> is standing on this <see cref="WallEntityBase"/>; otherwise
        /// false.</returns>
        public bool IsEntityStandingOn(ISpatial spatial)
        {
            var rect = spatial.GetStandingAreaRect();
            return this.Intersects(rect);
        }

        void Read(IValueReader r)
        {
            var position = r.ReadVector2(_positionValueKey);
            var size = r.ReadVector2(_sizeValueKey);
            IsPlatform = r.ReadBool(_isPlatformValueKey);

            SetPositionRaw(position);
            SetSizeRaw(size);
        }

        public void Write(IValueWriter w)
        {
            w.Write(_positionValueKey, Position);
            w.Write(_sizeValueKey, Size);
            w.Write(_isPlatformValueKey, IsPlatform);
        }
    }
}