using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using NetGore.Collections;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Implementation of <see cref="IKeyCodeReference"/> that references a key set in an <see cref="ApplicationSettingsBase"/>.
    /// </summary>
    public class SettingsKeyCodeReference : IKeyCodeReference
    {
        static readonly Dictionary<ApplicationSettingsBase, HashCache<string, SettingsKeyCodeReference>> _settingsCache =
            new Dictionary<ApplicationSettingsBase, HashCache<string, SettingsKeyCodeReference>>();

        readonly string _keySettingName;
        readonly ApplicationSettingsBase _settings;

        Keyboard.Key _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsKeyCodeReference"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="keySettingName">Name of the key setting.</param>
        /// <exception cref="ArgumentNullException"><see cref="keySettingName"/> is null or empty.</exception>
        SettingsKeyCodeReference(ApplicationSettingsBase settings, string keySettingName)
        {
            if (string.IsNullOrEmpty(keySettingName))
                throw new ArgumentNullException("keySettingName");

            _keySettingName = keySettingName;
            _settings = settings;

            settings.PropertyChanged -= settings_PropertyChanged;
            settings.PropertyChanged += settings_PropertyChanged;

            UpdateValue();
        }

        public string KeySettingName
        {
            get { return _keySettingName; }
        }

        /// <summary>
        /// Creates a <see cref="SettingsKeyCodeReference"/> instance.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="keySettingName">Name of the key setting.</param>
        /// <returns>The <see cref="SettingsKeyCodeReference"/> instance.</returns>
        public static SettingsKeyCodeReference Create(ApplicationSettingsBase settings, string keySettingName)
        {
            HashCache<string, SettingsKeyCodeReference> c;
            if (!_settingsCache.TryGetValue(settings, out c))
            {
                c = new HashCache<string, SettingsKeyCodeReference>(x => new SettingsKeyCodeReference(settings, x),
                    StringComparer.Ordinal);
                _settingsCache.Add(settings, c);
            }

            return c[keySettingName];
        }

        /// <summary>
        /// Grabs the <see cref="Keyboard.Key"/> value from the <see cref="_settings"/>.
        /// </summary>
        void UpdateValue()
        {
            _key = (Keyboard.Key)_settings[KeySettingName];
        }

        /// <summary>
        /// Handles the PropertyChanged event of the <see cref="_settings"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        void settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!StringComparer.Ordinal.Equals(KeySettingName, e.PropertyName))
                return;

            UpdateValue();
        }

        #region IKeyCodeReference Members

        /// <summary>
        /// Gets the referenced <see cref="Keyboard.Key"/>.
        /// </summary>
        public Keyboard.Key Key
        {
            get { return _key; }
        }

        #endregion
    }
}