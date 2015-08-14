using System;
using System.Configuration;

namespace mPacte.Orm.DAL
{
    public class OrmDal
    {
        private OrmDal()
        {
            Provider = ConfigurationManager.AppSettings["myProvider"];
            if (Provider != null)
            {
                if (Provider == "OLEDB")
                    DAL = OleDbDAL.Instance;
                else if (Provider == "SQL")
                    DAL = SqlDAL.Instance;
            }
            else
                throw new Exception("Le provider n'est pas renseigné dans la configuration de l'application.");


        }

        private static OrmDal instance;

        public static OrmDal Instance
        {
            get
            {
                if (instance == null)
                    instance = new OrmDal();
                return instance;
            }
        }

        public AbstractDAL DAL { get; set; }

        public String Provider { get; set; }

        public String ConnectionString { get; set; }

        public String Environment { get; set; }
    }
}