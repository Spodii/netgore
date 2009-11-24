using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Modifies the color of particles throughout their life.
    /// </summary>
    public class ColorModifier : ParticleModifier
    {
        const string _categoryName = "Color Modifier";

        byte _modifyFlags = byte.MaxValue;

        // TODO: !! Finish...

        const byte _rOffset = 1 << 0;
        const byte _gOffset = 1 << 1;
        const byte _bOffset = 1 << 2;
        const byte _aOffset = 1 << 3;

        bool IsBitSet(byte bit)
        {
            return ((1 << bit) & _modifyFlags) != 0;
        }

        void SetBit(byte bit, bool value)
        {
            if (value)
            {
                _modifyFlags |= (byte)(1 << bit);
                Debug.Assert(IsBitSet(bit));
            }
            else
            {
                _modifyFlags &= (byte)~(1<<bit);
                Debug.Assert(!IsBitSet(bit));
            }
        }

        public bool ModifyBlue { get { return IsBitSet(_bOffset); } set { SetBit(_bOffset, value); } }
        public bool ModifyRed { get { return IsBitSet(_rOffset); } set { SetBit(_rOffset, value); } }
        public bool ModifyGreen { get { return IsBitSet(_gOffset); } set { SetBit(_gOffset, value); } }
        public bool ModifyAlpha { get { return IsBitSet(_aOffset); } set { SetBit(_aOffset, value); } }

        /// <summary>
        /// Gets or sets the starting color.
        /// </summary>
        [Category(_categoryName)]
        [Description("The starting color.")]
        [DisplayName("Release Color")]
        [DefaultValue(typeof(Color), "{255, 255, 255, 255}")]
        public Color ReleaseColor { get; set; }

        /// <summary>
        /// Gets or sets the ending color.
        /// </summary>
        [Category(_categoryName)]
        [Description("The ending color.")]
        [DisplayName("Ultimate Color")]
        [DefaultValue(typeof(Color), "{255, 255, 255, 255}")]
        public Color UltimateColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorModifier"/> class.
        /// </summary>
        public ColorModifier()
            : base(true, true)
        {
            ReleaseColor = Color.White;
            ReleaseColor = Color.White;
        }

        /// <summary>
        /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
        /// it is released. Only valid if <see cref="ParticleModifier.ProcessOnRelease"/> is set.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
        /// came from.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        protected override void HandleProcessReleased(ParticleEmitter emitter, Particle particle)
        {
            particle.Color = ReleaseColor;
        }

        /// <summary>
        /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
        /// it is updated. Only valid if <see cref="ParticleModifier.ProcessOnUpdate"/> is set.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
        /// came from.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the <paramref name="emitter"/>
        /// was last updated.</param>
        protected override void HandleProcessUpdated(ParticleEmitter emitter, Particle particle, int elapsedTime)
        {
            var agePercent = particle.GetAgePercent(CurrentTime);
            particle.Color = ReleaseColor.Lerp(UltimateColor, agePercent);
        }
    }
}
