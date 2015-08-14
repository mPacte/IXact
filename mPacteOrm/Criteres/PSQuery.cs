using System.Collections.Generic;
using System.Data;

namespace mPacte.Orm.Criteres
{
    public class PSQuery
    {
        public string Name { get; set; }

        public List<IDataParameter> Parameters { get; set; }

        /// <summary>
        /// Nouveau PSQuery
        /// </summary>
        /// <param name="name">Nom de la procédure stockée dans la base de données</param>
        public PSQuery(string name)
        {
            Name = name;
            Parameters = new List<IDataParameter>();
        }
    }
}