using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CarStore.ViewModels;
using CarStore.Core.Models;
using CommunityToolkit.WinUI.UI.Controls;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CarStore.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AuctionPage : Page
{
    public AuctionViewModel ViewModel
    {
        get;
    }

    
    public AuctionPage()
    {
        ViewModel = App.GetService<AuctionViewModel>();
        this.InitializeComponent();
        DataContext = ViewModel;
    }

    private async void JoinBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Auction selectedAuction)
        {
            if (!ViewModel.IsLoggedIn)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Thông báo",
                    Content = "Vui lòng đăng nhập để tham gia đấu giá",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
                return;
            }

            // Điều hướng đến trang chi tiết
            if (selectedAuction.condition == "Đang diễn ra")
            {
                Frame.Navigate(typeof(DetailAuctionPage), selectedAuction);
            }
            else if (selectedAuction.condition == "Sắp diễn ra")
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Thông báo",
                    Content = "Phiên đấu giá chưa bắt đầu",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
            else if (selectedAuction.condition == "Kết thúc")
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Thông báo",
                    Content = "Phiên đấu giá đã kết thúc",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
        
    }


    private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Auction selectedAuction)
        {
            await ViewModel.DeleteAuction(selectedAuction);
        }
    }
    private async void InforBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Auction selectedAuction)
        {
            var user = await ViewModel.GetInfor(selectedAuction);
            if (user != null)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Thông tin người chiến thắng",
                    Content = $"Tên: {user.Name}\nSố điện thoại: {user.Telephone}",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Thông tin người chiến thắng",
                    Content = "Không có thông tin người dùng.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
    }

    private void CreateReport(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(ReportPage));
    }
}
