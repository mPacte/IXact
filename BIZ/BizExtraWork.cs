using System;
using System.Collections.Generic;
using System.Data;
using DAO;
using EntityModele.Criteres;

namespace BIZ
{
    public class BizExtraWork
    {
        private DaoExtraWork daoEwtraWork;

        public DataTable GetByActivite(Int64 numeroActivite)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@numeroActivite", Value = numeroActivite},
                });

            daoEwtraWork = new DaoExtraWork();
            return daoEwtraWork.PsSelect(Params);
        }

        public DataTable GetByActivite(Int64 numeroActivite, int mois, int annee)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@numeroActivite", Value = numeroActivite},
                        new Filtre { Name = "@mois", Value = mois},
                        new Filtre { Name = "@annee", Value = annee}
                });

            daoEwtraWork = new DaoExtraWork();
            return daoEwtraWork.PsSelect(Params);
        }

        public Single GetWorkedHoliday(Int64 numeroActivite, DateTime day)
        {
            var nbr = new Single();
            var dt = GetByActivite(numeroActivite);
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToDateTime(dr["DateExtra"]) == day && Convert.ToSingle(dr["day"]) != 0)
                    nbr = Convert.ToSingle(dr["day"]);
            }
            return nbr;
        }

        public void Nouveau(Int64 numeroActivite, DateTime dateTravail, Single jour, Single heures, String remarques, String logUser)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@numeroActivite", Value = numeroActivite},
                        new Filtre { Name = "@dateExtra", Value = dateTravail},
                        new Filtre { Name = "@day", Value = jour},
                        new Filtre { Name = "@hours", Value = heures},
                        new Filtre { Name = "@Remarques", Value = remarques},
                        new Filtre { Name = "@LoginInsertion", Value = logUser},
                });

            var daoExtraWork = new DaoExtraWork();
            daoExtraWork.PsInsert(Params);
        }
    }
}