using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace СlinicReception.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        string show;
        public string Show
        {
            get => show;
            private set => this.RaiseAndSetIfChanged(ref show, value);
        }
        public void PassShow()
        {
            if (Show == "") Show = "*";
            else Show = "";
        }
        public LoginViewModel()
        {
            Show = "*";
        }
    }
}
