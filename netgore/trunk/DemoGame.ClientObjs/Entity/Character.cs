using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Collections;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Client character
    /// </summary>
    public class Character : CharacterEntity, IGetTime, IDrawableEntity
    {
        readonly BodyInfo _bodyInfo;
        readonly Map _map;
        readonly MeanStack<Vector2> _ms = new MeanStack<Vector2>(3, MeanStackExtras.Mean);
        readonly EventHandler _onLoopHandler;
        readonly SkeletonAnimation _skelAnim = null;
        readonly SkeletonManager _skelManager;

        string _currSkelSet;
        Vector2 _drawPos;
        CharacterState _lastState = CharacterState.Idle;

        /// <summary>
        /// Gets the location at which the character is to be drawn
        /// </summary>
        public Vector2 DrawPosition
        {
            get { return _drawPos; }
        }

        /// <summary>
        /// Gets the name of the character
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the map the character belongs to
        /// </summary>
        public Map Parent
        {
            get { return _map; }
        }

        /// <summary>
        /// Character constructor
        /// </summary>
        /// <param name="map">Map the character belongs to</param>
        /// <param name="position">Position of the character</param>
        /// <param name="bodyInfo">BodyInfo defining the character's body</param>
        /// <param name="skelManager">SkeletonManager used to load skeleton information</param>
        public Character(Map map, Vector2 position, BodyInfo bodyInfo, SkeletonManager skelManager)
        {
            _map = map;
            _skelManager = skelManager;
            _bodyInfo = bodyInfo;
            _ms.Fill(position);
            _onLoopHandler = skelAnim_OnLoop;

            // Set up the skeleton
            _currSkelSet = _bodyInfo.Stand;
            _skelAnim = new SkeletonAnimation(GetTime(), _skelManager.LoadSet(_currSkelSet));
            _skelAnim.SkeletonBody = new SkeletonBody(_skelManager.LoadBodyInfo(_bodyInfo.Body), _skelAnim.Skeleton);

            // Set the collision box and position
            CB = new CollisionBox(_bodyInfo.Width, _bodyInfo.Height);
            Position = position;
        }

        /// <summary>
        /// Makes the character show an attack
        /// </summary>
        public void Attack()
        {
            SkeletonSet set = _skelManager.LoadSet(_bodyInfo.Punch);
            set = SkeletonAnimation.CreateSmoothedSet(set, _skelAnim.Skeleton);
            SkeletonAnimation mod = new SkeletonAnimation(GetTime(), set);
            _skelAnim.AddModifier(mod);
        }

        /// <summary>
        /// Changes the SkeletonSet used if different
        /// </summary>
        /// <param name="setName">Name of the set to use</param>
        void ChangeSet(string setName)
        {
            // Check that the set has changed
            if (setName == _currSkelSet)
                return;

            _skelAnim.ChangeSet(_skelManager.LoadSet(setName));
            _currSkelSet = setName;
        }

        /// <summary>
        /// Gets the position for the Camera to focus on this Character
        /// </summary>
        /// <returns>Position for the Camera to focus on this Character</returns>
        public Vector2 GetCameraPos()
        {
            Vector2 p1 = DrawPosition + (Size / 2) - (GameData.ScreenSize / 2);
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
        /// Rounds a Vector2's values
        /// </summary>
        /// <param name="v">Vector2 to round</param>
        static void Round(ref Vector2 v)
        {
            v.X = (float)Math.Round(v.X);
            v.Y = (float)Math.Round(v.Y);
        }

        /// <summary>
        /// Sets the character's heading
        /// </summary>
        /// <param name="newHeading">New heading for the character</param>
        public override void SetHeading(Direction newHeading)
        {
            if (Heading == newHeading)
                return;

            base.SetHeading(newHeading);
        }

        /// <summary>
        /// Handles OnLoop events for the SkeletonAnimation to set the character's animation back to normal
        /// </summary>
        void skelAnim_OnLoop(object sender, EventArgs e)
        {
            _skelAnim.OnLoop -= _onLoopHandler;
            UpdateAnimation();
        }

        /// <summary>
        /// Teleports the character to a new location without any smooth translation
        /// from the last position like with UpdatePosition(). Intended to be used
        /// in cases where the character needs to be instantly put at a location
        /// such as character creation, teleportation spells, etc.
        /// </summary>
        /// <param name="position">Location to teleport the character</param>
        public override void Teleport(Vector2 position)
        {
            _ms.Fill(position);
            _drawPos = position;
            base.Teleport(position);
        }

        /// <summary>
        /// Updates the character
        /// </summary>
        public override void Update(IMap imap, float deltaTime)
        {
            base.Update(imap, deltaTime);

            // Update the animation if the state has changed
            UpdateAnimation();

            // Update the skeleton
            if (_skelAnim != null)
                _skelAnim.Update(GetTime());

            // Update the drawing position
            UpdateDrawPos();
        }

        /// <summary>
        /// Updates the character's sprites
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
                    ChangeSet(_bodyInfo.Stand);
                    break;

                case CharacterState.Falling:
                case CharacterState.FallingLeft:
                case CharacterState.FallingRight:
                    ChangeSet(_bodyInfo.Fall);
                    break;

                case CharacterState.Jumping:
                case CharacterState.JumpingLeft:
                case CharacterState.JumpingRight:
                    ChangeSet(_bodyInfo.Jump);
                    break;

                case CharacterState.WalkingLeft:
                case CharacterState.WalkingRight:
                    ChangeSet(_bodyInfo.Walk);
                    break;
            }
        }

        /// <summary>
        /// Interpolates the drawing position to the real position to help reduce skips
        /// </summary>
        void UpdateDrawPos()
        {
            // Push the current position onto the MeanStack
            _ms.Push(Position);

            // Get the mean position over the last few positions and round it
            _drawPos = _ms.Mean();
            Round(ref _drawPos);
        }

        #region IDrawableEntity Members

        /// <summary>
        /// Draws the character
        /// </summary>
        /// <param name="sb">SpriteBatch to draw the character with</param>
        public void Draw(SpriteBatch sb)
        {
            // Draw the character body
            if (_skelAnim != null)
            {
                SpriteEffects se = (Heading == Direction.East ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
                Vector2 p = DrawPosition;
                p.X += _bodyInfo.Width / 2f;
                p.Y += _bodyInfo.Height;
                _skelAnim.Draw(sb, p, se);
            }
        }

        /// <summary>
        /// Checks if in view of the specified camera
        /// </summary>
        /// <param name="camera">Camera to check if in view of</param>
        /// <returns>True if in view of the camera, else false</returns>
        public bool InView(Camera2D camera)
        {
            return camera.InView(this);
        }

        /// <summary>
        /// Notifies when the Character's render layer changes (which is never for Characters)
        /// </summary>
        public event MapRenderLayerChange OnChangeRenderLayer
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Gets the MapRenderLayer for the Character
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
        /// Gets the current game time where time 0 is when the application started
        /// </summary>
        /// <returns>Current game time in milliseconds</returns>
        public int GetTime()
        {
            return _map.GetTime();
        }

        #endregion
    }
}