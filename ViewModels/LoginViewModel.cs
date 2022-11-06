using Avalonia;
using ReactiveUI;
using System.Collections.Generic;
using СlinicReception.Services;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using Material.Colors;
using System.Linq;

namespace СlinicReception.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        MainWindowViewModel mw; 
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }
        bool show;
        public bool Show
        {
            get => show;
            private set => this.RaiseAndSetIfChanged(ref show, value);
        }
        string? login;
        public string? Login
        {
            get => login;
            private set => this.RaiseAndSetIfChanged(ref login, value);
        }
        string? password;
        public string? Password
        {
            get => password;
            private set => this.RaiseAndSetIfChanged(ref password, value);
        }
        public void Registration()
        {
            MW.Registration();
        }
        public void Theme()
        {
            MW.ChangeTheme();
        }
        public void CheckLogin()
        {
            using var db = new СlinicReceptionContext();
            if (db.Log_In.Any(x => x.Login == login && x.Password == Password))
            {
                if (db.Log_In.First(x => x.Login == Login).Role == "Админ БД") MW.DbAdmin();
                if (db.Log_In.First(x => x.Login == Login).Role == "Админ данных") MW.DataAdmin();
                if (db.Log_In.First(x => x.Login == Login).Role == "Главврач") MW.HeadDoctor();
                if (db.Log_In.First(x => x.Login == Login).Role == "Регистратор") MW.Registrar();
                if (db.Log_In.First(x => x.Login == Login).Role == "Врач") MW.Doctor();
                if (db.Log_In.First(x => x.Login == Login).Role == "Пациент") MW.Patient((int)db.Log_In.First(x => x.Login == Login).ID);
            }
            else Show = true;

        }
        public LoginViewModel(MainWindowViewModel mw)
        {
            MW = mw; show = false;
        }
    }
}
