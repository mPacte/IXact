using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using mPacte.Orm.Criteres;
using mPacte.Orm.DAL;
using log4net;
using System.Reflection;

namespace mPacte.Orm
{
    public class OrmEntity<T> where T : class
    {
        public XElement TableXml;
        private TableItem<T> TableItem;
        private AbstractDAL DAL;
        protected static readonly ILog logger = LogManager.GetLogger("ORM");

        public OrmEntity()
        {
            ORMMap.Type = typeof(T);
            var sequence = from el in ORMMap.Instance.Root.Elements("class")
                          where (string)el.Attribute("name") == (typeof(T)).Name
                          select el;

            if (sequence.Count() == 0)
            {
                throw new Exception("La classe " + typeof(T).Name + " n'existe pas dans le fichier de mapping.");
            }

            TableXml = sequence.First();

            TableItem = TableItem<T>.Parse(TableXml);

            DAL = OrmDal.Instance.DAL;
        }

        public List<Criteria> Criterias { get; set; }

        public virtual DataTable PsSelect()
        {
            var psName = "Select" + TableItem.Table;
            var query = new PSQuery(psName);

            //logger.Info("Méthode : " + MethodBase.GetCurrentMethod().Name);
            //logger.Info("PS : " + psName);

            if (this.Criterias != null && this.Criterias.Count > 0)
            {
                //logger.Info("Paramètres : ");
                //var paramLog = "";
                this.Criterias.ForEach(criteria =>
                {
                    var dbParam = OrmDal.Instance.DAL.NewParameter(criteria.Name, criteria.Value);
                    query.Parameters.Add(dbParam);
                    //paramLog += criteria.Name + " = " + criteria.Value + " , ";
                });
                //logger.Info(paramLog);
            }

            var dt = OrmDal.Instance.DAL.GetDataTable(query);
            //logger.Info("Lignes retournées : " + dt.Rows.Count);
            return dt;
        }

        public virtual DataSet PsSelectDs()
        {
            var psName = "Select" + TableItem.Table;
            var query = new PSQuery(psName);

            if (this.Criterias != null && this.Criterias.Count > 0)
            {
                this.Criterias.ForEach(criteria =>
                {
                    var dbParam = OrmDal.Instance.DAL.NewParameter(criteria.Name, criteria.Value);
                    query.Parameters.Add(dbParam);
                });
            }

            var ds = OrmDal.Instance.DAL.GetDataSet(query);
            return ds;
        }

        public virtual DataSet PsSelect<U, V>()
        {
            var uTable = GetEntity(typeof(U).Name);
            var vTable = GetEntity(typeof(V).Name);
            var strParam = uTable.Table + "," + vTable.Table;

            var psName = "MultipleSelect";
            var query = new PSQuery(psName);
            var dbParam = OrmDal.Instance.DAL.NewParameter("@ListSelect", strParam);
            query.Parameters.Add(dbParam);

            var dt = OrmDal.Instance.DAL.GetDataSet(query);
            return dt;
        }

        public virtual T PsSelectEntities()
        {
            var dt = PsSelect();
            var instanceT = Activator.CreateInstance(typeof(T));
            var props = typeof(T).GetProperties();
            foreach (DataColumn dc in dt.Columns)
            {
                var propInstance = typeof(T).GetProperty(dc.ColumnName);
                propInstance.SetValue(instanceT, dt.Rows[0][dc.ColumnName], null);
            }

            return instanceT as T;
        }

