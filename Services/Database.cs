using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СlinicReception.Services
{
    public class Patient
    {
        public int CardNumber { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public long Phone { get; set; }
        public DateTime DateBirthday { get; set; }
        public int AreaNumber { get; set; }
        public string Adress { get; set; }
        public Patient(int cardNumber, string surname, string name, string patronymic, long phone, DateTime dateOfBirthday, int areaNumber, string adress)
        {
            CardNumber = cardNumber;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Phone = phone;
            DateBirthday = dateOfBirthday;
            AreaNumber = areaNumber;
            Adress = adress;
        }
    }

    public class Area
    {
        public int AreaNumber { get; set; }
        public string AreaAdress { get; set; }
        public Area(int areaNumber, string areaAdress)
        {
            AreaNumber = areaNumber;
            AreaAdress = areaAdress;
        }
    }

    public class Preparation
    {
        public int PreparationCode { get; set; }
        public string PreparationName { get; set; }
        public Preparation(int preparationCode, string preparationName)
        {
            PreparationCode = preparationCode;
            PreparationName = preparationName;
        }
    }

    public class Diagnosis
    {
        public int DiagnosisCode { get; set; }
        public string DiagnosisTitle { get; set; }
        public int PreparationCode { get; set; }
        public Diagnosis(int diagnosisCode, string diagnosisTitle, int preparationCode)
        {
            DiagnosisCode = diagnosisCode;
            DiagnosisTitle = diagnosisTitle;
            PreparationCode = preparationCode;
        }
    }

    public class Complaints
    {
        public int VisitNumber { get; set; }
        public string ComplaintsList { get; set; }
        public int DiagnosisCode { get; set; }
        public Complaints(int visitNumber, string complaintsList, int diagnosisCode)
        {
            VisitNumber = visitNumber;
            ComplaintsList = complaintsList;
            DiagnosisCode = diagnosisCode;
        }
    }

    public class Visit
    {
        public int VisitNumber { get; set; }
        public int StaffNumber { get; set; }
        public int CardNumber { get; set; }
        public DateTime VisitDate { get; set; }
        public Visit(int visitNumber, int staffNumber, int cardNumber, DateTime visitDate)
        {
            VisitNumber = visitNumber;
            StaffNumber = staffNumber;
            CardNumber = cardNumber;
            VisitDate = visitDate;
        }
    }

    public class SickLeave
    {
        public int VisitNumber { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        public string Status { get; set; }
        public SickLeave(int visitNumber, DateTime open, DateTime close, string status)
        {
            VisitNumber = visitNumber;
            Open = open;
            Close = close;
            Status = status;
        }
    }

    public class Timetable
    {
        public int StaffNumber { get; set; }
        public string VisitDays { get; set; }
        public string VisitHours { get; set; }
        public int OfficeNumber { get; set; }
        public Timetable(int staffNumber, string visitDays, string visitHours, int officeNumber)
        {
            StaffNumber = staffNumber;
            VisitDays = visitDays;
            VisitHours = visitHours;
            OfficeNumber = officeNumber;
        }
    }

    public class Survey
    {
        public int SurveyNumber { get; set; }
        public string SurveyTitle { get; set; }
        public int VisitNumber { get; set; }
        public Survey(int surveyNumber, string surveyTitle, int visitNumber)
        {
            SurveyNumber = surveyNumber;
            SurveyTitle = surveyTitle;
            VisitNumber = visitNumber;
        }
    }

    public class Doctor
    {
        public int StaffNumber { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime HireDate { get; set; }
        public int Experience { get; set; }
        public string Adress { get; set; }
        public string Speciality { get; set; }
        public int AreaNumber { get; set; }
        public long Phone { get; set; }
        public Doctor(int staffNumber, string surname, string name, string patronymic, DateTime hireDate, int experience, string adress, string speciality, int areaNumber, long phone)
        {
            StaffNumber = staffNumber;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            HireDate = hireDate;
            Experience = experience;
            Adress = adress;
            Speciality = speciality;
            AreaNumber = areaNumber;
            Phone = phone;
        }
    }

    internal class Database
    {
    }
}
