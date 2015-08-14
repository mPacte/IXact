using System;

namespace EntityModele
{
    [Serializable]
    public class ActiviteEntity : BaseEntity
    {
        public Int64 NumeroActivite { get; set; }

        public String TypeActivite { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime? DateFinPrevue { get; set; }

        public DateTime? DateFinEffective { get; set; }

        public String PosteOccupe { get; set; }

        public String Email { get; set; }

        public String Telephone { get; set; }

        public Double PrixHT { get; set; }

        public String NomSociete { get; set; }

        public Int32 NumeroClient { get; set; }

        public String NomConsultant { get; set; }

        public Int32 NumeroConsultant { get; set; }

        public String Duree { get; set; }

        public String NomCommercial { get; set; }

        public Int32 NumeroCommercial { get; set; }

        public String Projet { get; set; }

        public String ResponsableClient { get; set; }

        public String TelephoneClient { get; set; }

        public String EmailClient { get; set; }
    }
}