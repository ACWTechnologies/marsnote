using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MarsNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public ObservableCollection<Profile> LoadedProfiles;
        
        public MainWindow()
        {
            Settings firstLoadSettings = Settings.Load();
            SaveWindowPosition = firstLoadSettings.SaveWindowPosition;

            InitializeComponent();
            
            button_renameProfile.IsEnabled = button_deleteProfile.IsEnabled = false;

            // Assign an event handler to ContentRendered
            ContentRendered += MainWindow_ContentRendered;
            
            UILoadNote(null);
            UILoadNotes(null);
            UILoadFolders(null);

            if (!File.Exists(FileHelper.SaveFileLocation))
            {
                // Save file does not exist
                FirstTimeLaunch();
            }
            else
            {
                try
                {
                    // Load profiles
                    LoadedProfiles = FileHelper.LoadProfiles(FileHelper.SaveFileLocation);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("A fatal error occurred while reading the save data file. This is likely due to malformed or missing information." + Environment.NewLine + "The application will now exit. If this error continues to occur, please contact ACW Technologies support." + Environment.NewLine + Environment.NewLine + "Message:" +Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine + "Save File Location: " + Environment.NewLine + "'" + FileHelper.SaveFileLocation + "'", "Save Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(1);
                }

                UILoadProfiles(LoadedProfiles);
                UILoadState(State.Load());
            }

            LoadAccentsToSettings();

            // Create a dispatcher timer for auto save
            autoSaveDT = new DispatcherTimer();
            // Add an event handler to the Tick event
            autoSaveDT.Tick += AutoSaveDT_Tick;
            // Set the interval
            ChangeAutoSaveInterval(firstLoadSettings.AutoSave);

            UILoadSettings(firstLoadSettings);

            AppHelper.ChangeAccent((string)comboBox_settings_accentColour.SelectedItem);

            AddSettingsEventHandlers();
        }
        
        DispatcherTimer autoSaveDT;
        private void AutoSaveDT_Tick(object sender, EventArgs e)
        {
            FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
        }

        /// <summary>
        /// Change the interval of the auto save timer.
        /// </summary>
        /// <param name="interval">The new interval.</param>
        private void ChangeAutoSaveInterval(int interval)
        {
            autoSaveDT.Stop();
            autoSaveDT.Interval = TimeSpan.FromMinutes(interval);
            if (interval != 0) autoSaveDT.Start();
        }

        /// <summary>
        /// Move a <see cref="Note"/> from one <see cref="Folder"/> to another.
        /// </summary>
        /// <param name="note">The note to move.</param>
        /// <param name="oldFolder">The folder to move the note from.</param>
        /// <param name="newFolder">The folder to move the note to.</param>
        public void MoveNoteToAnotherFolder(Note note, Folder oldFolder, Folder newFolder)
        {
            if (note == null || oldFolder == null || newFolder == null) { return; }

            // If the old folder contains the note
            if (oldFolder.Notes.Contains(note))
            {
                // Insert note to the start of the new folder
                newFolder.Notes.Insert(0, note);
                // Remove the note from the old folder
                oldFolder.Notes.Remove(note);

                UpdateMessages();
            }
        }

        /// <summary>
        /// Move a <see cref="Folder"/> from one <see cref="Profile"/> to another.
        /// </summary>
        /// <param name="folder">The folder to move.</param>
        /// <param name="oldProfile">The profile to move the folder from.</param>
        /// <param name="newProfile">The profile to move the folder to.</param>
        public void MoveFolderToAnotherProfile(Folder folder, Profile oldProfile, Profile newProfile)
        {
            if (folder == null) { throw new ArgumentNullException(nameof(folder)); }
            if (oldProfile == null) { throw new ArgumentNullException(nameof(oldProfile)); }
            if (newProfile == null) { throw new ArgumentNullException(nameof(newProfile)); }

            // If the old profile contains the folder
            if (oldProfile.Folders.Contains(folder))
            {
                // Insert folder to the start of the new profile
                newProfile.Folders.Insert(0, folder);
                // Remove the folder from the old profile
                oldProfile.Folders.Remove(folder);

                UpdateMessages();
            }
        }
        
        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            if (!this.AnyCornersOnScreen(AppHelper.WindowCorners.TopLeft | AppHelper.WindowCorners.TopRight))
            {
                // Window loaded off screen, most likely because of SaveWindowPosition setting
                WindowState = WindowState.Normal;
                Width = 950;
                Height = 550;
                this.CenterOnScreen();
            }
        }

        private void window_main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Save state to file
            State.Save(((Profile)comboBox_profiles.SelectedItem)?.Name, ((Folder)listBox_folders.SelectedItem)?.Name);
            // Deselect all profiles, folders and notes
            DeselectAll();
            // Save profiles to file
            FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
        }

        /// <summary>
        /// Sets up the application for first time use.
        /// </summary>
        private void FirstTimeLaunch()
        {
            // Create the save file
            var newSaveFile = FileHelper.CreateFileAndDirectory(FileHelper.SaveFileLocation);
            newSaveFile.Close();
            // Create a new collection of profiles
            LoadedProfiles = new ObservableCollection<Profile>();
            // Add a profile, folder and preformatted introduction note
            LoadedProfiles.Add(new Profile("Profile1"));
            LoadedProfiles[0].Folders.Add(new Folder("Folder1"));
            LoadedProfiles[0].Folders[0].Notes.Add(new Note("Welcome!", "An introduction to MarsNote", "{\\rtf1\\ansi\\ansicpg1252\\uc1\\htmautsp\\deff2{\\fonttbl{\\f0\\fcharset0 Times New Roman;}{\\f2\\fcharset0 Segoe UI;}}{\\colortbl\\red0\\green0\\blue0;\\red255\\green255\\blue255;}\r\n{\\*\\listtable\r\n{\\list\\listtemplateid1\\listhybrid\r\n{\\listlevel\\levelnfc23\\levelnfcn23\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace0\\levelindent0{\\leveltext\\leveltemplateid5\\'01\\'b7}{\\levelnumbers;}\\fi-360\\li720\\lin720\\jclisttab\\tx720}\r\n{\\listlevel\\levelnfc0\\levelnfcn0\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace0\\levelindent0{\\leveltext\\leveltemplateid6\\'02\\'01.;}{\\levelnumbers\\'01;}\\fi-360\\li1440\\lin1440\\jclisttab\\tx1440}\r\n{\\listlevel\\levelnfc0\\levelnfcn0\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace0\\levelindent0{\\leveltext\\leveltemplateid7\\'02\\'02.;}{\\levelnumbers\\'01;}\\fi-360\\li2160\\lin2160\\jclisttab\\tx2160}\r\n{\\listlevel\\levelnfc0\\levelnfcn0\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace0\\levelindent0{\\leveltext\\leveltemplateid8\\'02\\'03.;}{\\levelnumbers\\'01;}\\fi-360\\li2880\\lin2880\\jclisttab\\tx2880}\r\n{\\listlevel\\levelnfc0\\levelnfcn0\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace0\\levelindent0{\\leveltext\\leveltemplateid9\\'02\\'04.;}{\\levelnumbers\\'01;}\\fi-360\\li3600\\lin3600\\jclisttab\\tx3600}\r\n{\\listlevel\\levelnfc0\\levelnfcn0\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace0\\levelindent0{\\leveltext\\leveltemplateid10\\'02\\'05.;}{\\levelnumbers\\'01;}\\fi-360\\li4320\\lin4320\\jclisttab\\tx4320}\r\n{\\listlevel\\levelnfc0\\levelnfcn0\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace0\\levelindent0{\\leveltext\\leveltemplateid11\\'02\\'06.;}{\\levelnumbers\\'01;}\\fi-360\\li5040\\lin5040\\jclisttab\\tx5040}\r\n{\\listlevel\\levelnfc0\\levelnfcn0\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace0\\levelindent0{\\leveltext\\leveltemplateid12\\'02\\'07.;}{\\levelnumbers\\'01;}\\fi-360\\li5760\\lin5760\\jclisttab\\tx5760}\r\n{\\listlevel\\levelnfc0\\levelnfcn0\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace0\\levelindent0{\\leveltext\\leveltemplateid13\\'02\\'08.;}{\\levelnumbers\\'01;}\\fi-360\\li6480\\lin6480\\jclisttab\\tx6480}\r\n{\\listname ;}\\listid1}}\r\n{\\*\\listoverridetable\r\n{\\listoverride\\listid1\\listoverridecount0\\ls1}\r\n}\r\n\\loch\\hich\\dbch\\pard\\plain\\ltrpar\\itap0{\\lang1033\\fs18\\f2\\cf0 \\cf0\\ql{\\fs27\\f2 {\\b\\ul\\ltrch Welcome to MarsNote!}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\i\\ltrch A note taking application designed for simplicity, speed and customisability.}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch The MarsNote window has 3 panes: folders, notes and editor. The folder pane displays a list of all folders in the selected profile. The notes pane displays a list of all notes in the selected folder. The editor pane is where you can edit each note.}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs27\\f2 {\\lang2057\\ul\\ltrch Organisation}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch MarsNote allows you to organise your notes in two ways.}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24 {\\pntext \\'B7\\tab}{\\*\\pn\\pnlvlblt\\pnstart1{\\pntxtb\\'B7}}{\\lang2057\\b\\ltrch Profiles}{\\lang2057\\ltrch  are the first way you can separate notes. For example you could have a profile for all personal notes, and another for work notes. Add, rename, delete and switch between them by opening the profiles menu with the button at the top right of the window.}\\li795\\ri75\\sa75\\sb75\\jclisttab\\tx795\\fi-360\\ql\\par}\r\n{\\fs24 {\\pntext \\'B7\\tab}{\\*\\pn\\pnlvlblt\\pnstart1{\\pntxtb\\'B7}}{\\lang2057\\b\\ltrch Folders}{\\lang2057\\ltrch  are the way to organise notes inside each profile. Add and rename them with the buttons at the bottom of the folders pane. To delete them, hover over the folder and press the delete button that appears.}\\li795\\ri75\\sa75\\sb75\\jclisttab\\tx795\\fi-360\\ql\\par}\r\n{\\fs27\\f2 {\\lang2057\\ul\\ltrch Notes}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch Each note is comprised of a name (optional), description (optional), colour and content. The name, description and colour of the note, along with the last time it was modified is displayed in the notes pane where you can switch between notes.}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch To add a new note, make sure you have selected a profile and folder, and press the plus button at the bottom of the notes pane.}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch To delete a note, select it in the notes pane and press the delete button at the bottom of the editor pane}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch Notes can be }{\\lang2057\\b\\ltrch pinned}{\\lang2057\\ltrch  so that they are always at the top of the notes list as soon as you run MarsNote. To pin a note, select it in the notes pane and press the button star button at the bottom of the editor pane. Pinned notes also display a star in the notes pane.}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs27\\f2 {\\lang2057\\ul\\ltrch Editor}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch The note content editor has buttons at the top to modify text that is highlighted. For example, text can be made bold, a larger font size or aligned to the right hand side. Hotkeys can also be used to modify highlighted text.}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs27\\f2 {\\lang2057\\ul\\ltrch Settings}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch To access the settings menu, press the button at the top right of the window. Here you can change where your notes are saved, the accent colour of the window and much more. To contact support or report an issue with MarsNote, use the buttons at the bottom of the settings menu.}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 \\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch Feel free to delete the profile made for you already to make your own.}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 \\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch We hope you enjoy your experience with MarsNote,}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n{\\fs24\\f2 {\\lang2057\\ltrch The ACW Technologies team}\\li75\\ri75\\sa75\\sb75\\fi0\\ql\\par}\r\n}\r\n}", Brushes.Green, DateTime.Now, true));
            // Save profiles to file
            FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
            // Load the new collection of profiles into the UI
            UILoadProfiles(LoadedProfiles);
            // Select the first folder and note (just added)
            listBox_folders.SelectedIndex = 0;
            listBox_notes.SelectedIndex = 0;
        }

        /// <summary>
        /// Deselects all profiles, folders and notes.
        /// </summary>
        private void DeselectAll()
        {
            listBox_notes.SelectedIndex = -1;
            listBox_folders.SelectedIndex = -1;
            comboBox_profiles.SelectedIndex = -1;
        }

        #region UILoad
        /// <summary>
        /// Loads a collection of profiles into the profiles combobox.
        /// </summary>
        /// <param name="profiles">The collection of profiles.</param>
        private void UILoadProfiles(ObservableCollection<Profile> profiles)
        {
            comboBox_profiles.ItemsSource = profiles;
            if (LoadedProfiles.Count > 0)
            {
                comboBox_profiles.SelectedIndex = 0;
            }
            else
            {
                comboBox_profiles.SelectedIndex = -1;
            }
            UpdateMessages();
        }
        /// <summary>
        /// Loads a collection of folders into the folders listbox.
        /// </summary>
        /// <param name="folders">The collection of folders.</param>
        private void UILoadFolders(ObservableCollection<Folder> folders)
        {
            if (folders == null)
            {
                // No profile selected
                button_addFolder.IsEnabled = false;
            }
            else
            {
                // Profile selected
                button_addFolder.IsEnabled = true;
            }
            listBox_folders.ItemsSource = folders;
            UpdateMessages();
        }

        /// <summary>
        /// Loads a collection of notes into the notes listbox.
        /// </summary>
        /// <param name="notes">The collection of notes.</param>
        private void UILoadNotes(ObservableCollection<Note> notes)
        {
            if (notes == null)
            {
                // No folder selected
                button_addNote.IsEnabled = false;
            }
            else
            {
                // Folder selected
                button_addNote.IsEnabled = true;
            }
            listBox_notes.ItemsSource = notes;
            UpdateMessages();
        }

        /// <summary>
        /// Loads a single note into the editor.
        /// </summary>
        /// <param name="note">The note to load.</param>
        private void UILoadNote(Note note)
        {
            grid_editor.DataContext = note;
            button_pinNote.DataContext = note;
            grid_editor.IsEnabled = stackPanel_noteButtons.IsEnabled = note == null ? false : true;

            // Reset the undo queue
            richTextBox_content.UndoLimit = 0;
            richTextBox_content.UndoLimit = -1;
        }

        /// <summary>
        /// Loads a <see cref="State"/> into the application.
        /// </summary>
        /// <param name="state">The state to load.</param>
        private void UILoadState(State state)
        {
            if (state?.Profile != null)
            {
                foreach (Profile profile in LoadedProfiles)
                {
                    if (state.Profile == profile.Name)
                    {
                        comboBox_profiles.SelectedItem = profile;
                        break;
                    }
                }

                if (state.Folder != null)
                {
                    foreach (Folder folder in ((Profile)comboBox_profiles.SelectedItem).Folders ?? new ObservableCollection<Folder>())
                    {
                        if (state.Folder == folder.Name)
                        {
                            listBox_folders.SelectedItem = folder;
                            listBox_folders.ScrollIntoView(folder);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads <see cref="Settings"/> into the application.
        /// </summary>
        /// <param name="settings">The settings to load.</param>
        private void UILoadSettings(Settings settings)
        {
            if (settings != null)
            {
                textBox_settings_saveFileLocation.Text = settings.SaveFileLocation;
                comboBox_settings_accentColour.SelectedItem = settings.AccentColour;
                numericUpDown_settings_autoSave.Value = settings.AutoSave;
                toggleSwitch_settings_saveWindowPosition.IsChecked = settings.SaveWindowPosition;
                toggleSwitch_settings_startOnWindowsStartup.IsChecked = settings.StartOnWindowsStartup;
            }
            else
            {
                UILoadSettings(Settings.BlankSettings);
            }
        }
        #endregion
        
        #region Navigation Selection Changed
        private void comboBox_profiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button_renameProfile.IsEnabled = button_deleteProfile.IsEnabled = comboBox_profiles.SelectedIndex != -1;
            UILoadFolders(((Profile)comboBox_profiles.SelectedItem)?.Folders);
        }

        private void listBox_folders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UILoadNotes(((Folder)listBox_folders.SelectedItem)?.Notes);
        }

        private void listBox_notes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UILoadNote((Note)listBox_notes.SelectedItem);
            FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
        }
        #endregion
        
        #region Notes
        private void button_addNote_Click(object sender, RoutedEventArgs e)
        {
            AddNote();   
        }

        private void button_folderEmpty_addNote_Click(object sender, RoutedEventArgs e)
        {
            AddNote();
        }

        /// <summary>
        /// Adds a new note to the currently selected folder, at the top of the list.
        /// </summary>
        public void AddNote()
        {
            var n = new Note();
            ((Folder)listBox_folders.SelectedItem)?.Notes.Insert(0, n);
            if (listBox_notes.Items.Contains(n))
            {
                listBox_notes.SelectedItem = n;
            }

            UpdateMessages();
        }

        private async void button_removeNote_Click(object sender, RoutedEventArgs e)
        {
            var selectedNote = (Note)listBox_notes.SelectedItem;
            if (selectedNote == null) { return; }
            
            string message = string.IsNullOrWhiteSpace(selectedNote.Name) ? "Are you sure you want to delete this note? This action cannot be undone." : "Are you sure you want to delete the note '" + selectedNote.Name + "'? This action cannot be undone.";
            MessageDialogResult deleteAskResult = await this.ShowMessageAsync("Delete Note?", message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Delete", NegativeButtonText = "Cancel" });

            if (deleteAskResult == MessageDialogResult.Affirmative)
            {
                // Delete note
                ((Folder)listBox_folders.SelectedItem).Notes.Remove(selectedNote);
            }
            UpdateMessages();
        }

        private async void button_shareNote_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("Coming Soon", "The share notes feature will be added in a future version of MarsNote.");
        }
        #endregion

        #region Profiles
        private void button_noProfileSelected_viewProfiles_Click(object sender, RoutedEventArgs e)
        {
            profilesFlyout.IsOpen = true;
        }

        private async void button_addProfile_Click(object sender, RoutedEventArgs e)
        {
            string profileName = await this.ShowInputAsync("New Profile", "Enter a name for the new profile:", new MetroDialogSettings { AffirmativeButtonText = "Create", NegativeButtonText = "Cancel" });
            if (string.IsNullOrWhiteSpace(profileName)) { return; }

            foreach (Profile profile in LoadedProfiles)
            {
                if (profile.Name == profileName)
                {
                    // Profile with same name already exists
                    await this.ShowMessageAsync("New Profile", $"The name \'{profileName}\' is already in use by another profile. Please choose a different name and try again.");
                    return;
                }
            }

            // Create profile
            var p = new Profile(profileName);
            LoadedProfiles.Add(p);
            FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
            comboBox_profiles.SelectedItem = p;
        }

        private void button_renameProfile_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            RenameProfile((Profile)comboBox_profiles.SelectedItem);
        }

        /// <summary>
        /// Prompts the user to choose a new name for a <see cref="Profile"/>.
        /// </summary>
        /// <param name="profile">The profile to rename.</param>
        public async void RenameProfile(Profile profile)
        {
            if (profile == null) { return; }

            string newProfileName = await this.ShowInputAsync("Rename Profile", $"Enter a new name for the profile \'{profile.Name}\':", new MetroDialogSettings { AffirmativeButtonText = "Rename", NegativeButtonText = "Cancel" });
            if (string.IsNullOrWhiteSpace(newProfileName)) { return; }

            foreach (Profile existingProfile in LoadedProfiles)
            {
                if (existingProfile.Name == newProfileName)
                {
                    // Profile with same name already exists
                    await this.ShowMessageAsync("Rename Profile", $"The name \'{newProfileName}\' is already in use by another profile. Please choose a different name and try again.");
                    return;
                }
            }

            // Rename profile
            profile.Name = newProfileName;
            FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
        }

        private void button_deleteProfile_Click(object sender, RoutedEventArgs e)
        {
            DeleteProfile((Profile)comboBox_profiles.SelectedItem);
        }

        /// <summary>
        /// Deletes a <see cref="Profile"/>, with or without confirmation from the user.
        /// </summary>
        /// <param name="profile">The profile to delete.</param>
        /// <param name="requiresConfirmation">Whether or not the user has to confirm.</param>
        public async void DeleteProfile(Profile profile, bool requiresConfirmation = true)
        {
            if (profile == null) { return; }

            if (requiresConfirmation)
            {
                // User confirmation is required
                string result = await this.ShowInputAsync("Delete Profile?",
                    $"Are you sure you want to delete the profile \'{profile.Name}\'? This will delete ALL folders and notes inside this profile.\nType \"CONFIRM\" to confirm.", new MetroDialogSettings { AffirmativeButtonText = "Delete", NegativeButtonText = "Cancel" });

                if (result == "CONFIRM")
                {
                    // User typed "CONFIRM" correctly, delete profile
                    LoadedProfiles.Remove(profile);
                    FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
                    UILoadProfiles(LoadedProfiles);
                }
                else if (string.IsNullOrWhiteSpace(result))
                {
                    // User pressed 'cancel' or entered no text
                    return;
                }
                else
                {
                    // User did not type "CONFIRM" correctly
                    await this.ShowMessageAsync("Profile Not Deleted", "The confirmation text you entered did not match the required value. If you still wish to delete this profile, please try again.", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                }
            }
            else
            {
                // User confirmation is not required
                LoadedProfiles.Remove(profile);
                FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
                UILoadProfiles(LoadedProfiles);
            }
        }
        #endregion

        #region Folders
        private void button_addFolder_Click(object sender, RoutedEventArgs e)
        {
            NewFolder();
        }

        private void button_profileEmpty_addFolder_Click(object sender, RoutedEventArgs e)
        {
            NewFolder();
        }

        /// <summary>
        /// Prompts the user to create a new <see cref="Folder"/>.
        /// </summary>
        private async void NewFolder()
        {
            string folderName = await this.ShowInputAsync("New Folder", "Enter a name for the new folder:", new MetroDialogSettings { AffirmativeButtonText = "Create", NegativeButtonText = "Cancel" });
            if (string.IsNullOrWhiteSpace(folderName)) { return; }

            foreach (Folder folder in ((Profile)comboBox_profiles.SelectedItem).Folders)
            {
                if (folder.Name == folderName)
                {
                    // Folder with same name already exists
                    await this.ShowMessageAsync("New Folder", $"The name \'{folderName}\' is already in use by another folder. Please choose a different name and try again.");
                    return;
                }
            }

            // Create folder
            var f = new Folder(folderName);
            ((Profile)comboBox_profiles.SelectedItem).Folders.Add(f);
            FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
            listBox_folders.SelectedItem = f;
        }

        public void DeleteFolderFromListBoxItem(object sender)
        {
            var btn = (Button)sender;
            var folder = (Folder)btn.DataContext;
            DeleteFolder(folder);
        }

        /// <summary>
        /// Deletes a <see cref="Folder"/>, with or without confirmation from the user.
        /// </summary>
        /// <param name="folder">The folder to delete.</param>
        /// <param name="requiresConfirmation">Whether or not the user has to confirm.</param>
        public async void DeleteFolder(Folder folder, bool requiresConfirmation = true)
        {
            if (folder == null) { return; }

            if (requiresConfirmation)
            {
                MessageDialogResult deleteAskResult = await this.ShowMessageAsync("Delete Folder?", $"Are you sure you want to delete the folder \'{folder.Name}\'? This will delete ALL notes inside this folder.", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Delete", NegativeButtonText = "Cancel" });
                
                if (deleteAskResult == MessageDialogResult.Affirmative)
                {
                    // User confirmed, delete profile
                    ((Profile)comboBox_profiles.SelectedItem).Folders.Remove(folder);
                    FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
                    //UILoadProfiles(Profiles);
                }
            }
            else
            {
                // No confirmation required
                ((Profile)comboBox_profiles.SelectedItem).Folders.Remove(folder);
                FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
                //UILoadProfiles(Profiles);
            }

            UpdateMessages();
        }

        public void RenameFolderFromListBoxItem(object sender)
        {
            var btn = (Button)sender;
            var folder = (Folder)btn.DataContext;
            RenameFolder(folder);
        }

        /// <summary>
        /// Prompts the user to choose a new name for a <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder to rename.</param>
        public async void RenameFolder(Folder folder)
        {
            if (folder == null) { return; }

            string newFolderName = await this.ShowInputAsync("Rename Folder", $"Enter a new name for the folder \'{folder.Name}\':", new MetroDialogSettings { AffirmativeButtonText = "Rename", NegativeButtonText = "Cancel" });
            if (string.IsNullOrWhiteSpace(newFolderName)) { return; }

            foreach (Folder existingFolder in ((Profile)comboBox_profiles.SelectedItem).Folders)
            {
                if (existingFolder.Name == newFolderName)
                {
                    // Profile with same name already exists
                    await this.ShowMessageAsync("Rename Folder", "The name '" + newFolderName + "' is already in use by another folder. Please choose a different name and try again.");
                    return;
                }
            }
            
            // Rename profile
            folder.Name = newFolderName;
            FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);
        }
        #endregion

        /// <summary>
        /// Update the visibility of a series of UI messages based on selected items.
        /// </summary>
        void UpdateMessages()
        {
            if (LoadedProfiles == null) { return; }
            label_noProfiles.Visibility = LoadedProfiles.Count > 0 ? Visibility.Collapsed : Visibility.Visible;

            if (comboBox_profiles.SelectedIndex > -1)
            {
                // Profile selected
                grid_noProfileSelected.Visibility = Visibility.Collapsed;
                if (((Profile)comboBox_profiles.SelectedItem).Folders.Count == 0)
                {
                    // Empty profile
                    grid_profileEmpty.Visibility = Visibility.Visible;
                    grid_folderEmpty.Visibility = Visibility.Collapsed;
                    grid_noFolderSelected.Visibility = Visibility.Visible;
                }
                else
                {
                    // Non-empty profile
                    grid_profileEmpty.Visibility = Visibility.Collapsed;
                    if (listBox_folders.SelectedIndex == -1)
                    {
                        // No folder selected
                        grid_noFolderSelected.Visibility = Visibility.Visible;
                        grid_folderEmpty.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        // Folder Selected
                        grid_noFolderSelected.Visibility = Visibility.Collapsed;

                        if (((Folder)listBox_folders.SelectedItem).Notes.Count == 0)
                        {
                            // Empty folder
                            grid_folderEmpty.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            // Non-empty folder
                            grid_folderEmpty.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            else
            {
                // No profile selected
                grid_profileEmpty.Visibility = Visibility.Collapsed;
                grid_noProfileSelected.Visibility = Visibility.Visible;
                grid_folderEmpty.Visibility = Visibility.Collapsed;
                grid_noFolderSelected.Visibility = Visibility.Visible;
            }
        }

        #region Settings
        /// <summary>
        /// Gets a new <see cref="Settings"/> object, instantiated with data from the UI settings flyout.
        /// </summary>
        private Settings GetUISettings()
        {
            return new Settings(
                textBox_settings_saveFileLocation.Text,
                (string)comboBox_settings_accentColour.SelectedItem,
                (int)(numericUpDown_settings_autoSave.Value ?? 0),
                toggleSwitch_settings_saveWindowPosition.IsChecked
                );
        }

        /// <summary>
        /// Saves the <see cref="Settings"/> object from <see cref="GetUISettings"/>.
        /// </summary>
        private void SaveUISettings()
        {
            Settings.Save(GetUISettings());
        }

        /// <summary>
        /// Add event handlers for every control in the settings flyout.
        /// </summary>
        private void AddSettingsEventHandlers()
        {
            comboBox_settings_accentColour.SelectionChanged += comboBox_settings_accentColour_SelectionChanged;
            numericUpDown_settings_autoSave.ValueChanged += numericUpDown_settings_autoSave_ValueChanged;
            toggleSwitch_settings_saveWindowPosition.Checked += toggleSwitch_settings_saveWindowPosition_CheckChanged;
            toggleSwitch_settings_saveWindowPosition.Unchecked += toggleSwitch_settings_saveWindowPosition_CheckChanged;
            toggleSwitch_settings_startOnWindowsStartup.Checked += toggleSwitch_settings_startOnWindowsStartup_CheckChanged;
            toggleSwitch_settings_startOnWindowsStartup.Unchecked += toggleSwitch_settings_startOnWindowsStartup_CheckChanged;
        }

        // Save file location
        private void button_settings_defaultSaveFileLocation_Click(object sender, RoutedEventArgs e)
        {
            ChangeSaveFileLocation(FileHelper.DefaultSaveFileLocation);
        }

        private void button_settings_browseSaveFileLocation_Click(object sender, RoutedEventArgs e)
        {
            ChangeSaveFileLocation(FileHelper.BrowseForFolder("Select a location for the MarsNote save files to be saved."));
        }

        public enum NewSaveFileLocationMode { Overwrite, Load }
        /// <summary>
        /// Set a new save file location from the UI settings location.
        /// </summary>
        /// <param name="mode">The method used to set the new location.</param>
        private async void SetNewSaveFileLocationFromUI(NewSaveFileLocationMode mode)
        {
            Settings uiSettings = GetUISettings();
            switch (mode)
            {
                case NewSaveFileLocationMode.Overwrite:
                    {
                        // Display restart message asynchronously
                        Task<MessageDialogResult> restartMessage = this.ShowMessageAsync("Restart MarsNote", "MarsNote will now restart to save the notes to the new save location.", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });

                        // Save in new location
                        FileHelper.SaveProfiles(LoadedProfiles, Path.Combine(uiSettings.SaveFileLocation, FileHelper.DefaultSaveFileName));

                        // Write new location to settings
                        Settings.Save(uiSettings);

                        await restartMessage;

                        // Restart app
                        AppHelper.RestartApplication();
                        break;
                    }
                case NewSaveFileLocationMode.Load:
                    {
                        // Display restart message asynchronously
                        Task<MessageDialogResult> restartMessage = this.ShowMessageAsync("Restart MarsNote", "MarsNote will now restart to load the notes from the new save location.", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });

                        // Save in old location
                        FileHelper.SaveProfiles(LoadedProfiles, FileHelper.SaveFileLocation);

                        // Write new location to settings
                        Settings.Save(uiSettings);

                        await restartMessage;

                        // Restart app
                        AppHelper.RestartApplication();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <summary>
        /// Change the save file location.
        /// </summary>
        /// <param name="path">The new save file location.</param>
        private async void ChangeSaveFileLocation(string path)
        {
            if (path == null || path == textBox_settings_saveFileLocation.Text)
            {
                return;
            }

            if (FileHelper.FolderContainsSaveFile(path))
            {
                // New location already contains a save file, display warning
                MessageDialogResult result = await this.ShowMessageAsync("Warning",
                    "The folder you have selected already contains a MarsNote save file. If you proceed to select this folder you can either overwrite the current file with your open application data, or load the data from the files into the application.\nIf you choose to load the data, the open data will be saved to the old location and the new data will be opened.\n\nIf you are unsure which option is right for you, it is recommended to cancel to avoid data loss.",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Overwrite save file",
                        NegativeButtonText = "Load save file",
                        FirstAuxiliaryButtonText = "Cancel"
                    });
                
                switch (result) {
                    case MessageDialogResult.Affirmative:
                        // Overwrite save file
                        textBox_settings_saveFileLocation.Text = path;
                        SetNewSaveFileLocationFromUI(NewSaveFileLocationMode.Overwrite);
                        break;
                    case MessageDialogResult.Negative:
                        // Load save file
                        textBox_settings_saveFileLocation.Text = path;
                        SetNewSaveFileLocationFromUI(NewSaveFileLocationMode.Load);
                        break;
                    case MessageDialogResult.FirstAuxiliary:
                        // Cancel
                        return;
                }
            }
            else
            {
                // New location doesn't already contain a save file
                textBox_settings_saveFileLocation.Text = path;
                SetNewSaveFileLocationFromUI(NewSaveFileLocationMode.Overwrite);
            }
        }

        // Accent colour
        private void comboBox_settings_accentColour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_settings_accentColour.SelectedIndex != -1)
            {
                AppHelper.ChangeAccent((string)comboBox_settings_accentColour.SelectedItem);
                SaveUISettings();
            }
        }

        /// <summary>
        /// Load all available accents from <see cref="ThemeManager.Accents"/> into the accents combobox.
        /// </summary>
        private void LoadAccentsToSettings()
        {
            comboBox_settings_accentColour.Items.Clear();
            foreach (var accent in ThemeManager.Accents)
            {
                // Omit yellow as it causes text to be black on highlight, instead of white
                if (accent.Name != "Yellow")
                    comboBox_settings_accentColour.Items.Add(accent.Name);
            }
        }

        // Auto save
        private void numericUpDown_settings_autoSave_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            ChangeAutoSaveInterval((int)(numericUpDown_settings_autoSave.Value ?? 0));
            SaveUISettings();
        }

        // Start on Windows startup
        private async void toggleSwitch_settings_startOnWindowsStartup_CheckChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (toggleSwitch_settings_startOnWindowsStartup.IsChecked ?? false)
                {
                    AppHelper.AddCurrentUserStartup();
                }
                else
                {
                    AppHelper.RemoveCurrentUserStartup();
                }
            }
            catch (System.Security.SecurityException ex)
            {
                Task<MessageDialogResult> errorMessage = this.ShowMessageAsync("Insufficient Permissions", "You do not have the permissions required to change this setting. This process requires access to the Windows Registry.\nError: " + ex.Message);
                // Remove events
                toggleSwitch_settings_startOnWindowsStartup.Checked -= toggleSwitch_settings_startOnWindowsStartup_CheckChanged;
                toggleSwitch_settings_startOnWindowsStartup.Unchecked -= toggleSwitch_settings_startOnWindowsStartup_CheckChanged;

                // Switch toggleswitch back
                toggleSwitch_settings_startOnWindowsStartup.IsChecked = !toggleSwitch_settings_startOnWindowsStartup.IsChecked;

                // Add events
                toggleSwitch_settings_startOnWindowsStartup.Checked += toggleSwitch_settings_startOnWindowsStartup_CheckChanged;
                toggleSwitch_settings_startOnWindowsStartup.Unchecked += toggleSwitch_settings_startOnWindowsStartup_CheckChanged;

                await errorMessage;
            }
        }

        // Save window position
        private void toggleSwitch_settings_saveWindowPosition_CheckChanged(object sender, RoutedEventArgs e)
        {
            SaveWindowPosition = toggleSwitch_settings_saveWindowPosition.IsChecked ?? true;
            SaveUISettings();
        }

        // About
        private async void button_settings_about_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("A note taking application designed for simplicity, speed and customisability.");
            sb.AppendLine();
            sb.AppendLine("Version: " + AppHelper.GetCurrentVersion());
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("Licensed under a Creative Commons Attribution-NonCommercial 4.0 International license.");
            sb.AppendLine("Copyright © 2016 ACW Technologies");
            
            MessageDialogResult result = await this.ShowMessageAsync("About MarsNote", sb.ToString(), MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "OK", NegativeButtonText = "View License" });
            if (result == MessageDialogResult.Negative)
            {
                Process.Start(FileHelper.LicenseURL);
            }
        }

        // Help
        private void button_settings_help_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://acwtechnologies.co.uk/help/marsnote/?sender=mn");
        }

        // Report issue
        private void button_settings_reportIssue_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/ACWTechnologiesAdmin/MarsNote/issues");
        }
        #endregion
    }
}
