using System;

namespace EntityModele
{
    [Serializable]
    public class TypeMissionEntity : BaseEntity
    {
        public string Code { get; set; }

        public string Libelle { get; set; }
    }
}
