using ReactiveUI;
using System.Collections.ObjectModel;
using СlinicReception.Services;

namespace СlinicReception.ViewModels
{
    public class DataAdminViewModel : ViewModelBase
    {
        MainWindowViewModel mw;
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }

        //Расписание

        public ObservableCollection<Timetable> timetables = new();
        public ObservableCollection<Timetable> Timetables
        {
            get => timetables;
            set => this.RaiseAndSetIfChanged(ref timetables, value);
        }

        ObservableCollection<int?> doctorIds = new();
        public ObservableCollection<int?> DoctorIds
        {
            get => doctorIds;
            set => this.RaiseAndSetIfChanged(ref doctorIds, value);
        }

        int selectedDoctorId;
        public int SelectedDoctorId
        {
            get => selectedDoctorId;
            set => this.RaiseAndSetIfChanged(ref selectedDoctorId, value);
        }

        int? doctorId;
        public int? DoctorId
        {
            get => doctorId;
            set => this.RaiseAndSetIfChanged(ref doctorId, value);
        }

        int? numberOffice;
        public int? NumberOffice
        {
            get => numberOffice;
            set => this.RaiseAndSetIfChanged(ref numberOffice, value);
        }

        string? days;
        public string? Days
        {
            get => days;
            set => this.RaiseAndSetIfChanged(ref days, value);
        }

        string? hours;
        public string? Hours
        {
            get => hours;
            set => this.RaiseAndSetIfChanged(ref hours, value);
        }

        public void UpdateTimetables()
        {
            using var db = new СlinicReceptionContext();
            foreach (var timetable in Timetables)
            {
                var dbTimetable = db.Расписание.First(x => x.Табельный_номер == timetable.Табельный_номер);
                dbTimetable.Дни_приёма = timetable.Дни_приёма; dbTimetable.Часы_приёма = timetable.Часы_приёма; dbTimetable.Номер_кабинета = timetable.Номер_кабинета;
                db.SaveChanges();
            }
        }

        public void DeleteTimetable()
        {
            using var db = new СlinicReceptionContext();
            db.Расписание.Remove(db.Расписание.First(x => x.Табельный_номер == DoctorId));
            Timetables.Remove(Timetables.First(x => x.Табельный_номер == DoctorId));
            db.SaveChanges();
            DoctorIds.Add(DoctorId);
        }

        public void AddTimetable()
        {
            using var db = new СlinicReceptionContext();
            db.Расписание.Add(new Timetable(SelectedDoctorId, Days, Hours, NumberOffice));
            Timetables.Add(new Timetable(SelectedDoctorId, Days, Hours, NumberOffice));
            db.SaveChanges();
            DoctorIds.Remove(DoctorIds.First(x => x == SelectedDoctorId));
        }

        //Больничные листы

        ObservableCollection<SickLeave> sickLeaves = new();
        public ObservableCollection<SickLeave> SickLeaves
        {
            get => sickLeaves;
            set => this.RaiseAndSetIfChanged(ref sickLeaves, value);
        }

        public void UpdateStatuses()
        {
            using var db = new СlinicReceptionContext();
            foreach (var sickLeave in SickLeaves)
            {
                if (sickLeave.Статус == "открыт" && sickLeave.Закрыт < DateTime.Now)
                {
                    sickLeave.Статус = "закрыт";
                    db.Больничный_лист.Update(sickLeave);                  
                }
            }
            db.SaveChanges();
            SickLeaves.Clear();
            var sickLeaves = db.Больничный_лист.ToArray();
            foreach (var sickLeave in sickLeaves) SickLeaves.Add(sickLeave);
        }

        //Приёмы

        ObservableCollection<Visit> visits = new();
        public ObservableCollection<Visit> Visits
        {
            get => visits;
            set => this.RaiseAndSetIfChanged(ref visits, value);
        }

        int? numberVisit;
        public int? NumberVisit
        {
            get => numberVisit;
            set => this.RaiseAndSetIfChanged(ref numberVisit, value);
        }

        public void DeleteVisit()
        {
            using var db = new СlinicReceptionContext();
            db.Приём.Remove(db.Приём.FirstOrDefault(x => x.Номер_визита == NumberVisit));
            db.SaveChanges();
            Visits.Remove(Visits.First(x => x.Номер_визита == NumberVisit));
        }

        //Пациенты

        ObservableCollection<Patient> patients = new();
        public ObservableCollection<Patient> Patients
        {
            get => patients;
            set => this.RaiseAndSetIfChanged(ref patients, value);
        }

        int? numberCard;
        public int? NumberCard
        {
            get => numberCard;
            set => this.RaiseAndSetIfChanged(ref numberCard, value);
        }

        public void DeletePatient()
        {
            using var db = new СlinicReceptionContext();
            db.Пациент.Remove(db.Пациент.FirstOrDefault(x => x.Номер_карты == NumberCard));
            db.SaveChanges();
            Patients.Remove(Patients.First(x => x.Номер_карты == NumberCard));
        }

        public DataAdminViewModel(MainWindowViewModel mw)
        {
            MW = mw; using var db = new СlinicReceptionContext();

            var timetables = db.Расписание.ToArray();
            foreach (var timetable in timetables) Timetables.Add(timetable);
            var doctors = db.Врач.Where(x => db.Расписание.All(y => x.Табельный_номер != y.Табельный_номер));
            foreach (var doctor in doctors) DoctorIds.Add(doctor.Табельный_номер);

            var sickLeaves = db.Больничный_лист.ToArray();
            foreach (var sickLeave in sickLeaves) SickLeaves.Add(sickLeave);

            var visits = db.Приём.ToArray();
            foreach (var visit in visits) Visits.Add(visit);

            var patients = db.Пациент.ToArray();
            foreach (var patient in patients) Patients.Add(patient);
        }

        public void Logout()
        {
            MW.Login();
        }
    }
}
