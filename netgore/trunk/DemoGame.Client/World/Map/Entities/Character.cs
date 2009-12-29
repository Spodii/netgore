using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using IDrawable=NetGore.Graphics.IDrawable;

namespace DemoGame.Client
{
    /// <summary>
    /// Represents a single Character on the Client.
    /// </summary>
    public class Character : CharacterEntity, IGetTime, IDrawable
    {
        readonly EntityInterpolator _interpolator = new EntityInterpolator();
        ICharacterSprite _characterSprite;

        bool _hasChatDialog;
        bool _hasShop;
        int _lastDrawnTime;
#if !TOPDOWN
        CharacterState _lastState = CharacterState.Idle;
#endif
        Map _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterEntity"/> class.
        /// </summary>
        public Character() : base(Vector2.Zero, Vector2.One)
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
        /// When overridden in the derived class, gets or protected sets if the CharacterEntity
        /// has a chat dialog. The setter for this method should never be called directly.
        /// </summary>
        public override bool HasChatDialog
        {
            get { return _hasChatDialog; }
            protected set { _hasChatDialog = value; }
        }

        /// <summary>
        /// When overridden in the derived class, gets or protected sets if the CharacterEntity
        /// has a shop. The setter for this method should never be called directly.
        /// </summary>
        public override bool HasShop
        {
            get { return _hasShop; }
            protected set { _hasShop = value; }
        }

        /// <summary>
        /// Gets or sets the percent of HP the Character has remaining, where 0 is 0% and 100 is 100%.
        /// </summary>
        public byte HPPercent { get; set; }

        /// <summary>
        /// Gets or sets the percent of MP the Character has remaining, where 0 is 0% and 100 is 100%.
        /// </summary>
        public byte MPPercent { get; set; }

        /// <summary>
        /// Gets or sets the SpriteFont used to write the Character's name. If null, the names will not be drawn.
        /// </summary>
        public static SpriteFont NameFont { get; set; }

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
        public void Attack()
        {
            CharacterSprite.AddBodyModifier(BodyInfo.Punch);
        }

        /// <summary>
        /// Draws the Character's name.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to.</param>
        void DrawName(SpriteBatch sb)
        {
            SpriteFont font = NameFont;
            if (font != null && !string.IsNullOrEmpty(Name))
            {
                Vector2 nameSize = GetNameSize();
                Vector2 namePos = DrawPosition + new Vector2(Size.X / 2, 0) - new Vector2(nameSize.X / 2f, nameSize.Y);
                sb.DrawStringShaded(font, Name, namePos, Color.Green, Color.Black);
            }
        }

        /// <summary>
        /// Draws a bar for the Character's SP.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="percent">The percent of the SP being drawn.</param>
        /// <param name="index">The 0-based index of the bar being drawn.</param>
        /// <param name="color">The color to draw the bar.</param>
        void DrawSPBar(SpriteBatch sb, byte percent, byte index, Color color)
        {
            const float spBarWidth = 55;
            const float spBarHeight = 6;

            Vector2 pos = DrawPosition + new Vector2((CB.Size.X / 2f) - (spBarWidth / 2f),
                                      CB.Size.Y + (spBarHeight * index));

            Rectangle border = new Rectangle((int)pos.X, (int)pos.Y, (int)spBarWidth, (int)spBarHeight);
            Rectangle bar = border;
            bar.Width = (int)((spBarWidth * (percent / 100.0f))).Clamp(0.0f, spBarWidth);

            XNARectangle.Draw(sb, border, new Color(0, 0, 0, 0), Color.Black);
            XNARectangle.Draw(sb, bar, color);
        }

        /// <summary>
        /// Gets the position for the Camera to focus on this Character.
        /// </summary>
        /// <returns>Position for the Camera to focus on this Character.</returns>
        public Vector2 GetCameraPos()
        {
            Vector2 p1 = DrawPosition + (Size / 2.0f) - (GameData.ScreenSize / 2.0f);
            return new Vector2((float)Math.Round(p1.X), (float)Math.Round(p1.Y));
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
        /// Gets the size of the Character's name string.
        /// </summary>
        /// <returns>The size of the Character's name string.</returns>
        Vector2 GetNameSize()
        {
            return NameFont.MeasureString(Name);
        }

        /// <summary>
        /// Initializes the Character.
        /// </summary>
        /// <param name="map">The Map to place the Character on.</param>
        /// <param name="skelManager">The SkeletonManager to use for the Character's skeletons.</param>
        public void Initialize(Map map, SkeletonManager skelManager)
        {
            // HACK: This is quite a dirty way to do this
            _map = map;
            _interpolator.Teleport(Position);

#if !TOPDOWN
            _characterSprite = new SkeletonCharacterSprite(this, skelManager, GameData.AnimationSpeedModifier);
#else
            _characterSprite = new BasicGrhCharacterSprite(this, "Character.Top Down");
#endif

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

#if !TOPDOWN
        /// <summary>
        /// Updates the character's sprites.
        /// </summary>
        void UpdateAnimation()
        {
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
        }
#endif

        #region IDrawable Members

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(SpriteBatch sb)
        {
            // Get the delta time
            int currentTime = GetTime();
            int deltaTime = Math.Min(currentTime - _lastDrawnTime, GameData.MaxDrawDeltaTime);
            _lastDrawnTime = currentTime;

            // Update the drawable stuff
            _characterSprite.Update(currentTime);
            _interpolator.Update(this, deltaTime);

#if !TOPDOWN
            UpdateAnimation();
#endif

            // Draw the character body
            _characterSprite.Draw(sb, DrawPosition, Heading);

            // Draw the HP/MP
            DrawSPBar(sb, HPPercent, 0, new Color(255, 0, 0, 175));
            DrawSPBar(sb, MPPercent, 1, new Color(0, 0, 255, 175));

            // Draw the name
            DrawName(sb);
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
        /// Unused by the Character.
        /// </summary>
        public event MapRenderLayerChange OnChangeRenderLayer
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
                // Characters are always on the Character layer
                return MapRenderLayer.Chararacter;
            }
        }

        #endregion

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public int GetTime()
        {
            return _map.GetTime();
        }

        #endregion
    }
}