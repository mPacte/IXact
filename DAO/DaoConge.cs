using EntityModele;

namespace DAO
{
    public class DaoConge : DaoBase<CongeEntity>
    {
        //public DataTable GetAll()
        //{
        //    var Demandes = new DataTable();
        //    var requete = "SELECT * FROM Conge";
        //    var ds = DAL.GetDataSet(requete);
        //    if (ds != null)
        //    {
        //        Demandes = ds.Tables[0];
        //    }
        //    return Demandes;

        //}

        //public new List<CongeEntity> PsSelect(List<PSQueryParams> psParameters)
        //{
        //    var list = new List<CongeEntity>();
        //    var dt = base.PsSelect();
        //    foreach(DataColumn dc in dt.Columns)
        //    {
        //        typeof(CongeEntity).GetProperty("NumeroConge") dc.ColumnName
        //    }
        //    foreach(DataRow dr in dt.Rows)
        //    {
        //        dr.ItemArray.ToList().ForEach(column => column.
        //    }
        //    return list;
        //}

        //public DataTable GetByResponsable_Statuts(int numeroCollaborateur, List<StatutEnum> statuts)
        //{
        //    var sBuilder = new System.Text.StringBuilder();
        //    var i = 0;
        //    statuts.ForEach(p =>
        //        {
        //            sBuilder.Append((int)p);
        //            i++;
        //            if (i < statuts.Count)
        //                sBuilder.Append(",");
        //        });

        //    var Demandes = new DataTable();

        //    var requete = "SELECT Coll.*, Conge.*, TypeConge.Libelle, Res.Nom + ' ' + Res.Prenom as NomResponsable, Coll.Nom + ' ' + Coll.Prenom as NomDemandeur " +
        //                    "FROM  Conge " +
        //                    "INNER JOIN Collaborateur as Coll ON Conge.Demandeur = Coll.NumeroCollaborateur " +
        //                    "INNER JOIN TypeConge ON Conge.TypeConge = TypeConge.Numero " +
        //                    "INNER JOIN Collaborateur as Res ON Res.NumeroCollaborateur = Coll.Responsable " +
        //                    "WHERE Statut IN (" + sBuilder.ToString() + ") " +
        //                    //"WHERE Statut = " + (int)statut1 +
        //                    //" OR    Statut = " + (int)statut2 +
        //                    " AND Coll.Responsable = " + numeroCollaborateur +
        //                    " ORDER BY DateValidation";

        //    var ds = DAL.GetDataSet(requete);
        //    if (ds != null)
        //    {
        //        Demandes = ds.Tables[0];
        //    }
        //    return Demandes;

        //}
    }
}