using System.Windows;
using WpfApp1.Presenters;
using WpfApp1.Services;
using WpfApp1.Views;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            
            // Check if user is authenticated and show dashboard
            if (loginWindow.DataContext is AuthenticationViewModel authViewModel && 
                authViewModel.IsAuthenticated)
            {
                var dashboardWindow = new DashboardWindow(authViewModel);
                dashboardWindow.Show();
                this.Hide();
            }
        }
    }
}