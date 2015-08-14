using System;

namespace EntityModele
{
    [Serializable]
    public class DroitFonctionnaliteEntity : BaseEntity
    {
        public int NumeroFonctionnalite { get; set; }

        public string LibelleFonctionnalite { get; set; }

        public int NumeroProfil { get; set; }

        public string LibelleProfil { get; set; }

        public bool Droit { get; set; }
    }
}