using System;

namespace EntityModele
{
    public class ArticleEntity : BaseEntity
    {
        public int IdArticle { get; set; }

        public string Titre { get; set; }

        public string Texte { get; set; }

        public string Image { get; set; }

        public string ImageSize { get; set; }

        public string Type { get; set; }

        public DateTime DatePublication { get; set; }

        public int NumeroAuteur { get; set; }

        public int NumeroCreateur { get; set; }

        public CollaborateurEntity Auteur { get; set; }

        public CollaborateurEntity Createur { get; set; }

        public int Vues { get; set; }

        public string Modele { get; set; }

        public bool Afficher { get; set; }
    }
}