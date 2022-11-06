using Avalonia.Controls;
using FluentAvalonia.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using СlinicReception.Services;

namespace СlinicReception.ViewModels
{
    public class PatientViewModel: ViewModelBase
    {
        Dictionary<string, string> DaysWeek = new Dictionary<string, string>() { { "Пн-Пт", "Monday, Tuesday, Wednesday, Thursday, Friday" }, { "Пн-Чт", "Monday, Tuesday, Wednesday, Thursday" }, { "Вт-Пт", "Tuesday, Wednesday, Thursday, Friday" }, { "Вт-Сб", "Tuesday, Wednesday, Thursday, Friday, Saturday" } };
        int ID { get; set; } 
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
        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }
        bool showCard;
        public bool ShowCard
        {
            get => showCard;
            set => this.RaiseAndSetIfChanged(ref showCard, value);
        }
        bool showUserData;
        public bool ShowUserData
        {
            get => showUserData;
            set => this.RaiseAndSetIfChanged(ref showUserData, value);
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
        string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        bool onButton;
        public bool OnButton
        {
            get => onButton;
            set => this.RaiseAndSetIfChanged(ref onButton, value);
        }
        string? textVisitDate;
        public string? TextVisitDate
        {
            get => textVisitDate;
            set => this.RaiseAndSetIfChanged(ref textVisitDate, value);
        }
        string? textTime;
        public string? TextTime
        {
            get => textTime;
            set => this.RaiseAndSetIfChanged(ref textTime, value);
        }
        string? textSpeciality;
        public string? TextSpeciality
        {
            get => textSpeciality;
            set => this.RaiseAndSetIfChanged(ref textSpeciality, value);
        }       
        string? visitDate;
        public string? VisitDate
        {
            get => visitDate;
            private set => this.RaiseAndSetIfChanged(ref visitDate, value);
        }
        string? icon;
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
        MainWindowViewModel mw;
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }
        public void Logout()
        {
            MW.Login();
        }
        public void Appointment()
        {
            WrongSpecialities = null; WrongDate = null; WrongTime = null;
            if (Speciality == null) WrongSpecialities = "выберете специальность";
            if (VisitDate == null) WrongDate = "выберете дату";
            else if (Convert.ToDateTime(VisitDate) <= DateTime.Today) WrongDate = "некорректная дата";       
            if (Hours < 8 || Hours >= 20) WrongTime = "некорректное время";
            if (WrongSpecialities == null && WrongDate == null && WrongTime == null) CheckData();
        }
        public void CheckData()
        {
            ChooseVisible = false; ShowCard = true;
            using var db = new СlinicReceptionContext();
            var doctor = db.Врач.First(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == ID).Номер_участка && x.Специальность == Speciality.Text);
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
                        db.Приём.Add(new Visit(visitNumber, doctor.Табельный_номер, db.Пациент.First(x => x.Номер_карты == ID).Номер_карты, date));
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
                if (Convert.ToUInt32(temp[0].Substring(0,2)) <= hours && Convert.ToUInt32(temp[1].Substring(0, 2)) > hours) result = true;
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
            db.Приём.Add(new Visit(visitNumber, db.Врач.First(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == ID).Номер_участка && x.Специальность == Speciality.Text).Табельный_номер, db.Пациент.First(x => x.Номер_карты == ID).Номер_карты, date));
            db.Жалобы.Add(new Complaints(visitNumber, null, null));
            db.SaveChanges();
        }
        public void ChangeData()
        {
            OnButton = true; ChooseVisible = false; TextAppointmentVisible = false;
        }
        public void Switch()
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
        public PatientViewModel(int id, MainWindowViewModel mw)
        {
            MW = mw; ID = id; TextSpeciality = "Специальность врача"; OnButton = true; TextVisitDate = "Дата приёма"; Icon = "ToggleSwitchOffOutline"; EnabledHours = true; VisibleHours = true; Hours = 0; Minutes = 0; TextTime = "Время приёма"; OnButton = true;
            using var db = new СlinicReceptionContext();
            var listSpecialities = db.Врач.Where(x => x.Номер_участка == db.Пациент.First(y => y.Номер_карты == id).Номер_участка).Select(x => x.Специальность).Distinct();
            foreach (var item in listSpecialities)
            {
                Specialities.Add(new TextBlock { Text = item });
            }

            var user = db.Пациент.First(y => y.Номер_карты == id); var userl = db.Log_In.First(x => x.ID == ID && x.Role == "Пациент"); TextChangeUserData = "Изменить данные";
            Surname = user.Фамилия; Name = user.Имя; Patronymic = user.Отчество; Phone = user.Телефон; DateOfBirthday = user.Дата_рождения.ToString().Substring(0, 10); Adress = user.Адрес; Login = userl.Login; Password = userl.Password; 
        }
        string? textChangeUserData;
        public string? TextChangeUserData
        {
            get => textChangeUserData;
            private set => this.RaiseAndSetIfChanged(ref textChangeUserData, value);
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
        public void ChangeUserData()
        {
            if (TextChangeUserData == "Изменить данные")
            {
                TextChangeUserData = "Сохранить данные";
                ShowUserData = true;
            }
            else
            {
                TextChangeUserData = "Изменить данные";
                ShowUserData = false;
                using var db = new СlinicReceptionContext();
                var user = db.Пациент.First(y => y.Номер_карты == ID); var userl = db.Log_In.First(x => x.ID == ID && x.Role == "Пациент"); 
                user.Фамилия = Surname; user.Имя = Name; user.Отчество = Patronymic; user.Телефон = Phone; user.Дата_рождения = Convert.ToDateTime(DateOfBirthday); user.Адрес = Adress; userl.Login = Login; userl.Password = Password;
                db.SaveChanges();
            }
        }
    }
}
