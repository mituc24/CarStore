﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CarStore.Core.Contracts.Services;
using CarStore.Core.Models;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CarStore.Views;
public sealed partial class Compare : UserControl, INotifyPropertyChanged
{
    public readonly CarDetailViewModel ViewModel; // Has Compare Car

    private Car _car;
    public Car Car
    {
        get => _car;
        set
        {
            if (_car != value)
            {
                _car = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Car)));
            }
        }
    }
    private readonly IDao<Car> _carDao;

    public event PropertyChangedEventHandler? PropertyChanged;

    public List<Car> Cars
    {
        get; set;
    }

    public Compare(CarDetailViewModel VM)
    {
        ViewModel = VM;
        ViewModel.SelectedCar.DefautlImageLocation = "../" + ViewModel.SelectedCar.DefautlImageLocation;
        _carDao = App.GetService<IDao<Car>>();
        Task.Run(async () =>
        {
            Cars = await _carDao.GetAllAsync();
        }).Wait();
        foreach (Car car in Cars)
        {
            car.DefautlImageLocation = "../" + car.DefautlImageLocation;
        }
        this.InitializeComponent();
    }

    private void TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            // Filter cars by name or manufacturer
            var suitableItems = Cars
                .Where(car =>
                    car.Name.Contains(sender.Text, StringComparison.OrdinalIgnoreCase) ||
                    car.ManufacturerId.ToString().Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
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

    private void QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {

    }

    private void CarChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var car = args.SelectedItem as Car; // Get selected car
        Car = car;
        sender.Text = car.Name;
    }
}
