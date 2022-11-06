using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using System.Diagnostics;
using СlinicReception.ViewModels;

namespace СlinicReception.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new MainWindowViewModel();
        }
    }
}