using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace mPacte.Orm.DAL
{
    public class OleDbDAL : AbstractDAL
    {
        private OleDbDAL()
        {
            cnx = new OleDbConnection(ConnectionString);
        }

        ~OleDbDAL()
        {
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
                        cnxstring = ConfigurationManager.ConnectionStrings["OleDbDevConnection"].ConnectionString;
                        if (cnxstring == null)
                            throw new Exception("La chaine de connexion OleDbDevConnection est introuvable !");
                    }
                    else if (Environment == "PROD")
                    {
                        cnxstring = ConfigurationManager.ConnectionStrings["OleDbProdConnection"].ConnectionString;
                        if (cnxstring == null)
                            throw new Exception("La chaine de connexion OleDbProdConnection est introuvable !");
                    }
                    else
                        throw new Exception("L'environnement de l'application n'es pas défini !");
                }
                return cnxstring;
            }
        }

        private static OleDbDAL instance;

        public static OleDbDAL Instance
        {
            get
            {
                if (instance == null)
                    instance = new OleDbDAL();
                return instance;
            }
        }

        public override IDataParameter NewParameter(string name, object value)
        {
            return new OleDbParameter(name, value);
        }

        public override IDbCommand NewCommand(String query, IDbConnection cnx)
        {
            return new OleDbCommand(query, cnx as OleDbConnection);
        }

        public override IDbDataAdapter NewAdapter(IDbCommand cmd)
        {
            return new OleDbDataAdapter(cmd as OleDbCommand);
        }
    }
}