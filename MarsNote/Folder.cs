using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MarsNote
{
    /// <summary>
    /// A collection of <paramref name="Note"/>s.
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
        public Folder(string name, ObservableCollection<Note> notes, bool pinned)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException("name"); }
            Name = name;
            Notes = notes ?? new ObservableCollection<Note>();
            Pinned = pinned;
        }

        public Folder(string name) : this(name, new ObservableCollection<Note>(), false) { }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
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

        public string ToJson()
        {
            return JsonHelper.Serialize(this);
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
