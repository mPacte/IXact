using System;
namespace EntityModele
{
    [Serializable]
    public class ClientEntity : BaseEntity
    {
        public int NumeroClient { get; set; }

        public string NomSociete { get; set; }

        public string Secteur { get; set; }

        private String _Adresse;
        public string Adresse
        {
            get { return _Adresse + " " + CodePostal + " " + Ville; }
            set { _Adresse = value; }
        }

        public int CodePostal { get; set; }

        public string Ville { get; set; }

        public string NomResponsable { get; set; }

        public int nbrMissionsEnCours { get; set; }
        public int nbrConsultantsEnCours { get; set; }
        public int nbrCommerciauxEnCours { get; set; }
        public int nbrMissionsTot { get; set; }
        public int nbrConsultantsTot { get; set; }
        public int nbrCommerciauxTot { get; set; }
    }
}