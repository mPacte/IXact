using System.Xml.Linq;

namespace mPacte.Orm
{
    public class TableItem<T>
    {
        public string Class { get; set; }

        public string Table { get; set; }

        public static TableItem<T> Parse(XElement xElement)
        {
            var classItem = new TableItem<T>();

            classItem.Class = xElement.Attribute("name").Value;
            classItem.Table = xElement.Attribute("table").Value;

            return classItem;
        }
    }
}