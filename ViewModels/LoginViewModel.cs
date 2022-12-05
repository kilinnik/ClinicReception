using Avalonia;
using ReactiveUI;
using СlinicReception.Services;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using Microsoft.EntityFrameworkCore;

namespace СlinicReception.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        MainWindowViewModel mainWindow; //для смены view
        public MainWindowViewModel MainWindow
        {
            get => mainWindow;
            private set => this.RaiseAndSetIfChanged(ref mainWindow, value);
        }

        bool showMesWrongData; //для сообщения о неправильных данных
        public bool ShowMesWrongData
        {
            get => showMesWrongData;
            private set => this.RaiseAndSetIfChanged(ref showMesWrongData, value);
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

        private static readonly MaterialTheme MaterialThemeStyles = Application.Current!.LocateMaterialTheme<MaterialTheme>();

        bool isDark;

        public void ChangeTheme() 
        {
            MaterialThemeStyles.BaseTheme = isDark ? BaseThemeMode.Light : BaseThemeMode.Dark;
            isDark= !isDark;
        }

        private bool visibleLoad;
        public bool VisibleLoad
        {
            get => visibleLoad;
            set => this.RaiseAndSetIfChanged(ref visibleLoad, value);
        }

        public async void CheckLogin()
        {
            VisibleLoad = true;
            using var db = new СlinicReceptionContext();
            var user = await db.Log_In.FirstOrDefaultAsync(x => x.Login == login && x.Password == Password);
            if (user != null)
            {
                if (user.Role == "Админ БД") MainWindow.DbAdmin();
                if (user.Role == "Админ данных") MainWindow.DataAdmin();
                if (user.Role == "Главврач") MainWindow.HeadDoctor();
                if (user.Role == "Регистратор") MainWindow.Registrar();
                if (user.Role == "Врач") MainWindow.Doctor((int)user.ID);
                if (user.Role == "Пациент") MainWindow.Patient((int)user.ID);
            }
            else ShowMesWrongData = true;
            VisibleLoad = false;
        }

        public LoginViewModel(MainWindowViewModel mw)
        {
            isDark = MaterialThemeStyles.BaseTheme == BaseThemeMode.Dark; MainWindow = mw;
        }

        public void Registration() //view регистрации
        {
            MainWindow.Registration();
        }
    }
}
