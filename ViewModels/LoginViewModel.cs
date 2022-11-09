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
        MainWindowViewModel mw; //для смены view
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }

        bool showMesWrongData; //для сообщения о неправильных данных
        public bool ShowMesWrongData
        {
            get => showMesWrongData;
            private set => this.RaiseAndSetIfChanged(ref showMesWrongData, value);
        }

        string? login; //логин
        public string? Login
        {
            get => login;
            private set => this.RaiseAndSetIfChanged(ref login, value);
        }

        string? password; //пароль
        public string? Password
        {
            get => password;
            private set => this.RaiseAndSetIfChanged(ref password, value);
        }

        public void Registration() //view регистрации
        {
            MW.Registration();
        }

        public void Theme() //изменить тему
        {
            MW.ChangeTheme();
        }

        public void CheckLogin() //проверка данных и вызов соответсвующего view
        {
            using var db = new СlinicReceptionContext();
            if (db.Log_In.Any(x => x.Login == login && x.Password == Password))
            {
                if (db.Log_In.First(x => x.Login == Login).Role == "Админ БД") MW.DbAdmin();
                if (db.Log_In.First(x => x.Login == Login).Role == "Админ данных") MW.DataAdmin();
                if (db.Log_In.First(x => x.Login == Login).Role == "Главврач") MW.HeadDoctor();
                if (db.Log_In.First(x => x.Login == Login).Role == "Регистратор") MW.Registrar();
                if (db.Log_In.First(x => x.Login == Login).Role == "Врач") MW.Doctor((int)db.Log_In.First(x => x.Login == Login).ID);
                if (db.Log_In.First(x => x.Login == Login).Role == "Пациент") MW.Patient((int)db.Log_In.First(x => x.Login == Login).ID);
            }
            else ShowMesWrongData = true;
        }

        public LoginViewModel(MainWindowViewModel mw)
        {
            MW = mw;
        }
    }
}
