using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics
{
    public static class GameTimeExtensions
    {
        /// <summary>
        /// Gets the total milliseconds for a <see cref="GameTime"/>.
        /// </summary>
        /// <param name="gameTime">The <see cref="GameTime"/>.</param>
        /// <returns>The total milliseconds.</returns>
        public static int ToTotalMS(this GameTime gameTime)
        {
            return (int)gameTime.TotalRealTime.TotalMilliseconds;
        }
    }
}
