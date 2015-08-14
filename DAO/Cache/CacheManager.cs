using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using EntityModele;
using EntityModele.Criteres;
using System.Linq;

namespace DAO.Cache
{
    public class CacheManager
    {
        log4net.ILog logger = log4net.LogManager.GetLogger("CacheManager");

        public List<Type> CacheEntities { get; set; }

        private CacheManager()
        {
            CacheEntities = new List<Type>();
            CacheEntities.Add(typeof(HolidayEntity));
            CacheEntities.Add(typeof(DroitPageEntity));
            CacheEntities.Add(typeof(CollaborateurEntity));
            CacheEntities.Add(typeof(TypeCongeEntity));
            CacheEntities.Add(typeof(ArticleEntity));
            CacheEntities.Add(typeof(ClientEntity));
            CacheEntities.Add(typeof(TypeMissionEntity));
            CacheEntities.Add(typeof(CompetenceEntity));
        }

        public void Init()
        {
            instance = new CacheManager();
        }

        private static CacheManager instance;

        public static CacheManager Current
        {
            get
            {
                if (instance == null)
                    instance = new CacheManager();
                return instance;
            }
        }

        public DataTable GetData<T>() where T : BaseEntity
        {
            var type = CacheEntities.Find(t => t == typeof(T));
            if (type == null)
                return null;

            var dt = HttpRuntime.Cache[type.Name] as DataTable;
            return dt;
        }

        public List<T> GetEntities<T>() where T : BaseEntity
        {
            var type = CacheEntities.Find(t => t == typeof(T));
            if (type == null)
                return null;

            var entities = HttpRuntime.Cache[type.Name] as List<T>;
            return entities;
        }

        public void SetData<T>(DataTable dt) where T : BaseEntity
        {
            var type = CacheEntities.Find(t => t == typeof(T));
            if (type == null)
                return;

            HttpRuntime.Cache[type.Name] = dt;
        }

        public void SetEntities<T>(List<T> Entities) where T : BaseEntity
        {
            var type = CacheEntities.Find(t => t == typeof(T));
            if (type == null)
                return;

            HttpRuntime.Cache[type.Name] = Entities;
        }

        public void ResetCache<T>() where T : BaseEntity
        {
            var type = CacheEntities.Find(t => t == typeof(T));
            if (type == null)
                return;
            HttpRuntime.Cache.Remove(type.Name);
        }

        public int RemoveAll()
        {
            var nbr = HttpRuntime.Cache.Count;
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                HttpRuntime.Cache.Remove(enumerator.Key.ToString());
            }
            return nbr;
        }

        //public String LoadApplication(CollaborateurEntity currentUser)
        public String LoadApplication()
        {
            var daoCollaborateur = new DaoCollaborateur();
            var retour = "";

            if (General.HomeSociete == null)
            {
                var daoClient = new DaoClient();
                var res = daoClient.PsSelectEntities(new List<Filtre> { new Filtre { Name = "@ListSecteur", Value = "Home;" } });
                if (res != null)
                    General.HomeSociete = res.FirstOrDefault();
                retour += ("Chargement Home Société<br />");
            }

            //if (!(currentUser.Profil.Contains(EntityModele.Enums.ProfilEnum.Consultant) && currentUser.Profil.Count == 1))
            //{
                General.AllCollaborateurs = daoCollaborateur.PsSelectEntities();

                General.Administratifs = General.AllCollaborateurs.FindAll(c => c.Profil.Contains(EntityModele.Enums.ProfilEnum.Administratif));

                retour += ("Chargement de tous les collaborateurs et administratifs<br />");
            //}
            //else
            //{
                //var Params = new List<Filtre>(
                //new[] {
                //        new Filtre { Name = "@Profil", Value = EntityModele.Enums.ProfilEnum.Administratif},
                //});
                //General.Administratifs = daoCollaborateur.PsSelectEntities(Params);

                //retour += ("Chargement des administratifs<br />");
            //}

            var daoCompetence = new DaoCompetence();
            General.Competences = daoCompetence.PsSelectEntities();
            retour += ("Chargement du référentiel des compétences<br />");
            return retour;
        }

    }
}