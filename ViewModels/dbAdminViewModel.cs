using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using СlinicReception.Services;

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

        //Изменение данных о пользователях

        ObservableCollection<DataLogin> dataLogins = new();
        public ObservableCollection<DataLogin> DataLogins
        {
            get => dataLogins;
            set => this.RaiseAndSetIfChanged(ref dataLogins, value);
        }

        ObservableCollection<string> roles = new() { "Админ данных", "Главврач", "Регистратор", "Врач", "Пациент" };
        public ObservableCollection<string> Roles
        {
            get => roles;
            set => this.RaiseAndSetIfChanged(ref roles, value);
        }

        string? selectedRole;
        public string? SelectedRole
        {
            get => selectedRole;
            set => this.RaiseAndSetIfChanged(ref selectedRole, value);
        }

        string? loginForDelete;
        public string? LoginForDelete
        {
            get => loginForDelete;
            set => this.RaiseAndSetIfChanged(ref loginForDelete, value);
        }

        string? login;
        public string? Login
        {
            get => login;
            set => this.RaiseAndSetIfChanged(ref login, value);
        }

        string? password;
        public string? Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }

        int? id;
        public int? ID
        {
            get => id;
            set => this.RaiseAndSetIfChanged(ref id, value);
        }

        public void UpdateUsers()
        {
            using var db = new СlinicReceptionContext();
            foreach (var dataLogin in DataLogins)
            {
                var user = db.Log_In.First(x => x.Login == dataLogin.Login);
                user.Password = dataLogin.Password; user.Role = dataLogin.Role; user.ID = dataLogin.ID;
                db.SaveChanges();
            }
        }

        public void DeleteUser()
        {
            using var db = new СlinicReceptionContext();
            db.Log_In.Remove(db.Log_In.First(x => x.Login == Login));
            DataLogins.Remove(DataLogins.First(x => x.Login == Login));
            db.SaveChanges();
        }

        public void AddUser()
        {
            using var db = new СlinicReceptionContext();
            db.Log_In.Add(new DataLogin(Login, Password, SelectedRole, ID));
            DataLogins.Add(new DataLogin(Login, Password, SelectedRole, ID));
            db.SaveChanges();
        }

        //Препараты

        ObservableCollection<Preparation> preparations = new();
        public ObservableCollection<Preparation> Preparations
        {
            get => preparations;
            set => this.RaiseAndSetIfChanged(ref preparations, value);
        }

        int? preparationNumber;
        public int? PreparationNumber
        {
            get => preparationNumber;
            set => this.RaiseAndSetIfChanged(ref preparationNumber, value);
        }

        string? preparationName;
        public string? PreparationName
        {
            get => preparationName;
            set => this.RaiseAndSetIfChanged(ref preparationName, value);
        }

        public void UpdatePreparations()
        {
            using var db = new СlinicReceptionContext();
            foreach (var preparation in Preparations)
            {
                var dbPreparation = db.Препарат.First(x => x.Код_препарата == preparation.Код_препарата);
                dbPreparation.Название_препарата = preparation.Название_препарата;
                db.SaveChanges();
            }
        }

        public void DeletePreparation()
        {
            using var db = new СlinicReceptionContext();
            db.Препарат.Remove(db.Препарат.First(x => x.Код_препарата == PreparationNumber));
            Preparations.Remove(Preparations.First(x => x.Код_препарата == PreparationNumber));
            db.SaveChanges();
        }

        public void AddPreparation()
        {
            using var db = new СlinicReceptionContext();
            var lastPrep = db.Препарат.Last();
            db.Препарат.Add(new Preparation(lastPrep.Код_препарата + 1, PreparationName));
            Preparations.Add(new Preparation(lastPrep.Код_препарата + 1, PreparationName));
            db.SaveChanges();
        }

        //Диагнозы

        ObservableCollection<Diagnosis> diagnosises = new(); //список улиц
        public ObservableCollection<Diagnosis> Diagnosises
        {
            get => diagnosises;
            set => this.RaiseAndSetIfChanged(ref diagnosises, value);
        }

        int? diagnosisNumber;
        public int? DiagnosisNumber
        {
            get => diagnosisNumber;
            set => this.RaiseAndSetIfChanged(ref diagnosisNumber, value);
        }

        string? diagnosisName;
        public string? DiagnosisName
        {
            get => diagnosisName;
            set => this.RaiseAndSetIfChanged(ref diagnosisName, value);
        }

        int? preparationCode;
        public int? PreparationCode
        {
            get => preparationCode;
            set => this.RaiseAndSetIfChanged(ref preparationCode, value);
        }

        public void UpdateDiagnosises()
        {
            using var db = new СlinicReceptionContext();
            foreach (var diagnosis in Diagnosises)
            {
                var dbDiagnosis = db.Диагноз.First(x => x.Код_диагноза == diagnosis.Код_диагноза);
                dbDiagnosis.Название = DiagnosisName; dbDiagnosis.Код_препарата = PreparationCode;
                db.SaveChanges();
            }
        }

        public void DeleteDiagnosis()
        {
            using var db = new СlinicReceptionContext();
            db.Диагноз.Remove(db.Диагноз.First(x => x.Код_диагноза == DiagnosisNumber));
            Diagnosises.Remove(Diagnosises.First(x => x.Код_диагноза == DiagnosisNumber));
            db.SaveChanges();
        }

        public void AddDiagnosis()
        {
            using var db = new СlinicReceptionContext();
            var lastDiag = db.Диагноз.Last();
            db.Диагноз.Add(new Diagnosis(lastDiag.Код_диагноза + 1, DiagnosisName, PreparationCode));
            Diagnosises.Add(new Diagnosis(lastDiag.Код_диагноза + 1, DiagnosisName, PreparationCode));
            db.SaveChanges();
        }

        public DbAdminViewModel(MainWindowViewModel mw)
        {
            MW = mw; using var db = new СlinicReceptionContext();

            var dataLogins = db.Log_In.ToArray();
            foreach (var dataLogin in dataLogins) DataLogins.Add(dataLogin);

            var preparations = db.Препарат.ToArray();
            foreach (var preparation in preparations) Preparations.Add(preparation);

            var diagnosises = db.Диагноз.ToArray();
            foreach (var diagnosis in diagnosises) Diagnosises.Add(diagnosis);
        }

        public void Logout()
        {
            MW.Login();
        }
    }
}
