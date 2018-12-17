using System.Linq;
using NetGore;
using NetGore.Collections;
using NetGore.Graphics;
using NetGore.World;
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
        /// Creates a new <see cref="DamageText"/> and places it into the pool.
        /// </summary>
        /// <param name="damage">Damage value.</param>
        /// <param name="entity"><see cref="Entity"/> that was damaged.</param>
        /// <param name="currTime">Current time.</param>
        public void Create(int damage, Entity entity, TickCount currTime)
        {
            var obj = _pool.Acquire();
            obj.Activate(damage, entity, currTime);
        }

        /// <summary>
        /// Draws all the <see cref="DamageText"/>s in the pool.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="font"><see cref="Font"/> to draw the damage text with.</param>
        public void Draw(ISpriteBatch sb, Font font)
        {
            _pool.Perform(x => x.Draw(sb, font));
        }

        /// <summary>
        /// Updates all of the <see cref="DamageText"/>s in the pool.
        /// </summary>
        /// <param name="currentTime">Current time.</param>
        public void Update(TickCount currentTime)
        {
            var collectGarbage = false;

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