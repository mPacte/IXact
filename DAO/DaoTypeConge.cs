using System.Data;
using EntityModele;
using mPacte.Orm;

namespace DAO
{
    public class DaoTypeConge : DaoBase<TypeCongeEntity>
    {
        public DataTable GetAll()
        {
            var ormEntity = new OrmEntity<TypeCongeEntity>();
            var dt = ormEntity.PsSelect();
            return dt;

            //var TypeConges = new DataTable();
            //var requete = "SELECT * FROM TypeConge";
            //var ds = DAL.GetDataSet(requete);
            //if (ds != null)
            //{
            //    TypeConges = ds.Tables[0];
            //}
            //return TypeConges;
        }
    }
}