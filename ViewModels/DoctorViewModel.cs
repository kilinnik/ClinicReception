using NGS.Templater;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Data;
using СlinicReception.Services;

namespace СlinicReception.ViewModels
{
    public class DoctorViewModel: ViewModelBase
    {
        int ID { get; set; } //табельный номер врача

        MainWindowViewModel mw;
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }

        string? selectedVisitDate;
        public string? SelectedVisitDate
        {
            get => selectedVisitDate;
            set => this.RaiseAndSetIfChanged(ref selectedVisitDate, value);
        }

        ObservableCollection<string> listVisitDate = new ObservableCollection<string>(); //список приёмов
        public ObservableCollection<string> ListVisitDate
        {
            get => listVisitDate;
            set => this.RaiseAndSetIfChanged(ref listVisitDate, value);
        }

        private bool showPatient;
        public bool ShowPatient
        {
            get => showPatient;
            set => this.RaiseAndSetIfChanged(ref showPatient, value);
        }

        ObservableCollection<string> diagnosises = new ObservableCollection<string>();
        public ObservableCollection<string> Diagnosises
        {
            get => diagnosises;
            set => this.RaiseAndSetIfChanged(ref diagnosises, value);
        }

        string? selectedDiagnosis;
        public string? SelectedDiagnosis
        {
            get => selectedDiagnosis;
            set => this.RaiseAndSetIfChanged(ref selectedDiagnosis, value);
        }

        ObservableCollection<string> preparations = new ObservableCollection<string>();
        public ObservableCollection<string> Preparations
        {
            get => preparations;
            set => this.RaiseAndSetIfChanged(ref preparations, value);
        }

        string? selectedPreparation;
        public string? SelectedPreparation
        {
            get => selectedPreparation;
            set => this.RaiseAndSetIfChanged(ref selectedPreparation, value);
        }

        string? namePatient;
        public string? NamePatient
        {
            get => namePatient;
            set => this.RaiseAndSetIfChanged(ref namePatient, value);
        }

        string? survey;
        public string? Survey
        {
            get => survey;
            set => this.RaiseAndSetIfChanged(ref survey, value);
        }

        string? sickLeave;
        public string? SickLeave
        {
            get => sickLeave;
            set => this.RaiseAndSetIfChanged(ref sickLeave, value);
        }

        string? complaints;
        public string? Complaints
        {
            get => complaints;
            set => this.RaiseAndSetIfChanged(ref complaints, value);
        }

        public void FillInfo(string date)
        {
            if (date != null)
            {
                using var db = new СlinicReceptionContext();
                var dateTime = DateTime.Parse(date);
                var visit = db.Приём.First(x => x.Дата_приёма == dateTime && x.Табельный_номер == ID);
                var patient = db.Пациент.First(x => x.Номер_карты == visit.Номер_карты);
                NamePatient = $"{patient.Фамилия} {patient.Имя[0]}.{patient.Отчество[0]}.";
                var survey = "не назначено";
                SurveyButtonText = "Назначить обследование";
                if (db.Обследование.Any(x => x.Номер_визита == visit.Номер_визита))
                {
                    survey = "";
                    var surveys = db.Обследование.Where(x => x.Номер_визита == visit.Номер_визита).OrderBy(y => y.Название_обследования).ToArray();
                    foreach (var s in surveys)
                    {
                        survey += s.Название_обследования + ",";
                    }
                    survey = survey.Substring(0, survey.Length - 1);
                }
                Survey = survey;
                var sickLeave = "нет";
                VisibleSickLeaveButton = true;
                SickLeaveButtonText = "Создать больничный лист";
                if (db.Больничный_лист.Any(x => x.Номер_визита == visit.Номер_визита && x.Статус == "открыт"))
                {
                    sickLeave = "открыт";
                    VisibleSickLeaveButton = false;
                }
                else if (db.Больничный_лист.Any(x => x.Номер_визита == visit.Номер_визита && x.Статус == "закрыт"))
                {
                    sickLeave = "закрыт";
                    VisibleSickLeaveButton = false;
                }
                SickLeave = sickLeave;
                Complaints = db.Жалобы.First(x => x.Номер_визита == visit.Номер_визита).Жалобы;
                var diagnosises = db.Диагноз.OrderBy(y => y.Название).ToArray();
                Diagnosis? d = null;
                foreach (var diagnosis in diagnosises)
                {
                    if (db.Жалобы.First(x => x.Номер_визита == visit.Номер_визита).Код_диагноза == diagnosis.Код_диагноза)
                    {
                        var tb = diagnosis.Название;
                        Diagnosises.Add(tb);
                        SelectedDiagnosis = tb;
                        d = diagnosis;
                    }
                    else Diagnosises.Add(diagnosis.Название);
                }
                var preparations = db.Препарат.OrderBy(y => y.Название_препарата).ToArray();
                foreach (var preparation in preparations)
                {
                    if (d != null && d.Код_препарата == preparation.Код_препарата)
                    {
                        var tb = preparation.Название_препарата;
                        Preparations.Add(tb);
                        SelectedPreparation = tb;
                    }
                    else Preparations.Add(preparation.Название_препарата);                 
                }
                ShowPatient = true;
            }
        }

