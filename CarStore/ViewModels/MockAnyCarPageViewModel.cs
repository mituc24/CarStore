﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Contracts.Services;
using CarStore.Core.Contracts.Repository;
using CarStore.Core.Contracts.Services;
using CarStore.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarStore.ViewModels;

public class MockAnyCarPageViewModel : ObservableObject, INotifyPropertyChanged
{
    public readonly ICarRepository _carRepository;
    private readonly IShowroomRepository showroomRepository;
    private readonly IDao<Car> _carDao;
    private readonly IDao<Variant> _variantDao;
    public List<Car>? Cars
    {
        get; set;
    }
    public bool isLoggedIn = false;

    public User? ViewedUser
    {
        get; set;
    }

    private Showroom? _showroom;
    public Showroom? Showroom
    {
        get => _showroom;
        set
        {
            _showroom = value;
            Task.Run(async() =>
            {
                Cars = await showroomRepository.GetCarsOfShowroom(Showroom);
                foreach (var car in Cars)
                {
                    car.VariantOfCars = await _carRepository.GetVariantsOfCar(car.CarId);
                    foreach (var variant in car.VariantOfCars)
                    {
                        variant.Variant = await _variantDao.GetByIdAsync(variant.VariantId);
                    }
                }
            }).Wait();
        }
    }

    public MockAnyCarPageViewModel(ICarRepository carRepository, IDao<Car> carDao, IDao<Variant> variantDao, IShowroomRepository showroomRepository)
    {
        _carDao = carDao;
        _carRepository = carRepository;
        _variantDao = variantDao;
        this.showroomRepository = showroomRepository;
    }
}
