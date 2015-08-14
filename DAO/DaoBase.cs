using System;
using System.Collections.Generic;
using System.Data;
using EntityModele;
using EntityModele.Criteres;
using mPacte.Orm;

namespace DAO
{
    public class DaoBase<T> where T : BaseEntity
    {
        #region Public Methods

        public virtual DataTable PsSelect(List<Filtre> psParameters)
        {
            try
            {
                var ormEntity = new OrmEntity<T>();
                ormEntity.Criterias = CreateCriterias(psParameters);
                var dt = ormEntity.PsSelect();
                return dt;
            }
            catch(Exception ex1)
            {
                return null;
            }
        }

        public virtual DataSet PsSelectDs(List<Filtre> psParameters)
        {
            var ormEntity = new OrmEntity<T>();
            ormEntity.Criterias = CreateCriterias(psParameters);
            var ds = ormEntity.PsSelectDs();
            return ds;
        }

        //public virtual T PsSelectEntities(List<Filtre> psParameters)
        //{
        //    var ormEntity = new OrmEntity<T>();
        //    var dt = PsSelect(psParameters);
        //    var instanceT = Activator.CreateInstance(typeof(T));
        //    var props = typeof(T).GetProperties();
        //    var list = (from el in ormEntity.TableXml.Elements("property")

        //                select el).ToList();
        //    foreach (DataColumn dc in dt.Columns)
        //    {
        //        var propInstance = typeof(T).GetProperty(dc.ColumnName);
        //        propInstance.SetValue(instanceT, dt.Rows[0][dc.ColumnName], null);
        //    }

        //    return instanceT as T;
        //}

        public virtual List<T> PsSelectEntities()
        {
            var ormEntity = new OrmEntity<T>();
            var listInstances = ormEntity.PsSelectListEntities();
            return listInstances;
        }

        public virtual List<T> PsSelectEntities(List<Filtre> psParameters)
        {
            var ormEntity = new OrmEntity<T>();
            ormEntity.Criterias = CreateCriterias(psParameters);
            var listInstances = ormEntity.PsSelectListEntities();
            return listInstances;
        }

        public virtual DataTable PsSelect()
        {
            var ormEntity = new OrmEntity<T>();
            var dt = ormEntity.PsSelect();
            return dt;
        }

        public virtual DataSet PsSelectDs()
        {
            var ormEntity = new OrmEntity<T>();
            var ds = ormEntity.PsSelectDs();
            return ds;
        }

        public static DataSet PsSelect<U, V>()
        {
            var ormEntity = new OrmEntity<T>();
            var ds = ormEntity.PsSelect<U, V>();
            return ds;
        }

        public static List<U> PsSelectListEntities<U>(DataTable dt) where U : BaseEntity
        {
            var listInstances = OrmEntity<U>.PsSelectListEntities<U>(dt);
            return listInstances;
        }

        public virtual String PsInsert(List<Filtre> psParameters)
        {
            var ormEntity = new OrmEntity<T>();
            psParameters.Add(new Filtre { Name = "@LoginCreation", Value = "CurrentUser" });
            ormEntity.Criterias = CreateCriterias(psParameters);
            return ormEntity.PsInsert();
        }

        public virtual void PsUpdate(List<Filtre> psParameters)
        {
            var ormEntity = new OrmEntity<T>();
            psParameters.Add(new Filtre { Name = "@LoginModification", Value = "CurrentUser" });
            ormEntity.Criterias = CreateCriterias(psParameters);
            ormEntity.PsUpdate();
        }

        #endregion Public Methods

        private List<Criteria> CreateCriterias(List<Filtre> psParameters)
        {
            var criterias = new List<Criteria>();
            psParameters.ForEach(
                                    param => criterias.Add(
                                        OrmManager.CurrentSession.NewCriteria(param.Name, param.Value))
                                 );
            return criterias;
        }

        #region RequetesSQL

        //public string Insert(QueryFields fields, T objet)
        //{
        //    var sBuilder = new StringBuilder();

        //    //Construire la partie "Insert into" de la requête :
        //    sBuilder.Append("Insert Into " + typeof(T).Name + " (");
        //    var index = 0;
        //    foreach (var field in fields)
        //    {
        //        index++;
        //        var column = from el in Orm.TableXml.Elements("property")
        //                     where (string)el.Attribute("name") == field.Name
        //                     select el;

        //        sBuilder.Append(column.Attributes("column").First().Value);
        //        if (index < fields.Count)
        //        {
        //            sBuilder.Append(", ");
        //        }
        //        else
        //            sBuilder.Append(")");
        //    }

        //    //Construire la partie "Values" de la requête :
        //    index = 0;
        //    sBuilder.Append("Values (");
        //    fields.ForEach(prop =>
        //    {
        //        index++;
        //        sBuilder.Append("'" + prop.GetValue(objet, null) + "'");
        //        if (index < fields.Count)
        //            sBuilder.Append(",");
        //        else
        //            sBuilder.Append(")");
        //    });

        //    var Identity = "Yes";
        //    DAL.ExecuteNonQuery(sBuilder.ToString(), ref Identity);
        //    return Identity; ;

        //}

        //public string Update(QueryFields fields, T objet, PropertyInfo key)
        //{
        //    var sBuilder = new StringBuilder();

