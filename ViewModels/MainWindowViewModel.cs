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
        int width; //ширина окна
        public int Width
        {
            get => width;
            private set => this.RaiseAndSetIfChanged(ref width, value);
        }

        int height;//высота окна
        public int Height
        {
            get => height;
            private set => this.RaiseAndSetIfChanged(ref height, value);
        }

        ViewModelBase content;//текущее содержимое окна
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public void DbAdmin() //view админа бд
        {
            Content = new DbAdminViewModel(this);
        }

        public void DataAdmin() //view админа данных
        {
            Content = new DataAdminViewModel(this);
        }

        public void HeadDoctor() //view главврача
        {
            Content = new HeadDoctorViewModel(this);
        }

        public void Registrar() //view регистратора
        {
            Content = new RegistrarViewModel(this);
        }

        public void Doctor(int id) //view врача
        {
            Content = new DoctorViewModel(id, this);
        }

        public void Patient(int id) //view пациента
        {
            Content = new PatientViewModel(id, this);
        }

        public void Registration() //view регистрации
        {
            Content = new RegistrationViewModel(this);
        }

        public void Login() //view входа
        {
            Content = new LoginViewModel(this);
        }

        public void ChangeTheme() //изменение темы
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
