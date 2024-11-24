﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CarStore.Services.DataAccess;
using CarStore.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CarStore.Services;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.System;
using System.Diagnostics;
using CarStore.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CarStore.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CarDetailPage : Page
{
    public CarDetailViewModel? ViewModel
    {
        get; set;
    }

    public MainPageViewModel? MainViewModel { get; set; }

    public CarDetailPage()
    {
        ViewModel = App.GetService<CarDetailViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
        MainViewModel = App.GetService<MainPageViewModel>();

    }

    private void SeeThisCar(object sender, ItemClickEventArgs e)
    {
        var selectedCar = e.ClickedItem as Car;
        if (selectedCar != null)
        {
            Frame.Navigate(typeof(CarDetailPage), selectedCar);
        }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is Car selectedCar)
        {
            ViewModel.SelectedCar = selectedCar;
        }
    }

    private void ChooseThisPicture(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is string clickedImagePath)
        {
            // Find the index of clicked item
            int index = ViewModel.SelectedCarPictures.IndexOf(clickedImagePath);
            if (index >= 0)
            {
                // Update the FlipView's selected index directly
                Gallery.SelectedIndex = index;
            }
        }
    }

    private async void ClickHomePageButton(object sender, RoutedEventArgs e)
    {
        var uri = new Uri("https://anycar.vn/");
        await Launcher.LaunchUriAsync(uri);
    }

    private async void ClickFacebookButton(object sender, RoutedEventArgs e)
    {
        var uri = new Uri("https://www.facebook.com/anycar.vn/");
        await Launcher.LaunchUriAsync(uri);
    }

    private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;

        if (comboBox != null && comboBox.SelectedItem != null)
        {
            var selectedColor = comboBox.SelectedValue as VariantOfCar;

            ViewModel.SelectedCarColor = selectedColor.Name;
        }
    }

    private void Color_Loaded(object sender, RoutedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox != null && comboBox.Items.Count > 0)
        {
            comboBox.SelectedIndex = 0;
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (Frame.CanGoBack)
        {
            Frame.GoBack();
        }
    }

    private async void Schudele_btn_click(object sender, RoutedEventArgs e)
    {
        if (!MainViewModel.IsLogin)
        {
            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Bạn chưa đăng nhập",
                Content = "Vui lòng đăng nhập để đặt lịch hẹn!",
                CloseButtonText = "OK",
            }.ShowAsync();

            return;
        }
        Frame.Navigate(typeof(ScheduleForm));
    }

    private async void Contact_btn_click(object sender, RoutedEventArgs e)
    {
        await new ContentDialog()
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Tính năng chưa hoàn thiện",
            Content = "Vui lòng chờ đợi các bản cập nhật kế tiếp để có thể sử dụng!",
            CloseButtonText = "OK",
        }.ShowAsync();
    }

    private async void AddWishlist_btn_click(object sender, RoutedEventArgs e)
    {
        await new ContentDialog()
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Tính năng chưa hoàn thiện",
            Content = "Vui lòng chờ đợi các bản cập nhật kế tiếp để có thể sử dụng!",
            CloseButtonText = "OK",
        }.ShowAsync();
    }

    private async void SeeMoreCompetitor_btn_click(object sender, RoutedEventArgs e)
    {
        await new ContentDialog()
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Tính năng chưa hoàn thiện",
            Content = "Vui lòng chờ đợi các bản cập nhật kế tiếp để có thể sử dụng!",
            CloseButtonText = "OK",
        }.ShowAsync();
    }

    private async void SeeMoreProduct_btn_click(object sender, RoutedEventArgs e)
    {
        await new ContentDialog()
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Tính năng chưa hoàn thiện",
            Content = "Vui lòng chờ đợi các bản cập nhật kế tiếp để có thể sử dụng!",
            CloseButtonText = "OK",
        }.ShowAsync();
    }

    private async void Compare_btn_click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "So sánh xe",
            Content = new Compare(ViewModel), // Chèn UserControl vào ContentDialog
            PrimaryButtonText = "OK",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = this.XamlRoot // Quan trọng trong WinUI 3
        };

        // Hiển thị ContentDialog
        var result = await dialog.ShowAsync();

        // Xử lý kết quả của người dùng
        if (result == ContentDialogResult.Primary)
        {
            // Người dùng nhấn OK
        }
        else
        {
            // Người dùng nhấn Cancel
        }
    }
}
