using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MarsNote
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> object to a string representation.
    /// </summary>
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from DateTime to string
            var dateTime = (DateTime)value;
            DateTime now = DateTime.Now;
            if (dateTime.Date == now.Date)
            {
                return dateTime.ToString("\"Today\", h:mm tt");
            }
            else if (dateTime.Year == now.Year)
            {
                return dateTime.ToString("dd MMM");
            }
            else
            {
                return dateTime.ToString("dd MMM yyyy");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from string to DateTime
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns <see cref="FontStyles.Italic"/> if note name is null or whitespace.
    /// Else, returns <see cref="FontStyles.Normal"/>.
    /// </summary>
    public class NoteNameStringToFontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from string to FontStyle
            return string.IsNullOrWhiteSpace((string)value) ? FontStyles.Italic : FontStyles.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from FontStyle to string
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns "No Name" if note name is null or whitespace.
    /// Else, returns the note name.
    /// </summary>
    public class NoteNameStringToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from string to FontStyle
            return string.IsNullOrWhiteSpace((string)value) ? "No Name" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from FontStyle to string
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns <see cref="Visibility.Visible"/> if bool is true.
    /// Else, returns <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public class BoolToVisibilityCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from bool to Visibility
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from Visibility to bool
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns true if SelectedIndex is not -1.
    /// Else, returns false;
    /// </summary>
    public class SelectedIndexToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from int to bool
            return (int)value != -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from bool to int
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns true if SelectedIndex is not -1.
    /// Else, returns false;
    /// </summary>
    public class OneWayToSource_SelectedIndexToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from bool to int
            // Not implemented as not requred -- onewaytosource
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from int to bool
            return (int)value != -1;
        }
    }

    /// <summary>
    /// Returns parameter[0] if bool is true.
    /// Else, returns parameter[1].
    /// </summary>
    public class BoolToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var values = parameter as int[];
            if (values == null) { throw new ArgumentException("Integer array parameter required.", nameof(parameter)); }

            // Do the conversion from bool to int
            return (bool)value ? values[0] : values[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from int to bool
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns <see cref="Visibility.Visible"/> if bool is true.
    /// Else, returns <see cref="Visibility.Hidden"/>.
    /// </summary>
    public class BoolToVisibilityHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from bool to Visibility
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from Visibility to bool
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Inverts a boolean value.
    /// </summary>
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Invert bool
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Invert bool
            return !(bool)value;
        }
    }
}