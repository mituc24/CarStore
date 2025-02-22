﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Core.Dtos;
public class CarDto
{
    public int Id
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
    public int Price
    {
        get; set;
    }
    public string Image
    {
        get; set;
    }
    public string Manufacturer
    {
        get; set;
    }
    public string TypeOfCar
    {
        get; set;
    }
    public string EngineType
    {
        get; set;
    }
}
