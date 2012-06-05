using System.Linq;
using NetGore;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Persists the state of multiple GUI components.
    /// </summary>
    class GUIStatePersister : ObjectStatePersister
    {
        /// <summary>
        /// Name of the default profile.
        /// </summary>
        const string _defaultProfile = "Default";

        /// <summary>
        /// Root node name used for the settings file.
        /// </summary>
        const string _rootNodeName = "GUISettings";

        /// <summary>
        /// How frequently, in milliseconds, the GUISettings is saved.
        /// </summary>
        const int _saveFrequency = 60000;

        /// <summary>
        /// Path to the default profile.
        /// </summary>
        static readonly string _defaultProfilePath;

        TickCount _lastSaveTime = TickCount.MinValue;

        /// <summary>
        /// Initializes the <see cref="GUIStatePersister"/> class.
        /// </summary>
        static GUIStatePersister()
        {
            _defaultProfilePath = GetPath(_defaultProfile);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GUIStatePersister"/> class.
        /// </summary>
        /// <param name="profile">Name of the profile to use for the GUI settings.</param>
        public GUIStatePersister(string profile) : base(_rootNodeName, GetPath(profile), _defaultProfilePath)
        {
        }

        /// <summary>
        /// Gets the file path for the given profile.
        /// </summary>
        /// <param name="profile">Name of the profile to find the path for.</param>
        /// <returns>The file path for the given profile.</returns>
        static string GetPath(string profile)
        {
            return ContentPaths.Build.Settings.Join(profile).Join("gui" + EngineSettings.DataFileSuffix);
        }

        /// <summary>
        /// Updates the GUISettings.
        /// </summary>
        /// <param name="currentTime">Current time.</param>
        public void Update(TickCount currentTime)
        {
            if (_lastSaveTime + _saveFrequency > currentTime)
                return;

            _lastSaveTime = currentTime;
            AsyncSave();
        }
    }
}