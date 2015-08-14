using System.Collections.Generic;
using System.Data;
using DAO;
using EntityModele.Criteres;

namespace BIZ
{
    public class BizSoldeConges
    {
        public DaoSoldeConges daoSoldeConges { get; set; }

        public DataSet GetByCollaborateurAnnee(int numeroCollaborateur, int annee)
        {
            daoSoldeConges = new DaoSoldeConges();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Collaborateur", Value = numeroCollaborateur},
                        new Filtre { Name = "@Annee", Value = annee}
                });
            return daoSoldeConges.PsSelectDs(Params);
        }
    }
}