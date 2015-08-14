using System;
using System.Collections.Generic;
using System.Data;
using DAO;
using EntityModele;
using EntityModele.Criteres;
using DAO.Cache;
using System.Linq;

namespace BIZ
{
    public class BizArticle
    {
        private DaoArticle daoArticle;

        public List<ArticleEntity> GetArticles()
        {
            var Articles = CacheManager.Current.GetEntities<ArticleEntity>();
            if (Articles == null)
            {
                daoArticle = new DaoArticle();
                Articles = daoArticle.PsSelectEntities();
                CacheManager.Current.SetEntities<ArticleEntity>(Articles);
            }
            return Articles;
        }

        public List<ArticleEntity> GetArticles(short topArticles)
        {
            var articles = GetArticles().Where(a => a.Afficher == true).ToList();
            return topArticles <= articles.Count ? articles.GetRange(0, topArticles) : articles;
        }

        public ArticleEntity GetArticleById(int idArticle)
        {
            var Articles = GetArticles();
            var article = Articles.Find(art => art.IdArticle == idArticle);
            return article;
        }

        public string Nouveau(string Titre, string Image, string ImageSize, string Texte, DateTime datePublication, string Type,
            string Modele, int Auteur, int Createur)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@TitreArticle", Value = Titre},
                        new Filtre { Name = "@imageArticle", Value = Image},
                        new Filtre { Name = "@tailleImage", Value = ImageSize},
                        new Filtre { Name = "@texteArticle", Value = Texte},
                        new Filtre { Name = "@datePublication", Value = datePublication},
                        new Filtre { Name = "@typeArticle", Value = Type},
                        new Filtre { Name = "@modeleArticle", Value = Modele},
                        new Filtre { Name = "@auteur", Value = Auteur},
                        new Filtre { Name = "@createur", Value = Createur},
                });
            CacheManager.Current.ResetCache<ArticleEntity>();
            daoArticle = new DaoArticle();
            return daoArticle.PsInsert(Params);
        }

        public void Update(string Titre, string Image, string ImageSize, string Texte, DateTime? datePublication, string Type,
            string Modele, int Auteur, int Createur, int idArticle, bool? Afficher = null)
        {
            var Params = new List<Filtre>
                (new[] {
                        new Filtre { Name = "@IdArticle", Value = idArticle},
                        new Filtre { Name = "@TitreArticle", Value = Titre},
                        new Filtre { Name = "@imageArticle", Value = Image},
                        new Filtre { Name = "@tailleImage", Value = ImageSize},
                        new Filtre { Name = "@texteArticle", Value = Texte},
                        new Filtre { Name = "@datePublication", Value = datePublication},
                        new Filtre { Name = "@typeArticle", Value = Type},
                        new Filtre { Name = "@modeleArticle", Value = Modele},
                        new Filtre { Name = "@auteur", Value = Auteur},
                        new Filtre { Name = "@createur", Value = Createur},                        
                });

            if (Afficher != null) Params.Add(new Filtre { Name = "@Afficher", Value = Afficher });

            CacheManager.Current.ResetCache<ArticleEntity>();
            daoArticle = new DaoArticle();
            daoArticle.PsUpdate(Params);
        }

        public void Update(int idArticle, bool Afficher)
        {
            var Params = new List<Filtre>
                (new[] {
                        new Filtre { Name = "@IdArticle", Value = idArticle},
                        new Filtre { Name = "@Afficher", Value = Afficher }
                });

            CacheManager.Current.ResetCache<ArticleEntity>();
            daoArticle = new DaoArticle();
            daoArticle.PsUpdate(Params);
        }
        
        public void UpdateVues(int Vues, int idArticle)
        {
            var Params = new List<Filtre>
                (new[] {
                        new Filtre { Name = "@Vues", Value = Vues},
                        new Filtre { Name = "@IdArticle", Value = idArticle},
                });

            CacheManager.Current.ResetCache<ArticleEntity>();
            daoArticle = new DaoArticle();
            daoArticle.PsUpdate(Params);
        }
    }
}