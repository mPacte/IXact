using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ;
using EntityModele;
using EntityModele.Criteres;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace IntranetSSIIV2.Controllers
{
    public class HomeController : Controller
    {
       [Authorize]
        public ActionResult Index()
        {
            BizArticle bizArticle;
            List<ArticleEntity> Articles;
            CollaborateurEntity CurrentUser;
            ILog logger = LogManager.GetLogger("Page");
            //Dictionary<String, int> statutsCollab, situationCollab, activiteConsultants;

            ViewBag.lblNews1 = "";
            ViewBag.lblUser = "";
            ViewBag.linkprofil = "";
            ViewBag.linkmissions = "";
            ViewBag.tableResponsableVisible = false;
            ViewBag.tabArticlesVisible = true;
            ViewBag.Articles = new List<ArticleEntity>();

            //Récupérer les informations utilisateur en cours
            CurrentUser = Session["CurrentUser"] as CollaborateurEntity;
            if (CurrentUser == null)
            {
                logger.Warn("CurrentUser null ; redirect to login page...");
                return View();
            }

            ViewBag.lblNews1 = "Bonjour " + CurrentUser.Prenom;
            ViewBag.lblUser  = "<b>" + "Profil : </b>" + CurrentUser.Profil.First() + "<br/><br/>";
            if (!CurrentUser.DerniereConnexion.Equals(DateTime.MinValue))
                ViewBag.lblUser += "<b>Dernière connexion </b><br/>" + CurrentUser.DerniereConnexion.ToShortDateString() +
                    " à " + CurrentUser.DerniereConnexion.ToShortTimeString();
            else
                ViewBag.lblUser += "<b>Première connexion.</b>";

            ViewBag.linkprofil = "Profil.aspx?col=" + CurrentUser.NumeroCollaborateur;

            if (CurrentUser.Profil.Contains(EntityModele.Enums.ProfilEnum.Consultant))
                ViewBag.linkmissions = "MesActivites.aspx";
            else
                ViewBag.linkmissions = "GestionActivites.aspx";

                //News

                bizArticle = new BizArticle();
                Articles = bizArticle.GetArticles(General.TopArticles);
                if (Articles == null || Articles.Count == 0)
                {
                    //Log error
                    return View();
                }

                //var index = 0;
                //Articles.ForEach(article =>
                //{
                //    var ucArticle = new UserControls_Article();
                //    ucArticle.ID = "uc" + index.ToString();
                //    ucArticle = LoadControl(@"UserControls/Article.ascx") as UserControls_Article;
                //    ucArticle.Mode = ModeArticle.SmallView;
                //    ucArticle.TheArticle = article;
                //    TabAddLine(tabArticles);
                //    tabArticles.Rows[index].Cells[0].Controls.Add(ucArticle);
                //    index++;
                //});

                ViewBag.Articles = Articles;

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}