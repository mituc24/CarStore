﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CarStore.Helpers;

// Helper class to set the navigation target for a NavigationViewItem.
//
// Usage in XAML:
// <NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavigationHelper.NavigateTo="AppName.ViewModels.MainViewModel" />
//
// Usage in code:
// NavigationHelper.SetNavigateTo(navigationViewItem, typeof(MainViewModel).FullName);
public class NavigationHelper
{
    public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

    public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);

    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));

    // Hàm để lưu trữ tham số cho NavigationViewItem
    public static void SetNavigationParameter(NavigationViewItem item, object parameter)
    {
        item.SetValue(NavigationParameterProperty, parameter);
    }

    public static object GetNavigationParameter(NavigationViewItem item)
    {
        return item.GetValue(NavigationParameterProperty);
    }

    public static readonly DependencyProperty NavigationParameterProperty =
        DependencyProperty.RegisterAttached("NavigationParameter", typeof(object), typeof(NavigationHelper), new PropertyMetadata(null));
}
