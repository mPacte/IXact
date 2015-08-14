using EntityModele;
using System;
using System.Linq;

namespace DAO
{
    public class DaoActivite : DaoBase<ActiviteEntity>
    {
        //public override System.Collections.Generic.List<ActiviteEntity> PsSelectEntities()
        //{
        //    var activites = base.PsSelectEntities();
        //    foreach (ActiviteEntity activity in activites.Where(a => !String.IsNullOrEmpty(a.Duree)))
        //    {
        //        DateTime dateFin;
        //        if (activity.DateFinEffective != null)
        //        {
        //            dateFin = activity.DateFinEffective.Value.AddDays(1);
        //        }
        //        else
        //        {
        //            dateFin = DateTime.Now.AddDays(1);
        //        }
        //        var dateSpan = DateTimeSpan.CompareDates(activity.DateDebut, dateFin);
        //        if (activity.DateDebut > DateTime.Today) continue;

        //        if (dateSpan.Years != 0)
        //            activity.Duree = dateSpan.Years + " a, ";
        //        if (dateSpan.Months != 0)
        //            activity.Duree += dateSpan.Months + " m, ";
        //        if (dateSpan.Days != 0)
        //            activity.Duree += dateSpan.Days + " j";
        //    }
        //    return activites;
        //}
    }
}