using Avalonia;
using Avalonia.Controls;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Subjects;
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
        //string? adress;
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
        ObservableCollection<TextBlock>  streets = new ObservableCollection<TextBlock>();
        string? street;
        int? house;
        int? flat;
        int index;
        public int Index
        {
            get => index;
            set => this.RaiseAndSetIfChanged(ref index, value);
        }
        public string? Street
        {
            get => street;
            set => this.RaiseAndSetIfChanged(ref street, value);
        }
        public int? House
        {
            get => house;
            set => this.RaiseAndSetIfChanged(ref house, value);
        }
        public int? Flat
        {
            get => flat;
            set => this.RaiseAndSetIfChanged(ref flat, value);
        }
        public ObservableCollection<TextBlock> Streets
        { 
            get =>  streets;
            set => this.RaiseAndSetIfChanged(ref streets, value);
        }
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
        //public string? Adress
        //{
        //    get => adress;
        //    private set => this.RaiseAndSetIfChanged(ref adress, value);
        //}
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
            if (Phone == null) TextPhone = "Введите номер телефона";
            else if (Phone.ToString()?.Length != 10) TextPhone = "Неверный формат номера телефона";
            if (DateOfBirthday == null) TextDateOfBirthday = "Выберете дату рождения";
            else if (Convert.ToDateTime(DateOfBirthday) > DateTime.Today) TextDateOfBirthday = "Неверная дата рождения";
            if (Street == null || House == null) TextAdress = "Введите адрес";
            if (Login == null) TextLogin = "Введите логин";
            else if (db.Log_In.Any(x => x.Login == Login)) TextLogin = "Такой логин уже существует";
            if (Password == null) TextPassword = "Введите пароль";
            if (TextSurname == null && TextName == null && TextPatronymic == null && TextPhone == null && TextDateOfBirthday == null && TextAdress == null && TextLogin == null && TextPassword == null)
            {
                //List<string> months = new List<string>() { "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };
                int area = Index < ListOfArea[0].Count ? 1 : Index < (ListOfArea[0].Count + ListOfArea[1].Count) ? 2 : 3;
                string adress;
                if (Flat == null) adress = $"{Street}, д. {House}";
                else adress = $"{Street}, д. {House}, кв. {Flat}";
                db.Add(new Patient(db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1, Surname, Name, Patronymic, (long)Phone, Convert.ToDateTime(DateOfBirthday), area, adress));
                db.Add(new DataLogin(Login, Password, "Пациент", db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1));
                db.SaveChanges();
                MW.Patient();
            }
            else Show = true;
        }
        public RegistrationViewModel(MainWindowViewModel mw)
        {
            //DateOfBirthday = "Введите дату рождения";
            TextDateOfBirthday = "Дата рождения";
            MW = mw; Show = false; TextAdress = "Улица";
            for (int i = 0; i < ListOfArea.Count; i++)
            {
                for (int j = 0; j < ListOfArea[i].Count; j++)
                {
                    Streets.Add(new TextBlock { Text = ListOfArea[i][j] });
                }
            }
        }
    }
}
