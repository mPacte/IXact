using System.Xml.Linq;

namespace mPacte.Orm
{
    public class FieldItem<T>
    {
        public string Name { get; set; }

        public string Column { get; set; }

        public string Type { get; set; }

        public object Value { get; set; }

        public static FieldItem<T> Parse(XElement xElement, T objet)
        {
            var fieldItem = new FieldItem<T>();

            fieldItem.Name = xElement.Attribute("name").Value;
            fieldItem.Column = xElement.Attribute("column").Value;
            fieldItem.Type = xElement.Attribute("type").Value;

            var value = typeof(T).GetProperty(fieldItem.Name).GetValue(objet, null);
            if (value != null)
            {
                if (fieldItem.Type != "int")
                    fieldItem.Value = " '" + value + "' ";
                else
                    fieldItem.Value = value;
            }
            else
                fieldItem.Value = value;

            return fieldItem;
        }

        public static FieldItem<T> Parse(XElement xElement)
        {
            var fieldItem = new FieldItem<T>();

            fieldItem.Name = xElement.Attribute("name").Value;
            fieldItem.Column = xElement.Attribute("column").Value;

            return fieldItem;
        }
    }
}