using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using СlinicReception.Models;
using СlinicReception.Services;

namespace СlinicReception.ViewModels
{
    public class RegistrarViewModel: ViewModelBase
    {
        //Добавить пациента
        static List<List<string>> ListOfArea = new List<List<string>>() { new List<string> { "ул. им. 40-летия Победы", "ул. Островского", "Коллективная ул." }, new List<string> { "ул. Жлобы", "ул. МОПР", "Первомайская ул.", "ул. 1 Мая" }, new List<string> { "ул. Филатова", "Школьная ул." } };
        MainWindowViewModel mw;
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
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
        TextBlock? street;
        public TextBlock? Street
        {
            get => street;
            set => this.RaiseAndSetIfChanged(ref street, value);
        }
        int? house;
        public int? House
        {
            get => house;
            set => this.RaiseAndSetIfChanged(ref house, value);
        }
        int? flat;
        public int? Flat
        {
            get => flat;
            set => this.RaiseAndSetIfChanged(ref flat, value);
        }
        ObservableCollection<TextBlock> streets = new ObservableCollection<TextBlock>();
        public ObservableCollection<TextBlock> Streets
        {
            get => streets;
            set => this.RaiseAndSetIfChanged(ref streets, value);
        }
        int? index;
        public int? Index
        {
            get => index;
            set => this.RaiseAndSetIfChanged(ref index, value);
        }
        string? textSurname;
        public string? TextSurname
        {
            get => textSurname;
            set => this.RaiseAndSetIfChanged(ref textSurname, value);
        }
        string? textName;
        public string? TextName
        {
            get => textName;
            set => this.RaiseAndSetIfChanged(ref textName, value);
        }
        string? textPatronymic;
        public string? TextPatronymic
        {
            get => textPatronymic;
            set => this.RaiseAndSetIfChanged(ref textPatronymic, value);
        }
        string? textPhone;
        public string? TextPhone
        {
            get => textPhone;
            set => this.RaiseAndSetIfChanged(ref textPhone, value);
        }
        string? textDateOfBirthday;
        public string? TextDateOfBirthday
        {
            get => textDateOfBirthday;
            set => this.RaiseAndSetIfChanged(ref textDateOfBirthday, value);
        }
        string? textAdress;
        public string? TextAdress
        {
            get => textAdress;
            set => this.RaiseAndSetIfChanged(ref textAdress, value);
        }
        bool show;
        public bool Show
        {
            get => show;
            private set => this.RaiseAndSetIfChanged(ref show, value);
        }
        bool showSuccessAddPatient;
        public bool ShowSuccessAddPatient
        {
            get => showSuccessAddPatient;
            private set => this.RaiseAndSetIfChanged(ref showSuccessAddPatient, value);
        }
        public void AddPatient()
        {
            using var db = new СlinicReceptionContext();
            TextSurname = null; TextName = null; TextPatronymic = null; TextPhone = null; TextDateOfBirthday = null; TextAdress = null; ShowSuccessAddPatient = false;
            if (Surname == null) TextSurname = "Введите фамилию";
            else if (!Surname.All(Char.IsLetter)) TextSurname = "Фамилия содержит недопустимые символы";
            if (Name == null) TextName = "Введите имя";
            else if (!Name.All(Char.IsLetter)) TextName = "Имя содержит недопустимые символы";
            if (Patronymic == null) TextPatronymic = "Введите отчество";
            else if (!Patronymic.All(Char.IsLetter)) TextPatronymic = "Отчество содержит недопустимые символы";
            if (Phone == null) TextPhone = "Введите номер телефона";
            else if (Phone.ToString()?.Length != 10) TextPhone = "Неверный формат номера телефона";
            if (DateOfBirthday == null) TextDateOfBirthday = "Выберете дату рождения";
            else if (Convert.ToDateTime(DateOfBirthday) > DateTime.Today) TextDateOfBirthday = "Некорректная дата рождения";
            if (Street == null || House == null) TextAdress = "Введите адрес";
            if (TextSurname == null && TextName == null && TextPatronymic == null && TextPhone == null && TextDateOfBirthday == null && TextAdress == null)
            {              
                int area = Index < ListOfArea[0].Count ? 1 : Index < (ListOfArea[0].Count + ListOfArea[1].Count) ? 2 : 3;
                string adress;
                if (Flat == null) adress = $"{Street}, д. {House}";
                else adress = $"{Street}, д. {House}, кв. {Flat}";
                var number = db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1;
                db.Add(new Patient(number, Surname, Name, Patronymic, (long)Phone, Convert.ToDateTime(DateOfBirthday), area, adress));
                db.Add(new DataLogin(number.ToString(), number.ToString(), "Пациент", db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1));
                db.SaveChanges();
                ShowSuccessAddPatient = true;
            }
            else Show = true;
        }
        //Запись на приём
        Dictionary<string, string> DaysWeek = new Dictionary<string, string>() { { "Пн-Пт", "Monday, Tuesday, Wednesday, Thursday, Friday" }, { "Пн-Чт", "Monday, Tuesday, Wednesday, Thursday" }, { "Вт-Пт", "Tuesday, Wednesday, Thursday, Friday" }, { "Вт-Сб", "Tuesday, Wednesday, Thursday, Friday, Saturday" } };
        ObservableCollection<TextBlock> specialities = new ObservableCollection<TextBlock>();
        public ObservableCollection<TextBlock> Specialities
        {
            get => specialities;
            set => this.RaiseAndSetIfChanged(ref specialities, value);
        }
        TextBlock? speciality;
        public TextBlock? Speciality
        {
            get => speciality;
            set => this.RaiseAndSetIfChanged(ref speciality, value);
        }

