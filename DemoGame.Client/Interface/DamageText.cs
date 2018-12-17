using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Collections;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Represents the text used to display the damage to an entity
    /// </summary>
    public class DamageText : IPoolable
    {
        /// <summary>
        /// Gravity to apply to the text (has nothing to do with the World's gravity).
        /// </summary>
        static readonly Vector2 _gravity = new Vector2(0, 0.05f);

        /// <summary>
        /// Random number generator.
        /// </summary>
        static readonly SafeRandom _random = new SafeRandom();

        /// <summary>
        /// Current alpha of the text.
        /// </summary>
        float _alpha;

        /// <summary>
        /// Time at which the text was last updated.
        /// </summary>
        TickCount _lastUpdate;

        /// <summary>
        /// Current position of the text.
        /// </summary>
        Vector2 _pos;

        /// <summary>
        /// The text to draw.
        /// </summary>
        string _text;

        /// <summary>
        /// Current velocity of the text.
        /// </summary>
        Vector2 _velocity;

        /// <summary>
        /// Gets the alpha value used when drawing the DamageText.
        /// </summary>
        public float Alpha
        {
            get { return _alpha; }
        }

        /// <summary>
        /// Activates the DamageText.
        /// </summary>
        /// <param name="damage">Damage value to display.</param>
        /// <param name="entity">Entity the damage was done to.</param>
        /// <param name="currTime">Current time.</param>
        public void Activate(int damage, Entity entity, TickCount currTime)
        {
            if (entity == null)
            {
                Debug.Fail("entity is null.");
                return;
            }

            // Set the starting values
            _alpha = 255;
            _pos = entity.Position;
            _lastUpdate = currTime;
            _text = damage.ToString();

            // Get a random velocity
            _velocity = new Vector2(-1.0f + (float)_random.NextDouble() * 2.0f, -2.0f - (float)_random.NextDouble() * 0.25f);
        }

        /// <summary>
        /// Draws the <see cref="DamageText"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="font"><see cref="Font"/> to draw with.</param>
        public void Draw(ISpriteBatch sb, Font font)
        {
            sb.DrawString(font, _text, _pos, new Color(255, 255, 255, (byte)_alpha));
        }

        /// <summary>
        /// Updates the <see cref="DamageText"/>.
        /// </summary>
        /// <param name="currTime">Current time.</param>
        public void Update(TickCount currTime)
        {
            // Get the delta time
            float delta = currTime - _lastUpdate;
            _lastUpdate = currTime;
            delta *= 0.2f;

            // Update the alpha
            _alpha -= delta;

            // Update the position and velocity
            _pos += _velocity * delta;
            _velocity += _gravity * delta;
        }

        #region IPoolable Members

        /// <summary>
        /// Gets or sets the index of the object in the pool. This value should never be used by anything
        /// other than the pool that owns this object.
        /// </summary>
        int IPoolable.PoolIndex { get; set; }

        #endregion
    }
}