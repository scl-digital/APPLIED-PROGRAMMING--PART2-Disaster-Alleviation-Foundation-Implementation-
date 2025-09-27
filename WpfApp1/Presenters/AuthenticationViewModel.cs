using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.Presenters
{
    public class AuthenticationViewModel : INotifyPropertyChanged
    {
        private readonly IAuthenticationService _authService;
        
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _message = string.Empty;
        private bool _isLoginMode = true;
        private bool _isLoading = false;
        private User? _currentUser;

        public AuthenticationViewModel(IAuthenticationService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(async () => await LoginAsync(), CanExecuteCommand);
            RegisterCommand = new RelayCommand(async () => await RegisterAsync(), CanExecuteCommand);
            ToggleModeCommand = new RelayCommand(ToggleMode);
            LogoutCommand = new RelayCommand(Logout);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public bool IsLoginMode
        {
            get => _isLoginMode;
            set => SetProperty(ref _isLoginMode, value);
        }

        public bool IsRegisterMode => !_isLoginMode;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public User? CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public bool IsAuthenticated => _authService.IsAuthenticated;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ToggleModeCommand { get; }
        public ICommand LogoutCommand { get; }

        private async Task LoginAsync()
        {
            IsLoading = true;
            Message = string.Empty;

            try
            {
                var request = new LoginRequest
                {
                    Email = Email,
                    Password = Password
                };

                var result = await _authService.LoginAsync(request);
                
                if (result.IsSuccess)
                {
                    CurrentUser = result.User;
                    Message = "Login successful!";
                    OnPropertyChanged(nameof(IsAuthenticated));
                }
                else
                {
                    Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                Message = $"Login failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task RegisterAsync()
        {
            IsLoading = true;
            Message = string.Empty;

            try
            {
                if (Password != ConfirmPassword)
                {
                    Message = "Passwords do not match";
                    return;
                }

                var request = new RegisterRequest
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword
                };

                var result = await _authService.RegisterAsync(request);
                
                if (result.IsSuccess)
                {
                    CurrentUser = result.User;
                    Message = "Registration successful!";
                    OnPropertyChanged(nameof(IsAuthenticated));
                }
                else
                {
                    Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                Message = $"Registration failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ToggleMode()
        {
            IsLoginMode = !IsLoginMode;
            ClearFields();
            Message = string.Empty;
            OnPropertyChanged(nameof(IsRegisterMode));
        }

        private void Logout()
        {
            _authService.Logout();
            CurrentUser = null;
            ClearFields();
            Message = "Logged out successfully";
            OnPropertyChanged(nameof(IsAuthenticated));
        }

        private void ClearFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private bool CanExecuteCommand()
        {
            return !IsLoading && 
                   !string.IsNullOrWhiteSpace(Email) && 
                   !string.IsNullOrWhiteSpace(Password) &&
                   (IsLoginMode || (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Func<Task> executeAsync, Func<bool> canExecute = null!)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute ?? (() => true);
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null!)
        {
            _executeAsync = () => { execute(); return Task.CompletedTask; };
            _canExecute = canExecute ?? (() => true);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute();
        }

        public async void Execute(object? parameter)
        {
            await _executeAsync();
        }
    }
}
