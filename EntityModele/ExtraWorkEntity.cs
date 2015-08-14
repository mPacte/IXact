using System;

namespace EntityModele
{
    [Serializable]
    public class ExtraWorkEntity : BaseEntity
    {
        public Int64 NumeroActivite { get; set; }

        public DateTime DateExtra { get; set; }

        public Single Hours { get; set; }

        public Single Day { get; set; }

        public String Remarques { get; set; }

        public String NomSociete { get; set; }

        public String TypeTravail { get; set; }
    }
}