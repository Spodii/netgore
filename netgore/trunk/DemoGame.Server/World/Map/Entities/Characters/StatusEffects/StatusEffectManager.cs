using System.Linq;
using NetGore.Features.StatusEffects;

namespace DemoGame.Server
{
    public sealed class StatusEffectManager : StatusEffectManager<StatType, StatusEffectType>
    {
        /// <summary>
        /// The <see cref="StatusEffectManager"/> instance.
        /// </summary>
        static readonly StatusEffectManager _instance;

        /// <summary>
        /// Initializes the <see cref="StatusEffectManager"/> class.
        /// </summary>
        static StatusEffectManager()
        {
            _instance = new StatusEffectManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectManager"/> class.
        /// </summary>
        StatusEffectManager()
        {
        }

        /// <summary>
        /// Gets the <see cref="StatusEffectManager"/> instance.
        /// </summary>
        public static StatusEffectManager Instance
        {
            get { return _instance; }
        }
    }
}