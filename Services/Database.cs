using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace СlinicReception.Services
{
    public class СlinicReceptionContext : DbContext
    {
        public DbSet<DataLogin> Log_In { get; set; }
        public DbSet<Patient> Пациент { get; set; }
        public DbSet<Area> Участок { get; set; }
        public DbSet<Preparation> Препарат { get; set; }
        public DbSet<Diagnosis> Диагноз { get; set; }
        public DbSet<Complaints> Жалобы { get; set; }
        public DbSet<Visit> Приём { get; set; }
        public DbSet<SickLeave> Больничный_лист { get; set; }
        public DbSet<Timetable> Расписание { get; set; }
        public DbSet<Survey> Обследование { get; set; }
        public DbSet<Doctor> Врач { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=HOME-PC;Initial Catalog=""Регистратура поликлиники"";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
    public class DataLogin
    {
        [Key]
        public string Login{ get; set; }
        public string Password{ get; set; }
        public string Role { get; set; }
        public int? ID { get; set; }
        public DataLogin(string login, string password, string role, int? iD)
        {
            Login = login;
            Password = password;
            Role = role;
            ID = iD;
        }
    }
    public class Patient
    {
        [Key]
        public int Номер_карты { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public long Телефон { get; set; }
        public DateTime Дата_рождения { get; set; }
        public int Номер_участка { get; set; }
        public string Адрес { get; set; }
        public Patient(int номер_карты, string фамилия, string имя, string отчество, long телефон, DateTime дата_рождения, int номер_участка, string адрес)
        {
            Номер_карты = номер_карты;
            Фамилия = фамилия;
            Имя = имя;
            Отчество = отчество;
            Телефон = телефон;
            Дата_рождения = дата_рождения;
            Номер_участка = номер_участка;
            Адрес = адрес;
        }
    }

    public class Area
    {
        [Key]
        public int Номер_участка { get; set; }
        public string Адрес_участка { get; set; }
        public Area(int номер_участка, string адрес_участка)
        {
            Номер_участка = номер_участка;
            Адрес_участка = адрес_участка;
        }
    }

    public class Preparation
    {
        [Key]
        public int Код_препарата { get; set; }
        public string Название_препарата { get; set; }
        public Preparation(int код_препарата, string название_препарата)
        {
            Код_препарата = код_препарата;
            Название_препарата = название_препарата;
        }
    }

    public class Diagnosis
    {
        [Key]
        public int Код_диагноза { get; set; }
        public string Название { get; set; }
        public int? Код_препарата { get; set; }
        public Diagnosis(int код_диагноза, string название, int? код_препарата)
        {
            Код_диагноза = код_диагноза;
            Название = название;
            Код_препарата = код_препарата;
        }
    }

    public class Complaints
    {
        [Key]
        public int Номер_визита { get; set; }
        public string? Жалобы { get; set; }
        public int? Код_диагноза { get; set; }
        public Complaints(int номер_визита, string? жалобы, int? код_диагноза)
        {
            Номер_визита = номер_визита;
            Жалобы = жалобы;
            Код_диагноза = код_диагноза;
        }
    }

    public class Visit
    {
        [Key]
        public int Номер_визита { get; set; }
        public int Табельный_номер { get; set; }
        public int Номер_карты { get; set; }
        public DateTime Дата_приёма { get; set; }
        public Visit(int номер_визита, int табельный_номер, int номер_карты, DateTime дата_приёма)
        {
            Номер_визита = номер_визита;
            Табельный_номер = табельный_номер;
            Номер_карты = номер_карты;
            Дата_приёма = дата_приёма;
        }
    }

    public class SickLeave
    {
        [Key]
        public int Номер_визита { get; set; }
        public DateTime Открыт { get; set; }
        public DateTime Закрыт { get; set; }
        public string Статус { get; set; }
        public SickLeave(int номер_визита, DateTime открыт, DateTime закрыт, string статус)
        {
            Номер_визита = номер_визита;
            Открыт = открыт;
            Закрыт = закрыт;
            Статус = статус;
        }
    }

    public class Timetable
    {
        [Key]
        public int Табельный_номер { get; set; }
        public string? Дни_приёма { get; set; }
        public string? Часы_приёма { get; set; }
        public int? Номер_кабинета { get; set; }
        public Timetable(int табельный_номер, string? дни_приёма, string? часы_приёма, int? номер_кабинета)
        {
            Табельный_номер = табельный_номер;
            Дни_приёма = дни_приёма;
            Часы_приёма = часы_приёма;
            Номер_кабинета = номер_кабинета;
        }
    }

    public class Survey
    {
        [Key]
        public int Номер_обследования { get; set; }
        public string Название_обследования { get; set; }
        public int Номер_визита { get; set; }
        public Survey(int номер_обследования, string название_обследования, int номер_визита)
        {
            Номер_обследования = номер_обследования;
            Название_обследования = название_обследования;
            Номер_визита = номер_визита;
        }
    }

    public class Doctor
    {
        [Key]
        public int Табельный_номер { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public DateTime Дата_приёма_на_работу { get; set; }
        public int Стаж { get; set; }
        public string Адрес { get; set; }
        public string Специальность { get; set; }
        public int Номер_участка { get; set; }
        public long Телефон { get; set; }
        public Doctor(int табельный_номер, string фамилия, string имя, string отчество, DateTime дата_приёма_на_работу, int стаж, string адрес, string специальность, int номер_участка, long телефон)
        {
            Табельный_номер = табельный_номер;
            Фамилия = фамилия;
            Имя = имя;
            Отчество = отчество;
            Дата_приёма_на_работу = дата_приёма_на_работу;
            Стаж = стаж;
            Адрес = адрес;
            Специальность = специальность;
            Номер_участка = номер_участка;
            Телефон = телефон;
        }
    }
}
