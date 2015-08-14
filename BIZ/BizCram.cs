using System;
using System.Collections.Generic;
using System.Data;
using DAO;
using EntityModele;
using EntityModele.Criteres;
using EntityModele.Enums;
using log4net;
using System.Linq;

namespace BIZ
{
    public class BizCram
    {
        private DaoCram daoCram;
        private DataTable dtCRAM;
        private Int32 nbrActivites;
        private Int32 nbrJours;
        protected static readonly ILog logger = LogManager.GetLogger("BizCram");

        //public DaoConge daoDemande { get; set; }
        private BizConge bizConge;

        public void Valider(DateTime Periode, int Collaborateur)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Collaborateur", Value = Collaborateur},
                        new Filtre { Name = "@Periode", Value= Periode},
                        new Filtre { Name = "@Etat", Value= 1},
                        new Filtre { Name = "@Validateur", Value = General.CurrentUser.NumeroCollaborateur},
                        new Filtre { Name = "@DateValidation", Value= DateTime.Now},
                });

            daoCram = new DaoCram();
            daoCram.PsUpdate(Params);
        }

        public void Confirmer(CramEntity Cram)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Collaborateur", Value = Cram.Collaborateur},
                        new Filtre { Name = "@Periode", Value= Cram.Periode},
                        new Filtre { Name = "@Etat", Value= 2},
                        new Filtre { Name = "@Confirmateur", Value = General.CurrentUser.NumeroCollaborateur},
                        new Filtre { Name = "@DateConfirmation", Value= DateTime.Now},
                });

            daoCram = new DaoCram();
            daoCram.PsUpdate(Params);
            NotificationManager.CurrentSession.ConfirmationCram(Cram);
        }

        public void AjouterPiece(CramEntity Cram)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Collaborateur", Value = Cram.Collaborateur},
                        new Filtre { Name = "@Periode", Value= Cram.Periode},
                        new Filtre { Name = "@FichierCram", Value= Cram.Fichier}
                });

            daoCram = new DaoCram();
            daoCram.PsUpdate(Params);
        }

        public List<CramEntity> Alerter(List<CramFicheEntity> Crams)
        {
            var listAlerted = new List<CramEntity>();
            if (DateTime.Today <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, General.DayValidationCram)) return null;
            foreach (var cram in Crams.Select(c => c.Cram))
            {
                if (cram.Etat == 0)
                {
                    if (DateTime.Today >= new DateTime(cram.Periode.Year, cram.Periode.Month, DateTime.DaysInMonth(cram.Periode.Year, cram.Periode.Month)))
                    {
                        NotificationManager.CurrentSession.AlertValidationEtJustificatifCram(cram);
                        listAlerted.Add(cram);
                    }
                    else
                    {
                        NotificationManager.CurrentSession.AlertValidationCram(cram);
                        listAlerted.Add(cram);
                    }
                }
                else if (cram.Etat == 1 && DateTime.Today >= new DateTime(cram.Periode.Year, cram.Periode.Month, DateTime.DaysInMonth(cram.Periode.Year, cram.Periode.Month)))
                {
                    NotificationManager.CurrentSession.AlertJustificatifCram(cram);
                    listAlerted.Add(cram);
                }
            }

            NotificationManager.CurrentSession.InformerDirectionAlertes(listAlerted);
            return listAlerted;
        }

        private void InformerDirectionAlertes(List<CramEntity> listAlerted)
        {

        }

        public CramFicheEntity GetFicheCRAM(int numeroCollaborateur, int mois, int annee)
        {
            var cramFicheEntity = new CramFicheEntity();
            cramFicheEntity.Cram = GetCram(numeroCollaborateur, new DateTime(annee, mois, 1));
            if (cramFicheEntity.Cram == null)
                return null;

            var bizActivite = new BizActivite();
            var bizExtraWork = new BizExtraWork();
            var ds = new DataSet();
            var dtActivites = new DataTable("Activites");
            var dtExtraWork = new DataTable();
            var dtConges = new DataTable("Conges");
            dtCRAM = new DataTable("CRAM");
            bizConge = new BizConge();
            //daoDemande = new DaoConge();
            nbrJours = DateTime.DaysInMonth(annee, mois);
            dtConges = bizConge.GetByDemandeur_Mois(numeroCollaborateur, mois, annee);
            //dtConges = daoDemande.GetByCollaborateur_Mois(numeroCollaborateur, mois);

            //Créer l'entête du CRAM
            for (int i = 0; i <= nbrJours + 1; i++)
            {
                var column = new DataColumn();
                if (i == 0)
                {
                    column.ColumnName = "Activité";
                    dtCRAM.Columns.Add(column);
                }
                else if (i == nbrJours + 1)
                {
                    column.ColumnName = "Total";
                    dtCRAM.Columns.Add(column);
                }
                else
                {
                    column.ColumnName = i.ToString();
                    if (BizHoliday.Instance.IsHoliday(new DateTime(annee, mois, i)))
                    {
                        column.ColumnName = i.ToString();
                        column.DefaultValue = "H";
                        //column.Unique = true;
                    }
                    dtCRAM.Columns.Add(column);
                }
            }

            //Créer les lignes de congés
            var TypesConges = BizTypeConge.GetAll();
            var j = -1;
            TypesConges.DefaultView.Sort = "Ordre";
            foreach (DataRow drConge in TypesConges.DefaultView.ToTable().Rows)
            {
                j++;
                var dr = dtCRAM.NewRow();
                dtCRAM.Rows.Add(dr);
                dtCRAM.Rows[j][0] = drConge["Libelle"];
            }

            //Remplir les lignes de congés
            var totalConges = new Single();
            foreach (DataRow dr in dtConges.Rows.Cast<DataRow>().Where(r => r["Statut"].ToString() != "2"))
            {
                var dateFin = ((DateTime)dr["DateFin"]).Month > mois ?
                    new DateTime(annee, mois, nbrJours) :
                    Convert.ToDateTime(dr["DateFin"]);

                var dateDebut = Convert.ToDateTime(dr["DateDebut"]);
                //var dateFin = Convert.ToDateTime(dr["DateFin"]);
                var debutMA = dr["DebutMA"].ToString();
                var finMA = dr["FinMA"].ToString();
                var typeConge = (Convert.ToInt32(dr["Ordre"])) - 1;

                if (dateDebut.Month == dateFin.Month)
                {
                    for (int i = dateDebut.Day; i <= dateFin.Day; i++)
                    {
                        if (dtCRAM.Columns[i].DefaultValue.ToString() == "H")
                            continue;
                        if ((i == dateDebut.Day && debutMA == "AM") || (i == dateFin.Day && finMA == "M"))
                            dtCRAM.Rows[typeConge][i] = "0,5";
                        else
                            dtCRAM.Rows[typeConge][i] = 1;
                    }
                }
                else if (dateDebut < new DateTime(annee, mois, 1))
                {
                    for (int i = 1; i <= dateFin.Day; i++)
                    {
                        if (dtCRAM.Columns[i].DefaultValue.ToString() == "H")
                            continue;
                        if ((i == dateDebut.Day && debutMA == "AM") || (i == dateFin.Day && finMA == "M"))
                            dtCRAM.Rows[typeConge][i] = "0,5";
                        else
                            dtCRAM.Rows[typeConge][i] = 1;
                    }
                }

                dtCRAM.Rows[typeConge]["Total"] = Convert.ToSingle(dtCRAM.Rows[typeConge]["Total"]
                                                == DBNull.Value ? 0 : dtCRAM.Rows[typeConge]["Total"])
                                                + Convert.ToSingle(dr["NbrJours"]);
                totalConges += Convert.ToSingle(dr["NbrJours"]);
            }

            //Créer et remplir les lignes d'activités
            dtActivites = bizActivite.GetByCollaborateur_Mois(numeroCollaborateur, mois, annee);
            //var drs = dtActivites.Select("Type = 'Mission'");
            nbrActivites = dtActivites.Rows.Count;
            j = -1;
            Single ActivitesMission = 0, ExtrasMission = 0, totalActivites = 0, totalExtra = 0;
            var totalHours = new List<ExtraWorkEntity>();
            for (int i = 0; i < nbrActivites; i++)
            {
                var numeroActivite = Convert.ToInt64(dtActivites.Rows[i]["NumeroActivite"]);
                dtExtraWork = bizExtraWork.GetByActivite(numeroActivite, mois, annee);
                var dateDebutMission = Convert.ToDateTime(dtActivites.Rows[i]["DateDebut"]);

                var dateFinMission = dtActivites.Rows[i]["DateFin"] == DBNull.Value ?
                    new DateTime(annee, mois, nbrJours) :
                    Convert.ToDateTime(dtActivites.Rows[i]["DateFin"]);

                var dr = dtCRAM.NewRow();
                ActivitesMission = 0; ExtrasMission = 0;
                dtCRAM.Rows.InsertAt(dr, i);
                j++;
                //Cas de missions facturables
                if (dtActivites.Rows[i]["TypeActivite"].ToString() == "Mission")
                {
                    dtCRAM.Rows[j][0] = "Jours facturables à " + dtActivites.Rows[i]["NomSociete"];
                }
                //Cas d'intercontrat ou de travaux internes
                else
                {
                    dtCRAM.Rows[j][0] = dtActivites.Rows[i]["TypeActivite"];
                }

                //Détermination des jours début et fin
                int jourDebut = 1, jourFin = nbrJours;
                if (dateDebutMission.Month == mois && dateDebutMission.Year == annee)
                {
                    jourDebut = dateDebutMission.Day;
                    jourFin = dateFinMission < new DateTime(annee, mois, nbrJours) ? dateFinMission.Day : nbrJours;
                }
                else if (dateDebutMission < new DateTime(annee, mois, 1))
                {
                    jourFin = dateFinMission < new DateTime(annee, mois, nbrJours) ? dateFinMission.Day : nbrJours;
                    jourDebut = 1;
                }
                for (int day = jourDebut; day <= jourFin; day++)
                {
                    Single extra = 0;
                    if (dtCRAM.Columns[day].DefaultValue.ToString() == "H")
                    {
                        extra = GetWorkedHoliday(dtExtraWork, numeroActivite, new DateTime(annee, mois, day));
                        if (extra != 0)
                        {
                            dtCRAM.Rows[j][day] = extra;
                            ExtrasMission += extra;
                            //totalActivite += extra;
                        }
                        continue;
                    }
                    var activite = 1 - CongesCeJour(day, i + 1);
                    dtCRAM.Rows[j][day] = activite != 0 ? activite.ToString() : String.Empty;
                    ActivitesMission += activite;
                }

                dtCRAM.Rows[j]["Total"] = ActivitesMission + ExtrasMission;
                totalHours.AddRange(GetExtraHours(dtExtraWork, numeroActivite, new DateTime(annee, mois, 1), new DateTime(annee, mois, nbrJours)));
                totalExtra += ExtrasMission;
                totalActivites += ActivitesMission;
            }

            ds.Tables.Add(dtConges.Copy());
            ds.Tables[0].TableName = "Conges";
            ds.Tables.Add(dtCRAM.Copy());
            ds.Tables.Add(dtActivites.Copy());
            ds.Tables[2].TableName = "Activites";
            ds.Tables.Add(dtExtraWork.Copy());
            ds.Tables[3].TableName = "ExtraWork";

            //cramFicheEntity.Periode = new DateTime(annee, mois, 1);
            cramFicheEntity.Donnees = ds;
            cramFicheEntity.TotalJoursTravailles = totalActivites;
            cramFicheEntity.TotalExtraTravailles = totalExtra;
            cramFicheEntity.HeuresSupplementaires = totalHours;
            cramFicheEntity.TotalConges = totalConges;
            //cramFicheEntity.Cram = GetCram(numeroCollaborateur, new DateTime(annee, mois, 1));
            var dv = dtActivites.DefaultView;
            dv.Sort = "DateDebut ASC";
            foreach (DataRowView dr in dv)
            {
                if (!String.IsNullOrEmpty(cramFicheEntity.Client))
                    cramFicheEntity.Client += " puis ";
                cramFicheEntity.Client += dr["NomSociete"].ToString();              
            }
            return cramFicheEntity;
        }

        public List<CramFicheEntity> GetFichesCRAM(int mois, int annee)
        {
            var listFiches = new List<CramFicheEntity>();
            var bizCollaborateur = new BizCollaborateur();
            var consultants = bizCollaborateur.GetCollaborateurByProfil(ProfilEnum.Consultant);
            foreach (var consultant in consultants)
            {
                var cramFicheEntity = GetFicheCRAM(consultant.NumeroCollaborateur, mois, annee);
                listFiches.Add(cramFicheEntity);
            }
            return listFiches;
        }

        public List<CramFicheEntity> GetFichesCRAM(DateTime Periode, List<EtatCramEnum> Etats)
        {
            var listFiches = new List<CramFicheEntity>();
            var crams = GetCrams(Periode, Etats);
            foreach (var cram in crams)
            {
                var cramFicheEntity = GetFicheCRAM(cram.Collaborateur, Periode.Month, Periode.Year);
                listFiches.Add(cramFicheEntity);
            }
            return listFiches;
        }

        public List<CramEntity> GetCrams(DateTime Periode)
        {
            var listCrams = new List<CramEntity>();
            daoCram = new DaoCram();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Periode", Value= Periode},
                });
            listCrams = daoCram.PsSelectEntities(Params);

            return listCrams;
        }

        public List<CramEntity> GetCrams(DateTime Periode, List<EtatCramEnum> Etats)
        {
            var Params = new List<Filtre>();
            if (Etats.Contains(EtatCramEnum.Justifié))
            {
                Params.Add(new Filtre { Name = "@Justificatif", Value = 1 });
                Etats.Remove(EtatCramEnum.Justifié);
            }

            var sBuilder = new System.Text.StringBuilder();
            var i = 0;
            Etats.ForEach(p =>
            {
                sBuilder.Append((int)p);
                i++;
                if (i < Etats.Count)
                    sBuilder.Append(",");
            });

            var listCrams = new List<CramEntity>();
            daoCram = new DaoCram();
            if (Etats.Count == 0)
                Params.Add(new Filtre { Name = "@ListeEtats", Value = DBNull.Value });
            else
                Params.Add(new Filtre { Name = "@ListeEtats", Value = sBuilder.ToString() });

            Params.AddRange(
                new[] {
                        new Filtre { Name = "@Periode", Value= Periode},
                });
            listCrams = daoCram.PsSelectEntities(Params);

            return listCrams;
        }

        public CramEntity GetCram(int numeroCollaborateur, DateTime Periode)
        {
            daoCram = new DaoCram();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Collaborateur", Value = numeroCollaborateur},
                        new Filtre { Name = "@Periode", Value = Periode}
                });
            var cramEntity = daoCram.PsSelectEntities(Params);
            if (cramEntity.Count == 0)
            {
                logger.Error("Le collaborateur numéro : " + numeroCollaborateur + " n'admets pas de cram pour cette période : " + Periode.ToString());
                return null;
            }
            return cramEntity[0];
        }

        #region privates

        private Single CongesCeJour(int day, int nbrActivitesActuel)
        {
            Single nbrConges = 0;
            for (int k = nbrActivitesActuel; k < dtCRAM.Rows.Count; k++)
            {
                nbrConges += dtCRAM.Rows[k][day] == DBNull.Value ? 0 : Convert.ToSingle(dtCRAM.Rows[k][day]);
            }
            return nbrConges;
        }

        private int GetLigneTypeConge(int typeConge)
        {
            return typeConge + nbrActivites - 1;
        }

        private bool IsHoliday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Single GetWorkedHoliday(DataTable dt, Int64 numeroActivite, DateTime day)
        {
            var nbr = new Single();
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToDateTime(dr["DateExtra"]) == day && Convert.ToSingle(dr["day"]) != 0)
                    nbr = Convert.ToSingle(dr["day"]);
            }
            return nbr;
        }

        private List<ExtraWorkEntity> GetExtraHours(DataTable dt, Int64 numeroActivite, DateTime dateDebut, DateTime dateFin)
        {
            var list = new List<ExtraWorkEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToDateTime(dr["DateExtra"]) >= dateDebut && Convert.ToDateTime(dr["DateExtra"]) <= dateFin && Convert.ToSingle(dr["hours"]) != 0)
                    list.Add(new ExtraWorkEntity()
                    {
                        NumeroActivite = numeroActivite,
                        DateExtra = Convert.ToDateTime(dr["DateExtra"]),
                        Hours = Convert.ToSingle(dr["hours"]),
                        Remarques = dr["Remarques"].ToString()
                    });
            }
            return list;
        }

        #endregion
    }
}