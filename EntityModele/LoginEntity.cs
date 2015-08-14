using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EntityModele
{
    public class LoginEntity : BaseEntity
    {
        public string Numero { get; set; }

        public string Username { get; set; }

        public DateTime DateLogin { get; set; }

        public string AdresseIp { get; set; }

        public void Insert(List<PropertyInfo> properties)
        {
            //var properties = typeof(Login).GetProperties().ToList();
            var sBuilder = new StringBuilder();
            sBuilder.Append("Insert Into " + typeof(LoginEntity).Name + " (");
            var index = 0;
            foreach (var prop in properties)
            {
                index++;
                if (index < properties.Count)
                {
                    sBuilder.Append(prop.Name + ", ");
                }
                else
                    sBuilder.Append(prop.Name + ")");
            }
            index = 0;
            sBuilder.Append("Values (");
            properties.ForEach(prop =>
                {
                    index++;
                    sBuilder.Append("'" + prop.GetValue(this, null) + "'");
                    if (index < properties.Count)
                        sBuilder.Append(",");
                    else
                        sBuilder.Append(")");
                });
        }
    }
}