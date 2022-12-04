using Avalonia.Controls;
using CsvHelper;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using СlinicReception.Models;
using СlinicReception.Services;
using System.IO;
using NGS.Templater;

namespace СlinicReception.ViewModels
{
    public class RegistrarViewModel: ViewModelBase
    {
        MainWindowViewModel mw;
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }
        //Добавить пациента
        //список улиц по участкам
        static List<List<string>> ListOfArea = new List<List<string>>() { new List<string> { "ул. им. 40-летия Победы", "ул. Островского", "Коллективная ул." }, new List<string> { "ул. Жлобы", "ул. МОПР", "Первомайская ул.", "ул. 1 Мая" }, new List<string> { "ул. Филатова", "Школьная ул." } };
       
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

        string? dateOfBirth;
        public string? DateOfBirth
        {
            get => dateOfBirth;
            private set => this.RaiseAndSetIfChanged(ref dateOfBirth, value);
        }

        ObservableCollection<string> streets = new ObservableCollection<string>(); //список улиц
        public ObservableCollection<string> Streets
        {
            get => streets;
            set => this.RaiseAndSetIfChanged(ref streets, value);
        }

        string? selectedStreet;
        public string? SelectedStreet
        {
            get => selectedStreet;
            set => this.RaiseAndSetIfChanged(ref selectedStreet, value);
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

        int? indexCurStreet;
        public int? IndexCurStreet
        {
            get => indexCurStreet;
            set => this.RaiseAndSetIfChanged(ref indexCurStreet, value);
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

        bool showFloatingWatermark;
        public bool ShowFloatingWatermark
        {
            get => showFloatingWatermark;
            private set => this.RaiseAndSetIfChanged(ref showFloatingWatermark, value);
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
            LabelSurname = null; LabelName = null; LabelPatronymic = null; LabelPhone = null; LabelDateOfBirth = null; LabelAdress = null; ShowSuccessAddPatient = false;
            if (Surname == null) LabelSurname = "Введите фамилию";
            else if (!Surname.All(Char.IsLetter)) LabelSurname = "Фамилия содержит недопустимые символы";
            if (Name == null) LabelName = "Введите имя";
            else if (!Name.All(Char.IsLetter)) LabelName = "Имя содержит недопустимые символы";
            if (Patronymic == null) LabelPatronymic = "Введите отчество";
            else if (!Patronymic.All(Char.IsLetter)) LabelPatronymic = "Отчество содержит недопустимые символы";
            if (Phone == null) LabelPhone = "Введите номер телефона";
            else if (Phone.ToString()?.Length != 10) LabelPhone = "Неверный формат номера телефона";
            if (DateOfBirth == null) LabelDateOfBirth = "Выберете дату рождения";
            else if (Convert.ToDateTime(DateOfBirth) > DateTime.Today) LabelDateOfBirth = "Некорректная дата рождения";
            if (SelectedStreet == null || House == null) LabelAdress = "Введите адрес";
            if (LabelSurname == null && LabelName == null && LabelPatronymic == null && LabelPhone == null && LabelDateOfBirth == null && LabelAdress == null)
            {              
                int area = IndexCurStreet < ListOfArea[0].Count ? 1 : IndexCurStreet < (ListOfArea[0].Count + ListOfArea[1].Count) ? 2 : 3;
                string adress;
                if (Flat == null) adress = $"{SelectedStreet}, д. {House}";
                else adress = $"{SelectedStreet}, д. {House}, кв. {Flat}";
                var number = db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1;
                db.Add(new Patient(number, Surname, Name, Patronymic, (long)Phone, Convert.ToDateTime(DateOfBirth), area, adress));
                db.Add(new DataLogin(number.ToString(), number.ToString(), "Пациент", db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1));
                db.SaveChanges();
                ShowSuccessAddPatient = true;
            }
            else ShowFloatingWatermark = true;
        }

        //Запись на приём
        //словарь для сопоставления выбранного дня приёма с расписанием врача
        Dictionary<string, string> DaysWeek = new Dictionary<string, string>() { { "Пн-Пт", "Monday, Tuesday, Wednesday, Thursday, Friday" }, { "Пн-Чт", "Monday, Tuesday, Wednesday, Thursday" }, { "Вт-Пт", "Tuesday, Wednesday, Thursday, Friday" }, { "Вт-Сб", "Tuesday, Wednesday, Thursday, Friday, Saturday" } };
        
        ObservableCollection<string> specialities = new ObservableCollection<string>(); //список специальностей
        public ObservableCollection<string> Specialities
        {
            get => specialities;
            set => this.RaiseAndSetIfChanged(ref specialities, value);
        }

        string? selectedSpeciality;
        public string? SelectedSpeciality
        {
            get => selectedSpeciality;
            set => this.RaiseAndSetIfChanged(ref selectedSpeciality, value);
        }

        bool visibleCard;
        public bool VisibleCard
        {
            get => visibleCard;
            set => this.RaiseAndSetIfChanged(ref visibleCard, value);
        }

        bool enabledHours; //для переключения часов
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

        bool onButton; //вкл/откл кнопки записи
        public bool OnButton
        {
            get => onButton;
            set => this.RaiseAndSetIfChanged(ref onButton, value);
        }

        string? labelVisitDate;
        public string? LabelVisitDate
        {
            get => labelVisitDate;
            set => this.RaiseAndSetIfChanged(ref labelVisitDate, value);
        }

        string? textTime;
        public string? TextTime
        {
            get => textTime;
            set => this.RaiseAndSetIfChanged(ref textTime, value);
        }

        string? labelSpeciality;
        public string? LabelSpeciality
        {
            get => labelSpeciality;
            set => this.RaiseAndSetIfChanged(ref labelSpeciality, value);
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

        public ObservableCollection<Patient> FindPatientForAddVisit { get; } = new(); //для поиска пациента

        private string? _searchPatientForAddVisit;
        public string? SearchPatientForAddVisit
        {
            get => _searchPatientForAddVisit;
            set => this.RaiseAndSetIfChanged(ref _searchPatientForAddVisit, value);
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
                        Specialities.Add(item);
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
            ChooseVisible = false; VisibleCard = true;
            int id = 0;
            foreach (var patinet in FindPatientForAddVisit) id = patinet.Номер_карты;
            using var db = new СlinicReceptionContext();
            var doctor = db.Врач.First(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == id).Номер_участка && x.Специальность == SelectedSpeciality);
            var doctorTimetable = db.Расписание.First(x => x.Табельный_номер == doctor.Табельный_номер);
            var date = Convert.ToDateTime(VisitDate);
            date = date.Date + new TimeSpan(Hours, Minutes, 0);
            if (DaysWeek[doctorTimetable.Дни_приёма].Contains(date.DayOfWeek.ToString()) && CheckTime(doctorTimetable, Hours))
            {
                if (Minutes % 30 == 0 && !db.Приём.Where(x => x.Табельный_номер == doctor.Табельный_номер).Any(x => x.Дата_приёма == date))
                {
                    TextAppointmentVisible = true; TextAppointment = "Пациент записан на приём";
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
            foreach (var patinet in FindPatientForAddVisit) id = patinet.Номер_карты;
            TextAppointment = "Пациент записан на приём"; ChooseVisible = false; OnButton = true;
            using var db = new СlinicReceptionContext();
            var date = Convert.ToDateTime(VisitDate);
            date = date.Date + new TimeSpan(Hours, Minutes, 0);
            int visitNumber = db.Приём.OrderBy(x => x.Номер_визита).Last().Номер_визита + 1;
            db.Приём.Add(new Visit(visitNumber, db.Врач.First(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == id).Номер_участка && x.Специальность == SelectedSpeciality).Табельный_номер, db.Пациент.First(x => x.Номер_карты == id).Номер_карты, date));
            db.Жалобы.Add(new Complaints(visitNumber, null, null));
            db.SaveChanges();
        }

        public void ChangeData()
        {
            OnButton = true; ChooseVisible = false; TextAppointmentVisible = false;
        }

        public void Switch() //переключение часов
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

        bool visibleListVisits;
        public bool VisibleListVisits
        {
            get => visibleListVisits;
            private set => this.RaiseAndSetIfChanged(ref visibleListVisits, value);
        }

        private string? selectedVisit;
        public string? SelectedVisit
        {
            get => selectedVisit;
            set => this.RaiseAndSetIfChanged(ref selectedVisit, value);
        }

        ObservableCollection<string> listVisitDate = new ObservableCollection<string>(); //список приёмов
        public ObservableCollection<string> ListVisitDate
        {
            get => listVisitDate;
            set => this.RaiseAndSetIfChanged(ref listVisitDate, value);
        }

        public ObservableCollection<Patient> SearchResultsHelp { get; } = new(); //для поиска пациента

        private void DoSearchPatientHelp(string s)
        {
            SearchResultsHelp.Clear();
            ListVisitDate.Clear();
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
                ShowHelpButtons = true; VisibleListVisits = true;
                var listVisits = db.Приём.Where(x => x.Номер_карты == SearchResultsHelp.First().Номер_карты).ToArray();
                foreach (var visit in listVisits) ListVisitDate.Add(visit.Дата_приёма.ToString("dd/MM/yyyy HH:mm"));
            }
            else { ShowHelpButtons = false; VisibleListVisits = false; }
        }

        public void PrintVisitDoctor()
        {
            using var db = new СlinicReceptionContext();
            string name = $"{SearchResultsHelp.First().Фамилия} {SearchResultsHelp.First().Имя} {SearchResultsHelp.First().Отчество}";
            using (var doc = Configuration.Factory.Open("Справка_о_посещении_врача.docx"))
            {
                doc.Process(new { Name = name, Age = SearchResultsHelp.First().Дата_рождения.ToString("dd/MM/yyyy"), DateTime = SelectedVisit, Date = SelectedVisit.Substring(0, SelectedVisit.Length - 5) });
            }
        }

        public void PrintSickLeave()
        {
            using var db = new СlinicReceptionContext();
            string namePatient = $"{SearchResultsHelp.First().Фамилия} {SearchResultsHelp.First().Имя} {SearchResultsHelp.First().Отчество}";
            var dateTime = DateTime.Parse(SelectedVisit);
            var visit = db.Приём.First(y => y.Номер_карты == SearchResultsHelp.First().Номер_карты && y.Дата_приёма == dateTime);
            var sicklLeave = db.Больничный_лист.First(x => x.Номер_визита == visit.Номер_визита);
            var doctor = db.Врач.First(x => x.Табельный_номер == visit.Табельный_номер);
            var nameDoctor = $"{doctor.Фамилия} {doctor.Имя} {doctor.Отчество}";
            using (var doc = Configuration.Factory.Open("Больничный_лист.docx"))
            {
                doc.Process(new { NamePatient = namePatient, Date = DateTime.Now.ToString("dd/MM/yyyy"), NameDoctor = nameDoctor, Period = $"{sicklLeave.Открыт.ToString("dd/MM/yyyy")}-{sicklLeave.Закрыт.ToString("dd/MM/yyyy")}", Status = sicklLeave.Статус });
            }
        }

        public void PrintListVisits()
        {
            var dt = new DataTable();
            dt.Columns.Add("Дата приёма");
            dt.Columns.Add("Специальность");
            dt.Columns.Add("ФИО врача");
            dt.Columns.Add("Кабинет");
            using var db = new СlinicReceptionContext();
            var visits = db.Приём.Where(x => x.Номер_карты == SearchResultsHelp.First().Номер_карты).OrderBy(x => x.Дата_приёма).ToArray();
            foreach (var visit in visits)
            {
                var doctor = db.Врач.First(x => x.Табельный_номер == visit.Табельный_номер);
                var timetable = db.Расписание.First(x => x.Табельный_номер == visit.Табельный_номер);
                string name = $"{doctor.Фамилия} {doctor.Имя[0]}.{doctor.Отчество[0]}.";
                dt.Rows.Add(visit.Дата_приёма, doctor.Специальность, name, timetable.Номер_кабинета);
            }
            using (var doc = Configuration.Factory.Open("Список_приёмов.xlsx"))
            {
                doc.Process(new { Table1 = dt });
            }
        }

        //Расписание врачей
        public ObservableCollection<TimetableDoctors> SearchResults { get; } = new();

        //private bool showTimetable;
        //public bool ShowTimetable
        //{
        //    get => showTimetable;
        //    set => this.RaiseAndSetIfChanged(ref showTimetable, value);
        //}

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
                }
            }
        }

        public void PrintTimetable()
        {
            var dt = new DataTable();
            dt.Columns.Add("ФИО врача");
            dt.Columns.Add("Специальность");
            dt.Columns.Add("Кабинет");
            dt.Columns.Add("Дни приёмов");
            dt.Columns.Add("Часы приёмов");
            foreach (var timetable in SearchResults) dt.Rows.Add(timetable.Name, timetable.Speciality, timetable.Office, timetable.Days, timetable.Time);
            using (var doc = Configuration.Factory.Open("Расписание.xlsx"))
            {
                doc.Process(new { Table1 = dt });
            }
        }

        public RegistrarViewModel(MainWindowViewModel mw)
        {
            MW = mw; LabelDateOfBirth = "Дата рождения"; LabelAdress = "Улица";
            for (int i = 0; i < ListOfArea.Count; i++)
            {
                for (int j = 0; j < ListOfArea[i].Count; j++)
                {
                    Streets.Add(ListOfArea[i][j]);
                }
            }

            SearchPatientForAddVisit = ""; LabelSpeciality = "Специальность врача"; OnButton = true; LabelVisitDate = "Дата приёма"; EnabledHours = true; VisibleHours = true; Hours = 0; Minutes = 0; TextTime = "Время приёма";
            this.WhenAnyValue(x => x.SearchPatientForAddVisit).Subscribe(DoSearchPatient!);

            this.WhenAnyValue(x => x.SearchTextHelp).Subscribe(DoSearchPatientHelp!);

            this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch!);

            SearchText = "";
        }

        public void Logout()
        {
            MW.Login();
        }
    }
}
