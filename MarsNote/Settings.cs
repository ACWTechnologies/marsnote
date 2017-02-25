using System;
using System.ComponentModel;
using System.Linq;
using MahApps.Metro;
using Newtonsoft.Json;

namespace MarsNote
{
    public sealed class Settings
    {
        private const string DefaultAccentColour = "Red";
        private string _accentColour;
        private bool _alwaysOnTop;
        private int _autoSave;
        private string _saveFileLocation;
        private bool _saveWindowPosition;
        private bool _startOnWindowsStartup;

        [JsonConstructor]
        public Settings(string saveFileLocation, string accentColour, int autoSave, bool? saveWindowPosition, bool? alwaysOnTop)
        {
            SaveFileLocation = saveFileLocation;
            AccentColour = accentColour;
            AutoSave = autoSave;
            SaveWindowPosition = saveWindowPosition ?? true;
            AlwaysOnTop = alwaysOnTop ?? false;
        }

        /// <summary>
        /// Gets a new empty instance of <see cref="Settings"/>.
        /// </summary>
        public static Settings BlankSettings => new Settings(null, null, 0, null, false);

        [DefaultValue(DefaultAccentColour)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public string AccentColour
        {
            get { return _accentColour; }
            set
            {
                if (value == null) { _accentColour = DefaultAccentColour; }
                if (ThemeManager.Accents.Any(a => string.Equals(a.Name, value, StringComparison.OrdinalIgnoreCase))) { _accentColour = value == "Yellow" ? DefaultAccentColour : value; }
                else { _accentColour = DefaultAccentColour; }
            }
        }

        [DefaultValue(false)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 5)]
        public bool AlwaysOnTop
        {
            get { return _alwaysOnTop; }
            set { _alwaysOnTop = value; }
        }

        [DefaultValue(0)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public int AutoSave
        {
            get { return _autoSave; }
            set
            {
                if (value < 0) { _autoSave = 0; }
                else if (value > 60) { _autoSave = 60; }
                else { _autoSave = value; }
            }
        }

        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public string SaveFileLocation
        {
            get { return _saveFileLocation; }
            set { _saveFileLocation = value ?? FileHelper.DefaultSaveFileLocation; }
        }

        [DefaultValue(true)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 4)]
        public bool SaveWindowPosition
        {
            get { return _saveWindowPosition; }
            set { _saveWindowPosition = value; }
        }

        [JsonIgnore]
        public bool StartOnWindowsStartup
        {
            get { return _startOnWindowsStartup; }
            set { _startOnWindowsStartup = value; }
        }

        /// <summary>
        /// Reads the settings saved in the save file.
        /// </summary>
        public static Settings Load()
        {
            try
            {
                var settings = JsonHelper.DeserializePath<Settings>(FileHelper.SettingsFileLocation,
                    new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        NullValueHandling = NullValueHandling.Include
                    });

                settings.StartOnWindowsStartup = AppHelper.IsInCurrentUserStartup();

                return settings;
            }
            catch
            {
                return BlankSettings;
            }
        }

        /// <summary>
        /// Saves this instance of <see cref="Settings"/> to the settings file.
        /// </summary>
        public void Save()
        {
            string json = JsonHelper.Serialize(this);

            FileHelper.Write(json, FileHelper.SettingsFileLocation);
        }
    }
}