        bool showCard;
        public bool ShowCard
        {
            get => showCard;
            set => this.RaiseAndSetIfChanged(ref showCard, value);
        }
        bool enabledHours;
        public bool EnabledHours
        {
            get => enabledHours;
            set => this.RaiseAndSetIfChanged(ref enabledHours, value);
        }
        bool visibleHours;
        public bool VisibleHours
        {
            get => visibleHours;
            set => this.RaiseAndSetIfChanged(ref visibleHours, value);
        }
        string? textVisitDate;
        public string? TextVisitDate
        {
            get => textVisitDate;
            set => this.RaiseAndSetIfChanged(ref textVisitDate, value);
        }
        string? visitDate;
        public string? VisitDate
        {
            get => visitDate;
            private set => this.RaiseAndSetIfChanged(ref visitDate, value);
        }
        int hours;
        public int Hours
        {
            get => hours;
            set => this.RaiseAndSetIfChanged(ref hours, value);
        }
        int minutes;
        public int Minutes
        {
            get => minutes;
            set => this.RaiseAndSetIfChanged(ref minutes, value);
        }
        string? textAppointment;
        public string? TextAppointment
        {
            get => textAppointment;
            private set => this.RaiseAndSetIfChanged(ref textAppointment, value);
        }
        bool textAppointmentVisible;
        public bool TextAppointmentVisible
        {
            get => textAppointmentVisible;
            set => this.RaiseAndSetIfChanged(ref textAppointmentVisible, value);
        }
        bool chooseVisible;
        public bool ChooseVisible
        {
            get => chooseVisible;
            set => this.RaiseAndSetIfChanged(ref chooseVisible, value);
        }
        bool enabledAddVisit;
        public bool EnabledAddVisit
        {
            get => enabledAddVisit;
            private set => this.RaiseAndSetIfChanged(ref enabledAddVisit, value);
        }
        string? textSpeciality;
        public string? TextSpeciality
        {
            get => textSpeciality;
            set => this.RaiseAndSetIfChanged(ref textSpeciality, value);
        }
        public ObservableCollection<Patient> FindPatientForAddVisit { get; } = new();
        private string? _searchPatientForAddVisit;
        public string? SearchPatientForAddVisit
        {
            get => _searchPatientForAddVisit;
            set => this.RaiseAndSetIfChanged(ref _searchPatientForAddVisit, value);
        }
        bool onButton;
        public bool OnButton
        {
            get => onButton;
            set => this.RaiseAndSetIfChanged(ref onButton, value);
        }
        string? textTime;
        public string? TextTime
        {
            get => textTime;
            set => this.RaiseAndSetIfChanged(ref textTime, value);
        }
        private void DoSearchPatient(string s)
        {
            FindPatientForAddVisit.Clear();
            using var db = new СlinicReceptionContext();
            var patients = db.Пациент.ToArray();
            foreach (var patient in patients)
            {
                string name = $"{patient.Фамилия} {patient.Имя} {patient.Отчество}";
                if (s != null && ((name.ToLower().Contains(s.ToLower())) || patient.Номер_карты.ToString() == s))
                {
                    FindPatientForAddVisit.Add(new Patient(patient.Номер_карты, patient.Фамилия, patient.Имя, patient.Отчество, patient.Телефон, patient.Дата_рождения, patient.Номер_участка, patient.Адрес));
                }
            }
            if (FindPatientForAddVisit.Count == 1)
            {
                Specialities.Clear();
                EnabledAddVisit = true;
                foreach (var patient in FindPatientForAddVisit)
                {
                    var listSpecialities = db.Врач.Where(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == patient.Номер_карты).Номер_участка).Select(x => x.Специальность).Distinct();
                    foreach (var item in listSpecialities)
                    {
                        Specialities.Add(new TextBlock { Text = item });
                    }
                }
            }
            else EnabledAddVisit = false;
        }
        public void Appointment()
        {
            CheckData();
        }
        public void CheckData()
        {
            ChooseVisible = false; ShowCard = true;
            int id = 0;
            foreach (var patinet in FindPatientForAddVisit)
            {
                id = patinet.Номер_карты;
            }
            using var db = new СlinicReceptionContext();
            var doctor = db.Врач.First(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == id).Номер_участка && x.Специальность == Speciality.Text);
            var doctorTimetable = db.Расписание.First(x => x.Табельный_номер == doctor.Табельный_номер);
            var date = Convert.ToDateTime(VisitDate);
            date = date.Date + new TimeSpan(Hours, Minutes, 0);
            if (DaysWeek[doctorTimetable.Дни_приёма].Contains(date.DayOfWeek.ToString()))
                if (CheckTime(doctorTimetable, Hours))
                    if (Minutes % 30 == 0 && !db.Приём.Where(x => x.Табельный_номер == doctor.Табельный_номер).Any(x => x.Дата_приёма == date))
                    {
                        TextAppointmentVisible = true; TextAppointment = "Вы записаны на приём";
                        date = date.Date + new TimeSpan(Hours, Minutes, 0);
                        int visitNumber = db.Приём.OrderBy(x => x.Номер_визита).Last().Номер_визита + 1;
                        db.Приём.Add(new Visit(visitNumber, doctor.Табельный_номер, db.Пациент.First(x => x.Номер_карты == id).Номер_карты, date));
                        db.Жалобы.Add(new Complaints(visitNumber, null, null));
                        db.SaveChanges();
                    }
                    else
                    {
                        int minutes1 = Minutes; int hours1 = Hours;
                        if (minutes1 > 15 && minutes1 < 45) minutes1 = 30;
                        else if (minutes1 <= 15) minutes1 = 0;
                        else { minutes1 = 0; hours1++; }
                        int minutes2 = minutes1; int hours2 = hours1;
                        for (int i = 0; ; i += 30)
                        {
                            minutes1 += i; minutes2 -= i;
                            if (CheckTime(doctorTimetable, hours1 + minutes1 / 60))
                            {
                                date = date.Date + new TimeSpan(hours1 + minutes1 / 60, minutes1 % 60, 0);
                                if (!db.Приём.Where(x => x.Табельный_номер == doctor.Табельный_номер).Any(x => x.Дата_приёма == date))
                                {
                                    TextAppointment = $"Время {Hours}:{Minutes} занято, ближайшее свободное {hours1 + minutes1 / 60}:{minutes1 % 60}. Записаться на это время?";
                                    Hours = hours1 + minutes1 / 60; Minutes = minutes1 % 60; ChooseVisible = true; TextAppointmentVisible = true; OnButton = false;
                                    break;
                                }
                            }
                            if (CheckTime(doctorTimetable, hours2 + (int)Math.Floor((double)minutes2 / 60)))
                            {
                                date = date.Date + new TimeSpan(hours2 + (int)Math.Floor((double)minutes2 / 60), Math.Abs(minutes2 % 60), 0);
                                if (!db.Приём.Where(x => x.Табельный_номер == doctor.Табельный_номер).Any(x => x.Дата_приёма == date))
                                {
                                    TextAppointment = $"Время {Hours}:{Minutes} занято, ближайшее свободное время {hours2 + (int)Math.Floor((double)minutes2 / 60)}:{Math.Abs(minutes2 % 60)}. Записаться на это время?";
                                    Hours = hours2 + (int)Math.Floor((double)minutes2 / 60); Minutes = Math.Abs(minutes2 % 60); ChooseVisible = true; TextAppointmentVisible = true; OnButton = false;
                                    break;
                                }
                            }
                            if (!CheckTime(doctorTimetable, hours1 + minutes1 / 60) && !CheckTime(doctorTimetable, hours2 + minutes2 / 60)) break;
                        }
                    }
                else
                {
                    TextAppointmentVisible = true; TextAppointment = "В это время врач не принимает";
                }

        }
        public bool CheckTime(Timetable timetable, int hours)
        {
            bool result = false;
            var time = timetable.Часы_приёма.Split(",");
            foreach (var t in time)
            {
                var temp = t.Split("-");
                if (Convert.ToUInt32(temp[0].Substring(0, 2)) <= hours && Convert.ToUInt32(temp[1].Substring(0, 2)) > hours) result = true;
            }
            return result;
        }
        public void Accept()
        {
            int id = 0;
            foreach (var patinet in FindPatientForAddVisit)
            {
                id = patinet.Номер_карты;
            }
            TextAppointment = "Вы записаны на приём"; ChooseVisible = false; OnButton = true;
            using var db = new СlinicReceptionContext();
            var date = Convert.ToDateTime(VisitDate);
            date = date.Date + new TimeSpan(Hours, Minutes, 0);
            int visitNumber = db.Приём.OrderBy(x => x.Номер_визита).Last().Номер_визита + 1;
            db.Приём.Add(new Visit(visitNumber, db.Врач.First(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == id).Номер_участка && x.Специальность == Speciality.Text).Табельный_номер, db.Пациент.First(x => x.Номер_карты == id).Номер_карты, date));
            db.Жалобы.Add(new Complaints(visitNumber, null, null));
            db.SaveChanges();
        }
        public void ChangeData()
        {
            OnButton = true; ChooseVisible = false; TextAppointmentVisible = false;
        }
        public void Switch()
        {
            EnabledHours = !EnabledHours; VisibleHours = !VisibleHours;       
        }
        //Печать справок
        private string? searchTextHelp;
        public string? SearchTextHelp
        {
            get => searchTextHelp;
            set => this.RaiseAndSetIfChanged(ref searchTextHelp, value);
        }
        bool showHelpButtons;
        public bool ShowHelpButtons
        {
            get => showHelpButtons;
            private set => this.RaiseAndSetIfChanged(ref showHelpButtons, value);
        }
        public ObservableCollection<Patient> SearchResultsHelp { get; } = new();
        private void DoSearchPatientHelp(string s)
        {
            SearchResultsHelp.Clear();
            using var db = new СlinicReceptionContext();
            var patients = db.Пациент.ToArray();
            foreach (var patient in patients)
            {
                string name = $"{patient.Фамилия} {patient.Имя} {patient.Отчество}";
                if (s != null && ((name.ToLower().Contains(s.ToLower())) || patient.Номер_карты.ToString() == s))
                {
                    SearchResultsHelp.Add(new Patient(patient.Номер_карты, patient.Фамилия, patient.Имя, patient.Отчество, patient.Телефон, patient.Дата_рождения, patient.Номер_участка, patient.Адрес));
                }
            }
            if (SearchResultsHelp.Count == 1)
            {
                ShowHelpButtons = true;
            }
            else ShowHelpButtons = false;
        }
        //Расписание врачей
        public ObservableCollection<TimetableDoctors> SearchResults { get; } = new();
        private bool showTimetable;
        public bool ShowTimetable
        {
            get => showTimetable;
            set => this.RaiseAndSetIfChanged(ref showTimetable, value);
        }
        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        private void DoSearch(string s)
        {
            SearchResults.Clear();
            using var db = new СlinicReceptionContext();
            var doctors = db.Врач.ToArray();
            foreach (var doctor in doctors)
            {
                string name = $"{doctor.Фамилия} {doctor.Имя[0]}.{doctor.Отчество[0]}.";
                var timetable = db.Расписание.First(x => x.Табельный_номер == doctor.Табельный_номер);
                if (s != null && (timetable.Номер_кабинета.ToString().Contains(s) || name.ToLower().Contains(s.ToLower())))
                {
                    SearchResults.Add(new TimetableDoctors() { Name = name, Speciality = doctor.Специальность, Office = (int)timetable.Номер_кабинета, Days = timetable.Дни_приёма, Time = timetable.Часы_приёма });
                    ShowTimetable = true;
                }
                if (s == "") ShowTimetable = false;
            }
        }
        public RegistrarViewModel(MainWindowViewModel mw)
        {
            MW = mw; TextDateOfBirthday = "Дата рождения"; TextAdress = "Улица";
            for (int i = 0; i < ListOfArea.Count; i++)
            {
                for (int j = 0; j < ListOfArea[i].Count; j++)
                {
                    Streets.Add(new TextBlock { Text = ListOfArea[i][j] });
                }
            }

            SearchPatientForAddVisit = ""; TextSpeciality = "Специальность врача"; OnButton = true; TextVisitDate = "Дата приёма"; EnabledHours = true; VisibleHours = true; Hours = 0; Minutes = 0; TextTime = "Время приёма";
            this.WhenAnyValue(x => x.SearchPatientForAddVisit).Subscribe(DoSearchPatient!);

            this.WhenAnyValue(x => x.SearchTextHelp).Subscribe(DoSearchPatientHelp!);

            this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch!);
        }
        public void Logout()
        {
            MW.Login();
        }
    }
}
