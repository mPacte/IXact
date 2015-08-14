using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityModele;
using DAO.Cache;

namespace BIZ
{
    public class ApplicationManager
    {
        public void Init()
        {
            instance = new ApplicationManager();
        }

        private static ApplicationManager instance;

        public static ApplicationManager Current
        {
            get
            {
                if (instance == null)
                    instance = new ApplicationManager();
                return instance;
            }
        }

        public String LoadApplication(CollaborateurEntity currentUser)
        {
            return CacheManager.Current.LoadApplication();
        }

        public String ReloadApplication()
        {
            //var currentUser = General.CurrentUser;
            var nbr = CacheManager.Current.RemoveAll().ToString();
            var res = "<br />" + nbr.ToString() + " éléments du Cache ont été supprimés. <br />";
            BizDroit.Instance.Init();
            res += "<br /> BizDroit initialisé.<br />";
            BizHoliday.Instance.Init();
            res += "<br /> BizHoliday initialisé.<br />";
            NotificationManager.CurrentSession.Init();
            res += "<br /> NotificationManager initialisé.<br />";
            CacheManager.Current.Init();
            res += "<br /> CacheManager initialisé.<br />";
            EntityModele.General.Init();
            //Session.Abandon();
            //res += "<br /> Abandon de session.<br />";
            //System.Web.Security.FormsAuthentication.SignOut();
            //res += "<br /> Déconnexion de l'application.<br />";
            res += CacheManager.Current.LoadApplication();
            return res;

        }
    }
}
