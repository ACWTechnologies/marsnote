using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MarsNote
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> object to a string representation.
    /// </summary>
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
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
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns <see cref="FontStyles.Italic"/> if note name is null or whitespace.
    /// Else, returns <see cref="FontStyles.Normal"/>.
    /// </summary>
    [ValueConversion(typeof(string), typeof(FontStyle))]
    public class NoteNameStringToFontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace((string)value) ? FontStyles.Italic : FontStyles.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns "No Name" if note name is null or whitespace.
    /// Else, returns the note name.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class NoteNameStringToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace((string)value) ? "No Name" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns <see cref="Visibility.Visible"/> if bool is true.
    /// Else, returns <see cref="Visibility.Collapsed"/>.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns true if SelectedIndex is not -1.
    /// Else, returns false;
    /// </summary>
    [ValueConversion(typeof(int), typeof(bool))]
    public class SelectedIndexToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value != -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns true if SelectedIndex is not -1.
    /// Else, returns false;
    /// </summary>
    [ValueConversion(typeof(int), typeof(bool))]
    public class OneWayToSource_SelectedIndexToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- onewaytosource
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value != -1;
        }
    }

    /// <summary>
    /// Returns parameter[0] if bool is true.
    /// Else, returns parameter[1].
    /// </summary>
    [ValueConversion(typeof(bool), typeof(int))]
    public class BoolToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var values = parameter as int[];
            if (values == null) { return DependencyProperty.UnsetValue; }
            
            return (bool)value ? values[0] : values[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns <see cref="Visibility.Visible"/> if bool is true.
    /// Else, returns <see cref="Visibility.Hidden"/>.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Inverts a boolean value.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
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

    /// <summary>
    /// Returns true if count is more than 0.
    /// Else, returns false.
    /// </summary>
    [ValueConversion(typeof(IEnumerable), typeof(bool))]
    public class ItemsSourceCountToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iEnum = value as IEnumerable;
            return iEnum != null && iEnum.Cast<object>().Any();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }
    
    /// <summary>
    /// Returns true if count is more than 0.
    /// Else, returns false.
    /// </summary>
    [ValueConversion(typeof(IEnumerable<Profile>), typeof(IEnumerable<Profile>))]
    public class MoveFolderItemsSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iEnum = value as IEnumerable;

            if (iEnum == null || !iEnum.Cast<object>().Any())
            {
                return new Collection<Profile> { new Profile("No other profiles") };
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns true if count is more than 0.
    /// Else, returns false.
    /// </summary>
    [ValueConversion(typeof(IEnumerable<Folder>), typeof(IEnumerable<Folder>))]
    public class MoveNoteItemsSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iEnum = value as IEnumerable;

            if (iEnum == null || !iEnum.Cast<object>().Any())
            {
                return new Collection<Folder> { new Folder("No other folders") };
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not implemented as not requred -- oneway
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns true if any value is true.
    /// Else, returns false.
    /// </summary>
    [ValueConversion(typeof(bool[]), typeof(bool))]
    public class MultiBoolToBoolAnyTrueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Any(System.Convert.ToBoolean);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Returns true if all values are true.
    /// Else, returns false.
    /// </summary>
    [ValueConversion(typeof(bool[]), typeof(bool))]
    public class MultiBoolToBoolAllTrueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(System.Convert.ToBoolean);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Returns <see cref="Visibility.Visible"/> if all values are true.
    /// Else, returns <see cref="Visibility.Collapsed"/>.
    /// </summary>
    [ValueConversion(typeof(bool[]), typeof(Visibility))]
    public class MultiBoolToVisibilityCollapsedAllTrueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(System.Convert.ToBoolean) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}