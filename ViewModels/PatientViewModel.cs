using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using СlinicReception.Models;
using СlinicReception.Services;

namespace СlinicReception.ViewModels
{
    public class PatientViewModel : ViewModelBase
    {
        MainWindowViewModel mw;
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }

        int CardNumber { get; set; } //номер карты текущего пациента

        //Запись на приём 
        //словарь для сопоставления выбранного дня приёма с расписанием врача
        readonly Dictionary<string, string> DaysWeek = new() { { "Пн-Пт", "Monday, Tuesday, Wednesday, Thursday, Friday" }, { "Пн-Чт", "Monday, Tuesday, Wednesday, Thursday" }, { "Вт-Пт", "Tuesday, Wednesday, Thursday, Friday" }, { "Вт-Сб", "Tuesday, Wednesday, Thursday, Friday, Saturday" } };

        ObservableCollection<string> specialities = new(); //список специальностей
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

        string? icon; //смена иконки переключателя часов
        public string? Icon
        {
            get => icon;
            private set => this.RaiseAndSetIfChanged(ref icon, value);
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

        string? wrongSpecialities;
        public string? WrongSpecialities
        {
            get => wrongSpecialities;
            private set => this.RaiseAndSetIfChanged(ref wrongSpecialities, value);
        }

        string? wrongDate;
        public string? WrongDate
        {
            get => wrongDate;
            private set => this.RaiseAndSetIfChanged(ref wrongDate, value);
        }

        string? wrongTime;
        public string? WrongTime
        {
            get => wrongTime;
            private set => this.RaiseAndSetIfChanged(ref wrongTime, value);
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

        public void Appointment()
        {
            WrongSpecialities = null; WrongDate = null; WrongTime = null;
            if (SelectedSpeciality == null) WrongSpecialities = "выберете специальность";
            if (VisitDate == null) WrongDate = "выберете дату";
            else if (Convert.ToDateTime(VisitDate) <= DateTime.Today) WrongDate = "некорректная дата";
            if (Hours < 8 || Hours >= 20) WrongTime = "некорректное время";
            if (WrongSpecialities == null && WrongDate == null && WrongTime == null) CheckData();
        }

        public void CheckData()
        {
            ChooseVisible = false; VisibleCard = true;
            using var db = new СlinicReceptionContext();
            var doctor = db.Врач.First(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == CardNumber).Номер_участка && x.Специальность == SelectedSpeciality);
            var doctorTimetable = db.Расписание.First(x => x.Табельный_номер == doctor.Табельный_номер);
            var date = Convert.ToDateTime(VisitDate);
            date = date.Date + new TimeSpan(Hours, Minutes, 0);
            if (DaysWeek[doctorTimetable.Дни_приёма].Contains(date.DayOfWeek.ToString()) && CheckTime(doctorTimetable, Hours))
            {
                if (Minutes % 30 == 0 && !db.Приём.Where(x => x.Табельный_номер == doctor.Табельный_номер).Any(x => x.Дата_приёма == date))
                {
                    TextAppointmentVisible = true; TextAppointment = "Вы записаны на приём";
                    date = date.Date + new TimeSpan(Hours, Minutes, 0);
                    int visitNumber = db.Приём.OrderBy(x => x.Номер_визита).Last().Номер_визита + 1;
                    db.Приём.Add(new Visit(visitNumber, doctor.Табельный_номер, db.Пациент.First(x => x.Номер_карты == CardNumber).Номер_карты, date));
                    db.Жалобы.Add(new Complaints(visitNumber, null, null));
                    db.SaveChanges();
                    FillGrid();
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

        public static bool CheckTime(Timetable timetable, int hours)
        {
            bool result = false;
            var time = timetable.Часы_приёма.Split(",");
            foreach (var t in time)
            {
                var temp = t.Split("-");
                if (Convert.ToUInt32(temp[0][..2]) <= hours && Convert.ToUInt32(temp[1][..2]) > hours) result = true;
            }
            return result;
        }

        public void Accept()
        {
            TextAppointment = "Вы записаны на приём"; ChooseVisible = false; OnButton = true;
            using var db = new СlinicReceptionContext();
            var date = Convert.ToDateTime(VisitDate);
            date = date.Date + new TimeSpan(Hours, Minutes, 0);
            int visitNumber = db.Приём.OrderBy(x => x.Номер_визита).Last().Номер_визита + 1;
            db.Приём.Add(new Visit(visitNumber, db.Врач.First(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == CardNumber).Номер_участка && x.Специальность == SelectedSpeciality).Табельный_номер, db.Пациент.First(x => x.Номер_карты == CardNumber).Номер_карты, date));
            db.Жалобы.Add(new Complaints(visitNumber, null, null));
            db.SaveChanges();
            FillGrid();
        }

        public void ChangeData()
        {
            OnButton = true; ChooseVisible = false; TextAppointmentVisible = false;
        }

        public void Switch() //переключение часов
        {
            if (Icon == "ToggleSwitchOffOutline")
            {
                Icon = "ToggleSwitchOutline"; EnabledHours = false; VisibleHours = false;
            }
            else
            {
                Icon = "ToggleSwitchOffOutline"; EnabledHours = true; VisibleHours = true;
            }
        }

        //Карта пациента
        public ObservableCollection<DataVisit> dataVisits; //список визитов пациента
        public ObservableCollection<DataVisit> DataVisits
        {
            get => dataVisits;
            private set => this.RaiseAndSetIfChanged(ref dataVisits, value);
        }

        bool enabledUserData; //редактирование данных о пациенте
        public bool EnabledUserData
        {
            get => enabledUserData;
            set => this.RaiseAndSetIfChanged(ref enabledUserData, value);
        }

        string? textButtonChangeUserData;
        public string? TextButtonChangeUserData
        {
            get => textButtonChangeUserData;
            private set => this.RaiseAndSetIfChanged(ref textButtonChangeUserData, value);
        }

        string surname;
        public string Surname
        {
            get => surname;
            private set => this.RaiseAndSetIfChanged(ref surname, value);
        }

        string name;
        public string Name
        {
            get => name;
            private set => this.RaiseAndSetIfChanged(ref name, value);
        }

        string patronymic;
        public string Patronymic
        {
            get => patronymic;
            private set => this.RaiseAndSetIfChanged(ref patronymic, value);
        }

        long phone;
        public long Phone
        {
            get => phone;
            private set => this.RaiseAndSetIfChanged(ref phone, value);
        }

        string dateOfBirthday;
        public string DateOfBirthday
        {
            get => dateOfBirthday;
            private set => this.RaiseAndSetIfChanged(ref dateOfBirthday, value);
        }

        string adress;
        public string Adress
        {
            get => adress;
            private set => this.RaiseAndSetIfChanged(ref adress, value);
        }

        string login;
        public string Login
        {
            get => login;
            private set => this.RaiseAndSetIfChanged(ref login, value);
        }

        string password;
        public string Password
        {
            get => password;
            private set => this.RaiseAndSetIfChanged(ref password, value);
        }

        public void FillGrid()
        {
            DataVisits = new ObservableCollection<DataVisit>(GenerateMockDataVisitTable());
        }

        public void ButtonChangeUserData()
        {
            if (TextButtonChangeUserData == "Изменить данные")
            {
                TextButtonChangeUserData = "Сохранить данные"; EnabledUserData = true;
            }
            else
            {
                TextButtonChangeUserData = "Изменить данные"; EnabledUserData = false;
                using var db = new СlinicReceptionContext();
                var user = db.Пациент.First(y => y.Номер_карты == CardNumber); var userl = db.Log_In.First(x => x.ID == CardNumber && x.Role == "Пациент");
                user.Фамилия = Surname; user.Имя = Name; user.Отчество = Patronymic; user.Телефон = Phone; user.Дата_рождения = Convert.ToDateTime(DateOfBirthday); user.Адрес = Adress; userl.Login = Login; userl.Password = Password;
                db.SaveChanges();
            }
        }

        private IEnumerable<DataVisit> GenerateMockDataVisitTable()
        {
            var dataVisit = new List<DataVisit>();
            using var db = new СlinicReceptionContext();
            var visits = db.Приём.Where(x => x.Дата_приёма > DateTime.Now && x.Номер_карты == CardNumber).OrderBy(x => x.Дата_приёма).ToArray();
            foreach (var item in visits)
            {
                var doctor = db.Врач.First(x => x.Табельный_номер == item.Табельный_номер);
                string name = $"{doctor.Фамилия} {doctor.Имя[0]}.{doctor.Отчество[0]}.";
                dataVisit.Add(new DataVisit() { DateTime = item.Дата_приёма.ToString("dd/MM/yyyy HH:mm"), Speciality = doctor.Специальность, Name = name, Office = (int)db.Расписание.First(x => x.Табельный_номер == doctor.Табельный_номер).Номер_кабинета });
            }
            return dataVisit;
        }

        //Расписание врачей
        public ObservableCollection<TimetableDoctors> SearchResults { get; } = new(); //список расписаний
                                                                                      //
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
            var doctors = db.Врач.Where(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == CardNumber).Номер_участка).ToArray();
            foreach (var doctor in doctors)
            {
                string name = $"{doctor.Фамилия} {doctor.Имя[0]}.{doctor.Отчество[0]}.";
                if (db.Расписание.Any(x => x.Табельный_номер == doctor.Табельный_номер))
                {
                    var timetable = db.Расписание.First(x => x.Табельный_номер == doctor.Табельный_номер);
                    if (s != null && (timetable.Номер_кабинета.ToString().Contains(s) || name.ToLower().Contains(s.ToLower())))
                    {
                        SearchResults.Add(new TimetableDoctors() { Name = name, Speciality = doctor.Специальность, Office = (int)timetable.Номер_кабинета, Days = timetable.Дни_приёма, Time = timetable.Часы_приёма });
                        ShowTimetable = true;
                    }
                }
                if (s == "") ShowTimetable = false;
            }
        }

        public PatientViewModel(int cardNumber, MainWindowViewModel mw)
        {
            using var db = new СlinicReceptionContext();

            MW = mw; CardNumber = cardNumber; LabelSpeciality = "Специальность врача"; OnButton = true; LabelVisitDate = "Дата приёма"; Icon = "ToggleSwitchOffOutline"; EnabledHours = true; VisibleHours = true; Hours = 0; Minutes = 0; TextTime = "Время приёма";
            var listSpecialities = db.Врач.Where(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == cardNumber).Номер_участка).Select(x => x.Специальность).Distinct();
            foreach (var item in listSpecialities) Specialities.Add(item);

            var user = db.Пациент.First(y => y.Номер_карты == cardNumber); var userl = db.Log_In.First(x => x.ID == CardNumber && x.Role == "Пациент"); TextButtonChangeUserData = "Изменить данные";
            Surname = user.Фамилия; Name = user.Имя; Patronymic = user.Отчество; Phone = user.Телефон; DateOfBirthday = user.Дата_рождения.ToString("dd/MM/yyyy"); Adress = user.Адрес; Login = userl.Login; Password = userl.Password;
            FillGrid();

            this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch!);
        }

        public void Logout()
        {
            MW.Login();
        }
    }
}
