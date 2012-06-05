using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Modifies the color of <see cref="Particle"/>s throughout their life.
    /// </summary>
    public class ParticleColorModifier : ParticleModifier
    {
        const byte _aOffset = 3;
        const byte _bOffset = 2;
        const string _categoryName = "Color Modifier";

        const byte _gOffset = 1;
        const string _modifyAlphaKeyName = "ModifyAlpha";
        const string _modifyBlueKeyName = "ModifyBlue";
        const string _modifyGreenKeyName = "ModifyGreen";
        const string _modifyRedKeyName = "ModifyRed";
        const byte _rOffset = 0;
        const string _releaseColorKeyName = "ReleaseColor";
        const string _ultimateColorKeyName = "UltimateColor";
        byte _modifyFlags = byte.MaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleColorModifier"/> class.
        /// </summary>
        public ParticleColorModifier() : base(true, true)
        {
            ReleaseColor = Color.White;
            ReleaseColor = Color.White;
        }

        /// <summary>
        /// Gets or sets if the Alpha color channel is modified by this modifier.
        /// </summary>
        [Category(_categoryName)]
        [Description("If the Alpha color channel is modified by this modifier.")]
        [DisplayName("Modify Alpha")]
        [DefaultValue(true)]
        public bool ModifyAlpha
        {
            get { return IsBitSet(_aOffset); }
            set { SetBit(_aOffset, value); }
        }

        /// <summary>
        /// Gets or sets if the Blue color channel is modified by this modifier.
        /// </summary>
        [Category(_categoryName)]
        [Description("If the Blue color channel is modified by this modifier.")]
        [DisplayName("Modify Blue")]
        [DefaultValue(true)]
        public bool ModifyBlue
        {
            get { return IsBitSet(_bOffset); }
            set { SetBit(_bOffset, value); }
        }

        /// <summary>
        /// Gets or sets if the Green color channel is modified by this modifier.
        /// </summary>
        [Category(_categoryName)]
        [Description("If the Green color channel is modified by this modifier.")]
        [DisplayName("Modify Green")]
        [DefaultValue(true)]
        public bool ModifyGreen
        {
            get { return IsBitSet(_gOffset); }
            set { SetBit(_gOffset, value); }
        }

        /// <summary>
        /// Gets or sets if the Red color channel is modified by this modifier.
        /// </summary>
        [Category(_categoryName)]
        [Description("If the Red color channel is modified by this modifier.")]
        [DisplayName("Modify Red")]
        [DefaultValue(true)]
        public bool ModifyRed
        {
            get { return IsBitSet(_rOffset); }
            set { SetBit(_rOffset, value); }
        }

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
        /// Checks if all 4 bits are set.
        /// </summary>
        /// <returns>If all 4 bits are set.</returns>
        bool AllBitsSet()
        {
            const int mask = ((1 << 5) - 1);
            return (_modifyFlags & mask) == mask;
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
            if (AllBitsSet())
                particle.Color = ReleaseColor;
            else
            {
                byte r, g, b, a;

                if (ModifyRed)
                    r = ReleaseColor.R;
                else
                    r = particle.Color.R;

                if (ModifyGreen)
                    g = ReleaseColor.G;
                else
                    g = particle.Color.G;

                if (ModifyBlue)
                    b = ReleaseColor.B;
                else
                    b = particle.Color.B;

                if (ModifyAlpha)
                    a = ReleaseColor.A;
                else
                    a = particle.Color.A;

                particle.Color = new Color(r, g, b, a);
            }
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

            if (AllBitsSet())
                particle.Color = ReleaseColor.Lerp(UltimateColor, agePercent);
            else
            {
                byte r, g, b, a;

                if (ModifyRed)
                    r = (byte)MathHelper.Lerp(ReleaseColor.R, UltimateColor.R, agePercent);
                else
                    r = particle.Color.R;

                if (ModifyGreen)
                    g = (byte)MathHelper.Lerp(ReleaseColor.G, UltimateColor.G, agePercent);
                else
                    g = particle.Color.G;

                if (ModifyBlue)
                    b = (byte)MathHelper.Lerp(ReleaseColor.B, UltimateColor.B, agePercent);
                else
                    b = particle.Color.B;

                if (ModifyAlpha)
                    a = (byte)MathHelper.Lerp(ReleaseColor.A, UltimateColor.A, agePercent);
                else
                    a = particle.Color.A;

                particle.Color = new Color(r, g, b, a);
            }
        }

        /// <summary>
        /// Checks if a bit is set.
        /// </summary>
        /// <param name="bit">The bit.</param>
        /// <returns>True if the bit is set; otherwise false.</returns>
        bool IsBitSet(byte bit)
        {
            return ((1 << bit) & _modifyFlags) != 0;
        }

        /// <summary>
        /// Reads the <see cref="ParticleModifier"/>'s custom values from the <see cref="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the custom values from.</param>
        protected override void ReadCustomValues(IValueReader reader)
        {
            ModifyRed = reader.ReadBool(_modifyRedKeyName);
            ModifyGreen = reader.ReadBool(_modifyGreenKeyName);
            ModifyBlue = reader.ReadBool(_modifyBlueKeyName);
            ModifyAlpha = reader.ReadBool(_modifyAlphaKeyName);

            ReleaseColor = reader.ReadColor(_releaseColorKeyName);
            UltimateColor = reader.ReadColor(_ultimateColorKeyName);
        }

        /// <summary>
        /// Sets a bit in <see cref="_modifyFlags"/>.
        /// </summary>
        /// <param name="bit">The bit to set.</param>
        /// <param name="value">The value.</param>
        void SetBit(byte bit, bool value)
        {
            if (value)
            {
                _modifyFlags |= (byte)(1 << bit);
                Debug.Assert(IsBitSet(bit));
            }
            else
            {
                _modifyFlags &= (byte)~(1 << bit);
                Debug.Assert(!IsBitSet(bit));
            }
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(_modifyRedKeyName, ModifyRed);
            writer.Write(_modifyGreenKeyName, ModifyGreen);
            writer.Write(_modifyBlueKeyName, ModifyBlue);
            writer.Write(_modifyAlphaKeyName, ModifyAlpha);

            writer.Write(_releaseColorKeyName, ReleaseColor);
            writer.Write(_ultimateColorKeyName, UltimateColor);
        }
    }
}