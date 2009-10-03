using System.Linq;
using DemoGame;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Client
{
    /// <summary>
    /// ObjectPool and pool handler for all of the DamageTexts
    /// </summary>
    public class DamageTextPool : ObjectPool<DamageText>
    {
        /// <summary>
        /// Creates a new DamageText and places it into the pool
        /// </summary>
        /// <param name="damage">Damage value</param>
        /// <param name="entity">Entity that was damaged</param>
        /// <param name="currTime">Current time</param>
        public void Create(int damage, Entity entity, int currTime)
        {
            DamageText obj = Create();
            obj.Activate(damage, entity, currTime);
        }

        /// <summary>
        /// Draws all the DamageTexts in the pool
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="sf">SpriteFont to draw the damage text with</param>
        public void Draw(SpriteBatch sb, SpriteFont sf)
        {
            foreach (DamageText obj in this)
            {
                obj.Draw(sb, sf);
            }
        }

        /// <summary>
        /// Updates all of the DamageTexts in the pool
        /// </summary>
        /// <param name="currentTime">Current time</param>
        public void Update(int currentTime)
        {
            foreach (DamageText obj in this)
            {
                // Perform the individual DamageText update
                obj.Update(currentTime);

                // Kill off the object if its alpha falls below 20
                if (obj.Alpha < 20)
                    Destroy(obj);
            }
        }
    }
}