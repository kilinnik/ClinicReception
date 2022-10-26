using Avalonia;
using ReactiveUI;
using System.Collections.Generic;
using СlinicReception.Services;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using Material.Colors;

namespace СlinicReception.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase content;
        int height;
        int width;
        string password;
        string login;
        public string Login
        {
            get => login;
            private set => this.RaiseAndSetIfChanged(ref login, value);
        }
        public string Password
        {
            get => password;
            private set => this.RaiseAndSetIfChanged(ref password, value);
        }
        public int Width
        {
            get => width;
            private set => this.RaiseAndSetIfChanged(ref width, value);
        }
        public int Height
        {
            get => height;
            private set => this.RaiseAndSetIfChanged(ref height, value);
        }
        public void UserLogin()
        {
            if (Login == "qwerty" && Password == "123") Patient();
        }
        public void Registration()
        {
            Content = new RegistrationViewModel();           
        }
        public void Themes()
        {
            var themeBootstrap = Application.Current.LocateMaterialTheme<MaterialThemeBase>();
            if (BaseThemeMode.Dark == themeBootstrap.CurrentTheme.GetBaseTheme())
            {
                var primary = PrimaryColor.Purple;
                var primaryColor = SwatchHelper.Lookup[(MaterialColor)primary];
                var secondary = SecondaryColor.Lime;
                var secondaryColor = SwatchHelper.Lookup[(MaterialColor)secondary];
                var theme = Theme.Create(Theme.Light, primaryColor, secondaryColor);
                themeBootstrap.CurrentTheme = theme;
            }
            else
            {
                var primary = PrimaryColor.Purple;
                var primaryColor = SwatchHelper.Lookup[(MaterialColor)primary];
                var secondary = SecondaryColor.Lime;
                var secondaryColor = SwatchHelper.Lookup[(MaterialColor)secondary];
                var theme = Theme.Create(Theme.Dark, primaryColor, secondaryColor);
                themeBootstrap.CurrentTheme = theme;
            }
        }
        public MainWindowViewModel()
        {
            Content = new LoginViewModel();
            width = 1100; height = 700;
        }
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }
        public void Patient()
        {
            Content = new PatientViewModel();
        }
    }
}
