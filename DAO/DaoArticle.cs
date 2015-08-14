using System;
using System.Data;
using EntityModele;
using mPacte.Orm.DAL;
using System.Collections.Generic;
using EntityModele.Criteres;

namespace DAO
{
    public class DaoArticle : DaoBase<ArticleEntity>
    {
        public override List<ArticleEntity> PsSelectEntities()
        {
            var articles = new List<ArticleEntity>();
            var dt = PsSelect();

            //var articles = new List<ArticleEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                var article = new ArticleEntity();
                article.IdArticle = Convert.ToInt16(dr["IdArticle"]);
                article.Titre = dr["TitreArticle"].ToString();
                article.Texte = dr["TexteArticle"].ToString();
                if (!String.IsNullOrEmpty(dr["ImageArticle"].ToString()))
                    article.Image = General.RepertoireImgArticles + article.IdArticle +
                        dr["ImageArticle"].ToString();
                article.ImageSize = dr["TailleImage"].ToString();
                article.Auteur = new CollaborateurEntity();
                article.Auteur.NomComplet = dr["NomAuteur"].ToString();
                article.Auteur.NumeroCollaborateur = Convert.ToInt16(dr["Auteur"]);
                article.Auteur.Username = dr["Username"].ToString();
                article.Auteur.Fonction = dr["Fonction"].ToString();
                article.Createur = new CollaborateurEntity();
                article.Createur.NomComplet = dr["NomCreateur"].ToString();
                article.DatePublication = Convert.ToDateTime(dr["DatePublication"]);
                article.Vues = dr["Vues"] != DBNull.Value ? Convert.ToInt16(dr["Vues"]) : 0;
                article.Afficher = dr["Afficher"] != DBNull.Value ? Convert.ToBoolean(dr["Afficher"]) : false;
                articles.Add(article);
            }
            return articles;
        }
        //public DataTable GetArticles()
        //{
        //    var Articles = new DataTable();

        //    var requete = "SELECT Article.*, Aut.Nom + ' ' + Aut.Prenom as NomAuteur, Cre.Nom + ' ' + Cre.Prenom as NomCreateur, " +
        //                          "Aut.NumeroCollaborateur, Aut.Username, Aut.Profil, Aut.Fonction " +
        //                  "FROM (Article " +
        //                  "INNER JOIN Collaborateur as Aut ON Article.Auteur = Aut.NumeroCollaborateur) " +
        //                  "INNER JOIN Collaborateur as Cre ON Article.Createur = Cre.NumeroCollaborateur " +
        //                  "ORDER BY DatePublication DESC";

        //    var ds = OrmDal.Instance.DAL.GetDataSet(requete);
        //    if (ds != null)
        //    {
        //        Articles = ds.Tables[0];
        //    }
        //    return Articles;
        //}

        //public DataTable GetArticles(string topArticles)
        //{
        //    var Articles = new DataTable();

        //    var requete = "SELECT top" + topArticles + "Article.*, Aut.Nom + ' ' + Aut.Prenom as NomAuteur, Cre.Nom + ' ' + Cre.Prenom as NomCreateur, " +
        //                          "Aut.NumeroCollaborateur, Aut.Username, Aut.Profil, Aut.Fonction " +
        //                  "FROM (Article " +
        //                  "INNER JOIN Collaborateur as Aut ON Article.Auteur = Aut.NumeroCollaborateur) " +
        //                  "INNER JOIN Collaborateur as Cre ON Article.Createur = Cre.NumeroCollaborateur " +
        //                  "ORDER BY DatePublication DESC";

        //    var ds = OrmDal.Instance.DAL.GetDataSet(requete);
        //    if (ds != null)
        //    {
        //        Articles = ds.Tables[0];
        //    }
        //    return Articles;
        //}

        //public string Insert(string Titre, string Image, string ImageSize, string Texte, DateTime datePublication, string Type, string Modele,
        //    int Auteur, int Createur)
        //{
        //    Titre = Titre.Replace("'", "''");
        //    Texte = Texte.Replace("'", "''");
        //    var Identity = "Yes";
        //    var requete = "INSERT INTO Article (TitreArticle, ImageArticle, TailleImage, TexteArticle, DatePublication, Typearticle, ModeleArticle, Auteur, Createur) " +
        //                  "VALUES ('" + Titre + "','" + Image + "','" + ImageSize + "','" + Texte + "','" + datePublication + "','" +
        //                                Type + "','" + Modele + "','" + Auteur + "','" + Createur + "')";
        //    OrmDal.Instance.DAL.ExecuteNonQuery(requete, ref Identity);
        //    return Identity; ;
        //}

        //public DataTable GetArticleById(int idArticle)
        //{
        //    var Articles = new DataTable();

        //    var requete = "SELECT Article.*, Aut.Nom + ' ' + Aut.Prenom as NomAuteur, Cre.Nom + ' ' + Cre.Prenom as NomCreateur, " +
        //                    "Aut.NumeroCollaborateur, Aut.Username, Aut.Profil, Aut.Fonction " +
        //                  "FROM (Article " +
        //                  "INNER JOIN Collaborateur as Aut ON Article.Auteur = Aut.NumeroCollaborateur) " +
        //                  "INNER JOIN Collaborateur as Cre ON Article.Createur = Cre.NumeroCollaborateur " +
        //                  "WHERE idArticle = " + idArticle +
        //                  " ORDER BY DatePublication DESC";

        //    var ds = OrmDal.Instance.DAL.GetDataSet(requete);
        //    if (ds != null)
        //    {
        //        Articles = ds.Tables[0];
        //    }
        //    return Articles;
        //}

        //public void Update(int Vues, int idArticle)
        //{
        //    var requete = "UPDATE Article " +
        //        "SET Vues = " + Vues +
        //        " WHERE IdArticle = " + idArticle;
        //    OrmDal.Instance.DAL.ExecuteNonQuery(requete);
        //}
    }
}