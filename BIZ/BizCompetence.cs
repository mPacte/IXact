using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityModele;
using DAO;
using DAO.Cache;

namespace BIZ
{
    public class BizCompetence
    {
        private DaoCompetence daoCompetence;

        public List<CompetenceEntity> GetAll()
        {
            daoCompetence = new DaoCompetence();
            var list = CacheManager.Current.GetEntities<CompetenceEntity>();
            if (list == null)
            {
                list = daoCompetence.PsSelectEntities();
                CacheManager.Current.SetEntities<CompetenceEntity>(list);
            }
            return list;
        } 
    }
}
