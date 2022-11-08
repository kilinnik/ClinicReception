using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using СlinicReception.Models;
using СlinicReception.Services;

namespace СlinicReception.ViewModels
{
    public class DoctorViewModel: ViewModelBase
    {
        int ID { get; set; }
        MainWindowViewModel mw;
        public MainWindowViewModel MW
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }
        TextBlock? visitDate;
        public TextBlock? VisitDate
        {
            get => visitDate;
            set => this.RaiseAndSetIfChanged(ref visitDate, value);
        }
        ObservableCollection<TextBlock> listVisitDate = new ObservableCollection<TextBlock>();
        public ObservableCollection<TextBlock> ListVisitDate
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
        ObservableCollection<TextBlock> diagnosises = new ObservableCollection<TextBlock>();
        public ObservableCollection<TextBlock> Diagnosises
        {
            get => diagnosises;
            set => this.RaiseAndSetIfChanged(ref diagnosises, value);
        }
        TextBlock? diagnosis;
        public TextBlock? Diagnosis
        {
            get => diagnosis;
            set => this.RaiseAndSetIfChanged(ref diagnosis, value);
        }
        ObservableCollection<TextBlock> preparations = new ObservableCollection<TextBlock>();
        public ObservableCollection<TextBlock> Preparations
        {
            get => preparations;
            set => this.RaiseAndSetIfChanged(ref preparations, value);
        }
        TextBlock? preparation;
        public TextBlock? Preparation
        {
            get => preparation;
            set => this.RaiseAndSetIfChanged(ref preparation, value);
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
        public void Logout()
        {
            MW.Login();
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
                    var surveys = db.Обследование.Where(x => x.Номер_визита == visit.Номер_визита);
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
                        var tb = new TextBlock() { Text = diagnosis.Название };
                        Diagnosises.Add(tb);
                        Diagnosis = tb;
                        d = diagnosis;
                    }
                    else Diagnosises.Add(new TextBlock() { Text = diagnosis.Название });
                }
                var preparations = db.Препарат.OrderBy(y => y.Название_препарата).ToArray();
                foreach (var preparation in preparations)
                {
                    if (d != null && d.Код_препарата == preparation.Код_препарата)
                    {
                        var tb = new TextBlock() { Text = preparation.Название_препарата };
                        Preparations.Add(tb);
                        Preparation = tb;
                    }
                    else Preparations.Add(new TextBlock() { Text = preparation.Название_препарата });                 
                }
                ShowPatient = true;
            }
        }
        public void SavePatientData()
        {
            using var db = new СlinicReceptionContext();
            var dateTime = DateTime.Parse(VisitDate.Text);
            var visit = db.Приём.First(x => x.Дата_приёма == dateTime && x.Табельный_номер == ID);
            var complaint = db.Жалобы.First(x => x.Номер_визита == visit.Номер_визита);
            if (complaint.Жалобы != Complaints)
            {
                db.Жалобы.First(x => x.Номер_визита == visit.Номер_визита).Жалобы = Complaints;
                db.Жалобы.First(x => x.Номер_визита == visit.Номер_визита).Код_диагноза = db.Диагноз.First(x => x.Название == Diagnosis.Text).Код_диагноза;
                db.Диагноз.First(x => x.Название == Diagnosis.Text).Код_препарата = db.Препарат.First(x => x.Название_препарата == Preparation.Text).Код_препарата;
                db.SaveChanges();
            }
            
        }
        public DoctorViewModel(int id, MainWindowViewModel mw)
        {
            ID = id; MW = mw;
            using var db = new СlinicReceptionContext();
            var visitDates = db.Приём.Where(x => x.Табельный_номер == ID).OrderBy(z => z.Дата_приёма).Select(y => y.Дата_приёма);
            foreach (var date in visitDates)
            {
                ListVisitDate.Add(new TextBlock() { Text = date.ToString("dd/MM/yyyy HH:mm") });
            }
            this.WhenAnyValue(x => x.VisitDate.Text).Subscribe(FillInfo!);


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
        ObservableCollection<TextBlock> surveyList = new ObservableCollection<TextBlock>();
        public ObservableCollection<TextBlock> SurveyList
        {
            get => surveyList;
            set => this.RaiseAndSetIfChanged(ref surveyList, value);
        }
        TextBlock? curSurvey;
        public TextBlock? CurSurvey
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
                    SurveyList.Add(new TextBlock() { Text = survey.Название_обследования});
                }
                SurveyButtonText = "Сохранить";
            }
            else
            {
                using var db = new СlinicReceptionContext();
                var dateTime = DateTime.Parse(VisitDate.Text);
                db.Обследование.Add(new Survey(db.Обследование.OrderBy(x => x.Номер_обследования).Last().Номер_обследования + 1, CurSurvey.Text, db.Приём.First(x => x.Дата_приёма == dateTime && x.Табельный_номер == ID).Номер_визита));
                db.SaveChanges();
                ShowSurvey = false;
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
                using var db = new СlinicReceptionContext();
                var dateTime = DateTime.Parse(VisitDate.Text);
                db.Больничный_лист.Add(new SickLeave(db.Приём.First(x => x.Дата_приёма == dateTime && x.Табельный_номер == ID).Номер_визита, DateTime.Parse(SickLeaveDateOpen), DateTime.Parse(SickLeaveDateClose), "открыт"));
                db.SaveChanges();
                ShowSickLeave = false;
            }
        }
    }
}
