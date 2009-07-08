using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Collections;

namespace DemoGame
{
    /// <summary>
    /// Represents the text used to display the damage to an entity
    /// </summary>
    public class DamageText : IPoolable<DamageText>
    {
        /// <summary>
        /// Gravity to apply to the text (has nothing to do with the World's gravity)
        /// </summary>
        static readonly Vector2 _gravity = new Vector2(0, 0.05f);

        /// <summary>
        /// Random number generator
        /// </summary>
        static readonly Random _random = new Random();

        /// <summary>
        /// Current alpha of the text
        /// </summary>
        float _alpha;

        /// <summary>
        /// Time at which the text was last updated
        /// </summary>
        int _lastUpdate;

        PoolData<DamageText> _poolData;

        /// <summary>
        /// Current position of the text
        /// </summary>
        Vector2 _pos;

        /// <summary>
        /// The text to draw
        /// </summary>
        string _text;

        /// <summary>
        /// Current velocity of the text
        /// </summary>
        Vector2 _velocity;

        /// <summary>
        /// Gets the alpha value used when drawing the DamageText
        /// </summary>
        public float Alpha
        {
            get { return _alpha; }
        }

        /// <summary>
        /// Activates the DamageText
        /// </summary>
        /// <param name="damage">Damage value to display</param>
        /// <param name="entity">Entity the damage was done to</param>
        /// <param name="currTime">Current time</param>
        public void Activate(int damage, Entity entity, int currTime)
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
        /// Draws the DamageText
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="sf">SpriteFont to draw with</param>
        public void Draw(SpriteBatch sb, SpriteFont sf)
        {
            // Do not draw if it is not alive
            if (!_poolData.IsActivated)
            {
                Debug.Fail("Tried to update inactivated pool item.");
                return;
            }

            sb.DrawString(sf, _text, _pos, new Color(255, 255, 255, (byte)_alpha));
        }

        /// <summary>
        /// Updates the DamageText
        /// </summary>
        /// <param name="currTime">Current time</param>
        public void Update(int currTime)
        {
            // Do not update if it is not alive
            if (!_poolData.IsActivated)
            {
                Debug.Fail("Tried to update inactivated pool item.");
                return;
            }

            // Get the delta time
            float delta = currTime - _lastUpdate;
            _lastUpdate = currTime;
            delta *= 0.1f;

            // Update the alpha
            _alpha -= delta;

            // Update the position and velocity
            _pos += _velocity * delta;
            _velocity += _gravity * delta;
        }

        #region IPoolable<DamageText> Members

        PoolData<DamageText> IPoolable<DamageText>.PoolData
        {
            get { return _poolData; }
        }

        void IPoolable<DamageText>.SetPoolData(ObjectPool<DamageText> objectPool, PoolData<DamageText> poolData)
        {
            _poolData = poolData;
        }

        void IPoolable<DamageText>.Activate()
        {
        }

        void IPoolable<DamageText>.Deactivate()
        {
        }

        #endregion
    }
}