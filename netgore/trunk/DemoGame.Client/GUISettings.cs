using System.Linq;
using NetGore;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages the settings of multiple GUI components.
    /// </summary>
    class GUISettings : SettingsManager
    {
        /// <summary>
        /// Name of the default profile.
        /// </summary>
        const string _defaultProfile = "Default";

        /// <summary>
        /// Root n name used for the settings file.
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

        int _lastSaveTime = int.MinValue;

        /// <summary>
        /// Initializes the <see cref="GUISettings"/> class.
        /// </summary>
        static GUISettings()
        {
            _defaultProfilePath = GetPath(_defaultProfile);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GUISettings"/> class.
        /// </summary>
        /// <param name="profile">Name of the profile to use for the GUI settings.</param>
        public GUISettings(string profile) : base(_rootNodeName, GetPath(profile), _defaultProfilePath)
        {
        }

        /// <summary>
        /// Gets the file path for the given profile.
        /// </summary>
        /// <param name="profile">Name of the profile to find the path for.</param>
        /// <returns>The file path for the given profile.</returns>
        static string GetPath(string profile)
        {
            return ContentPaths.Build.Settings.Join(profile).Join("gui.xml");
        }

        /// <summary>
        /// Updates the GUISettings.
        /// </summary>
        /// <param name="currentTime">Current time.</param>
        public void Update(int currentTime)
        {
            if (_lastSaveTime + _saveFrequency > currentTime)
                return;

            _lastSaveTime = currentTime;
            AsyncSave();
        }
    }
}