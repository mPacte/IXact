using System;

namespace EntityModele
{
    [Serializable]
    public class TypeCongeEntity : BaseEntity
    {
        public int Code { get; set; }

        public string Libelle { get; set; }
    }
}