        public void SavePatientData()
        {
            using var db = new СlinicReceptionContext();
            var dateTime = DateTime.Parse(SelectedVisitDate);
            var visit = db.Приём.First(x => x.Дата_приёма == dateTime && x.Табельный_номер == ID);
            var complaint = db.Жалобы.First(x => x.Номер_визита == visit.Номер_визита);
            if (complaint.Жалобы != Complaints)
            {
                db.Жалобы.First(x => x.Номер_визита == visit.Номер_визита).Жалобы = Complaints;
                db.Жалобы.First(x => x.Номер_визита == visit.Номер_визита).Код_диагноза = db.Диагноз.First(x => x.Название == SelectedDiagnosis).Код_диагноза;
                db.Диагноз.First(x => x.Название == SelectedDiagnosis).Код_препарата = db.Препарат.First(x => x.Название_препарата == SelectedPreparation).Код_препарата;
                db.SaveChanges();
            }        
        }

        public void PrintPatinetInfo()
        {
            var dt = new DataTable();
            dt.Columns.Add("ФИО пациента");
            dt.Columns.Add("Обследование");
            dt.Columns.Add("Больничный лист");
            dt.Columns.Add("Жалобы");
            dt.Columns.Add("Диагноз");
            dt.Columns.Add("Препарат");
            dt.Rows.Add(NamePatient, Survey, SickLeave, Complaints, SelectedDiagnosis, SelectedPreparation);
            using (var doc = Configuration.Factory.Open("Информация_о_пациенте.xlsx"))
            {
                doc.Process(new { Table1 = dt });
            }
        }

        public DoctorViewModel(int id, MainWindowViewModel mw)
        {
            ID = id; MW = mw;
            using var db = new СlinicReceptionContext();
            var visitDates = db.Приём.Where(x => x.Табельный_номер == ID).OrderBy(z => z.Дата_приёма).Select(y => y.Дата_приёма);
            foreach (var date in visitDates) ListVisitDate.Add(date.ToString("dd/MM/yyyy HH:mm"));
            this.WhenAnyValue(x => x.SelectedVisitDate).Subscribe(FillInfo!);
        }

        public void Logout()
        {
            MW.Login();
        }

        private bool visibleSickLeaveButton;
        public bool VisibleSickLeaveButton
        {
            get => visibleSickLeaveButton;
            set => this.RaiseAndSetIfChanged(ref visibleSickLeaveButton, value);
        }

        private bool showSurvey;
        public bool ShowSurvey
        {
            get => showSurvey;
            set => this.RaiseAndSetIfChanged(ref showSurvey, value);
        }

