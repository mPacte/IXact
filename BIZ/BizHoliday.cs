using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAO;
using DAO.Cache;
using EntityModele;
using EntityModele.Criteres;

namespace BIZ
{
    public class BizHoliday
    {
        private BizHoliday()
        {
            Holidays = CacheManager.Current.GetData<HolidayEntity>();
            if (Holidays == null)
            {
                Holidays = GetHolidays();
                CacheManager.Current.SetData<HolidayEntity>(Holidays);
            }
        }

        public void Init()
        {
            instance = new BizHoliday();
        }

        public DataTable Holidays { get; set; }

        private static BizHoliday instance;

        public static BizHoliday Instance
        {
            get
            {
                if (instance == null)
                    instance = new BizHoliday();
                return instance;
            }
        }

        private DataTable GetHolidays()
        {
            var Params = new List<Filtre>();
            //new[] {
            //        new PSQueryParams { Name = "@AnneeHoliday", Value = Annee},
            //});

            var daoHoliday = new DaoHoliday();
            return daoHoliday.PsSelect(Params);
        }

        public bool IsHoliday(DateTime date)
        {
            var holiday = false;
            //var dt = GetHolidays(date.Year);
            var results = from DataRow myRow in Holidays.Rows
                          where Convert.ToDateTime(myRow["DateHoliday"]) == date 
                            && (int)myRow["AnneeHoliday"] == date.Year
                          select myRow;
            if (results.Count() > 0)
            {
                holiday = true;
            }
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                holiday = true;
            }
            return holiday;
        }

        public int NbrJoursOuvresMois(int Mois, int Annee)
        {
            var nbrJours = DateTime.DaysInMonth(Annee, Mois);
            var dateDebut = new DateTime(Annee, Mois, 1);
            var dateFin = new DateTime(Annee, Mois, nbrJours);

            foreach (DateTime day in General.EachDay(dateDebut, dateFin))
            {
                if (IsHoliday(day))
                {
                    nbrJours--;
                }
            }

            return nbrJours;
        }

        public DateTime NextWorkDay(DateTime date)
        {
            var returnDate = date;
            if (IsHoliday(date))
                returnDate = NextWorkDay(date.AddDays(1));
            else
                returnDate = date;
            return returnDate;
        }
    }
}