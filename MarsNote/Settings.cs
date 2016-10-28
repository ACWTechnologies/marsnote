using Newtonsoft.Json;

namespace MarsNote
{
    public class Settings
    {
        private string _saveFileLocation;
        private string _accentColour;
        private int _autoSave;
        private bool _saveWindowPosition;
        private bool _startOnWindowsStartup;

        public string SaveFileLocation
        {
            get { return _saveFileLocation; }
            set { _saveFileLocation = value ?? FileHelper.DefaultSaveFileLocation; }
        }

        public string AccentColour
        {
            get { return _accentColour; }
            set { _accentColour = value ?? "Red"; }
        }

        public int AutoSave
        {
            get
            {
                return _autoSave;
            }
            set
            {
                if (value < 0) { _autoSave = 0; }
                else if (value > 60) { _autoSave = 60; }
                else { _autoSave = value; }
            }
        }
        
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

        [JsonConstructor]
        public Settings(string saveFileLocation, string accentColour, int autoSave, bool? saveWindowPosition)
        {
            SaveFileLocation = saveFileLocation;
            AccentColour = accentColour;
            AutoSave = autoSave;
            SaveWindowPosition = saveWindowPosition ?? true;
        }

        /// <summary>
        /// Gets a new empty instance of <see cref="Settings"/>.
        /// </summary>
        public static Settings BlankSettings => new Settings(null, null, 0, null);

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
        /// Saves an instance of <see cref="Settings"/> to the settings file.
        /// </summary>
        /// <param name="settings"></param>
        public static void Save(Settings settings)
        {
            string json = JsonHelper.Serialize(settings);

            FileHelper.Write(json, FileHelper.SettingsFileLocation);
        }
    }
}
