using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MarsNote
{
    /// <summary>
    /// A collection of <paramref name="Folder"/>s.
    /// </summary>
    public class Profile : INotifyPropertyChanged
    {
        /// <summary>
        /// Private member for <see cref="Name"/>.
        /// </summary>
        private string _name;

        /// <summary>
        /// Private member for <see cref="Folders"/>.
        /// </summary>
        private ObservableCollection<Folder> _folders;

        [JsonConstructor]
        public Profile(string name, ObservableCollection<Folder> folders)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException(nameof(name)); }
            Name = name;
            Folders = folders ?? new ObservableCollection<Folder>();
        }

        public Profile(string name) : this(name, new ObservableCollection<Folder>()) { }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name of the profile.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the folders included in the profile.
        /// </summary>
        public ObservableCollection<Folder> Folders
        {
            get
            {
                return _folders;
            }
            set
            {
                _folders = value;
                NotifyPropertyChanged();
            }
        }

        public string ToJson() => JsonHelper.Serialize(this);

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
