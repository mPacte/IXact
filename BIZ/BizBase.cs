using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using EntityModele;
using System.Collections;

namespace BIZ
{
    public class BizBase
    {
        public virtual List<T> LoadEntitiesFromData<T>(DataTable dataTable)
        {
            var list = new List<T>();
            foreach (DataRow dr in dataTable.Rows)
            {
                var instanceT = Activator.CreateInstance(typeof(T));
                foreach (PropertyInfo property in (typeof(T)).GetProperties())
                {
                    if (property.PropertyType.BaseType.Equals(typeof(BaseEntity)) && !property.PropertyType.Equals(typeof(T)))
                    {
                        MethodInfo method = typeof(BizBase).GetMethod("LoadEntitiesFromData");
                        MethodInfo generic = method.MakeGenericMethod(property.PropertyType);
                        var dt = dataTable.Clone();
                        dt.LoadDataRow(dr.ItemArray, true);
                        var listType = typeof(List<>).MakeGenericType(new Type[] { property.PropertyType });
                        var childEntity = generic.Invoke(this, new object[] { dt }) as IList;
                        property.SetValue(instanceT, childEntity[0], null);
                    }
                    else
                    {
                        if (dr.Table.Columns.Contains(property.Name))
                        {
                            if (property.PropertyType.Equals(dr[property.Name].GetType()))
                                property.SetValue(instanceT, dr[property.Name], null);
                        }
                    }
                }
                list.Add((T)instanceT);
            }

            return list;
        }
    }
}
