using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityModele
{
    [Serializable]
    public class CompetenceEntity : BaseEntity
    {
        public int NumeroCompetence { get; set; }

        public String LibelleCompetence { get; set; }

        public int NiveauCompetence { get; set; }

        public override string ToString()
        {
            return LibelleCompetence;
        }
    }
}
