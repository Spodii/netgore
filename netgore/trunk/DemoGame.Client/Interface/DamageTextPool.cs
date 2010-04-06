using System.Linq;
using NetGore;
using NetGore.Collections;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// ObjectPool and pool handler for all of the <see cref="DamageText"/>s.
    /// </summary>
    public class DamageTextPool
    {
        readonly ObjectPool<DamageText> _pool = new ObjectPool<DamageText>(x => new DamageText(), false);

        /// <summary>
        /// Creates a new DamageText and places it into the pool
        /// </summary>
        /// <param name="damage">Damage value</param>
        /// <param name="entity">Entity that was damaged</param>
        /// <param name="currTime">Current time</param>
        public void Create(int damage, Entity entity, int currTime)
        {
            DamageText obj = _pool.Acquire();
            obj.Activate(damage, entity, currTime);
        }

        /// <summary>
        /// Draws all the DamageTexts in the pool.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="sf">SpriteFont to draw the damage text with.</param>
        public void Draw(ISpriteBatch sb, Font sf)
        {
            _pool.Perform(x => x.Draw(sb, sf));
        }

        /// <summary>
        /// Updates all of the DamageTexts in the pool
        /// </summary>
        /// <param name="currentTime">Current time</param>
        public void Update(int currentTime)
        {
            bool collectGarbage = false;

            _pool.Perform(delegate(DamageText x)
            {
                x.Update(currentTime);

                if (x.Alpha < 20)
                    collectGarbage = true;
            });

            if (collectGarbage)
                _pool.FreeAll(x => x.Alpha < 20);
        }
    }
}