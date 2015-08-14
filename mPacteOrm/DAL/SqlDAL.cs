using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace mPacte.Orm.DAL
{
    public class SqlDAL : AbstractDAL
    {
        private SqlDAL()
        {
            cnx = new SqlConnection(ConnectionString);
            ds = new DataSet();
        }

        ~SqlDAL()
        {
            if (cnx.State != ConnectionState.Closed)
                cnx.Close();
            cnx.Dispose();
        }

        public override String ConnectionString
        {
            get
            {
                String cnxstring = "";
                var Environment = ConfigurationManager.AppSettings["Environment"];
                if (Environment != null)
                {
                    if (Environment == "DEV")
                    {
                        cnxstring = ConfigurationManager.ConnectionStrings["SqlDevConnection"].ConnectionString;
                        if (cnxstring == null)
                            throw new Exception("La chaine de connexion SqlDevConnection est introuvable !");
                    }
                    else if (Environment == "PROD")
                    {
                        cnxstring = ConfigurationManager.ConnectionStrings["SqlProdConnection"].ConnectionString;
                        if (cnxstring == null)
                            throw new Exception("La chaine de connexion SqlProdConnection est introuvable !");
                    }
                    else
                        throw new Exception("L'environnement de l'application n'es pas défini !");
                }
                return cnxstring;
            }
        }

        private static SqlDAL instance;

        public static SqlDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new SqlDAL();
                return instance;
            }
        }

        public override IDataParameter NewParameter(string name, object value)
        {
            return new SqlParameter(name, value);
        }

        public override IDbCommand NewCommand(String query, IDbConnection cnx)
        {
            return new SqlCommand(query, cnx as SqlConnection);
        }

        public override IDbDataAdapter NewAdapter(IDbCommand cmd)
        {
            return new SqlDataAdapter(cmd as SqlCommand);
        }
    }
}