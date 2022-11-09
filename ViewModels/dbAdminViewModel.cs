using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СlinicReception.ViewModels
{
    public class DbAdminViewModel : ViewModelBase
    {
        MainWindowViewModel mw;
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }
        public DbAdminViewModel(MainWindowViewModel mw)
        {
            MW = mw;
        }
    }
}