        private bool showSickLeave;
        public bool ShowSickLeave
        {
            get => showSickLeave;
            set => this.RaiseAndSetIfChanged(ref showSickLeave, value);
        }

        string? surveyButtonText;
        public string? SurveyButtonText
        {
            get => surveyButtonText;
            set => this.RaiseAndSetIfChanged(ref surveyButtonText, value);
        }

        string? sickLeaveButtonText;
        public string? SickLeaveButtonText
        {
            get => sickLeaveButtonText;
            set => this.RaiseAndSetIfChanged(ref sickLeaveButtonText, value);
        }

        ObservableCollection<string> surveyList = new ObservableCollection<string>();
        public ObservableCollection<string> SurveyList
        {
            get => surveyList;
            set => this.RaiseAndSetIfChanged(ref surveyList, value);
        }

        string? curSurvey;
        public string? CurSurvey
        {
            get => curSurvey;
            set => this.RaiseAndSetIfChanged(ref curSurvey, value);
        }

        string? sickLeaveDateOpen;
        public string? SickLeaveDateOpen
        {
            get => sickLeaveDateOpen;
            set => this.RaiseAndSetIfChanged(ref sickLeaveDateOpen, value);
        }

        string? sickLeaveDateClose;
        public string? SickLeaveDateClose
        {
            get => sickLeaveDateClose;
            set => this.RaiseAndSetIfChanged(ref sickLeaveDateClose, value);
        }

        public void SurveyButton()
        {
            if (SurveyButtonText == "Назначить обследование")
            {
                ShowSurvey = true;
                using var db = new СlinicReceptionContext();
                var surveys = db.Обследование.Distinct().ToArray();
                foreach (var survey in surveys)
                {
                    SurveyList.Add(survey.Название_обследования);
                }
                SurveyButtonText = "Сохранить";
            }
            else
            {
                using var db = new СlinicReceptionContext();
                var dateTime = DateTime.Parse(SelectedVisitDate);
                db.Обследование.Add(new Survey(db.Обследование.OrderBy(x => x.Номер_обследования).Last().Номер_обследования + 1, CurSurvey, db.Приём.First(x => x.Дата_приёма == dateTime && x.Табельный_номер == ID).Номер_визита));
                db.SaveChanges();
                ShowSurvey = false;
                var survey = "не назначено";
                SurveyButtonText = "Назначить обследование";
                var visit = db.Приём.First(x => x.Дата_приёма == dateTime && x.Табельный_номер == ID);
                if (db.Обследование.Any(x => x.Номер_визита == visit.Номер_визита))
                {
                    survey = "";
                    var surveys = db.Обследование.Where(x => x.Номер_визита == visit.Номер_визита).OrderBy(y => y.Название_обследования).ToArray();
                    foreach (var s in surveys)
                    {
                        survey += s.Название_обследования + ",";
                    }
                    survey = survey.Substring(0, survey.Length - 1);
                }
                Survey = survey;
            }
        }

        public void SickLeaveButton()
        {
            if (SickLeaveButtonText == "Создать больничный лист")
            {
                ShowSickLeave = true;
                SickLeaveButtonText = "Сохранить";
            }
            else
            {
                SickLeaveButtonText = "Создать больничный лист";
                VisibleSickLeaveButton = false;
                using var db = new СlinicReceptionContext();
                var dateTime = DateTime.Parse(SelectedVisitDate);
                var visit = db.Приём.First(x => x.Дата_приёма == dateTime && x.Табельный_номер == ID);
                SickLeave = db.Больничный_лист.First(x => x.Номер_визита == visit.Номер_визита).Статус;
                db.Больничный_лист.Add(new SickLeave(db.Приём.First(x => x.Дата_приёма == dateTime && x.Табельный_номер == ID).Номер_визита, DateTime.Parse(SickLeaveDateOpen), DateTime.Parse(SickLeaveDateClose), "открыт"));
                db.SaveChanges();
                ShowSickLeave = false;
            }
        }
    }
}
