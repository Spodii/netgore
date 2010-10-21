using System;
using System.Linq;

namespace NetGore.IO
{
    public interface IRuntimeSettings : IPersistable
    {
        /// <summary>
        /// Notifies listeners when the settings have been loaded.
        /// </summary>
        event RuntimeSettingsEventHandler Loaded;

        /// <summary>
        /// Notifies listeners when the settings have been reset.
        /// </summary>
        event RuntimeSettingsEventHandler Resetted;

        /// <summary>
        /// Notifies listeners when the settings have been saved.
        /// </summary>
        event RuntimeSettingsEventHandler Saved;

        /// <summary>
        /// Gets the path to the file containing the settings.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Gets the name of the root node in the settings file.
        /// </summary>
        string RootNodeName { get; }

        /// <summary>
        /// Restores the settings from file.
        /// </summary>
        void Load();

        /// <summary>
        /// Restores all the settings to their default values.
        /// </summary>
        void Reset();

        /// <summary>
        /// Saves the settings to file.
        /// </summary>
        void Save();
    }
}