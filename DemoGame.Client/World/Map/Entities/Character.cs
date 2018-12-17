using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NetGore;
using NetGore.Features.Quests;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Represents a single Character on the Client.
    /// </summary>
    public class Character : CharacterEntity, IGetTime, IDrawableTarget
    {
        static readonly IEnumerable<QuestID> _emptyQuestIDs = new QuestID[0];

        readonly EntityInterpolator _interpolator = new EntityInterpolator();

        IEnumerable<QuestID> _providedQuests = _emptyQuestIDs;
        ICharacterSprite _characterSprite;
        TickCount _lastDrawnTime;
        Color _color = Color.White;
        bool _isVisible = true;
        Vector2 _lastScreenPosition;
        bool _isCastingSkill;
        Map _map;

#if SKELETON_SPRITES
        CharacterState _lastState = CharacterState.Idle;
#endif

        /// <summary>
        /// Notifies listeners when the <see cref="Character.IsCastingSkill"/> property has changed.
        /// </summary>
        public event TypedEventHandler<Character> IsCastingSkillChanged;

        /// <summary>
        /// Gets or sets if this <see cref="Character"/> is currently casting a skill.
        /// </summary>
        public bool IsCastingSkill
        {
            get { return _isCastingSkill; }
            set
            {
                if (_isCastingSkill == value)
                    return;

                _isCastingSkill = value;

                if (IsCastingSkillChanged != null)
                    IsCastingSkillChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterEntity"/> class.
        /// </summary>
        public Character()
            : base(Vector2.Zero, Vector2.One)
        {
        }

        public ICharacterSprite CharacterSprite
        {
            get { return _characterSprite; }
        }

        /// <summary>
        /// Gets the location at which the character is to be drawn.
        /// </summary>
        public Vector2 DrawPosition
        {
            get { return _interpolator.DrawPosition; }
        }

        /// <summary>
        /// Gets the last screen position used to draw this <see cref="Character"/>. This should be used instead of
        /// using <see cref="DrawPosition"/> and offsetting it by the position of the <see cref="ICamera2D"/> since
        /// the camera is mutable, and thus can be pointing to a different position than the last draw position.
        /// </summary>
        public Vector2 LastScreenPosition
        {
            get { return _lastScreenPosition; }
        }

        /// <summary>
        /// When overridden in the derived class, gets or protected sets if the CharacterEntity
        /// has a chat dialog. The setter for this method should never be called directly.
        /// </summary>
        public override bool HasChatDialog { get; protected set; }

        /// <summary>
        /// When overridden in the derived class, gets or protected sets if the CharacterEntity
        /// has a shop. The setter for this method should never be called directly.
        /// </summary>
        public override bool HasShop { get; protected set; }

        /// <summary>
        /// Gets or sets the percent of HP the Character has remaining, where 0 is 0% and 100 is 100%.
        /// </summary>
        public byte HPPercent { get; set; }

        /// <summary>
        /// Gets or sets the percent of MP the Character has remaining, where 0 is 0% and 100 is 100%.
        /// </summary>
        public byte MPPercent { get; set; }

        /// <summary>
        /// Gets or sets the Font used to write the Character's name. If null, the names will not be drawn.
        /// </summary>
        public static Font NameFont { get; set; }

        /// <summary>
        /// Gets the map the character belongs to.
        /// </summary>
        public Map Parent
        {
            get { return _map; }
        }

        /// <summary>
        /// Makes the character show an attack.
        /// </summary>
        public virtual void Attack()
        {
            CharacterSprite.AddBodyModifier(BodyInfo.Attack);
        }

        /// <summary>
        /// Draws the Character's name.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="font">The <see cref="Font"/> to use for the name text. May be null.</param>
        protected virtual void DrawName(ISpriteBatch sb, Font font)
        {
            // Ensure we have a valid font and name first
            if (font == null || string.IsNullOrEmpty(Name))
                return;

            // Get the size of the name
            var nameSize = GetNameSize();

            // Get the character's center
            var namePos = DrawPosition;

            // Center horizontally
            namePos.X += Size.X / 2f; // Move the left side of the name to the center of the character
            namePos.X -= (float)Math.Round(nameSize.X / 2f); // Move the center to the center of the character

            // Move below the character
            namePos.Y -= 10 + (CharacterSprite.SpriteSize.Y - Size.Y);

            // Draw
            sb.DrawStringShaded(font, Name, namePos, Color.White, Color.Black);
        }

        /// <summary>
        /// Draws a bar for the Character's SP.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="percent">The percent of the SP being drawn.</param>
        /// <param name="index">The 0-based index of the bar being drawn.</param>
        /// <param name="color">The color to draw the bar.</param>
        protected virtual void DrawSPBar(ISpriteBatch sb, byte percent, byte index, Color color)
        {
            const float spBarWidth = 40;
            const float spBarHeight = 4;
            const int padding = 5;

            var pos = DrawPosition + new Vector2((Size.X / 2f) - (spBarWidth / 2f), Size.Y + (spBarHeight * index) + padding).Round();

            var border = new Rectangle(pos.X, pos.Y, spBarWidth, spBarHeight);
            var bar = border;
            bar.Width = (int)((spBarWidth * (percent / 100.0f))).Clamp(0.0f, spBarWidth);

            RenderRectangle.Draw(sb, border, new Color(0, 0, 0, 0), Color.Black);
            RenderRectangle.Draw(sb, bar, color);
        }

        /// <summary>
        /// Gets the position for the Camera to focus on this Character.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to get the position for.</param>
        /// <returns>
        /// Position for the Camera to focus on this Character.
        /// </returns>
        public Vector2 GetCameraPos(ICamera2D camera)
        {
            var pos = DrawPosition + (Size / 2.0f) - (camera.Size / 2.0f);
            return pos.Round();
        }

        /// <summary>
        /// Gets the map interface used by the Character, primarily for when referencing by the CharacterEntity.
        /// Can return null if the Character is not on a map, and null returns should be supported.
        /// </summary>
        /// <returns>Map interface used by the Character. Can be null.</returns>
        protected override IMap GetIMap()
        {
            return _map;
        }

        /// <summary>
        /// Performs the actual disposing of the Entity. This is called by the base Entity class when
        /// a request has been made to dispose of the Entity. This is guarenteed to only be called once.
        /// All classes that override this method should be sure to call base.DisposeHandler() after
        /// handling what it needs to dispose.
        /// </summary>
        /// <param name="disposeManaged">When true, <see cref="IDisposable.Dispose"/> was explicitly called and managed resources need to be
        /// disposed. When false, managed resources do not need to be disposed since this object was garbage-collected.</param>
        protected override void HandleDispose(bool disposeManaged)
        {
            IsCastingSkill = false;

            base.HandleDispose(disposeManaged);
        }

        /// <summary>
        /// Gets the size of the Character's name string.
        /// </summary>
        /// <returns>The size of the Character's name string.</returns>
        protected virtual Vector2 GetNameSize()
        {
            var ret = NameFont.MeasureString(Name);
            return ret;
        }

        /// <summary>
        /// Creates the <see cref="ICharacterSprite"/> to be used by a <see cref="Character"/>.
        /// </summary>
        /// <param name="getTime">The <see cref="IGetTime"/> provider.</param>
        /// <param name="entity">The <see cref="Entity"/> the sprite is for (usually the <see cref="Character"/>).</param>
        /// <param name="skeletonManager">The <see cref="SkeletonManager"/> to use (ignored in TopDown builds).</param>
        /// <returns>The <see cref="ICharacterSprite"/> instance.</returns>
        public static ICharacterSprite CreateCharacterSprite(IGetTime getTime, Entity entity, SkeletonManager skeletonManager)
        {
#if SKELETON_SPRITES
            return new SkeletonCharacterSprite(getTime, entity, skeletonManager, GameData.AnimationSpeedModifier);
#else
            return new GrhCharacterSprite(entity, "Character");
#endif
        }

        /// <summary>
        /// Initializes the Character.
        /// </summary>
        /// <param name="map">The Map to place the Character on.</param>
        /// <param name="skelManager">The SkeletonManager to use for the Character's skeletons.</param>
        public virtual void Initialize(Map map, SkeletonManager skelManager)
        {
            // HACK: This is quite a dirty way to do this
            _map = map;
            _interpolator.Teleport(Position);

            _characterSprite = CreateCharacterSprite(this, this, skelManager);

            CharacterSprite.SetSet(BodyInfo.Stand, BodyInfo.Size);
            CharacterSprite.SetBody(BodyInfo.Body);
        }

        /// <summary>
        /// Sets the character's heading.
        /// </summary>
        /// <param name="newHeading">New heading for the character.</param>
        public override void SetHeading(Direction newHeading)
        {
            if (Heading == newHeading)
                return;

            base.SetHeading(newHeading);
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, int deltaTime)
        {
            base.HandleUpdate(imap, deltaTime);

            // Get the delta time
            var currentTime = GetTime();
            var lastDrawnDelta = Math.Min(currentTime - _lastDrawnTime, GameData.MaxDrawDeltaTime);
            _lastDrawnTime = currentTime;

            // Update the sprite
            if (BodyInfo != null)
                _characterSprite.Paperdoll = BodyInfo.Paperdoll;

            _characterSprite.Update(currentTime);

            UpdateAnimation();

            // Update the interpolation
            _interpolator.Update(this, (int)lastDrawnDelta);
        }

        /// <summary>
        /// Gets or sets the IDs of the quests provided by this <see cref="Character"/>.
        /// </summary>
        public IEnumerable<QuestID> ProvidedQuests
        {
            get { return _providedQuests; }
            set { _providedQuests = value ?? _emptyQuestIDs; }
        }

        /// <summary>
        /// Updates the character's sprites.
        /// Only used in sidescroller builds.
        /// </summary>
        protected virtual void UpdateAnimation()
        {
#if SKELETON_SPRITES
            // Only update if the state has changed
            if (_lastState == State)
                return;

            _lastState = State;

            switch (State)
            {
                case CharacterState.Idle:
                    CharacterSprite.SetSet(BodyInfo.Stand, BodyInfo.Size);
                    break;

                case CharacterState.Falling:
                case CharacterState.FallingLeft:
                case CharacterState.FallingRight:
                    CharacterSprite.SetSet(BodyInfo.Fall, BodyInfo.Size);
                    break;

                case CharacterState.Jumping:
                case CharacterState.JumpingLeft:
                case CharacterState.JumpingRight:
                    CharacterSprite.SetSet(BodyInfo.Jump, BodyInfo.Size);
                    break;

                case CharacterState.WalkingLeft:
                case CharacterState.WalkingRight:
                    CharacterSprite.SetSet(BodyInfo.Walk, BodyInfo.Size);
                    break;
            }
#endif
        }

        #region IDrawable Members

        /// <summary>
        /// Gets the depth of the object for the <see cref="IDrawable.MapRenderLayer"/> the object is on. A higher
        /// layer depth results in the object being drawn on top of (in front of) objects with a lower value.
        /// </summary>
        [Browsable(false)]
        public virtual int LayerDepth
        {
            get
            {
                // Just put all characters on the same depth of 0 and let them be drawn "as they appear"
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IDrawable.Color"/> to use when drawing this <see cref="IDrawable"/>. By default, this
        /// value will be equal to white (ARGB: 255,255,255,255).
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color == value)
                    return;

                _color = value;

                if (ColorChanged != null)
                    ColorChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.Color"/> property has changed.
        /// </summary>
        public event TypedEventHandler<IDrawable> ColorChanged;

        /// <summary>
        /// Gets or sets if this <see cref="IDrawable"/> will be drawn. All <see cref="IDrawable"/>s are initially
        /// visible.
        /// </summary>
        [Browsable(false)]
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible == value)
                    return;

                _isVisible = value;

                if (VisibleChanged != null)
                    VisibleChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Notifies listeners immediately after this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> AfterDraw;

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(ISpriteBatch sb)
        {
            if (BeforeDraw != null)
                BeforeDraw.Raise(this, EventArgsHelper.Create(sb));

            var drawPos = DrawPosition;
            _lastScreenPosition = drawPos - Parent.Camera.Min;

            if (IsVisible && !Invisible)
            {
                // Draw the character body
                _characterSprite.Draw(sb, drawPos, Heading, Color);

                // Draw the HP/MP
                DrawSPBar(sb, HPPercent, 0, new Color(255, 0, 0, 175));
                DrawSPBar(sb, MPPercent, 1, new Color(0, 0, 255, 175));

                // Draw the name
                DrawName(sb, NameFont);
            }

            if (AfterDraw != null)
                AfterDraw.Raise(this, EventArgsHelper.Create(sb));
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>
        /// True if the object is in view of the camera, else False.
        /// </returns>
        public bool InView(ICamera2D camera)
        {
            return camera.InView(this);
        }

        /// <summary>
        /// Unused by the <see cref="Character"/> since the layer never changes.
        /// </summary>
        event TypedEventHandler<IDrawable, ValueChangedEventArgs<MapRenderLayer>> IDrawable.RenderLayerChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that this object is rendered on.
        /// </summary>
        public MapRenderLayer MapRenderLayer
        {
            get
            {
                // Characters are always on the Dynamic layer
                return MapRenderLayer.Dynamic;
            }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.IsVisible"/> property has changed.
        /// </summary>
        public event TypedEventHandler<IDrawable> VisibleChanged;

        /// <summary>
        /// Notifies listeners immediately before this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> BeforeDraw;

        #endregion

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public TickCount GetTime()
        {
            return _map.GetTime();
        }

        #endregion
    }
}