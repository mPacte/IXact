using System;
using System.Collections.Generic;
using System.Reflection;
using EntityModele.Enums;

namespace EntityModele
{
    [Serializable]
    public class CollaborateurEntity : BaseEntity
    {
        public class Prop
        {
            public static readonly PropertyInfo NumeroCollaborateur = typeof(CollaborateurEntity).GetProperty("NumeroCollaborateur");
            public static readonly PropertyInfo Username = typeof(CollaborateurEntity).GetProperty("Username");
            public static readonly PropertyInfo Nom = typeof(CollaborateurEntity).GetProperty("Nom");
            public static readonly PropertyInfo Prenom = typeof(CollaborateurEntity).GetProperty("Prenom");
            public static readonly PropertyInfo NomComplet = typeof(CollaborateurEntity).GetProperty("NomComplet");
            public static readonly PropertyInfo Fonction = typeof(CollaborateurEntity).GetProperty("Fonction");
        }

        public int NumeroCollaborateur { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        private string _nomComplet;
        public string NomComplet { get { return _nomComplet ?? Nom + ' ' + Prenom; } set { _nomComplet = value; } }

        public string Username { get; set; }

        public string Fonction { get; set; }

        public List<ProfilEnum> Profil { get; set; }

        public int? Responsable { get; set; }

        public string Emails { get; set; }

        public List<int> ArticlesConsultes { get; set; }

        public string Photo { get; set; }

        public string CV { get; set; }

        public DateTime LastUpdateCV { get; set; }

        public string Adresse { get; set; }

        public string Telephone { get; set; }

        public string Password { get; set; }

        public string Niveau { get; set; }

        public string Diplome { get; set; }

        public string Etudes { get; set; }

        public string Statut { get; set; }

        public string Situation { get; set; }

        public string Contrat { get; set; }

        public string SocieteOrigine { get; set; }

        public DateTime? DateEntree { get; set; }

        public DateTime DateNaissance { get; set; }

        public string Nationalite { get; set; }

        public string Matricule { get; set; }

        public string LieuNaissance { get; set; }

        public List<DroitPageEntity> DroitPage { get; set; }

        public List<DroitFonctionnaliteEntity> DroitFonctionnalite { get; set; }

        public bool IsResponsable { get; set; }

        public DateTime DerniereConnexion { get; set; }

        public String ActiviteActuelle { get; set; }

        public String ClientActuel { get; set; }

        public String SecteurClient { get; set; }

        public DateTime? DateSortie { get; set; }

        public DateTime? CongeActuelDebut { get; set; }

        public DateTime? CongeActuelFin { get; set; }

        public string Competences { get; set; }

        public List<CompetenceEntity> CompetencesList { get; set; }

        public override string ToString()
        {
            return this.NomComplet;
        }
    }
}