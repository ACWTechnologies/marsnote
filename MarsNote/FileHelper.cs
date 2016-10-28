using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MarsNote
{
    public static class FileHelper
    {
        /// <summary>
        /// The default directory for all MarsNote files.
        /// </summary>
        private static readonly string MarsNoteDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ACW Technologies\MarsNote\";

        /// <summary>
        /// The path to the state file.
        /// </summary>
        public static readonly string StateFileLocation = MarsNoteDirectory + "mn-state.json";

        /// <summary>
        /// The path to the settings file.
        /// </summary>
        public static readonly string SettingsFileLocation = MarsNoteDirectory + "mn-settings.json";

        /// <summary>
        /// The path to the startup file.
        /// </summary>
        public static readonly string StartupFileLocation = MarsNoteDirectory + "startup.acwmn";

        /// <summary>
        /// The URL to the license.
        /// </summary>
        public static readonly string LicenseURL = @"http://acwtechnologies.co.uk/software/marsnote#license";

        /// <summary>
        /// The default location the save file should be saved in, if no other directory is specified.
        /// </summary>
        public static readonly string DefaultSaveFileLocation = MarsNoteDirectory;

        /// <summary>
        /// The default name of the save file.
        /// </summary>
        public static readonly string DefaultSaveFileName = "mn-save.json";

        private static string _saveFileLocation;

        /// <summary>
        /// The location of the save file.
        /// </summary>
        public static string SaveFileLocation
        {
            get
            {
                // Combine the directory and file name, then return
                return Path.Combine(_saveFileLocation, DefaultSaveFileName);
            }
            set
            {
                // Set the value only if the directory exists
                if (Directory.Exists(value))
                    _saveFileLocation = value;
            }
        }

        static FileHelper()
        {
            // Create the default MarsNote directory if it does not already exist
            if (!Directory.Exists(MarsNoteDirectory)) { Directory.CreateDirectory(MarsNoteDirectory); }
            LoadSaveFileLocation();
        }

        /// <summary>
        /// Load the save file location from the settings file.
        /// </summary>
        public static void LoadSaveFileLocation()
        {
            // Load settings into 's'
            Settings s = Settings.Load();
            // If the save file location stored in 's' exists
            if (!string.IsNullOrWhiteSpace(s.SaveFileLocation) && Directory.Exists(s.SaveFileLocation))
            {
                // Set the location to the location stored in 's'
                SaveFileLocation = s.SaveFileLocation;
            }
            else
            {
                // Set the location to default
                SaveFileLocation = DefaultSaveFileLocation;
            }
        }

        /// <summary>
        /// Saves a collection of serialised <see cref="Profile"/>s to a file.
        /// </summary>
        /// <param name="profiles">The profiles to save.</param>
        /// <param name="path">The file path to save the profiles to.</param>
        public static void SaveProfiles(ObservableCollection<Profile> profiles, string path)
        {
            // If the collection of profiles is null, create a new empty collection
            if (profiles == null) { profiles = new ObservableCollection<Profile>(); }
            // Serialise and write the profiles to the specified file path
            Write(JsonHelper.Serialize(SortProfiles(profiles)), path);
        }

        /// <summary>
        /// Reads all text of a file.
        /// </summary>
        /// <param name="path">The path of the file to read.</param>
        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// Reads all lines of a file.
        /// </summary>
        /// <param name="path">The path of the file to read.</param>
        public static string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        /// <summary>
        /// Writes the specified contents to a file.
        /// </summary>
        /// <param name="contents">The contents of the file to be written.</param>
        /// <param name="path">The path to the file to write to.</param>
        /// <param name="append">Whether or not to append the contents to the file.</param>
        public static void Write(string contents, string path, bool append = false)
        {
            if (contents == null)
            {
                return;
            }

            // If the file does not exist
            if (!File.Exists(path))
            {
                FileStream fs = null;
                try
                {
                    // Create the file (and the directory if it does not exist)
                    fs = CreateFileAndDirectory(path);
                }
                finally
                {
                    fs?.Close();
                }
            }

            if (append)
            {
                // Append the contents to the file
                File.AppendAllText(path, contents);
            }
            else
            {
                // Write the contents to the file
                File.WriteAllText(path, contents);
            }
        }

        /// <summary>
        /// Checks if a specified directory contains a save file.
        /// </summary>
        /// <param name="path">The directory to check.</param>
        public static bool FolderContainsSaveFile(string path)
        {
            if (Directory.Exists(path))
            {
                if (File.Exists(Path.Combine(path, Path.GetFileName(SaveFileLocation))))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Allows the user to browse for a folder.
        /// </summary>
        /// <param name="description">The descriptive text displayed above the tree view control in the dialog box.</param>
        /// <param name="rootFolder">The root folder where the browsing starts from.</param>
        /// <param name="showNewFolderButton">A value indicating whether the New Folder button appears in the folder browser dialog box.</param>
        /// <returns>The path of the folder, or null if browse cancelled.</returns>
        public static string BrowseForFolder(string description = "", Environment.SpecialFolder rootFolder = Environment.SpecialFolder.Desktop, bool showNewFolderButton = true)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = description,
                RootFolder = rootFolder,
                ShowNewFolderButton = showNewFolderButton
            };

            return dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ? dialog.SelectedPath : null;
        }

        /// <summary>
        /// Sorts a collection of profiles and returns the new sorted collection.
        /// </summary>
        /// <param name="profiles">The collection of profiles to sort.</param>
        private static ObservableCollection<Profile> SortProfiles(ObservableCollection<Profile> profiles)
        {
            ObservableCollection<Profile> profilesDeepClone = JsonHelper.DeepClone(profiles);

            // Sort Profiles
            profilesDeepClone = profilesDeepClone.Sort("Name", OrderType.Ascending);
            
            foreach (Profile profile in profilesDeepClone)
            {
                // Sort Folders
                profile.Folders = profile.Folders.Sort("Name", OrderType.Ascending);
                
                foreach (Folder folder in profile.Folders)
                {
                    // Sort notes
                    folder.Notes = folder.Notes.Sort("LastModified", OrderType.Descending);
                }
            }

            return profilesDeepClone;
        }

        /// <summary>
        /// An order direction of ascending or descending.
        /// </summary>
        public enum OrderType { Ascending, Descending }

        /// <summary>
        /// Sorts a collection of type T by a specific property, in either ascending or descending order.
        /// </summary>
        /// <typeparam name="T">The object type to sort.</typeparam>
        /// <param name="collection">The collection of type T.</param>
        /// <param name="property">The name of the property to sort by.</param>
        /// <param name="orderType">The order in which to sort.</param>
        public static ObservableCollection<T> Sort<T>(this IEnumerable<T> collection, string property, OrderType orderType)
        {
            PropertyInfo p = typeof(T).GetProperty(property);
            if (property == null) { throw new ArgumentException("No such property exists"); }

            List<T> sorted = orderType == OrderType.Ascending
                ? collection.OrderBy(x => p.GetValue(x, null)).ToList()
                : collection.OrderByDescending(x => p.GetValue(x, null)).ToList();
            

            // If T implements IPinnable
            if (typeof(T).GetInterface("IPinnable") != null)
            {
                // Remember number of pinned items moved to the top of the list
                int moved = 0;

                // Move pinned to top, still in sorted order, by working from the bottom up
                for (int i = sorted.Count - 1; i >= moved; i--)
                {
                    T temp = sorted[i];
                    var pinnable = (IPinnable)temp;

                    if (pinnable.Pinned)
                    {
                        sorted.RemoveAt(i);
                        sorted.Insert(0, temp);

                        moved++;
                        i++;
                    }
                }
            }

            return new ObservableCollection<T>(sorted);
        }

        /// <summary>
        /// Returns a deserialised collection of profiles from the specified path.
        /// </summary>
        /// <param name="path">The path to the file that contains the serialised collection of profiles.</param>
        public static ObservableCollection<Profile> LoadProfiles(string path)
        {
            // Deserialise the file into a collection of profiles, or a new empty collection if that returns null
            return JsonHelper.DeserializePath<ObservableCollection<Profile>>(path) ?? new ObservableCollection<Profile>();
        }

        /// <summary>
        /// Creates all directories and subdirectories unless they already exist and creates or overwrites a file in the specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public static FileStream CreateFileAndDirectory(string path)
        {
            if (path == null) { throw new ArgumentNullException(nameof(path)); }
            else if (string.IsNullOrWhiteSpace(path)) { throw new ArgumentException("path cannot be empty."); }

            string fileName = Path.GetFileName(path);
            string directory = Path.GetDirectoryName(path);
            return CreateFileAndDirectory(fileName, directory);
        }

        /// <summary>
        /// Creates all directories and subdirectories unless they already exist and creates or overwrites a file in the same path.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="directory">The directory to create.</param>
        public static FileStream CreateFileAndDirectory(string fileName, string directory)
        {
            if (fileName == null) { throw new ArgumentNullException(nameof(fileName)); }
            else if (string.IsNullOrWhiteSpace(fileName)) { throw new ArgumentException("fileName cannot be empty."); }

            if (directory == null) { throw new ArgumentNullException(nameof(directory)); }
            else if (string.IsNullOrWhiteSpace(directory)) { throw new ArgumentException("directory cannot be empty."); }

            Directory.CreateDirectory(directory);

            string path = directory + (directory.EndsWith("\\") ? string.Empty : "\\") + fileName;
            return File.Create(path);
        }
    }
}
