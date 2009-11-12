using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace NetGore
{
    /// <summary>
    /// The base class of all entities, which are physical objects that reside in the virtual world, and can interact
    /// with other entities.
    /// </summary>
    public abstract class Entity : IDisposable
    {
        CollisionBox _collisionBox;
        CollisionType _ct = CollisionType.Full;
        bool _isDisposed;
        bool _onGround = false;
        Vector2 _velocity;
        float _weight = 1.0f;

        /// <summary>
        /// Notifies listeners that the Entity is being disposed. This event is raised before any actual
        /// disposing takes place, but the Entity's IsDisposed property will be true. This event
        /// is guarenteed to only be raised once.
        /// </summary>
        [Browsable(false)]
        public event EntityEventHandler OnDispose;

        /// <summary>
        /// Notifies listeners that the Entity was moved, and passes the old position of the Entity.
        /// </summary>
        [Browsable(false)]
        public event EntityEventHandler<Vector2> OnMove;

        /// <summary>
        /// Notifies listeners that the Entity was resized, and passes the old size of the Entity.
        /// </summary>
        [Browsable(false)]
        public event EntityEventHandler<Vector2> OnResize;

        /// <summary>
        /// Gets or sets the collision box for the Entity.
        /// </summary>
        [Browsable(false)]
        public CollisionBox CB
        {
            get { return _collisionBox; }
            set
            {
                // TODO: The setter on this should be private, and the CollisionBox object should never be changed.
                _collisionBox = value;
            }
        }

        /// <summary>
        /// Gets the position of the center of the Entity.
        /// </summary>
        [Browsable(false)]
        public Vector2 Center
        {
            get { return Position + (Size / 2); }
        }

        /// <summary>
        /// Gets or sets the collision type used for the entity.
        /// </summary>
        [Category("Entity")]
        [DisplayName("CollisionType")]
        [Description("The collision type used for the Entity.")]
        [DefaultValue(CollisionType.Full)]
        [Browsable(true)]
        public CollisionType CollisionType
        {
            get { return _ct; }
            set { _ct = value; }
        }

        /// <summary>
        /// Gets if this Entity has been disposed, or is in the process of being disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets or sets if the character is currently on the ground. This value only applies to moving entities.
        /// </summary>
        [Browsable(false)]
        public bool OnGround
        {
            get { return _onGround; }
            set { 
                // TODO: The setter on this shouldn't be public... or even protected. How will we accomplish that?
                _onGround = value; 
            }
        }

        /// <summary>
        /// Gets or sets the position of the top-left corner of the entity.
        /// </summary>
        [Category("Entity")]
        [DisplayName("Position")]
        [Description("Location of the top-left corner of the Entity on the map.")]
        [Browsable(true)]
        public Vector2 Position
        {
            get { return CB.Min; }
            set { Teleport(value); }
        }

        /// <summary>
        /// Gets or sets the size of the Entity.
        /// </summary>
        [Category("Entity")]
        [DisplayName("Size")]
        [Description("Size of the Entity.")]
        [Browsable(true)]
        public Vector2 Size
        {
            get { return _collisionBox.Size; }
            set { Resize(value); }
        }

        /// <summary>
        /// Gets the velocity of the Entity.
        /// </summary>
        [Browsable(false)]
        public Vector2 Velocity
        {
            get { return _velocity; }
        }

        /// <summary>
        /// Gets or sets the weight of the Entity (used in gravity calculations).
        /// </summary>
        [Category("Entity")]
        [DisplayName("Weight")]
        [Description(
            "The weight of the Entity. Higher the weight, the greater the effects of the gravity, where 0 is unaffected by gravity."
            )]
        [DefaultValue(0.0f)]
        [Browsable(true)]
        public virtual float Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        /// <summary>
        /// Entity constructor
        /// </summary>
        protected Entity()
        {
            CB = new CollisionBox(Vector2.Zero, 1.0f, 1.0f);
        }

        /// <summary>
        /// Entity constructor
        /// </summary>
        /// <param name="position">Initial position of the entity</param>
        /// <param name="size">Initial size of the entity</param>
        protected Entity(Vector2 position, Vector2 size)
        {
            CB = new CollisionBox(position, size.X, size.Y);
        }

        /// <summary>
        /// Handles when another Entity collides into us. Not synonymous CollideInto since the
        /// <paramref name="collider"/> Entity is the one who collided into us. For example, if the
        /// two entities in question were a moving Character and a stationary wall, this Entity would be
        /// the Wall and <paramref name="collider"/> would be the Character.
        /// </summary>
        /// <param name="collider">Entity that collided into us.</param>
        /// <param name="displacement">Displacement between the two Entities.</param>
        public virtual void CollideFrom(Entity collider, Vector2 displacement)
        {
        }

        /// <summary>
        /// Handles when the Entity collides into another entity. Not synonymous with CollideFrom we
        /// were the ones who collided into the <paramref name="collideWith"/> Entity. For example, if the
        /// two Entities in question were a moving Character and a stationary Wall, this Entity would be
        /// the Character and <paramref name="collideWith"/> would be the Wall.
        /// </summary>
        /// <param name="collideWith">Entity that this Entity collided with.</param>
        /// <param name="displacement">Displacement between the two Entities.</param>
        public virtual void CollideInto(Entity collideWith, Vector2 displacement)
        {
        }

        /// <summary>
        /// Performs the actual disposing of the Entity. This is called by the base Entity class when
        /// a request has been made to dispose of the Entity. This is guarenteed to only be called once.
        /// All classes that override this method should be sure to call base.DisposeHandler() after
        /// handling what it needs to dispose.
        /// </summary>
        protected virtual void HandleDispose()
        {
        }

        /// <summary>
        /// Checks if this <see cref="Entity"/> contains a point.
        /// </summary>
        /// <param name="p">Point to check against.</param>
        /// <returns>True if this <see cref="Entity"/> contains point <paramref name="p"/>; otherwise false.</returns>
        public bool HitTest(Vector2 p)
        {
            return CB.HitTest(p);
        }

        /// <summary>
        /// Checks if the <see cref="Entity"/> intersects with a CollisionBox.
        /// </summary>
        /// <param name="collisionBox">CollisionBox to check against.</param>
        /// <returns>True if the two occupy any common space, else false.</returns>
        public bool Intersect(CollisionBox collisionBox)
        {
            return CB.Intersect(collisionBox);
        }

        /// <summary>
        /// Checks if the <see cref="Entity"/> intersects with another <see cref="Entity"/>.
        /// </summary>
        /// <param name="other"><see cref="Entity"/> to check against.</param>
        /// <returns>True if this <see cref="Entity"/> and the <paramref name="other"/> <see cref="Entity"/>
        /// occupy any of the same world space; otherwise false.</returns>
        public bool Intersect(Entity other)
        {
            return CB.Intersect(other.CB);
        }

        /// <summary>
        /// Loads the Entity's values directly, completely bypassing any events or updating. This should only be used
        /// for when constructing an Entity when these values cannot be passed directly to the constructor or Create().
        /// </summary>
        /// <param name="position">New position value.</param>
        /// <param name="size">New size value.</param>
        /// <param name="velocity">New velocity value.</param>
        /// <param name="weight">New weight value.</param>
        /// <param name="collisionType">New collision type.</param>
        protected internal void LoadEntityValues(Vector2 position, Vector2 size, Vector2 velocity, float weight,
                                                 CollisionType collisionType)
        {
            CB.Teleport(position);
            CB.Resize(size);
            _velocity = velocity;
            _weight = weight;
            _ct = collisionType;
        }

        /// <summary>
        /// Translates the entity from its current position.
        /// </summary>
        /// <param name="adjustment">Amount to move.</param>
        public virtual void Move(Vector2 adjustment)
        {
            if (adjustment == Vector2.Zero)
                return;

            Vector2 oldPos = _collisionBox.Min;

            _collisionBox.Move(adjustment);

            // Notify of movement
            if (OnMove != null)
                OnMove(this, oldPos);
        }

        /// <summary>
        /// Resizes the <see cref="Entity"/>.
        /// </summary>
        /// <param name="size">The new size of this <see cref="Entity"/>.</param>
        public virtual void Resize(Vector2 size)
        {
            if (size == _collisionBox.Size)
                return;

            Vector2 oldSize = _collisionBox.Size;

            _collisionBox.Resize(size);

            if (OnResize != null)
                OnResize(this, oldSize);
        }

        /// <summary>
        /// Sets the <see cref="Entity"/>'s <see cref="CollisionType"/> directly without any chance to be overridden.
        /// This should only be used for synchronization.
        /// </summary>
        /// <param name="newCollisionType">The new <see cref="CollisionType"/> value.</param>
        protected internal void SetCollisionTypeRaw(CollisionType newCollisionType)
        {
            _ct = newCollisionType;
        }

        /// <summary>
        /// Sets the <see cref="Entity"/>'s <see cref="Position"/> directly without any chance to be overridden.
        /// This should only be used for synchronization.
        /// </summary>
        /// <param name="newPosition">The new <see cref="Position"/> value.</param>
        protected internal void SetPositionRaw(Vector2 newPosition)
        {
            CB.Teleport(newPosition);
        }

        /// <summary>
        /// Sets the <see cref="Entity"/>'s <see cref="Size"/> directly without any chance to be overridden.
        /// This should only be used for synchronization.
        /// </summary>
        /// <param name="newSize">The new <see cref="Size"/> value.</param>
        protected internal void SetSizeRaw(Vector2 newSize)
        {
            CB.Resize(newSize);
        }

        /// <summary>
        /// Sets the velocity of the <see cref="Entity"/>.
        /// </summary>
        /// <param name="newVelocity">The new velocity.</param>
        public virtual void SetVelocity(Vector2 newVelocity)
        {
            _velocity = newVelocity;
        }


        /// <summary>
        /// Sets the <see cref="Entity"/>'s <see cref="Velocity"/> directly without any chance to be overridden.
        /// This should only be used for synchronization.
        /// </summary>
        /// <param name="newVelocity">The new <see cref="Velocity"/> value.</param>
        protected internal void SetVelocityRaw(Vector2 newVelocity)
        {
            _velocity = newVelocity;
        }

        /// <summary>
        /// Sets the <see cref="Entity"/>'s <see cref="Weight"/> directly without any chance to be overridden.
        /// This should only be used for synchronization.
        /// </summary>
        /// <param name="newWeight">The new <see cref="Weight"/> value.</param>
        protected internal void SetWeightRaw(float newWeight)
        {
            _weight = newWeight;
        }

        /// <summary>
        /// Moves the Entity to a new location instantly.
        /// </summary>
        /// <param name="newPosition">New position for the Entity.</param>
        public virtual void Teleport(Vector2 newPosition)
        {
            // Do not update if we're already at the specified position
            if (newPosition == Position)
                return;

            Vector2 oldPos = Position;

            // Treat as if not on ground, and move the CollisionBox
            _onGround = false;
            _collisionBox.Teleport(newPosition);

            // Notify of movement
            if (OnMove != null)
                OnMove(this, oldPos);
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected virtual void HandleUpdate(IMap imap, float deltaTime)
        {
            // If the Y velocity is non-zero, assume not on the ground
            if (Velocity.Y != 0)
                _onGround = false;

            // If moving, perform collision detection
            if (Velocity != Vector2.Zero)
                imap.CheckCollisions(this);
        }

        /// <summary>
        /// Perform pre-collision velocity and position updating.
        /// </summary>
        /// <param name="deltaTime">The amount of that that has elapsed time since last update.</param>
        public virtual void UpdateVelocity(float deltaTime)
        {
            // Only perform movement if moving
            if (_onGround && Velocity == Vector2.Zero)
                return;

            // Increase the velocity by the gravity
            _velocity += WorldSettings.Gravity * (Weight * deltaTime);

            // Check for surpassing the maximum velocity
            if (_velocity.X > WorldSettings.MaxVelocity.X)
                _velocity.X = WorldSettings.MaxVelocity.X;
            else if (_velocity.X < -WorldSettings.MaxVelocity.X)
                _velocity.X = -WorldSettings.MaxVelocity.X;

            if (_velocity.Y > WorldSettings.MaxVelocity.Y)
                _velocity.Y = WorldSettings.MaxVelocity.Y;
            else if (_velocity.Y < -WorldSettings.MaxVelocity.Y)
                _velocity.Y = -WorldSettings.MaxVelocity.Y;

            // Move according to the velocity
            Move(_velocity * deltaTime);
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the <see cref="Entity"/>.
        /// </summary>
        public void Dispose()
        {
            ThreadAsserts.IsMainThread();

            // Check if the Entity has already been disposed
            if (IsDisposed)
                return;

            _isDisposed = true;

            // Notify listeners that the Entity is being disposed
            if (OnDispose != null)
                OnDispose(this);

            // Handle the disposing
            HandleDispose();
        }

        #endregion
    }
}