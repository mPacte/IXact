using System;
using System.Reflection;

namespace EntityModele
{
    [Serializable]
    public class CongeEntity : BaseEntity
    {
        public class Prop
        {
            public static readonly PropertyInfo NumeroConge = typeof(CongeEntity).GetProperty("NumeroConge");
            public static readonly PropertyInfo DateDemande = typeof(CongeEntity).GetProperty("DateDemande");
            public static readonly PropertyInfo TypeConge = typeof(CongeEntity).GetProperty("TypeConge");
            public static readonly PropertyInfo DateDebut = typeof(CongeEntity).GetProperty("DateDebut");
            public static readonly PropertyInfo DateFin = typeof(CongeEntity).GetProperty("DateFin");
            public static readonly PropertyInfo DebutMA = typeof(CongeEntity).GetProperty("DebutMA");
            public static readonly PropertyInfo FinMA = typeof(CongeEntity).GetProperty("FinMA");
            public static readonly PropertyInfo DateValidation = typeof(CongeEntity).GetProperty("DateValidation");
            public static readonly PropertyInfo DateEnregistrement = typeof(CongeEntity).GetProperty("DateEnregistrement");
            public static readonly PropertyInfo Statut = typeof(CongeEntity).GetProperty("Statut");
            public static readonly PropertyInfo Remarques = typeof(CongeEntity).GetProperty("Remarques");
            public static readonly PropertyInfo Demandeur = typeof(CongeEntity).GetProperty("Demandeur");
            public static readonly PropertyInfo Decideur = typeof(CongeEntity).GetProperty("Decideur");
            public static readonly PropertyInfo Enregistreur = typeof(CongeEntity).GetProperty("Enregistreur");
            public static readonly PropertyInfo MotifDecision = typeof(CongeEntity).GetProperty("MotifDecision");
        }

        public long NumeroDemande { get; set; }

        public DateTime DateDemande { get; set; }

        public string TypeConge { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public string DebutMA { get; set; }

        public string FinMA { get; set; }

        public DateTime DateValidation { get; set; }

        public DateTime DateEnregistrement { get; set; }

        public int Statut { get; set; }

        public string Remarques { get; set; }

        public int NumeroDemandeur { get; set; }

        public int Decideur { get; set; }

        public int Enregistreur { get; set; }

        public string MotifDecision { get; set; }

        public Double NbrJours { get; set; }

        public CollaborateurEntity Demandeur { get; set; }

        public CongeEntity Old { get; set; }
    }
}