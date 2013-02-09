using System;
using System.Linq;
using NetGore.World;
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
        const string _dataFileSuffix = ".dat";
        const string _imageFileSuffix = ".png";

#if !TOPDOWN
        readonly Vector2 _gravity;
#endif

        readonly Vector2 _maxVelocity;

#if !TOPDOWN
        readonly int _maxWallStepUpHeight;
#endif

        readonly float _soundAttenuation;
        readonly float _soundListenerDepth;
        readonly float _soundMinDistance;
        readonly uint _soundUpdateRate;

        /// <summary>
        /// Gets the attenuation factor for the spatialized sounds. A larger attenuation value results in a faster decrease in
        /// volume for sounds that exceed the <see cref="EngineSettings.SoundMinDistance"/>.
        /// </summary>
        public float SoundAttenuation
        {
            get { return _soundAttenuation; }
        }

        /// <summary>
        /// Gets the depth to set the sound listener at.
        /// </summary>
        public float SoundListenerDepth
        {
            get { return _soundListenerDepth; }
        }

        /// <summary>
        /// Gets the minimum distance the listener must be from a spatialized sound before the gain starts to decrease.
        /// </summary>
        public float SoundMinDistance
        {
            get { return _soundMinDistance; }
        }

        /// <summary>
        /// Gets the maximum number of pixels an <see cref="Entity"/> can "step up" when walking into a wall. If the top of the
        /// wall that they walk into the side of minus the bottom of the entity is less than or equal to this value, the
        /// <see cref="Entity"/> will be moved on top of the wall instead of being blocked off by it.
        /// Only applicable in sidescroller. When used in TopDown builds, this will always return 0.
        /// </summary>
        public int MaxWallStepUpHeight
        {
            get
            {
#if TOPDOWN
                return 0;
#else
                return _maxWallStepUpHeight;
#endif
            }
        }

        /// <summary>
        /// Gets the frequently, in milliseconds, that spatialized sounds are updated.
        /// </summary>
        public uint SoundUpdateRate
        {
            get { return _soundUpdateRate; }
        }

        /// <summary>
        /// Gets the suffix given to general data files. Includes the prefixed period, if one is used. Can be empty, but cannot
        /// be null.
        /// </summary>
        public static string DataFileSuffix
        {
            get { return _dataFileSuffix; }
        }

        /// <summary>
        /// Gets the suffix given to png image files. Includes the prefixed period, if one is used. Can be empty, but cannot
        /// be null.
        /// </summary>
        public static string ImageFileSuffix
        {
            get { return _imageFileSuffix; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSettings"/> class.
        /// </summary>
        /// <param name="gravity">The world gravity. Only valid if not using a top-down perspective.</param>
        /// <param name="maxVelocity">The max velocity for an <see cref="Entity"/>.</param>
        /// <param name="soundListenerDepth">The <see cref="SoundListenerDepth"/>.</param>
        /// <param name="soundMinDistance">The <see cref="SoundMinDistance"/>.</param>
        /// <param name="soundAttenuation">The <see cref="SoundAttenuation"/>.</param>
        /// <param name="maxWallStepUpHeight">The <see cref="MaxWallStepUpHeight"/>.</param>
        /// <param name="soundUpdateRate">The <see cref="SoundUpdateRate"/>.</param>
        public EngineSettings(Vector2 gravity, Vector2 maxVelocity, float soundListenerDepth = 300f, float soundMinDistance = 600f,
                              float soundAttenuation = 3f, int maxWallStepUpHeight = 5, uint soundUpdateRate = 1000u)
        {
#if !TOPDOWN
            _gravity = gravity;
            _maxWallStepUpHeight = maxWallStepUpHeight;
#endif

            _maxVelocity = maxVelocity.Abs();

            _soundAttenuation = soundAttenuation;
            _soundMinDistance = soundMinDistance;
            _soundListenerDepth = soundListenerDepth;
            _soundUpdateRate = soundUpdateRate;
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