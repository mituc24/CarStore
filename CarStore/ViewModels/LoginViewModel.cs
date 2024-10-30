﻿using CarStore.Contracts.Services;
using CarStore.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public class LoginViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authService;

    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading;
    private bool _rememberMe;

    public string Username
    {
        get => _username;
        set
        {
            if (SetProperty(ref _username, value))
            {
                LoginCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
            {
                LoginCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool RememberMe
    {
        get => _rememberMe;
        set => SetProperty(ref _rememberMe, value);
    }

    public IAsyncRelayCommand LoginCommand
    {
        get;
    }
    public IRelayCommand NavigateToRegisterCommand
    {
        get;
    }
    //public IRelayCommand ForgotPasswordCommand
    //{
    //    get;
    //}

    public LoginViewModel(INavigationService navigationService, IAuthenticationService authService)
    {
        _navigationService = navigationService;
        _authService = authService;
        NavigateToRegisterCommand = new RelayCommand(ExecuteNavigateToRegister);
        LoginCommand = new AsyncRelayCommand(ExecuteLoginAsync, CanExecuteLogin);
      
        //ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);

        // Load saved credentials if they exist
        LoadSavedCredentialsAsync();
    }

    private async void LoadSavedCredentialsAsync()
    {
        var savedCredentials = await _authService.GetSavedCredentialsAsync();
        if (savedCredentials.HasValue)
        {
            Username = savedCredentials.Value.username;
            Password = savedCredentials.Value.password;
            RememberMe = true;
        }
    }

    private bool CanExecuteLogin()
    {
        return !string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Password) &&
               !IsLoading;
    }

    private async Task ExecuteLoginAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var loginSuccess = await _authService.LoginAsync(Username, Password);

            if (loginSuccess)
            {
                if (RememberMe)
                {
                    await _authService.SaveCredentialsAsync(Username, Password);
                }
                else
                {
                    await _authService.ClearSavedCredentialsAsync();
                }

                _navigationService.NavigateTo(typeof(MainViewModel).FullName!);
            }
            else
            {
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác";
            }
        }
        catch (Exception)
        {
            ErrorMessage = "Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại sau.";
        }
        finally
        {
            IsLoading = false;
        }
    }
    private void ExecuteNavigateToRegister()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("NavigateToRegisterCommand executed.");
            _navigationService.NavigateTo(typeof(FilterViewModel).FullName!);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
        }
    }


    //private void ExecuteForgotPassword()
    //{
    //    _navigationService.NavigateTo(typeof(ForgotPasswordViewModel).FullName!);
    //}
    // ... rest of the ViewModel implementation remains the same
}