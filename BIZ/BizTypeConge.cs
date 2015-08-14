using System.Data;
using DAO;
using DAO.Cache;
using EntityModele;

namespace BIZ
{
    public class BizTypeConge
    {
        public static DataTable GetAll()
        {
            var dt = CacheManager.Current.GetData<TypeCongeEntity>();
            if (dt == null)
            {
                var daoTypeConge = new DaoTypeConge();
                dt = daoTypeConge.PsSelect();
                CacheManager.Current.SetData<TypeCongeEntity>(dt);
            }
            return dt;
        }
    }
}