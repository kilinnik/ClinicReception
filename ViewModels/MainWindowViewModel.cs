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
        int width;
        public int Width
        {
            get => width;
            private set => this.RaiseAndSetIfChanged(ref width, value);
        }
        int height;
        public int Height
        {
            get => height;
            private set => this.RaiseAndSetIfChanged(ref height, value);
        }
        ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }
        public void DbAdmin()
        {
            Content = new DbAdminViewModel();
        }
        public void DataAdmin()
        {
            Content = new DataAdminViewModel();
        }
        public void HeadDoctor()
        {
            Content = new HeadDoctorViewModel(this);
        }
        public void Registrar()
        {
            Content = new RegistrarViewModel();
        }
        public void Doctor(int id)
        {
            Content = new DoctorViewModel(id, this);
        }
        public void Patient(int id)
        {
            Content = new PatientViewModel(id, this);
        }
        public void Registration()
        {
            Content = new RegistrationViewModel(this);
        }
        public void Login()
        {
            Content = new LoginViewModel(this);
        }
        public void ChangeTheme()
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
            Content = new LoginViewModel(this);
            Width = 1100; Height = 700;
        }
    }
}
