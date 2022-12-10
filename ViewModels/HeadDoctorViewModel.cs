using ReactiveUI;
using System.Collections.ObjectModel;
using СlinicReception.Services;

namespace СlinicReception.ViewModels
{
    public class HeadDoctorViewModel : ViewModelBase
    {
        MainWindowViewModel mw;
        public ObservableCollection<Doctor> SearchResults { get; } = new();
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }
        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        private bool showTimetable;
        public bool ShowTimetable
        {
            get => showTimetable;
            set => this.RaiseAndSetIfChanged(ref showTimetable, value);
        }
        private bool showDeleteButton;
        public bool ShowDeleteButton
        {
            get => showDeleteButton;
            set => this.RaiseAndSetIfChanged(ref showDeleteButton, value);
        }
        public void Logout()
        {
            MW.Login();
        }
        public void DeleteDoctor()
        {
            using var db = new СlinicReceptionContext();
            foreach (var result in SearchResults)
            {
                if (db.Расписание.Any(x => x.Табельный_номер == result.Табельный_номер)) db.Remove(db.Расписание.FirstOrDefault(x => x.Табельный_номер == result.Табельный_номер));
                db.SaveChanges();
                db.Remove(db.Врач.First(x => x.Табельный_номер == result.Табельный_номер));
                if (db.Приём.Any(x => x.Табельный_номер == result.Табельный_номер)) db.Remove(db.Приём.Where(x => x.Табельный_номер == result.Табельный_номер));
                db.SaveChanges();
            }
            SearchResults.Clear();
        }
        private void DoSearch(string s)
        {
            SearchResults.Clear();
            using var db = new СlinicReceptionContext();
            var doctors = db.Врач.ToArray();
            foreach (var doctor in doctors)
            {
                string name = $"{doctor.Фамилия} {doctor.Имя} {doctor.Отчество}";
                if (s != null && (name.ToLower().Contains(s.ToLower())))
                {
                    SearchResults.Add(new Doctor(doctor.Табельный_номер, doctor.Фамилия, doctor.Имя, doctor.Отчество, doctor.Дата_приёма_на_работу, doctor.Стаж, doctor.Адрес, doctor.Специальность, doctor.Номер_участка, doctor.Телефон));
                    ShowTimetable = true;
                }
            }
            if (s == "") ShowTimetable = false;
            if (SearchResults.Count == 1) ShowDeleteButton = true;
            else ShowDeleteButton = false;
        }
        public HeadDoctorViewModel(MainWindowViewModel mw)
        {
            MW = mw;

            this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch!);
        }
        private bool successAddDoctor;
        public bool SuccessAddDoctor
        {
            get => successAddDoctor;
            set => this.RaiseAndSetIfChanged(ref successAddDoctor, value);
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
        string? hireDate;
        public string? HireDate
        {
            get => hireDate;
            private set => this.RaiseAndSetIfChanged(ref hireDate, value);
        }
        int? seniority;
        public int? Seniority
        {
            get => seniority;
            private set => this.RaiseAndSetIfChanged(ref seniority, value);
        }
        string? adress;
        public string? Adress
        {
            get => adress;
            private set => this.RaiseAndSetIfChanged(ref adress, value);
        }
        string? speciality;
        public string? Speciality
        {
            get => speciality;
            private set => this.RaiseAndSetIfChanged(ref speciality, value);
        }
        int? areaNumber;
        public int? AreaNumber
        {
            get => areaNumber;
            private set => this.RaiseAndSetIfChanged(ref areaNumber, value);
        }
        public void AddDoctorButton()
        {
            using var db = new СlinicReceptionContext();
            var number = db.Врач.OrderBy(x => x.Табельный_номер).Last().Табельный_номер + 1;
            db.Врач.Add(new Doctor(number, Surname, Name, Patronymic, DateTime.Parse(HireDate), (int)Seniority, Adress, Speciality, (int)AreaNumber, (long)Phone));
            db.SaveChanges();
            SuccessAddDoctor = true;
        }
    }
}
