using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics
{
    public sealed class SpriteBlendModeHelper : EnumIOHelper<SpriteBlendMode>
    {
        static readonly SpriteBlendModeHelper _instance;

        /// <summary>
        /// Initializes the <see cref="SpriteBlendModeHelper"/> class.
        /// </summary>
        static SpriteBlendModeHelper()
        {
            _instance = new SpriteBlendModeHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBlendModeHelper"/> class.
        /// </summary>
        SpriteBlendModeHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="SpriteBlendModeHelper"/> instance.
        /// </summary>
        public static SpriteBlendModeHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="SpriteBlendModeHelper"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="SpriteBlendModeHelper"/>.</returns>
        protected override SpriteBlendMode FromInt(int value)
        {
            return (SpriteBlendMode)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="SpriteBlendModeHelper"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(SpriteBlendMode value)
        {
            return (int)value;
        }
    }
}
