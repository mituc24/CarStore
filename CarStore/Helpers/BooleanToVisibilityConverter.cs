﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace CarStore.Helpers;
public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool val)
        {
            bool inverse = parameter?.ToString() == "Inverse";
            return (val ^ inverse) ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

