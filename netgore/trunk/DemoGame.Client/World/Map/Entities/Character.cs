using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.IO;
using IDrawable=NetGore.Graphics.IDrawable;

namespace DemoGame.Client
{
    /// <summary>
    /// Represents a single Character on the Client.
    /// </summary>
    public class Character : CharacterEntity, IGetTime, IDrawable
    {
        readonly EntityInterpolator _interpolator = new EntityInterpolator();
        string _currSkelSet;
        bool _hasChatDialog;
        bool _hasShop;

        /// <summary>
        /// The time that Draw() was last called.
        /// </summary>
        int _lastDrawnTime;

        CharacterState _lastState = CharacterState.Idle;
        Map _map;
        EventHandler _onLoopHandler;
        SkeletonAnimation _skelAnim = null;
        SkeletonManager _skelManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterEntity"/> class.
        /// </summary>
        public Character() : base(Vector2.Zero, Vector2.One)
        {
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
            SkeletonSet set = _skelManager.LoadSet(BodyInfo.Punch, ContentPaths.Build);
            set = SkeletonAnimation.CreateSmoothedSet(set, _skelAnim.Skeleton);
            SkeletonAnimation mod = new SkeletonAnimation(GetTime(), set);
            _skelAnim.AddModifier(mod);
        }

        /// <summary>
        /// Changes the SkeletonSet used if different.
        /// </summary>
        /// <param name="setName">Name of the set to use.</param>
        void ChangeSet(string setName)
        {
            // Check that the set has changed
            if (setName == _currSkelSet)
                return;

            SkeletonSet newSet = _skelManager.LoadSet(setName, ContentPaths.Build);
            _skelAnim.ChangeSet(newSet);
            _currSkelSet = setName;
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

            Vector2 pos = new Vector2(DrawPosition.X + (CB.Width / 2f) - (spBarWidth / 2f),
                                      DrawPosition.Y + CB.Height + (spBarHeight * index));

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
            _skelManager = skelManager;
            _onLoopHandler = skelAnim_OnLoop;
            _interpolator.Teleport(Position);

            // Set up the skeleton
            _currSkelSet = BodyInfo.Stand;
            SkeletonSet newSet = _skelManager.LoadSet(_currSkelSet, ContentPaths.Build);
            _skelAnim = new SkeletonAnimation(GetTime(), newSet);
            SkeletonBodyInfo bodyInfo = _skelManager.LoadBodyInfo(BodyInfo.Body, ContentPaths.Build);
            _skelAnim.SkeletonBody = new SkeletonBody(bodyInfo, _skelAnim.Skeleton);
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

        public void SetPaperDoll(IEnumerable<string> layers)
        {
            _skelAnim.BodyLayers.Clear();
            foreach (var layer in layers)
            {
                var bodyInfo = _skelManager.LoadBodyInfo(layer, ContentPaths.Build);
                if (bodyInfo == null)
                    continue;

                _skelAnim.BodyLayers.Add(new SkeletonBody(bodyInfo, _skelAnim.Skeleton));
            }
        }

        /// <summary>
        /// Handles OnLoop events for the SkeletonAnimation to set the character's animation back to normal.
        /// </summary>
        void skelAnim_OnLoop(object sender, EventArgs e)
        {
            _skelAnim.OnLoop -= _onLoopHandler;
            UpdateAnimation();
        }

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
                    ChangeSet(BodyInfo.Stand);
                    break;

                case CharacterState.Falling:
                case CharacterState.FallingLeft:
                case CharacterState.FallingRight:
                    ChangeSet(BodyInfo.Fall);
                    break;

                case CharacterState.Jumping:
                case CharacterState.JumpingLeft:
                case CharacterState.JumpingRight:
                    ChangeSet(BodyInfo.Jump);
                    break;

                case CharacterState.WalkingLeft:
                case CharacterState.WalkingRight:
                    ChangeSet(BodyInfo.Walk);
                    break;
            }
        }

        /// <summary>
        /// The velocity to assume all character movement animations are made at. That is, if the character is moving
        /// with a velocity equal to this value, the animation will update at the usual speed. If it is twice as much
        /// as this value, the character's animation will update twice as fast. This is to make the rate a character
        /// moves proportionate to the rate their animation is moving.
        /// </summary>
        const float _animationSpeedModifier = 0.13f;

        #region IDrawable Members

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(SpriteBatch sb)
        {
            if (_skelAnim == null)
                return;

            // Get the delta time
            int currentTime = GetTime();
            int deltaTime = Math.Min(currentTime - _lastDrawnTime, GameData.MaxDrawDeltaTime);
            _lastDrawnTime = currentTime;

            // Update the drawable stuff
            UpdateAnimation();
            _skelAnim.Update(currentTime);
            _interpolator.Update(this, deltaTime);

            // Update the animation's speed
            if (Velocity.X != 0)
                _skelAnim.Speed = Math.Abs(Velocity.X) / _animationSpeedModifier;
            else
                _skelAnim.Speed = 1.0f;

            // Draw the character body
            SpriteEffects se = (Heading == Direction.East ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            Vector2 p = DrawPosition;
            p.X += BodyInfo.Width / 2f;
            p.Y += BodyInfo.Height;

            _skelAnim.Draw(sb, p, se);

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