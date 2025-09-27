using System.Windows;
using WpfApp1.Presenters;
using WpfApp1.Services;

namespace WpfApp1.Views
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow(AuthenticationViewModel authViewModel)
        {
            InitializeComponent();
            DataContext = authViewModel;
        }
    }
}
