using System.Windows;
using LinqToSQL.ViewModels;

namespace LinqToSQL.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
