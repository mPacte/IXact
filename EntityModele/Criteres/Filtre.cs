using System;

namespace EntityModele.Criteres
{
    public class Filtre
    {
        public String Name { get; set; }

        public Object Value { get; set; }

        public override string ToString()
        {
            return "Name : " + Name + ", Value : " + Value;
        }
    }
}