using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Provides an instance of a single sprite defined by a <see cref="GrhData"/>.
    /// </summary>
    public class Grh : ISprite
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Type of animation the Grh uses
        /// </summary>
        AnimType _anim = AnimType.Loop;

        /// <summary>
        /// Current frame (if animated)
        /// </summary>
        float _frame = 0;

        /// <summary>
        /// Root GrhData referenced by this Grh
        /// </summary>
        GrhData _grhData;

        /// <summary>
        /// Tick count at which the Grh was last updated (only needed if animated)
        /// </summary>
        TickCount _lastUpdated = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Grh"/> class.
        /// </summary>
        /// <param name="grhIndex">Index of the stationary Grh.</param>
        public Grh(GrhIndex grhIndex)
        {
            SetGrh(grhIndex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grh"/> class.
        /// </summary>
        /// <param name="grhData">GrhData to create from.</param>
        public Grh(GrhData grhData)
        {
            SetGrh(grhData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grh"/> class.
        /// </summary>
        public Grh()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grh"/> class.
        /// </summary>
        /// <param name="grhIndex">Index of the Grh.</param>
        /// <param name="anim">Animation type.</param>
        /// <param name="currentTime">Current time.</param>
        public Grh(GrhIndex grhIndex, AnimType anim, TickCount currentTime)
        {
            SetGrh(grhIndex, anim, currentTime);
        }

        /// <summary>
        /// Creates a Grh.
        /// </summary>
        /// <param name="grhData">GrhData to create from.</param>
        /// <param name="anim">Animation type.</param>
        /// <param name="currentTime">Current time.</param>
        public Grh(GrhData grhData, AnimType anim, TickCount currentTime)
        {
            SetGrh(grhData, anim, currentTime);
        }

        /// <summary>
        /// Gets or sets the animation type for the Grh.
        /// </summary>
        public AnimType AnimType
        {
            get { return _anim; }
            set { _anim = value; }
        }

        /// <summary>
        /// Gets the GrhData to use for drawing based on the current frame. 
        /// </summary>
        public StationaryGrhData CurrentGrhData
        {
            get
            {
                if (GrhData == null)
                    return null;

                return GrhData.GetFrame((int)_frame);
            }
        }

        /// <summary>
        /// Gets the current frame of the animation.
        /// </summary>
        public float Frame
        {
            get { return _frame; }
        }

        /// <summary>
        /// Gets the root GrhData referenced by this Grh.
        /// </summary>
        public GrhData GrhData
        {
            get { return _grhData; }
        }

        /// <summary>
        /// Gets the tick count at which the Grh was last updated (only for animated Grhs).
        /// </summary>
        public TickCount LastUpdated
        {
            get { return _lastUpdated; }
        }

        /// <summary>
        /// Performs a detailed check to ensure the Grh can be drawn without problem. This should be called before
        /// any drawing is done!
        /// </summary>
        /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <returns>True if it is safe for the Grh to draw to the <paramref name="spriteBatch"/>, else false.</returns>
        bool CanDrawGrh(ISpriteBatch spriteBatch)
        {
            // Invalid GrhData
            if (GrhData == null)
            {
                const string errmsg = "Failed to render Grh - GrhData is null!";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                return false;
            }

            // Invalid texture
            if (Texture == null)
            {
                if (log.IsWarnEnabled)
                {
                    var sgd = GrhData as StationaryGrhData;
                    if (sgd == null)
                    {
                        const string errmsg =
                            "Failed to render Grh `{0}` - GrhData `{1}` is of type `{2}` instead of the expected type `{3}`!";
                        log.ErrorFormat(errmsg, this, GrhData, GrhData.GetType(), typeof(StationaryGrhData));
                    }
                    else
                    {
                        const string errmsg = "Failed to render Grh `{0}` - GrhData returning null texture for `{1}`!";
                        log.WarnFormat(errmsg, this, sgd.TextureName);
                    }
                }
                return false;
            }

            // Invalid SpriteBatch
            if (spriteBatch == null)
            {
                const string errmsg = "Failed to render Grh `{0}` - SpriteBatch is null!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                return false;
            }

            if (spriteBatch.IsDisposed)
            {
                const string errmsg = "Failed to render Grh `{0}` - SpriteBatch is disposed!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                return false;
            }

            // All is good
            return true;
        }

        /// <summary>
        /// Creates a duplicate (deep copy) of the <see cref="Grh"/>.
        /// </summary>
        /// <returns>The deep copy of the <see cref="Grh"/>.</returns>
        public Grh DeepCopy()
        {
            return new Grh(_grhData, _anim, _lastUpdated) { _frame = _frame };
        }

        /// <summary>
        /// Sets the Grh to a new index.
        /// </summary>
        /// <param name="grhIndex">New Grh index to use for the stationary Grh.</param>
        public void SetGrh(GrhIndex grhIndex)
        {
            SetGrh(grhIndex, AnimType, LastUpdated);
        }

        /// <summary>
        /// Sets the Grh to a new index.
        /// </summary>
        /// <param name="grhData">New GrhData to use for the Grh.</param>
        /// <param name="anim">Type of animation.</param>
        /// <param name="currentTime">Current time.</param>
        public void SetGrh(GrhData grhData, AnimType anim, TickCount currentTime)
        {
            _grhData = grhData;
            _frame = 0;
            _anim = anim;
            _lastUpdated = currentTime;
        }

        /// <summary>
        /// Sets the Grh to a new index.
        /// </summary>
        /// <param name="grhData">New GrhData to use for the stationary Grh.</param>
        public void SetGrh(GrhData grhData)
        {
            if (GrhData == grhData)
                return;

            SetGrh(grhData, AnimType, LastUpdated);
        }

        /// <summary>
        /// Sets the Grh to a new index.
        /// </summary>
        /// <param name="grhIndex">New Grh index to use.</param>
        /// <param name="anim">Type of animation.</param>
        /// <param name="currentTime">Current time.</param>
        public void SetGrh(GrhIndex grhIndex, AnimType anim, TickCount currentTime)
        {
            var grhData = GrhInfo.GetData(grhIndex);
            if (grhData == null && grhIndex != 0)
            {
                const string errmsg = "Failed to set Grh - GrhIndex `{0}` does not exist.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, grhIndex);
                return;
            }

            SetGrh(grhData, anim, currentTime);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Grh: " + (GrhData != null ? GrhData.ToString() : "(No GrhData loaded)");
        }

        /// <summary>
        /// Updates the current frame.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void UpdateFrameIndex(TickCount currentTime)
        {
            var elapsedTime = currentTime - _lastUpdated;
            Debug.Assert(elapsedTime >= 0, "How is the elapsed time negative? Did the computer fall into a wormhole?");
            if (elapsedTime <= 0)
                return;

            // Store the temporary new frame
            var tmpFrame = _frame + (elapsedTime * GrhData.Speed);

            // Check if the frame limit has been exceeded
            if (tmpFrame >= GrhData.FramesCount)
            {
                if (_anim == AnimType.LoopOnce)
                {
                    // The animation was only looping once, so end it and set at the first frame
                    _anim = AnimType.None;
                    _frame = 0;
                    return;
                }
                else
                {
                    // Animation is looping so get the frame back into range
                    tmpFrame = tmpFrame % GrhData.FramesCount;
                }
            }

            // Set the new frame
            _frame = tmpFrame;
        }

        #region ISprite Members

        /// <summary>
        /// Gets the size of the current frame in pixels.
        /// </summary>
        public Vector2 Size
        {
            get { return CurrentGrhData != null ? CurrentGrhData.Size : Vector2.Zero; }
        }

        /// <summary>
        /// Gets the source rectangle for the current frame.
        /// </summary>
        public Rectangle Source
        {
            get
            {
                var asStationary = CurrentGrhData;
                if (asStationary == null)
                    return Rectangle.Empty;

                return asStationary.SourceRect;
            }
        }

        /// <summary>
        /// Gets the texture for the current frame.
        /// </summary>
        public Texture Texture
        {
            get
            {
                var asStationary = CurrentGrhData;
                if (asStationary == null)
                    return null;

                return asStationary.Texture;
            }
        }

        /// <summary>
        /// Draws the current frame of the <see cref="Grh"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        public void Draw(ISpriteBatch sb, Vector2 dest)
        {
            if (!CanDrawGrh(sb))
                return;

            sb.Draw(Texture, dest, Source, Color.White);
        }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="dest">Destination to draw the sprite.</param>
        public void Draw(ISpriteBatch sb, Rectangle dest)
        {
            if (!CanDrawGrh(sb))
                return;

            sb.Draw(Texture, dest, Source, Color.White);
        }

        /// <summary>
        /// Draws the current frame of the <see cref="Grh"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        public void Draw(ISpriteBatch sb, Vector2 dest, Color color, SpriteEffects effect)
        {
            if (!CanDrawGrh(sb))
                return;

            sb.Draw(Texture, dest, Source, color, 0, Vector2.Zero, 1.0f, effect);
        }

        /// <summary>
        /// Draws the current frame of the <see cref="Grh"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin (default 0).</param>
        /// <param name="origin">The origin of the sprite to rotate around (default Vector2.Zero).</param>
        /// <param name="scale">Uniform multiply by which to scale the width and height.</param>
        public void Draw(ISpriteBatch sb, Vector2 dest, Color color, SpriteEffects effect, float rotation, Vector2 origin,
                         float scale)
        {
            if (!CanDrawGrh(sb))
                return;

            sb.Draw(Texture, dest, Source, color, rotation, origin, scale, effect);
        }

        /// <summary>
        /// Draws the current frame of the <see cref="Grh"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin (default 0).</param>
        /// <param name="origin">The origin of the sprite to rotate around (default Vector2.Zero).</param>
        /// <param name="scale">Vector2 defining the scale.</param>
        public void Draw(ISpriteBatch sb, Vector2 dest, Color color, SpriteEffects effect, float rotation, Vector2 origin,
                         Vector2 scale)
        {
            if (!CanDrawGrh(sb))
                return;

            sb.Draw(Texture, dest, Source, color, rotation, origin, scale, effect);
        }

        /// <summary>
        /// Draws the current frame of the <see cref="Grh"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin (default 0).</param>
        /// <param name="origin">The origin of the sprite to rotate around (default Vector2.Zero).</param>
        public void Draw(ISpriteBatch sb, Rectangle dest, Color color, SpriteEffects effect, float rotation, Vector2 origin)
        {
            if (!CanDrawGrh(sb))
                return;

            sb.Draw(Texture, dest, Source, color, rotation, origin, effect);
        }

        /// <summary>
        /// Draws the current frame of the <see cref="Grh"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        public void Draw(ISpriteBatch sb, Vector2 dest, Color color)
        {
            if (!CanDrawGrh(sb))
                return;

            sb.Draw(Texture, dest, Source, color);
        }

        /// <summary>
        /// Draws the current frame of the <see cref="Grh"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Destination rectangle to draw to.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        public void Draw(ISpriteBatch sb, Rectangle dest, Color color)
        {
            if (!CanDrawGrh(sb))
                return;

            sb.Draw(Texture, dest, Source, color);
        }

        /// <summary>
        /// Updates the Grh if it is animated.
        /// </summary>
        /// <param name="currentTime">Current total real time in total milliseconds.</param>
        public virtual void Update(TickCount currentTime)
        {
            // We only need to update the frame if we are animating, have a valid GrhData, and if we have more
            // than one frame in the GrhData
            if (_anim != AnimType.None && GrhData != null && GrhData.FramesCount > 0)
                UpdateFrameIndex(currentTime);

            // Set the last updated time to now
            _lastUpdated = currentTime;
        }

        #endregion
    }
}