using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAO;
using DAO.Cache;
using EntityModele;
using EntityModele.Enums;

namespace BIZ
{
    public class BizDroit
    {
        private DataSet dsDroits;
        private List<DroitPageEntity> DroitsPage;
        private List<DroitFonctionnaliteEntity> DroitsFonctionnalite;

        private static BizDroit instance;

        public static BizDroit Instance
        {
            get
            {
                if (instance == null)
                    instance = new BizDroit();
                return instance;
            }
        }

        public void Init()
        {
            instance = new BizDroit();
        }

        private BizDroit()
        {
            DroitsPage = CacheManager.Current.GetEntities<DroitPageEntity>();
            if (DroitsPage == null)
            {
                GetDroits();
                DroitsPage = DaoBase<DroitPageEntity>.PsSelectListEntities<DroitPageEntity>(dsDroits.Tables[0]);
                CacheManager.Current.SetEntities<DroitPageEntity>(DroitsPage);
            }

            DroitsFonctionnalite = CacheManager.Current.GetEntities<DroitFonctionnaliteEntity>();
            if (DroitsFonctionnalite == null)
            {
                GetDroits();
                DroitsFonctionnalite = DaoBase<DroitFonctionnaliteEntity>
                                        .PsSelectListEntities<DroitFonctionnaliteEntity>(dsDroits.Tables[1]);
                CacheManager.Current.SetEntities<DroitFonctionnaliteEntity>(DroitsFonctionnalite);
            }
        }

        private void GetDroits()
        {
            if (dsDroits == null)
            {
                dsDroits = DaoBase<DroitPageEntity>.PsSelect<DroitPageEntity, DroitFonctionnaliteEntity>();
            }
        }

        public List<DroitPageEntity> GetDroitsPage(List<ProfilEnum> profils)
        {
            return DroitsPage.Where(droit =>
                                        profils.Any(profil => Convert.ToInt32(profil) == droit.NumeroProfil)
                                        && !String.IsNullOrEmpty(droit.ReadWritePage)
                                    ).ToList();
        }

        public List<DroitFonctionnaliteEntity> GetDroitsFonctionnalite(List<ProfilEnum> profils)
        {
            return DroitsFonctionnalite.Where(droit =>
                                        profils.Any(profil => Convert.ToInt32(profil) == droit.NumeroProfil)
                                        && droit.Droit
                                    ).ToList();
        }
    }
}