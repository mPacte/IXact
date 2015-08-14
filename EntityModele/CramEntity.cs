using System;
using System.Collections.Generic;
using System.Data;

namespace EntityModele
{
    [Serializable]
    public class CramEntity : BaseEntity
    {
        public Int64 Numero { get; set; }

        public DateTime Periode { get; set; }

        public Int32 Collaborateur { get; set; }

        public String NomCollaborateur { get; set; }

        public String Fichier { get; set; }

        public Int16 Etat { get; set; }

        public DateTime DateValidation { get; set; }

        public DateTime DateConfirmation { get; set; }

        public int Confirmateur { get; set; }
    }

    [Serializable]
    public class CramFicheEntity : BaseEntity
    {
        public CramEntity Cram { get; set; }

        public Single TotalJoursTravailles { get; set; }

        public Single TotalExtraTravailles { get; set; }

        public Single TotalConges { get; set; }

        public List<ExtraWorkEntity> HeuresSupplementaires { get; set; }

        public DataSet Donnees { get; set; }

        public String Client { get; set; }
    }
}