using System;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Features.StatusEffects
{
    /// <summary>
    /// Contains the settings for status effects.
    /// </summary>
    public class StatusEffectsSettings
    {
        /// <summary>
        /// The settings instance.
        /// </summary>
        static StatusEffectsSettings _instance;

        readonly ushort _maxStatusEffectPower;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectsSettings"/> class.
        /// </summary>
        /// <param name="maxStatusEffectPower">The max number of items in a single shop.</param>
        public StatusEffectsSettings(ushort maxStatusEffectPower)
        {
            _maxStatusEffectPower = maxStatusEffectPower;
        }

        /// <summary>
        /// Gets the <see cref="StatusEffectsSettings"/> instance.
        /// </summary>
        public static StatusEffectsSettings Instance
        {
            get
            {
                Debug.Assert(_instance != null, "The settings instance should not be null!");
                return _instance;
            }
        }

        /// <summary>
        /// Gets the maximum power of a <see cref="IStatusEffect{StatType, StatusEffectType}"/>.
        /// </summary>
        public ushort MaxStatusEffectPower
        {
            get { return _maxStatusEffectPower; }
        }

        /// <summary>
        /// Initializes the <see cref="StatusEffectsSettings"/>. This must only be called once and called as early as possible.
        /// </summary>
        /// <param name="settings">The settings instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings" /> is <c>null</c>.</exception>
        /// <exception cref="MethodAccessException">This method must be called once and only once.</exception>
        public static void Initialize(StatusEffectsSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (_instance != null)
                throw new MethodAccessException("This method must be called once and only once.");

            _instance = settings;
        }
    }
}