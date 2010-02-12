using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.Audio;

namespace NetGore
{
    /// <summary>
    /// The base class of all entities, which are physical objects that reside in the virtual world, and can interact
    /// with other entities.
    /// </summary>
    public abstract class Entity : ISpatial, IAudioEmitter, IDisposable
    {
        const string _entityCategoryString = "Entity";

#if !TOPDOWN
        static readonly Vector2 _gravity;
#endif

        static readonly Vector2 _maxVelocity;

        bool _isDisposed;

        Vector2 _position;
        Vector2 _size;

#if !TOPDOWN
        WallEntityBase _standingOn;
#endif

        Vector2 _velocity;
        float _weight = 1.0f;

        /// <summary>
        /// Notifies listeners that the Entity is being disposed. This event is raised before any actual
        /// disposing takes place, but the Entity's IsDisposed property will be true. This event
        /// is guarenteed to only be raised once.
        /// </summary>
        [Browsable(false)]
        public event EntityEventHandler Disposed;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        [Browsable(false)]
        public event SpatialEventHandler<Vector2> Moved;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        [Browsable(false)]
        public event SpatialEventHandler<Vector2> Resized;

        /// <summary>
        /// Initializes the <see cref="Entity"/> class.
        /// </summary>
        static Entity()
        {
            var settings = EngineSettings.Instance;

            // Cache the settings we care about
#if !TOPDOWN
            _gravity = settings.Gravity;
#endif
            _maxVelocity = settings.MaxVelocity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity() : this(Vector2.Zero, Vector2.One)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="position">The initial world position.</param>
        /// <param name="size">The initial size.</param>
        protected Entity(Vector2 position, Vector2 size)
        {
            _position = position;
            _size = size;
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
        /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
        /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
        /// </summary>
        public abstract bool CollidesAgainstWalls { get; }

        /// <summary>
        /// Gets if this Entity has been disposed, or is in the process of being disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets the <see cref="WallEntityBase"/> that the <see cref="Entity"/> is standing on, or null if they are
        /// not standing on anything (are not on the ground). If using a top-down perspective, this value is always null.
        /// </summary>
        [Browsable(false)]
        public WallEntityBase StandingOn
        {
            get
            {
#if TOPDOWN
                return null;
#else
                return _standingOn;
#endif
            }

            internal set
            {
#if !TOPDOWN
                if (StandingOn == value)
                    return;

                _standingOn = value;

                if (StandingOn != null)
                {
                    _velocity.Y = 0;
                    _position = new Vector2(Position.X, StandingOn.Position.Y - Size.Y);
                }
#endif
            }
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/> occupies.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/>
        /// occupies.</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        /// <summary>
        /// Gets or sets the position of the top-left corner of the entity.
        /// </summary>
        [Category(_entityCategoryString)]
        [DisplayName("Position")]
        [Description("Location of the top-left corner of the Entity on the map.")]
        [Browsable(true)]
        public Vector2 Position
        {
            get { return _position; }
            set { Teleport(value); }
        }

        Vector2 _lastPosition;

        /// <summary>
        /// Gets the position that this <see cref="Entity"/> was at on their last update. If they teleport, this
        /// value will be set to the current position.
        /// </summary>
        [Browsable(false)]
        public Vector2 LastPosition
        {
            get { return _lastPosition; }
        }

        /// <summary>
        /// Gets or sets the size of the Entity.
        /// </summary>
        [Category(_entityCategoryString)]
        [DisplayName("Size")]
        [Description("Size of the Entity.")]
        [Browsable(true)]
        public Vector2 Size
        {
            get { return _size; }
            set { Resize(value); }
        }

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Max
        {
            get { return Position + Size; }
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
        [Category(_entityCategoryString)]
        [DisplayName("Weight")]
        [Description(
            "The weight of the Entity." +
            " Higher the weight, the greater the effects of the gravity, where 0 is unaffected by gravity.")]
        [DefaultValue(0.0f)]
        [Browsable(true)]
        public virtual float Weight
        {
            get { return _weight; }
            set { _weight = value; }
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
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected virtual void HandleUpdate(IMap imap, float deltaTime)
        {
            // If moving, perform collision detection
            if (Velocity != Vector2.Zero)
                imap.CheckCollisions(this);

#if !TOPDOWN
            // If the entity is standing on a wall, make sure they are still standing on it. If they aren't, check if they
            // are standing on top of something else.
            if (StandingOn != null)
            {
                if (!StandingOn.IsEntityStandingOn(this))
                    StandingOn = imap.FindStandingOn(this);
            }
#endif
        }

        /// <summary>
        /// Loads the Entity's values directly, completely bypassing any events or updating. This should only be used
        /// for when constructing an Entity when these values cannot be passed directly to the constructor or Create().
        /// </summary>
        /// <param name="position">New position value.</param>
        /// <param name="size">New size value.</param>
        /// <param name="velocity">New velocity value.</param>
        /// <param name="weight">New weight value.</param>
        protected internal void LoadEntityValues(Vector2 position, Vector2 size, Vector2 velocity, float weight)
        {
            _position = position;
            _size = size;
            _velocity = velocity;
            _weight = weight;
        }

        /// <summary>
        /// Translates the entity from its current position.
        /// </summary>
        /// <param name="adjustment">Amount to move.</param>
        public virtual void Move(Vector2 adjustment)
        {
            if (adjustment == Vector2.Zero)
                return;

            Vector2 oldPos = Position;

            _position += adjustment;

            // Notify of movement
            if (Moved != null)
                Moved(this, oldPos);
        }

        /// <summary>
        /// Resizes the <see cref="Entity"/>.
        /// </summary>
        /// <param name="newSize">The new size of this <see cref="Entity"/>.</param>
        public virtual void Resize(Vector2 newSize)
        {
            if (newSize == Size)
                return;

            Vector2 oldSize = Size;

            _size = newSize;

            if (Resized != null)
                Resized(this, oldSize);
        }

        /// <summary>
        /// Sets the <see cref="Entity"/>'s <see cref="Position"/> directly without any chance to be overridden.
        /// This should only be used for synchronization.
        /// </summary>
        /// <param name="newPosition">The new <see cref="Position"/> value.</param>
        protected internal void SetPositionRaw(Vector2 newPosition)
        {
            _position = newPosition;
        }

        /// <summary>
        /// Sets the <see cref="Entity"/>'s <see cref="Size"/> directly without any chance to be overridden.
        /// This should only be used for synchronization.
        /// </summary>
        /// <param name="newSize">The new <see cref="Size"/> value.</param>
        protected internal void SetSizeRaw(Vector2 newSize)
        {
            _size = newSize;
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

            // Assume they are not on the ground after teleporting
            StandingOn = null;

            // Move the entity
            Vector2 oldPos = Position;
            _position = newPosition;
            _lastPosition = newPosition;

            if (Moved != null)
                Moved(this, oldPos);
        }

        /// <summary>
        /// Perform pre-collision velocity and position updating.
        /// </summary>
        /// <param name="deltaTime">The amount of that that has elapsed time since last update.</param>
        public virtual void UpdateVelocity(float deltaTime)
        {
            _lastPosition = Position;

            // Only perform movement if moving
            if (StandingOn != null && Velocity == Vector2.Zero)
                return;

#if !TOPDOWN
            if (StandingOn != null)
            {
                if (!StandingOn.IsEntityStandingOn(this))
                    StandingOn = null;
            }

            if (StandingOn == null)
            {
                // Increase the velocity by the gravity
                Vector2 displacement = _gravity * (Weight * deltaTime);
                Vector2.Add(ref _velocity, ref displacement, out _velocity);
            }
#endif

            // Check for surpassing the maximum velocity
            if (_velocity.X > _maxVelocity.X)
                _velocity.X = _maxVelocity.X;
            else if (_velocity.X < -_maxVelocity.X)
                _velocity.X = -_maxVelocity.X;

            if (_velocity.Y > _maxVelocity.Y)
                _velocity.Y = _maxVelocity.Y;
            else if (_velocity.Y < -_maxVelocity.Y)
                _velocity.Y = -_maxVelocity.Y;

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
            if (Disposed != null)
                Disposed(this);

            // Handle the disposing
            HandleDispose();
        }

        #endregion
    }
}