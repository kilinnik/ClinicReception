using ReactiveUI;

namespace СlinicReception.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
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

        public MainWindowViewModel()
        {
            Content = new LoginViewModel(this);
        }
    }
}
