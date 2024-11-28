﻿using System.Diagnostics;
using CarStore.Models;
using System.Diagnostics;
using CarStore.Core.Contracts.Repository;
using CarStore.Core.Models;
using CarStore.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Devices.Enumeration;
using CarStore.Services.DataAccess;
using CarStore.Core.Contracts.Services;
using CarStore.Helpers;
using System.Collections.ObjectModel;
namespace CarStore.Views;

public sealed partial class MainPage : Page
{
    IDao dao;

    public MainPageViewModel ViewModel
    {
        get; set;
    }

    public FilterViewModel filterViewModel
    {
        get;
        set;
    }
    private readonly IDao<Car> _carDao;
    private readonly IDao<TypeOfCar> _typeOfCarDao;
    private bool flag;
    public FullObservableCollection<Car> Cars
    {
        get; set;
    }

    public FullObservableCollection<TypeOfCar> TypeOfCars
    {
        get; set;
    }
    private async Task LoadInitialDataAsync()
    {
        if (_carDao == null)
        {
            // Handle the null case appropriately, e.g., log an error or initialize _carDao
            throw new InvalidOperationException("_carDao is not initialized.");
        }

        var cars = await _carDao.GetAllAsync();
        var typeOfCars = await _typeOfCarDao.GetAllAsync();
 
        Cars = new FullObservableCollection<Car>(cars);
        TypeOfCars = new FullObservableCollection<TypeOfCar>(typeOfCars);

    }

    public MainPage()
    {
        flag = false;
        _carDao = App.GetService<IDao<Car>>();
        _typeOfCarDao = App.GetService<IDao<TypeOfCar>>();
        ViewModel = App.GetService<MainPageViewModel>();
        Task.Run(async () => await LoadInitialDataAsync()).Wait();
        Loaded += async (s, e) =>
        {
            await MainPage_Loaded(s, e);
            InitializeComponent();
            DataContext = ViewModel;
        };
    }

    private async Task MainPage_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadInitialDataAsync();
    }
    private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
    {
        //Frame.Navigate(typeof(LoginPage), name.Text);
    }

    private void BtnLogin_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private void BtnSignup_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    //See detail of car
    private void InfoBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedCar = (sender as Button).DataContext as Car;

        // Kiểm tra nếu selectedCar không null, sau đó chuyển đến CarDetailPage và truyền xe đã chọn
        if (selectedCar != null)
        {
            Frame.Navigate(typeof(CarDetailPage), selectedCar);
        }
    }
    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            // Filter cars by name or manufacturer
            var suitableItems = Cars
                .Where(car =>
                    car.Name.Contains(sender.Text, StringComparison.OrdinalIgnoreCase) ||
                    car.Manufacturer.ToString().Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
                .Select(car => new
                {
                    car.Name,
                    car.DefautlImageLocation // Path to car image
                })
                .ToList();

            // If no match, display "No results found"
            if (suitableItems.Count == 0)
            {
                sender.ItemsSource = new[] { new { Name = "No results found", DefautlImageLocation = string.Empty } };
            }
            else
            {
                sender.ItemsSource = suitableItems;
            }
        }
    }
    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        // Cast the selected item to the appropriate type
        var selectedCar = args.SelectedItem as dynamic;
        if (selectedCar != null)
        {
            // Find the actual car object based on the selected name
            var car = Cars.FirstOrDefault(c => c.Name == selectedCar.Name);
            if (car != null)
            {
                flag = true;
                // Navigate to CarDetailPage with the selected car as a parameter
                Frame.Navigate(typeof(CarDetailPage), car);
            }
        }
    }

    private void Control2_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        // Get the query text entered by the user
        var queryText = args.QueryText;
        if (flag)
            return;
        if (!string.IsNullOrWhiteSpace(queryText))
        {
            // Find the relevant TypeOfCar object based on queryText
            Frame.Navigate(typeof(FilterPage), queryText);
        }
        else
        {
            Debug.WriteLine("Query text is empty.");
        }
    }
    private void BuyBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedCar = (sender as Button).DataContext as Car;

        // Kiểm tra nếu selectedCar không null, sau đó chuyển đến CarDetailPage và truyền xe đã chọn
        if (selectedCar != null)
        {
            Frame.Navigate(typeof(CarDetailPage), selectedCar);
        }
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox)sender;
        if (comboBox?.DataContext is Car currentItem && comboBox.SelectedItem is VariantOfCar selectedVariant)
        {
            // Update ImageLocation based on selected Variant 
            var variantCode = "";
            Task.Run(async () => { 
                variantCode = await ViewModel._carRepository.GetVariantsCodeByName(selectedVariant.Name);
            }).Wait();
            currentItem.DefautlImageLocation = $"../Assets/Cars/{currentItem.Images}/{variantCode}/1{Path.GetExtension(currentItem.DefautlImageLocation)}";
        }
    }

    private void Categories_btn_click(object sender, ItemClickEventArgs e)
    {
        var typeOfCar = e.ClickedItem as TypeOfCar;
        if (typeOfCar != null && Frame != null)
        {
            Frame.Navigate(typeof(FilterPage), typeOfCar);
        }
        else
        {
            // Handle the case where Frame is null
            Debug.WriteLine("Frame is null or typeOfCar is null");
        }
    }

    private void BtnAccount_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(Account),ViewModel.CurrentUser);
    }
}