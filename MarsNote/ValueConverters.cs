using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MarsNote
{
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

    public class ValueToBoolConverter : IValueConverter
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
}