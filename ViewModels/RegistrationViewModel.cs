using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using СlinicReception.Services;

namespace СlinicReception.ViewModels
{
    public class RegistrationViewModel: ViewModelBase
    {
        static List<List<string>> ListOfArea = new List<List<string>>() { new List<string> {"ул. им. 40-летия Победы", "ул. Островского", "Коллективная ул." }, new List<string> {"ул. Жлобы", "ул. МОПР", "Первомайская ул.", "ул. 1 Мая" }, new List<string> {"ул. Филатова", "Школьная ул." } };
        bool show;
        MainWindowViewModel mw;
        string? surname;
        string? name;
        string? patronymic;
        long? phone;
        string? dateOfBirthday;
        string? adress;
        string? login;
        string? password;
        string? textSurname;
        string? textName;
        string? textPatronymic;
        string? textPhone;
        string? textDateOfBirthday;
        string? textAdress;
        string? textLogin;
        string? textPassword;
        public string? TextSurname
        {
            get => textSurname;
            set => this.RaiseAndSetIfChanged(ref textSurname, value);
        }
        public string? TextName
        {
            get => textName;
            set => this.RaiseAndSetIfChanged(ref textName, value);
        }
        public string? TextPatronymic
        {
            get => textPatronymic;
            set => this.RaiseAndSetIfChanged(ref textPatronymic, value);
        }
        public string? TextPhone
        {
            get => textPhone;
            set => this.RaiseAndSetIfChanged(ref textPhone, value);
        }
        public string? TextDateOfBirthday
        {
            get => textDateOfBirthday;
            set => this.RaiseAndSetIfChanged(ref textDateOfBirthday, value);
        }
        public string? TextAdress
        {
            get => textAdress;
            set => this.RaiseAndSetIfChanged(ref textAdress, value);
        }
        public string? TextLogin
        {
            get => textLogin; 
            set => this.RaiseAndSetIfChanged(ref textLogin, value);
        }
        public string? TextPassword
        {
            get => textPassword;
            set => this.RaiseAndSetIfChanged(ref textPassword, value);
        }
        public string? Surname
        {
            get => surname;
            private set => this.RaiseAndSetIfChanged(ref surname, value);
        }
        public string? Name
        {
            get => name;
            private set => this.RaiseAndSetIfChanged(ref name, value);
        }
        public string? Patronymic
        {
            get => patronymic;
            private set => this.RaiseAndSetIfChanged(ref patronymic, value);
        }
        public long? Phone
        {
            get => phone;
            private set => this.RaiseAndSetIfChanged(ref phone, value);
        }
        public string? DateOfBirthday
        {
            get => dateOfBirthday;
            private set => this.RaiseAndSetIfChanged(ref dateOfBirthday, value);
        }
        public string? Adress
        {
            get => adress;
            private set => this.RaiseAndSetIfChanged(ref adress, value);
        }
        public string? Login
        {
            get => login;
            private set => this.RaiseAndSetIfChanged(ref login, value);
        }
        public string? Password
        {
            get => password;
            private set => this.RaiseAndSetIfChanged(ref password, value);
        }
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }
        public bool Show
        {
            get => show;
            private set => this.RaiseAndSetIfChanged(ref show, value);
        }
        public void Back()
        {
            MW.Login();
        }
        public void Theme()
        {
            MW.ChangeTheme();
        }
        public void Streets()
        {
            for (int i = 0; i < ListOfArea.Count; i++)
            {
                for (int j = 0; j < ListOfArea[i].Count; j++)
                {
                    var textBlock = new TextBlock { Text = ListOfArea[i][j] };
                }
            }
        }
        public void Registration()
        {
            using var db = new СlinicReceptionContext();
            TextSurname = null; TextName = null; TextPatronymic = null; TextPhone = null; TextDateOfBirthday = null; TextAdress = null; TextLogin = null; TextPassword = null;
            if (Surname == null) TextSurname = "Введите фамилию";
            else if (!Surname.All(Char.IsLetter)) TextSurname = "Фамилия содержит недопустимые символы";
            if (Name == null) TextName = "Введите имя";
            else if (!Name.All(Char.IsLetter)) TextName = "Имя содержит недопустимые символы";
            if (Patronymic == null) TextPatronymic = "Введите отчество";
            else if (!Patronymic.All(Char.IsLetter)) TextPatronymic = "Отчество содержит недопустимые символы";
            if (Phone == null) TextPhone = "Введите телефон";
            if (DateOfBirthday == null) TextDateOfBirthday = "Выберете дату рождения";
            if (Adress == null) TextAdress = "Введите адрес";
            if (Login == null) TextLogin = "Введите логин";
            else if (db.Log_In.Any(x => x.Login == Login)) TextLogin = "Такой логин уже существует";
            if (Password == null) TextPassword = "Введите пароль";
            if (TextSurname == null && TextName == null && TextPatronymic == null && TextPhone == null && TextDateOfBirthday == null && TextAdress == null && TextLogin == null && TextPassword == null)
            {
                //db.Add(new Patient(db.Пациент.Last().Номер_карты + 1, Surname, Name, Patronymic, (long)Phone, DateOfBirthday, 3, Adress));
                db.Add(new DataLogin(Login, Password, "Пациент", db.Пациент.Last().Номер_карты + 1));
                db.SaveChanges();
                MW.Patient();
            }
            else Show = true;
        }
        public RegistrationViewModel(MainWindowViewModel mw)
        {
            //DateOfBirthday = "Введите дату рождения";
            TextDateOfBirthday = "Дата рождения";
            MW = mw; Show = false;
        }
    }
}
