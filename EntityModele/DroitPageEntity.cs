using System;

namespace EntityModele
{
    [Serializable]
    public class DroitPageEntity : BaseEntity
    {
        public int NumeroPage { get; set; }

        public string NomPage { get; set; }

        public string CaptionPage { get; set; }

        public string UrlPage { get; set; }

        public int NumeroProfil { get; set; }

        public string LibelleProfil { get; set; }

        public string ReadWritePage { get; set; }

        public int Ordre { get; set; }
    }
}