using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EntityModele
{
    public static class General
    {
        public static void Init()
        {
            AllCollaborateurs = null;
            Administratifs = null;
            Competences = null;
        }

        public static string Med;
        public const string FormatDate = "dd/MM/yyyy";
        public const string NewLine = "<br />";
        public const short TopArticles = 3;
        public const string RepertoireImgArticles = @"~/Images/ImagesArticles/";
        public const string RepertoiretmpImgArticles = @"~/Images/tmp/";
        public const int SmallViewNumber = 10;
        public const string RepertoirePhotos = @"~/Images/Photos/";
        public const string RepertoireCvs = @"~/Cvs/";
        public const string RepertoirePieces = @"~/Pieces/";
        public const string DebutM = " Matin";
        public const string DebutAM = " Après-Midi";
        public const string FinM = " Midi";
        public const string FinAM = " Soir";
        public const short DayValidationCram = 10;
        public const string MsgValidCram1 = "Vous êtes sur le point de valider votre Rapport d'activité : ";
        public const string MsgValidCram2 = "Je certifie l'exactitude des informations cités ci-dessus.";
        //public const string MsgValidCram2 = "Veuillez vous assurer que toutes les informations suivantes sont correctes avant de procéder à la validation.";
        public const short NbrMoisCram = 3;
        public const short NbrMoisGestionConge = -12;
        public const short NbrMoisGestionMission = 36;
        public static string SignatureEmail = Environment.NewLine + Environment.NewLine + "Cordialement," + Environment.NewLine + "La direction, Diracet.";

        public static ClientEntity HomeSociete { get; set; }

        public static CollaborateurEntity CurrentUser
        {
            get
            {
                return HttpContext.Current.Session["CurrentUser"] as CollaborateurEntity;
            }
            set
            {
                HttpContext.Current.Session["CurrentUser"] = value;
            }
        }

        public static List<CollaborateurEntity> AllCollaborateurs { get; set; }

        public static List<CollaborateurEntity> Administratifs { get; set; }

        public static List<CompetenceEntity> Competences { get; set; }

            public static bool IsNumber(this object value)
        {
            if (value is sbyte) return true;HttpContext.Current.Session["m"] = 5;
            if (value is byte) return true;
            if (value is short) return true;
            if (value is ushort) return true;
            if (value is int) return true;
            if (value is uint) return true;
            if (value is long) return true;
            if (value is ulong) return true;
            if (value is float) return true;
            if (value is double) return true;
            if (value is decimal) return true;
            return false;
        }

        public static string ConvertToString(List<Email> Emails)
        {
            if (Emails == null)
                return null;
            //Convertir en String et Séparer les emails par des ';'
            var sBuilder = new System.Text.StringBuilder();
            Emails.ForEach(p => sBuilder.Append(p.email).Append(";"));
            return sBuilder.ToString();
        }

        public static string Crypter(string p_input)
        {
            // Source : http://msdn2.microsoft.com/fr-fr/library/system.security.cryptography.md5(VS.80).aspx

            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(p_input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool verifyMd5Hash(string p_input, string p_hash)
        {
            // Source : http://msdn2.microsoft.com/fr-fr/library/system.security.cryptography.md5(VS.80).aspx

            // Hash the input.
            string hashOfInput = Crypter(p_input);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, p_hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static string GetmaDebut(object ma)
        {
            return ma.ToString() == "M" ? General.DebutM : General.DebutAM;
        }

        public static string GetmaFin(object ma)
        {
            return ma.ToString() == "M" ? General.FinM : General.FinAM;
        }

    }

    [Serializable]
    public class Email
    {
        public string email { get; set; }
    }

    public enum ModeArticle
    {
        SmallView = 1,
        FullView = 2,
        New = 3,
        Modify = 4
    }

    public enum SensOrderBy
    {
        ASC = 1,
        DESC = 2
    }

    public struct DateTimeSpan
    {
        private readonly int years;
        private readonly int months;
        private readonly int days;
        private readonly int hours;
        private readonly int minutes;
        private readonly int seconds;
        private readonly int milliseconds;

        public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            this.years = years;
            this.months = months;
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            this.milliseconds = milliseconds;
        }

        public int Years { get { return years; } }
        public int Months { get { return months; } }
        public int Days { get { return days; } }
        public int Hours { get { return hours; } }
        public int Minutes { get { return minutes; } }
        public int Seconds { get { return seconds; } }
        public int Milliseconds { get { return milliseconds; } }

        enum Phase { Years, Months, Days, Done }

        public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                var sub = date1;
                date1 = date2;
                date2 = sub;
            }

            DateTime current = date1;
            int years = 0;
            int months = 0;
            int days = 0;

            Phase phase = Phase.Months;
            DateTimeSpan span = new DateTimeSpan();

            while (phase != Phase.Done)
            {
                switch (phase)
                {
                    case Phase.Years:
                        if (current.AddYears(years + 1) > date2)
                        {
                            phase = Phase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }
                        break;
                    case Phase.Months:
                        if (current.AddMonths(months + 1) > date2)
                        {
                            phase = Phase.Days;
                            current = current.AddMonths(months);
                        }
                        else
                        {
                            months++;
                        }
                        break;
                    case Phase.Days:
                        if (current.AddDays(days + 1) > date2)
                        {
                            current = current.AddDays(days);
                            var timespan = date2 - current;
                            span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                            phase = Phase.Done;
                        }
                        else
                        {
                            days++;
                        }
                        break;
                }
            }

            return span;
        }
    }
}