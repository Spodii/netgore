using System;
using System.ComponentModel;
using System.Linq;
using NetGore.Audio;
using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// The base class of all entities, which are physical objects that reside in the virtual world, and can interact
    /// with other entities.
    /// </summary>
    public abstract class Entity : ISpatial, IAudioEmitter, IDisposable
    {
        const string _entityCategoryString = "Entity";

#if !TOPDOWN
        static readonly Vector2 _defaultGravity;
#endif

        static readonly Vector2 _maxVelocity;

        bool _isDisposed;

        Vector2 _position;
        Vector2 _size;

#if !TOPDOWN
        Entity _standingOn;
#endif

        Vector2 _velocity;
        float _weight = 1.0f;

        /// <summary>
        /// Notifies listeners that the Entity is being disposed. This event is raised before any actual
        /// disposing takes place, but the Entity's IsDisposed property will be true. This event
        /// is guarenteed to only be raised once.
        /// </summary>
        [Browsable(false)]
        public event TypedEventHandler<Entity> Disposed;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        [Browsable(false)]
        public event TypedEventHandler<ISpatial, EventArgs<Vector2>> Moved;

        /// <summary>
        /// When overridden in the derived class, allows for handling for when the <see cref="ISpatial.Moved"/> event occurs.
        /// This is the same as listening to the event directly, but with less overhead.
        /// </summary>
        /// <param name="oldPos">The old position.</param>
        protected virtual void OnMoved(Vector2 oldPos)
        {
        }

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        [Browsable(false)]
        public event TypedEventHandler<ISpatial, EventArgs<Vector2>> Resized;

        /// <summary>
        /// When overridden in the derived class, allows for handling for when the <see cref="ISpatial.Resized"/> event occurs.
        /// This is the same as listening to the event directly, but with less overhead.
        /// </summary>
        /// <param name="oldSize">The old size.</param>
        protected virtual void OnResized(Vector2 oldSize)
        {
        }

        /// <summary>
        /// Tries to move the <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="newPos">The new position.</param>
        /// <returns>True if the <see cref="ISpatial"/> was moved to the <paramref name="newPos"/>; otherwise false.</returns>
        bool ISpatial.TryMove(Vector2 newPos)
        {
            Position = newPos;
            return true;
        }

        /// <summary>
        /// Gets if this <see cref="ISpatial"/> can ever be moved with <see cref="ISpatial.TryMove"/>.
        /// </summary>
        bool ISpatial.SupportsMove
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if this <see cref="ISpatial"/> can ever be resized with <see cref="ISpatial.TryResize"/>.
        /// </summary>
        bool ISpatial.SupportsResize
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes the <see cref="Entity"/> class.
        /// </summary>
        static Entity()
        {
            var settings = EngineSettings.Instance;

            // Cache the settings we care about
#if !TOPDOWN
            _defaultGravity = settings.Gravity;
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
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Entity"/> is reclaimed by garbage collection.
        /// </summary>
        ~Entity()
        {
            if (IsDisposed)
                return;

            _isDisposed = true;

            if (Disposed != null)
                Disposed.Raise(this, EventArgs.Empty);

            HandleDispose(false);
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
        /// Tries to resize the <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        /// <returns>True if the <see cref="ISpatial"/> was resized to the <paramref name="newSize"/>; otherwise false.</returns>
        bool ISpatial.TryResize(Vector2 newSize)
        {
            Size = newSize;
            return true;
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
        [Browsable(false)]
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
        /// Gets if this <see cref="Entity"/> is currently on the ground. For top-down, this will always return true.
        /// </summary>
        [Browsable(false)]
        public bool IsOnGround
        {
            get
            {
#if TOPDOWN
                return true;
#else
                return StandingOn != null;
#endif
            }
        }

        /// <summary>
        /// Gets the <see cref="WallEntityBase"/> that the <see cref="Entity"/> is standing on, or null if they are
        /// not standing on anything (are not on the ground). If using a top-down perspective, this value is always null.
        /// </summary>
        [Browsable(false)]
        public Entity StandingOn
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

                    // Move them so that they really are standing right on top of the entity
                    var newPos = new Vector2(Position.X, StandingOn.Position.Y - Size.Y);
                    SetPositionRaw(newPos);
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
            return SpatialHelper.ToRectangle(this);
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
        [Description("The weight of the Entity. Higher the weight, the greater the effects of the gravity, where 0 is unaffected by gravity.")]
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
        /// <param name="disposeManaged">When true, <see cref="IDisposable.Dispose"/> was explicitly called and managed resources need to be
        /// disposed. When false, managed resources do not need to be disposed since this object was garbage-collected.</param>
        protected virtual void HandleDispose(bool disposeManaged)
        {
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected virtual void HandleUpdate(IMap imap, int deltaTime)
        {
            // If moving, perform collision detection
            if (Velocity != Vector2.Zero)
                imap.CheckCollisions(this);

#if !TOPDOWN
            // If the entity is standing on a wall, make sure they are still standing on it. If they aren't, check if they
            // are standing on top of something else.
            if (StandingOn != null)
            {
                if (!WallEntityBase.IsEntityStandingOn(StandingOn, this))
                    StandingOn = imap.FindStandingOn(this);
            }
#endif
        }

        /// <summary>
        /// Translates the entity from its current position.
        /// </summary>
        /// <param name="adjustment">Amount to move.</param>
        public virtual void Move(Vector2 adjustment)
        {
            if (adjustment == Vector2.Zero)
                return;

            var newPos = Position + adjustment;
            SetPositionRaw(newPos);
        }

        /// <summary>
        /// Resizes the <see cref="Entity"/>.
        /// </summary>
        /// <param name="newSize">The new size of this <see cref="Entity"/>.</param>
        protected virtual void Resize(Vector2 newSize)
        {
            SetSizeRaw(newSize);
        }

        /// <summary>
        /// Sets the <see cref="Entity"/>'s <see cref="Position"/> directly without any chance to be overridden.
        /// This should only be used for synchronization.
        /// </summary>
        /// <param name="newPosition">The new <see cref="Position"/> value.</param>
        protected internal void SetPositionRaw(Vector2 newPosition)
        {
            if (newPosition == Position)
                return;

            var oldPos = Position;

            _position = newPosition;

            // Notify listeners
            OnMoved(oldPos);
            if (Moved != null)
                Moved.Raise(this, EventArgsHelper.Create(oldPos));
        }

        /// <summary>
        /// Sets the <see cref="Entity"/>'s <see cref="Size"/> directly without any chance to be overridden.
        /// This should only be used for synchronization.
        /// </summary>
        /// <param name="newSize">The new <see cref="Size"/> value.</param>
        protected internal void SetSizeRaw(Vector2 newSize)
        {
            if (newSize == Size)
                return;

            var oldSize = Size;

            _size = newSize;

            // Notify listeners
            OnResized(oldSize);
            if (Resized != null)
                Resized.Raise(this, EventArgsHelper.Create(oldSize));
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
        /// Moves the <see cref="Entity"/> to a new location instantly.
        /// </summary>
        /// <param name="newPosition">New position for the <see cref="Entity"/>.</param>
        protected virtual void Teleport(Vector2 newPosition)
        {
            // Do not update if we're already at the specified position
            if (newPosition == Position)
                return;

            // Assume they are not on the ground after teleporting
            StandingOn = null;

            // Move the entity
            SetPositionRaw(newPosition);
        }

        /// <summary>
        /// Perform pre-collision velocity and position updating.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="deltaTime">The amount of that that has elapsed time since last update.</param>
        public virtual void UpdateVelocity(IMap map, int deltaTime)
        {
            _lastPosition = Position;

            // Only perform movement if moving
            if (IsOnGround && Velocity == Vector2.Zero)
                return;

#if !TOPDOWN
            Vector2 gravity;
            if (map == null)
                gravity = _defaultGravity;
            else
                gravity = map.Gravity;

            if (StandingOn != null)
            {
                if (!WallEntityBase.IsEntityStandingOn(StandingOn, this))
                    StandingOn = null;
            }

            if (StandingOn == null)
            {
                // Increase the velocity by the gravity
                var displacement = gravity * (Weight * deltaTime);
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
            // Check if the Entity has already been disposed
            if (IsDisposed)
                return;

            GC.SuppressFinalize(this);

            _isDisposed = true;

            // Notify listeners that the Entity is being disposed
            if (Disposed != null)
                Disposed.Raise(this, EventArgs.Empty);

            // Handle the disposing
            HandleDispose(true);
        }

        #endregion
    }
}