        public virtual List<T> PsSelectListEntities()
        {
            var dt = PsSelect();

            var xProperties = (from el in TableXml.Elements("property")
                               select el).ToList();

            var listInstances = new List<T>();
            //Parcours des lignes qui représentent les instances de la classe U
            foreach (DataRow dr in dt.Rows)
            {
                var instanceT = Activator.CreateInstance(typeof(T));
                //Parcours des colonnes qui représentent les propriétés de la classe
                foreach (DataColumn dc in dt.Columns)
                {
                    var xProperty = xProperties.Find(xp => xp.Attribute("column").Value == dc.ColumnName);
                    if (xProperty == null)
                        continue;
                    var propInstance = typeof(T).GetProperty(xProperty.Attribute("name").Value);
                    if (propInstance == null)
                    {
                        throw new Exception("La propriété " + xProperty.Attribute("name").Value +
                            " est inexistante dans la classe " + typeof(T).Name);
                    }
                    if (dr[dc.ColumnName] != DBNull.Value)
                        propInstance.SetValue(instanceT, dr[dc.ColumnName], null);
                }
                listInstances.Add(instanceT as T);
            }
            return listInstances;
        }

        /// <summary>
        /// Méthode qui transforme un datatable en une liste d'entité en se basant sur le fichier de mapping
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<U> PsSelectListEntities<U>(DataTable dt) where U : class
        {
            var TableXml = (from el in ORMMap.Instance.Root.Elements("class")
                            where (string)el.Attribute("name") == (typeof(U)).Name
                            select el).First();

            //var TableItem = TableItem<T>.Parse(TableXml);

            var xProperties = (from el in TableXml.Elements("property")
                               select el).ToList();

            var listInstances = new List<U>();
            //Parcours des lignes qui représentent les instances de la classe U
            foreach (DataRow dr in dt.Rows)
            {
                var instanceT = Activator.CreateInstance(typeof(T));
                //Parcours des colonnes qui représentent les propriétés de la classe
                foreach (DataColumn dc in dt.Columns)
                {
                    var xProperty = xProperties.Find(xp => xp.Attribute("column").Value == dc.ColumnName);
                    var propInstance = typeof(U).GetProperty(xProperty.Attribute("name").Value);
                    if (propInstance == null)
                    {
                        throw new Exception("La propriété " + xProperty.Attribute("name").Value +
                            " est inexistante dans la classe " + typeof(U).Name);
                    }
                    if (dr[dc.ColumnName] != DBNull.Value)
                        propInstance.SetValue(instanceT, dr[dc.ColumnName], null);
                }
                listInstances.Add(instanceT as U);
            }
            return listInstances;
        }

        public virtual String PsInsert()
        {
            var psName = "Insert" + TableItem.Table;
            var query = new PSQuery(psName);
            this.Criterias.ForEach(param =>
            {
                var dbParam = DAL.NewParameter(param.Name, param.Value);
                query.Parameters.Add(dbParam);
            });

            var Identity = new Object(); ;
            DAL.ExecuteNonQuery(query, ref Identity);
            return Identity.ToString(); ;
        }

        public virtual void PsUpdate()
        {
            var psName = "Update" + TableItem.Table;
            var query = new PSQuery(psName);
            this.Criterias.ForEach(param =>
            {
                var dbParam = DAL.NewParameter(param.Name, param.Value);
                query.Parameters.Add(dbParam);
            });

            DAL.ExecuteNonQuery(query);
        }

        public TableItem<T> GetEntity(string className)
        {
            return TableItem<T>.Parse((from el in ORMMap.Instance.Root.Elements("class")
                                       where (string)el.Attribute("name") == className
                                       select el).First());
        }

        /// <summary>
        /// Retourner toutes les relations de l'entité en question
        /// </summary>
        /// <returns></returns>
        public List<Relations> GetRelations()
        {
            var dict = new List<Relations>();
            var exts = (from el in TableXml.Elements("property")
                        where el.HasElements
                        select el).ToList();
            exts.ForEach(ext =>
            {
                dict.Add(new Relations
                {
                    ThisTable = TableItem.Table,
                    ThisField = ext.Attribute("name").Value,
                    JoinTable = ext.Element("ext").Attribute("table").Value,
                    JoinField = ext.Element("ext").Attribute("field").Value
                });
            });

            return dict;
        }
    }

    public class Relations
    {
        public string ThisTable { get; set; }

        public string ThisField { get; set; }

        public string JoinTable { get; set; }

        public string JoinField { get; set; }
    }
}