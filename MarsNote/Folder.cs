using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;

namespace MarsNote
{
    /// <summary>
    /// A collection of <see cref="Note"/>s.
    /// </summary>
    public class Folder : INotifyPropertyChanged, IPinnable
    {
        /// <summary>
        /// Private member for <see cref="Name"/>.
        /// </summary>
        private string _name;

        /// <summary>
        /// Private member for <see cref="Notes"/>.
        /// </summary>
        private ObservableCollection<Note> _notes;

        /// <summary>
        /// Private member for <see cref="Pinned"/>.
        /// </summary>
        private bool _pinned;

        [JsonConstructor]
        public Folder(string name, IEnumerable<Note> notes, bool pinned)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException(nameof(name)); }
            Name = name;
            Notes = notes == null ? new ObservableCollection<Note>() : new ObservableCollection<Note>(notes);
            Pinned = pinned;
        }

        public Folder(string name) : this(name, null, false) { }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a collection of profiles that this folder can move to.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Profile> MoveProfiles
        {
            get
            {
                var window = Application.Current.MainWindow as MainWindow;
                return window?.GetProfilesWithoutFolder(this);
            }
        }

        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        [JsonProperty(Required = Required.Always, Order = 1)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value ?? string.Empty;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the notes included in the folder.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public ObservableCollection<Note> Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the folder is pinned.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
        public bool Pinned
        {
            get
            {
                return _pinned;
            }
            set
            {
                _pinned = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
