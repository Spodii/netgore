using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// Abstract class for an Entity that is a Character.
    /// </summary>
    public abstract partial class CharacterEntity : DynamicEntity, IUpdateableEntity
    {
        Direction _heading = Direction.East;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterEntity"/> class.
        /// </summary>
        /// <param name="position">The initial world position.</param>
        /// <param name="size">The initial size.</param>
        protected CharacterEntity(Vector2 position, Vector2 size) : base(position, size)
        {
        }

        /// <summary>
        /// Synchronizes the BodyInfo index for the CharacterEntity.
        /// </summary>
        [SyncValue("BodyID")]
        protected internal BodyID BodyID
        {
            get { return BodyInfo.ID; }
            set { BodyInfo = BodyInfoManager.Instance.GetBody(value); }
        }

        /// <summary>
        /// Gets or sets (protected) the CharacterEntity's BodyInfo.
        /// </summary>
        public BodyInfo BodyInfo { get; protected set; }

        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
        /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
        /// </summary>
        [Browsable(false)]
        public override bool CollidesAgainstWalls
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in the derived class, gets or protected sets if the CharacterEntity has a chat dialog.
        /// </summary>
        [SyncValue("HasChatDialog")]
        [Browsable(false)]
        public abstract bool HasChatDialog { get; protected set; }

        /// <summary>
        /// When overridden in the derived class, gets or protected sets if the CharacterEntity has a shop.
        /// </summary>
        [SyncValue("HasShop")]
        [Browsable(false)]
        public abstract bool HasShop { get; protected set; }

        /// <summary>
        /// Gets or sets the direction the character is currently facing.
        /// </summary>
        [SyncValue("Heading")]
        public Direction Heading
        {
            get { return _heading; }
            set { SetHeading(value); }
        }

        /// <summary>
        /// Gets if the character is moving left or right.
        /// </summary>
        [Browsable(false)]
        public bool IsMoving
        {
            get { return Velocity.X != 0; }
        }

        /// <summary>
        /// Gets if the character is moving to the left.
        /// </summary>
        [Browsable(false)]
        public bool IsMovingLeft
        {
            get { return Velocity.X < 0; }
        }

        /// <summary>
        /// Gets if the character is moving to the right.
        /// </summary>
        [Browsable(false)]
        public bool IsMovingRight
        {
            get { return Velocity.X > 0; }
        }

        /// <summary>
        /// Gets or sets the name of the CharacterEntity.
        /// </summary>
        [SyncValue]
        public virtual string Name { get; set; }


        /// <summary>
        /// A flag that determines whether or not this <see cref="CharcterEntity"/> is visible to other clients.
        /// </summary>
        [SyncValue]
        public bool Invisible
        {
            get;
            set;
        }


        /// <summary>
        /// Handles collision against other entities.
        /// </summary>
        /// <param name="collideWith">Entity the character collided with.</param>
        /// <param name="displacement">Displacement between the character and entity.</param>
        public override void CollideInto(Entity collideWith, Vector2 displacement)
        {
        }

        /// <summary>
        /// Gets the map interface for the CharacterEntity. If no valid map interface is supplied,
        /// no map-based collision detection and updating can be used.
        /// </summary>
        /// <returns>Map interface for the CharacterEntity.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected abstract IMap GetIMap();

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, int deltaTime)
        {
            ThreadAsserts.IsMainThread();
            Debug.Assert(imap != null, "How the hell is a null Map updating?");
            Debug.Assert(deltaTime >= 0, "Unless we're going back in time, deltaTime < 0 makes no sense at all.");

            // Perform pre-collision detection updating
            UpdatePreCollision(imap, deltaTime);

            // Performs basic entity updating
            base.HandleUpdate(imap, deltaTime);

            // Perform post-collision detection updating
            UpdatePostCollision(deltaTime);
        }

        /// <summary>
        /// Sets the character's heading.
        /// </summary>
        /// <param name="newHeading">New heading for the character.</param>
        public virtual void SetHeading(Direction newHeading)
        {
            _heading = newHeading;
        }

        /// <summary>
        /// Moves the character to a new location instantly. The character's velocity will
        /// also be set to zero upon teleporting.
        /// </summary>
        /// <param name="position">New position</param>
        protected override void Teleport(Vector2 position)
        {
            // Force the character to stop moving
#if !TOPDOWN
            _state = CharacterState.Falling;
#endif
            SetVelocity(Vector2.Zero);
            StopMoving();

            // Teleport
            base.Teleport(position);
        }

        /// <summary>
        /// Moves the character to the new location. Unlike Teleport(), this will not set the
        /// velocity to zero, and is intended for position corrections / resynchronization.
        /// </summary>
        /// <param name="position">Correct position.</param>
        public virtual void UpdatePosition(Vector2 position)
        {
            base.Teleport(position);
        }

        /// <summary>
        /// Performs the post-collision detection updating.
        /// </summary>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update.</param>
        protected virtual void UpdatePostCollision(int deltaTime)
        {
            // Update the character's state
            UpdateState();
        }

        /// <summary>
        /// Performs the pre-collision detection updating.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update.</param>
        protected virtual void UpdatePreCollision(IMap map, int deltaTime)
        {
            // Update velocity
            UpdateVelocity(map, deltaTime);
        }

        #region IUpdateableEntity Members

        /// <summary>
        /// Updates the <see cref="IUpdateableEntity"/>.
        /// </summary>
        /// <param name="imap">The map that this <see cref="IUpdateableEntity"/> is on.</param>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update.</param>
        public void Update(IMap imap, int deltaTime)
        {
            HandleUpdate(imap, deltaTime);
        }

        #endregion
    }
}