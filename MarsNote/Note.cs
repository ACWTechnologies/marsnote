using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MarsNote
{
    /// <summary>
    /// A single note.
    /// </summary>
    public class Note : INotifyPropertyChanged, IPinnable
    {
        /// <summary>
        /// Private member for <see cref="Name"/>.
        /// </summary>
        private string _name;

        /// <summary>
        /// Private member for <see cref="Description"/>.
        /// </summary>
        private string _description;

        /// <summary>
        /// Private member for <see cref="Content"/>.
        /// </summary>
        private string _content;

        /// <summary>
        /// Private member for <see cref="Colour"/>.
        /// </summary>
        private Brush _colour;

        /// <summary>
        /// Private member for <see cref="LastModified"/>.
        /// </summary>
        private DateTime _lastModified;

        /// <summary>
        /// Private member for <see cref="Pinned"/>.
        /// </summary>
        private bool _pinned;

        /// <summary>
        /// A value indicating whether the <see cref="Note"/> instance is ready for modification.
        /// </summary>
        private bool _readyForModification = false;

        [JsonConstructor]
        public Note(string name, string description, string content, Brush colour, DateTime lastModified, bool pinned)
        {
            Name = name;
            Description = description;
            Content = content;
            Colour = colour;
            LastModified = lastModified;
            Pinned = pinned;
            _readyForModification = true;
        }

        public Note() : this(null, null, null, null, DateTime.Now, false) { }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Modified()
        {
            if (_readyForModification) { LastModified = DateTime.Now; }
        }

        /// <summary>
        /// Gets or sets the name of the note.
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
                Modified();
            }
        }

        /// <summary>
        /// Gets or sets the description of the note.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value ?? string.Empty;
                NotifyPropertyChanged();
                Modified();
            }
        }

        /// <summary>
        /// Gets or sets the content of the note.
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value ?? string.Empty;
                NotifyPropertyChanged();
                Modified();
            }
        }

        /// <summary>
        /// Gets or sets the colour <see cref="Brush"/> of the note.
        /// </summary>
        public Brush Colour
        {
            get
            {
                return _colour;
            }
            set
            {
                _colour = value ?? Brushes.Transparent;
                NotifyPropertyChanged();
                Modified();
            }
        }

        /// <summary>
        /// Gets or sets the colour <see cref="Color"/> of the note.
        /// </summary>
        [JsonIgnore]
        public Color ColorColour
        {
            get
            {
                return ((SolidColorBrush)Colour).Color;
            }
            set
            {
                Colour = new SolidColorBrush(value);
            }
        }

        /// <summary>
        /// Gets or sets the last modified datetime of the note.
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return _lastModified;
            }
            set
            {
                _lastModified = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the note is pinned.
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

        public string ToJson() => JsonHelper.Serialize(this);

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
