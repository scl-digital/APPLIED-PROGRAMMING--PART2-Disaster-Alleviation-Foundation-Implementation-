using System.Windows;
using WpfApp1.Presenters;
using WpfApp1.Services;

namespace WpfApp1.Views
{
    public partial class LoginWindow : Window
    {
        private AuthenticationViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            
            var authService = new AuthenticationService();
            _viewModel = new AuthenticationViewModel(authService);
            DataContext = _viewModel;
            
            // Subscribe to property changes to handle visibility
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            
            // Set initial visibility
            UpdateVisibility();
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.IsLoginMode) || 
                e.PropertyName == nameof(_viewModel.IsLoading) ||
                e.PropertyName == nameof(_viewModel.Message))
            {
                UpdateVisibility();
            }
        }

        private void UpdateVisibility()
        {
            // Show/hide login and register panels
            LoginPanel.Visibility = _viewModel.IsLoginMode ? Visibility.Visible : Visibility.Collapsed;
            RegisterPanel.Visibility = _viewModel.IsLoginMode ? Visibility.Collapsed : Visibility.Visible;
            
            // Show/hide loading indicator
            var loadingPanel = (StackPanel)FindName("LoadingPanel");
            if (loadingPanel != null)
            {
                loadingPanel.Visibility = _viewModel.IsLoading ? Visibility.Visible : Visibility.Collapsed;
            }
            
            // Show/hide message
            var messageBorder = (Border)FindName("MessageBorder");
            if (messageBorder != null)
            {
                messageBorder.Visibility = !string.IsNullOrEmpty(_viewModel.Message) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
