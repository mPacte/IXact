using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityModele;
using DAO.Cache;
using DAO;

namespace BIZ
{
    public class BizTypeMission
    {
        public static List<TypeMissionEntity> GetAll()
        {
            var typesMissions = CacheManager.Current.GetEntities<TypeMissionEntity>();
            if (typesMissions == null)
            {
                var daoTypeMission = new DaoTypeMission();
                typesMissions = daoTypeMission.PsSelectEntities();
                CacheManager.Current.SetEntities<TypeMissionEntity>(typesMissions);
            }
            return typesMissions;
        }
    }
}
