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
using Material.Dialog;
using Material.Dialog.Enums;

namespace СlinicReception.ViewModels
{
    public class RegistrationViewModel: ViewModelBase
    {
        //список улиц по участкам
        static List<List<string>> ListOfArea = new List<List<string>>() { new List<string> {"ул. им. 40-летия Победы", "ул. Островского", "Коллективная ул." }, new List<string> {"ул. Жлобы", "ул. МОПР", "Первомайская ул.", "ул. 1 Мая" }, new List<string> {"ул. Филатова", "Школьная ул." } };
       
        int? indexCurStreet; //индекс текущей улицы
        public int? IndexCurStreet 
        {
            get => indexCurStreet;
            set => this.RaiseAndSetIfChanged(ref indexCurStreet, value);
        }

        ObservableCollection<TextBlock> streets = new ObservableCollection<TextBlock>(); //список улиц
        public ObservableCollection<TextBlock> Streets
        {
            get => streets;
            set => this.RaiseAndSetIfChanged(ref streets, value);
        }

        TextBlock? selectedStreet; //выбраная улица
        public TextBlock? SelectedStreet
        {
            get => selectedStreet;
            set => this.RaiseAndSetIfChanged(ref selectedStreet, value);
        }

        int? house; //№ дома
        public int? House
        {
            get => house;
            set => this.RaiseAndSetIfChanged(ref house, value);
        }

        int? flat; //№ квартиры
        public int? Flat
        {
            get => flat;
            set => this.RaiseAndSetIfChanged(ref flat, value);
        }

        string? labelSurname;
        public string? LabelSurname
        {
            get => labelSurname;
            set => this.RaiseAndSetIfChanged(ref labelSurname, value);
        }
        string? labelName;
        public string? LabelName
        {
            get => labelName;
            set => this.RaiseAndSetIfChanged(ref labelName, value);
        }
        string? labelPatronymic;
        public string? LabelPatronymic
        {
            get => labelPatronymic;
            set => this.RaiseAndSetIfChanged(ref labelPatronymic, value);
        }
        string? labelPhone;
        public string? LabelPhone
        {
            get => labelPhone;
            set => this.RaiseAndSetIfChanged(ref labelPhone, value);
        }
        string? labelDateOfBirth;
        public string? LabelDateOfBirth
        {
            get => labelDateOfBirth;
            set => this.RaiseAndSetIfChanged(ref labelDateOfBirth, value);
        }
        string? labelAdress;
        public string? LabelAdress
        {
            get => labelAdress;
            set => this.RaiseAndSetIfChanged(ref labelAdress, value);
        }
        string? labelLogin;
        public string? LabelLogin
        {
            get => labelLogin; 
            set => this.RaiseAndSetIfChanged(ref labelLogin, value);
        }
        string? labelPassword;
        public string? LabelPassword
        {
            get => labelPassword;
            set => this.RaiseAndSetIfChanged(ref labelPassword, value);
        }
        string? surname;
        public string? Surname
        {
            get => surname;
            private set => this.RaiseAndSetIfChanged(ref surname, value);
        }
        string? name;
        public string? Name
        {
            get => name;
            private set => this.RaiseAndSetIfChanged(ref name, value);
        }
        string? patronymic;
        public string? Patronymic
        {
            get => patronymic;
            private set => this.RaiseAndSetIfChanged(ref patronymic, value);
        }
        long? phone;
        public long? Phone
        {
            get => phone;
            private set => this.RaiseAndSetIfChanged(ref phone, value);
        }
        string? dateOfBirthday;
        public string? DateOfBirthday
        {
            get => dateOfBirthday;
            private set => this.RaiseAndSetIfChanged(ref dateOfBirthday, value);
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
            LabelSurname = null; LabelName = null; LabelPatronymic = null; LabelPhone = null; LabelDateOfBirth = null; LabelAdress = null; LabelLogin = null; LabelPassword = null;
            if (Surname == null) LabelSurname = "Введите фамилию";
            else if (!Surname.All(Char.IsLetter)) LabelSurname = "Фамилия содержит недопустимые символы";
            if (Name == null) LabelName = "Введите имя";
            else if (!Name.All(Char.IsLetter)) LabelName = "Имя содержит недопустимые символы";
            if (Patronymic == null) LabelPatronymic = "Введите отчество";
            else if (!Patronymic.All(Char.IsLetter)) LabelPatronymic = "Отчество содержит недопустимые символы";
            if (Phone == null) LabelPhone = "Введите номер телефона";
            else if (Phone.ToString()?.Length != 10) LabelPhone = "Неверный формат номера телефона";
            if (DateOfBirthday == null) LabelDateOfBirth = "Выберете дату рождения";
            else if (Convert.ToDateTime(DateOfBirthday) > DateTime.Today) LabelDateOfBirth = "Некорректная дата рождения";
            if (SelectedStreet == null || House == null) LabelAdress = "Введите адрес";
            if (Login == null) LabelLogin = "Введите логин";
            else if (db.Log_In.Any(x => x.Login == Login)) LabelLogin = "Такой логин уже существует";
            if (Password == null) LabelPassword = "Введите пароль";
            if (LabelSurname == null && LabelName == null && LabelPatronymic == null && LabelPhone == null && LabelDateOfBirth == null && LabelAdress == null && LabelLogin == null && LabelPassword == null)
            {
                //List<string> months = new List<string>() { "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };
                int area = IndexCurStreet < ListOfArea[0].Count ? 1 : IndexCurStreet < (ListOfArea[0].Count + ListOfArea[1].Count) ? 2 : 3;
                string adress;
                if (Flat == null) adress = $"{SelectedStreet}, д. {House}";
                else adress = $"{SelectedStreet}, д. {House}, кв. {Flat}";
                db.Add(new Patient(db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1, Surname, Name, Patronymic, (long)Phone, Convert.ToDateTime(DateOfBirthday), area, adress));
                db.Add(new DataLogin(Login, Password, "Пациент", db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1));
                db.SaveChanges();
                MW.Patient(db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты);
            }
            else Show = true;
        }
        public RegistrationViewModel(MainWindowViewModel mw)
        {
            LabelDateOfBirth = "Дата рождения"; MW = mw; LabelAdress = "Улица";
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
