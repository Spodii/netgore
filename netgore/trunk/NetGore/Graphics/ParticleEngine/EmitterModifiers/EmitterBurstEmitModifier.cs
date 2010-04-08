using System.ComponentModel;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// ParticleModifiers a <see cref="ParticleEmitter"/> to make it emit <see cref="Particle"/>s in bursts.
    /// </summary>
    public class EmitterBurstEmitModifier : EmitterModifier
    {
        const string _categoryName = "Burst Emit Modifier";
        const int _defaultEmitPeriod = 500;
        const int _defaultRestPeriod = 2000;
        const string _emitPeriodKeyName = "EmitPeriod";
        const string _restPeriodKeyName = "RestPeriod";

        VariableUShort _emitterReleaseAmount = 0;
        bool _isBursting = false;
        int _timeout = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmitterBurstEmitModifier"/> class.
        /// </summary>
        public EmitterBurstEmitModifier()
        {
            EmitPeriod = _defaultEmitPeriod;
            RestPeriod = _defaultRestPeriod;
        }

        /// <summary>
        /// Gets or sets the radius of the circle to move the emitter around.
        /// </summary>
        [Category(_categoryName)]
        [Description("Number of milliseconds each burst of emitting will last.")]
        [DisplayName("Emit Period")]
        [DefaultValue(_defaultEmitPeriod)]
        public int EmitPeriod { get; set; }

        /// <summary>
        /// Gets or sets the speed at which the emitter rotates in radians per second. 2pi equals one complete
        /// rotation per second.
        /// </summary>
        [Category(_categoryName)]
        [Description("Number of milliseconds to wait between each burst.")]
        [DisplayName("Rest Period")]
        [DefaultValue(_defaultRestPeriod)]
        public int RestPeriod { get; set; }

        /// <summary>
        /// When overridden in the derived class, handles reverting changes made to the <see cref="ParticleEmitter"/>
        /// by this <see cref="EmitterModifier"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to revert the changes to.</param>
        protected override void HandleRestore(ParticleEmitter emitter)
        {
            // Restore the release amount
            emitter.ReleaseAmount = _emitterReleaseAmount;
        }

        /// <summary>
        /// When overridden in the derived class, handles updating the <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to be modified.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the last update.</param>
        protected override void HandleUpdate(ParticleEmitter emitter, int elapsedTime)
        {
            _timeout -= elapsedTime;

            // Store the original value
            _emitterReleaseAmount = emitter.ReleaseAmount;

            // If bursting, set the release amount to 0 (it will be restored later in HandleRestore)
            if (!_isBursting)
                emitter.ReleaseAmount = 0;

            // After enough time has elapsed, flip between bursting and not bursting
            if (_timeout <= 0)
            {
                if (_isBursting)
                    _timeout = RestPeriod;
                else
                    _timeout = EmitPeriod;

                _isBursting = !_isBursting;
            }
        }

        /// <summary>
        /// When overridden in the derived class, reads the <see cref="EmitterModifier"/>'s custom values
        /// from the <see cref="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the custom values from.</param>
        protected override void ReadCustomValues(IValueReader reader)
        {
            EmitPeriod = reader.ReadInt(_emitPeriodKeyName);
            RestPeriod = reader.ReadInt(_restPeriodKeyName);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(_emitPeriodKeyName, EmitPeriod);
            writer.Write(_restPeriodKeyName, RestPeriod);
        }
    }
}