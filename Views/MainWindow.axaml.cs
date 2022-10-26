using Avalonia.Controls;
using Avalonia.ReactiveUI;
using СlinicReception.ViewModels;

namespace СlinicReception.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}