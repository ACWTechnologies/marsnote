using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
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
        /// Gets a collection of folders that this note can move to.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Folder> MoveFolders
        {
            get
            {
                var window = Application.Current.MainWindow as MainWindow;
                return window?.GetFoldersWithoutNote(this);
            }
        }

        /// <summary>
        /// Gets or sets the name of the note.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
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
        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
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
        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 3)]
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
        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 4)]
        [JsonConverter(typeof(RGBHexBrushAllowFullyTransparentConverter))]
        public Brush Colour
        {
            get
            {
                return _colour;
            }
            set
            {
                if (value != null)
                {
                    var scb = (SolidColorBrush)value;
                    if (!Color.Equals(scb.Color, Brushes.Transparent.Color) && scb.Color.A != 255)
                    {
                        // Set the alpha channel to max, transparency is disallowed
                        scb.Color = Color.FromArgb(255, scb.Color.R, scb.Color.G, scb.Color.B);
                    }
                    _colour = scb;
                }
                else
                {
                    _colour = Brushes.Transparent;
                }
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
        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 5)]
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
        [DefaultValue(false)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 6)]
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
