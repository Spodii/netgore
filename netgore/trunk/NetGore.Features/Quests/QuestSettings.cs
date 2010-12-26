using System;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Features.Quests
{
    public class QuestSettings
    {
        /// <summary>
        /// The settings instance.
        /// </summary>
        static QuestSettings _instance;

        readonly byte _maxActiveQuests;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestSettings"/> class.
        /// </summary>
        public QuestSettings(byte maxActiveQuests)
        {
            _maxActiveQuests = maxActiveQuests;
        }

        /// <summary>
        /// Gets the <see cref="QuestSettings"/> instance.
        /// </summary>
        public static QuestSettings Instance
        {
            get
            {
                Debug.Assert(_instance != null, "The settings instance should not be null!");
                return _instance;
            }
        }

        /// <summary>
        /// Gets the maximum number of active quests a single quest performer may have at once.
        /// </summary>
        public byte MaxActiveQuests
        {
            get { return _maxActiveQuests; }
        }

        /// <summary>
        /// Initializes the <see cref="QuestSettings"/>. This must only be called once and called as early as possible.
        /// </summary>
        /// <param name="settings">The settings instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings" /> is <c>null</c>.</exception>
        /// <exception cref="MethodAccessException">This method must be called once and only once.</exception>
        public static void Initialize(QuestSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (_instance != null)
                throw new MethodAccessException("This method must be called once and only once.");

            _instance = settings;
        }
    }
}