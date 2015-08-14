using System.Xml.Linq;

namespace mPacte.Orm
{
    public class MapProperty<T>
    {
        public string Property { get; set; }

        public string Field { get; set; }

        public static TableItem<T> Parse(XElement xElement)
        {
            var classItem = new TableItem<T>();

            classItem.Class = xElement.Attribute("name").Value;
            classItem.Table = xElement.Attribute("table").Value;

            return classItem;
        }
    }
}