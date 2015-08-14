using EntityModele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntranetSSIIV2.Controllers
{
    public class CommunController : Controller
    {
        // GET: Commun
        public ActionResult Index()
        {
            return View();
        }

        // saad Nom utilisateur visible sur toutes les pages
        public ActionResult GetUserData()
        {
            CollaborateurEntity CurrentUser;
            CurrentUser = Session["CurrentUser"] as CollaborateurEntity;
            string Nom = CurrentUser == null ? "" : "Bonjour " + CurrentUser.Prenom;
            string dernconn = "";

            if (CurrentUser != null)
            {
                if (!CurrentUser.DerniereConnexion.Equals(DateTime.MinValue))
                    dernconn = "Dernière connexion " + CurrentUser.DerniereConnexion.ToShortDateString() +
                        " à " + CurrentUser.DerniereConnexion.ToShortTimeString();
                else
                    dernconn = "Première connexion.";
            }
            ViewBag.Nom = Nom;
            ViewBag.News = dernconn;

            return PartialView("_Nom");
        }
        // fin

        // saad menu selon les droits de l'utilisateur

        public ActionResult GetMenu()
        {
            CollaborateurEntity CurrentUser = Session["CurrentUser"] as CollaborateurEntity;
            var droits = new List<DroitPageEntity>();
            if (CurrentUser != null)
            {
                droits = CurrentUser.DroitPage;
                for (int i = 0; i < droits.Count; i++)
                {
                    if (droits[i].UrlPage.EndsWith(".aspx"))
                    {
                        string anc = droits[i].UrlPage;
                        string str = anc.Remove(anc.Length - 5);
                        droits[i].UrlPage = str;
                    }
                }
            }
            ViewBag.droits = droits;

            return PartialView("_DroitsMenu");

            //list.ForEach(elem =>
            //{
            //    var menuElement = new HyperLink();
            //    menuElement.ID = "hyp" + elem.NomPage;
            //    menuElement.NavigateUrl = elem.UrlPage;
            //    menuElement.Text = elem.CaptionPage;
            //    menuElement.Height = 25;
            //    menuElement.Width = 100;
            //    if (elem.NumeroPage != 103)
            //    {
            //        divMenu.Controls.Add(menuElement);
            //    }
            //    else
            //        if (elem.NumeroProfil == 101) //Page "mon profil" n'est dans le menu que pour les consultants
            //            divMenu.Controls.Add(menuElement);
            //    //divMenu.Controls.Cast<Control>().OfType<HyperLink>()
            //    //      .FirstOrDefault(cont => cont.ID == "hyp" + elem.NomPage).Visible = true
            //});


        }
        // fin

    }
}

