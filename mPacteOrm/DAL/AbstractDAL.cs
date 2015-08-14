using System;
using System.Configuration;
using System.Data;
using System.Reflection;
using log4net;
using mPacte.Orm.Criteres;

namespace mPacte.Orm.DAL
{
    public abstract class AbstractDAL
    {
        protected static readonly ILog logger = LogManager.GetLogger("DAL");

        public AbstractDAL()
        {
        }

        public IDbConnection cnx;
        public DataSet ds;

        public DataTable GetDataTable(PSQuery psQuery)
        {
            try
            {
                var method = MethodBase.GetCurrentMethod();
                logger.Info(method.Name + " : " + psQuery.Name);

                var cmd = NewCommand(psQuery.Name, cnx);
                var adap = NewAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                var paramLog = "";
                psQuery.Parameters.ForEach(param =>
                {
                    cmd.Parameters.Add(param);
                    paramLog += param.ParameterName + " = " + param.Value + " , ";
                });
                logger.Info("Paramètres : " + paramLog);
                ds = new DataSet();
                if (cnx.State == ConnectionState.Closed)
                    cnx.Open();
                adap.Fill(ds);
                logger.Info("Lignes retournées : " + ds.Tables[0].Rows.Count);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
            finally
            {
                if (cnx.State != ConnectionState.Closed)
                {
                    cnx.Close();
                    logger.Info("Fermeture de connexion.............");
                }
            }
        }

        public DataSet GetDataSet(PSQuery psQuery)
        {
            try
            {
                var method = MethodBase.GetCurrentMethod();
                logger.Info(method.Name + " / " + psQuery.Name);

                var cmd = NewCommand(psQuery.Name, cnx);
                var adap = NewAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                var paramLog = "";
                psQuery.Parameters.ForEach(param =>
                {
                    cmd.Parameters.Add(param);
                    paramLog += param.ParameterName + " = " + param.Value + " , ";
                });
                logger.Info("Paramètres : " + paramLog);
                ds = new DataSet();
                if (cnx.State == ConnectionState.Closed)
                    cnx.Open();
                adap.Fill(ds);
                logger.Info("Tables retournées : " + ds.Tables.Count);
                foreach (DataTable dt in ds.Tables)
                {
                    logger.Info(dt.TableName + " : " + dt.Rows.Count + " lignes.");
                }
                return ds;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
            finally
            {
                if (cnx.State != ConnectionState.Closed)
                {
                    cnx.Close();
                    logger.Info("Fermeture de connexion.............");
                }
            }
        }

        public DataSet GetDataSet(String Query)
        {
            var cmd = NewCommand(Query, cnx);
            var adap = NewAdapter(cmd);
            cmd.CommandType = CommandType.Text;
            if (cnx.State == ConnectionState.Closed)
                cnx.Open();
            ds = new DataSet();
            adap.Fill(ds);
            cnx.Close();
            return ds;
        }

        public void ExecuteNonQuery(string Query, ref string Identity)
        {
            var cmd = NewCommand(Query, cnx);
            cmd.CommandType = CommandType.Text;
            if (cnx.State == ConnectionState.Closed)
                cnx.Open();
            cmd.ExecuteNonQuery();
            if (Identity == "Yes")
            {
                cmd.CommandText = "SELECT @@IDENTITY";
                Identity = cmd.ExecuteScalar().ToString();
            }
            cnx.Close();
        }

        public void ExecuteNonQuery(string Query)
        {
            var cmd = NewCommand(Query, cnx);
            cmd.CommandType = CommandType.Text;
            if (cnx.State == ConnectionState.Closed)
                cnx.Open();
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public void ExecuteNonQuery(PSQuery psQuery)
        {
            var method = MethodBase.GetCurrentMethod();
            logger.Info(method.Name + " / " + psQuery.Name);

            var cmd = NewCommand(psQuery.Name, cnx);
            cmd.CommandType = CommandType.StoredProcedure;
            var paramLog = "";
            psQuery.Parameters.ForEach(param =>
            {
                cmd.Parameters.Add(param);
                paramLog += param.ParameterName + " = " + param.Value + " , ";
            });
            logger.Info("Paramètres : " + paramLog);
            if (cnx.State == ConnectionState.Closed)
                cnx.Open();
            var retour = cmd.ExecuteNonQuery();
            logger.Info("Retour exécutions : " + retour.ToString());
            cnx.Close();
            logger.Info("Fermeture de connexion.............");
        }

        public void ExecuteNonQuery(PSQuery psQuery, ref Object Identity)
        {
            var method = MethodBase.GetCurrentMethod();
            logger.Info(method.Name + " / " + psQuery.Name);

            var cmd = NewCommand(psQuery.Name, cnx);
            cmd.CommandType = CommandType.StoredProcedure;
            var paramLog = "";
            psQuery.Parameters.ForEach(param =>
            {
                cmd.Parameters.Add(param);
                paramLog += param.ParameterName + " = " + param.Value + " , ";
            });
            logger.Info("Paramètres : " + paramLog);
            var p = NewParameter("@Identity", null);
            p.DbType = DbType.Int64;
            p.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(p);
            if (cnx.State == ConnectionState.Closed)
                cnx.Open();
            var retour = cmd.ExecuteNonQuery();
            logger.Info("Retour exécutions : " + retour.ToString() + ". Identity DB : " + p.Value);
            cnx.Close();
            logger.Info("Fermeture de connexion.............");
            Identity = p.Value;
        }

        public string ExecuteScalar(string Query)
        {
            var method = MethodBase.GetCurrentMethod();
            logger.Info(method.Name + " / " + Query);
            
            var cmd = NewCommand(Query, cnx);
            cmd.CommandType = CommandType.Text;
            if (cnx.State == ConnectionState.Closed)
                cnx.Open();
            string res = cmd.ExecuteScalar().ToString();
            logger.Info("Retour exécutions : " + res);
            cnx.Close();
            logger.Info("Fermeture de connexion.............");
            return res;
        }

        public abstract IDataParameter NewParameter(string name, object value);

        public abstract IDbCommand NewCommand(String query, IDbConnection cnx);

        public abstract IDbDataAdapter NewAdapter(IDbCommand cmd);

        /// <summary>
        /// Chaine de connexion
        /// </summary>
        public abstract String ConnectionString { get; }

        private string connection()
        {
            var cnx = String.Empty;
            var myEnvironment = ConfigurationManager.AppSettings["Environment"];
            if (myEnvironment == "Prod")
            {
                cnx = ConfigurationManager.ConnectionStrings["OledbProdConnection"].ConnectionString;
                if (cnx == null)
                {
                    throw new Exception("La chaine de connexion de production est introuvable !");
                }
            }
            if (myEnvironment == "DEV")
            {
                var myProvider = ConfigurationManager.AppSettings["myProvider"];
                if (myProvider == "OLEDB")
                {
                    cnx = ConfigurationManager.ConnectionStrings["OledbDevConnection"].ConnectionString;
                    if (cnx == null)
                    {
                        throw new Exception("La chaine de connexion OledbDevConnection est introuvable !");
                    }
                }

                if (myProvider == "SQL")
                {
                    cnx = ConfigurationManager.ConnectionStrings["SqlDevConnection"].ConnectionString;
                    if (cnx == null)
                    {
                        throw new Exception("La chaine de connexion SqlDevConnection est introuvable !");
                    }
                }
            }
            return cnx;
        }
    }
}