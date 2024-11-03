﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture.Frames;

namespace CarStore.Models;
public class User
{
    public string? Id
    {
        get; 
        set;
    }
    public string? Name
    {
        get; set;
    }
    public string? Email
    {
        get; set;
    }
    public string? Telephone
    {
        get; set;
    }
    public string? AccountType
    {
        get; set;
    }

    public string? Password
    {
        get; set;
    }
    public string? PasswordHash
    {
        get; set;
    }

    public string? Salt
    {
        get; set;
    }

    public string? Username
    {
        get; set;
    }

    public string? firstName
    {
        get; set;
    }
    public string? lastName
    {
        get; set;
    }

}
