using System;
using System.Configuration;
using System.IO;
using System.Xml.Linq;

namespace mPacte.Orm
{
    public class ORMMap
    {
        private ORMMap()
        {
            try
            {
                //var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ORM.Configuration.Mapping.xml");
                var fichierMapping = ConfigurationManager.AppSettings["ORM"].ToString();
                var stream = Type.Assembly.GetManifestResourceStream(fichierMapping);
                var reader = new StreamReader(stream);
                Root = XElement.Load(reader);
            }
            catch(Exception ex1)
            {

            }
        }

        private static ORMMap instance;

        public static Type Type;

        public static ORMMap Instance
        {
            get
            {
                if (instance == null)
                    instance = new ORMMap();
                return instance;
            }
        }

        public XElement Root { get; set; }
    }
}