        //    //Construire la partie "Insert into" de la requête :
        //    sBuilder.Append("UPDATE " + typeof(T).Name + " SET ");
        //    var index = 0;
        //    foreach (var field in fields)
        //    {
        //        index++;
        //        var fieldItem = FieldItem<T>.Parse((from el in Orm.TableXml.Elements("property")
        //                                            where (string)el.Attribute("name") == field.Name
        //                                            select el).First(), objet);
        //        if (fieldItem.Value == null) continue;
        //        sBuilder.Append(fieldItem.Column);
        //        sBuilder.Append(" = ");
        //        sBuilder.Append(fieldItem.Value);

        //        if (index < fields.Count)
        //        {
        //            sBuilder.Append(", ");
        //        }
        //    }
        //    sBuilder.Append(" WHERE " + key.Name + " = ");
        //    if (key.PropertyType != typeof(int))
        //        sBuilder.Append(" '");
        //    sBuilder.Append(key.GetValue(objet, null));
        //    if (key.PropertyType != typeof(int))
        //        sBuilder.Append(" '");

        //    DAL.ExecuteNonQuery(sBuilder.ToString());

        //    return key.GetValue(objet, null).ToString();

        //}

        //public DataTable Select()
        //{
        //    //var classItem = ClassItem<T>.Parse(TableXml);
        //    var sBuilder = new StringBuilder();

        //    sBuilder.Append("SELECT * FROM " + Orm.TableItem.Table);

        //    var ds = DAL.GetDataSet(sBuilder.ToString());
        //    var dtArticles = new DataTable();
        //    if (ds != null)
        //        dtArticles = ds.Tables[0];

        //    return dtArticles;
        //}
        /*
                public DataTable Select(Query query)
                {
                    var sBuilder = new StringBuilder();
                    sBuilder.Append("SELECT ");
                    sBuilder.Append(MakeFields(query.Fields));
                    sBuilder.Append(" FROM " + Orm.TableItem.Table);
                    sBuilder.Append(MakeJoins(query.Joins));
                    sBuilder.Append(MakeConditions(query.Conditions));
                    sBuilder.Append(MakeOrderBy(query.OrderBy));

                    var ds = DAL.GetDataSet(sBuilder.ToString());
                    var dtArticles = new DataTable();
                    if (ds != null)
                        dtArticles = ds.Tables[0];

                    return dtArticles;
                }

                public DataTable Select(String query)
                {
                    var ds = DAL.GetDataSet(query);
                    var dtArticles = new DataTable();
                    if (ds != null)
                        dtArticles = ds.Tables[0];

                    return dtArticles;
                }

                #region private Methods

                private string MakeFields(QueryFields fields)
                {
                    if (fields == null)
                        return "*";
                    if (!String.IsNullOrEmpty(fields.Fields))
                        return fields.Fields;
                    var sBuilder = new StringBuilder();
                    var index = 0;
                    fields.ForEach(prop =>
                    {
                        index++;
                        var fieldItem = FieldItem<T>.Parse((from el in Orm.TableXml.Elements("property")
                                                            where (string)el.Attribute("name") == prop.Name
                                                            select el).First());
                        sBuilder.Append(fieldItem.Column);
                        if (index < fields.Count)
                            sBuilder.Append(", ");
                    });

                    return sBuilder.ToString();
                }

                private string MakeConditions(QueryConditions conditions)
                {
                    if (conditions == null)
                        return String.Empty;
                    var sBuilder = new StringBuilder();
                    sBuilder.Append(" WHERE ");
                    var index = 0;
                    foreach (var cond in conditions)
                    {
                        index++;
                        sBuilder.Append(cond.Key.Name + " = ");
                        if (!General.IsNumber(cond.Value))
                            sBuilder.Append(" '" + cond.Value + "' ");
                        else
                            sBuilder.Append(cond.Value);
                        if (index < conditions.Count)
                            sBuilder.Append(" AND ");
                    }

                    return sBuilder.ToString();
                }

                private string MakeOrderBy(QueryOrderBy orderBy)
                {
                    if (orderBy == null)
                        return String.Empty;
                    var sBuilder = new StringBuilder();
                    sBuilder.Append(" ORDER BY ");
                    var index = 0;
                    foreach (var cond in orderBy)
                    {
                        index++;
                        sBuilder.Append(cond.Key.Name);
                        if (cond.Value != null)
                            sBuilder.Append(" " + cond.Value.ToString());
                        if (index < orderBy.Count)
                            sBuilder.Append(", ");
                    }

                    return sBuilder.ToString();
                }

                private string MakeJoins(QueryJoins joins)
                {
                    if (joins == null)
                        return String.Empty;
                    var relations = Orm.GetRelations();

                    var sBuilder = new StringBuilder();
                    joins.ForEach(join =>
                        {
                            var tableJoin = Orm.GetEntity(join).Table;
                            var thisRel = relations.Where(rel => rel.JoinTable == tableJoin).ToList();
                            thisRel.ForEach(rel =>
                                {
                                    sBuilder.Append(" INNER JOIN ");
                                    var alias = tableJoin + rel.ThisField;
                                    sBuilder.Append(tableJoin + " AS " + alias + " ON ");
                                    sBuilder.Append(alias + "." + rel.JoinField + " = ");
                                    sBuilder.Append(Orm.TableItem.Table + "." + rel.ThisField);
                                });
                        }
                    );

                    return sBuilder.ToString();
                }

                #endregion private Methods

                */

        #endregion RequetesSQL
    }
}