using System;
using System.IO;
using System.Linq;
using SFML.Graphics;

namespace NetGore
{
    /// <summary>
    /// Contains all the engine settings. All values in this class are guaranteed to be immutable and thus may
    /// be cached locally when needed.
    /// </summary>
    public class EngineSettings
    {
        static EngineSettings _instance;
        static readonly string _dataFileSuffix = ".dat";

#if !TOPDOWN
        readonly Vector2 _gravity;
#endif

        readonly Vector2 _maxVelocity;


        /// <summary>
        /// Gets the suffix given to general data files. Includes the prefixed period, if one is used. Can be empty, but cannot
        /// be null.
        /// </summary>
        public static string DataFileSuffix { get { return _dataFileSuffix; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSettings"/> class.
        /// </summary>
        /// <param name="gravity">The world gravity. Only valid if not using a top-down perspective.</param>
        /// <param name="maxVelocity">The max velocity for an <see cref="Entity"/>.</param>
        public EngineSettings(Vector2 gravity, Vector2 maxVelocity)
        {
#if !TOPDOWN
            _gravity = gravity;
#endif

            _maxVelocity = maxVelocity.Abs();
        }

        /// <summary>
        /// Gets if a top-down perspective is used. If false, a side-view persective is being used.
        /// </summary>
        public bool IsTopDown
        {
            get
            {
#if TOPDOWN
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets the amount of velocity added to an <see cref="Entity"/> every millisecond.
        /// </summary>
        public Vector2 Gravity
        {
            get
            {
#if TOPDOWN
                return Vector2.Zero;
#else
                return _gravity;
#endif
            }
        }

        /// <summary>
        /// The <see cref="Color"/> used for transparency.
        /// </summary>
        public static readonly Color TransparencyColor = Color.Magenta;

        /// <summary>
        /// Gets the <see cref="EngineSettings"/> instance. This value will be null until it is set through
        /// <see cref="EngineSettings.Initialize"/>. After being set, this value is guaranteed to contain
        /// the same <see cref="EngineSettings"/> reference.
        /// </summary>
        public static EngineSettings Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the maximum velocity allowed for an <see cref="Entity"/> in any direction. This will always
        /// contain positive values.
        /// </summary>
        public Vector2 MaxVelocity
        {
            get { return _maxVelocity; }
        }

        /// <summary>
        /// Initializes the core engine's settings. This value may only be called once, and must be called as early
        /// as possible before the engine is used.
        /// </summary>
        /// <param name="settings">The engine's settings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        /// <exception cref="MethodAccessException">The settings have already been set.</exception>
        public static void Initialize(EngineSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            if (_instance != null)
                throw new MethodAccessException("This method must be called once and only once.");

            _instance = settings;
        }
    }
}