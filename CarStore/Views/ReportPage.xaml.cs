using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CarStore.Models.AI;
using CarStore.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CarStore.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReportPage : Page
{
    public ReportViewModel ViewModel { get; }
    private string ReportText;
    public ReportPage()
    {
        ViewModel = App.GetService<ReportViewModel>();
        this.InitializeComponent();
    }

    private async Task genReport()
    {
        var promt ="Generate report of auctions: " + ViewModel.CreateListInfoOfAuction(ViewModel.Auctions);
        ReportText =  await ViewModel.gemini.GenerateResponseAsync(promt, new List<ChatHistory>());
        ReportText ??= "Error in generating report";
        
    }

    private async void GenerateReportButton_Click(object sender, RoutedEventArgs e)
    {
        var promt = "Generate report of auctions (How many reports this months, what are these car, show price and more detail,...): " + ViewModel.CreateListInfoOfAuction(ViewModel.Auctions);
        ReportText = await ViewModel.gemini.GenerateResponseAsync(promt, new List<ChatHistory>());
        ReportText ??= "Error in generating report";
        ReportSection.Text = ReportText;
    }

    private void SaveReportButton_Click(object sender, RoutedEventArgs e)
    {
        // Copy the text from the ReportSection to the clipboard
        var dataPackage = new DataPackage();
        dataPackage.SetText(ReportSection.Text);
        Clipboard.SetContent(dataPackage);
    }


}
