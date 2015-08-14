using System;
using System.Collections.Generic;
using System.Data;
using DAO;
using EntityModele;
using EntityModele.Criteres;
using EntityModele.Enums;
using System.Linq;

namespace BIZ
{
    public class BizConge : BizBase
    {
        public DaoConge daoDemande { get; set; }

        public BizConge()
        {
            daoDemande = new DaoConge();
        }

        public void NouvelleDemande(CongeEntity Conge, out List<String> retourMsg, bool? parAdministration = null)
        {
            retourMsg = new List<String>();
            if (IsAllHoliday(Conge.DateDebut, Conge.DateFin))
            {
                retourMsg.Add("Le congé couvre en totalité des jours fériés");
                return;
            }

            if (IsSimilar(Conge.DateDebut, Conge.DebutMA, Conge.DateFin, Conge.FinMA, Conge.Demandeur.NumeroCollaborateur))
            {
                retourMsg.Add("Le congé comporte d'autres jours de congé demandés");
                return;
            }

            Conge.NbrJours = NbrJoursConge(Conge.DateDebut, Conge.DebutMA, Conge.DateFin, Conge.FinMA);
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@TypeConge", Value = Conge.TypeConge},
                        new Filtre { Name = "@DateDemande", Value = DateTime.Now},
                        new Filtre { Name = "@DateDebut", Value = Conge.DateDebut},
                        new Filtre { Name = "@Demandeur", Value = Conge.Demandeur.NumeroCollaborateur},
                        new Filtre { Name = "@DateFin", Value = Conge.DateFin},
                        new Filtre { Name = "@debutMA", Value = Conge.DebutMA},
                        new Filtre { Name = "@finMA", Value = Conge.FinMA},
                        (parAdministration == true) ? new Filtre { Name = "@Statut", Value = StatutEnum.Validée} :
                                                      new Filtre { Name = "@Statut", Value = StatutEnum.EnCours},
                        //new Filtre { Name = "@Remarques", Value = remarques},
                        new Filtre { Name = "@NbrJours", Value = Conge.NbrJours}
                });

            if (DateTime.MinValue != Conge.DateValidation)
                Params.Add(new Filtre { Name = "@DateDecision", Value = Conge.DateValidation });

            Conge.NumeroDemande = Convert.ToInt64(daoDemande.PsInsert(Params));

            if (parAdministration == true)
                NotificationManager.CurrentSession.NouveauCongeParAdministration(Conge);
            else
                NotificationManager.CurrentSession.NouveauConge(Conge);
        }

        public DataTable GetByDemandeur(int numeroCollaborateur)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Demandeur", Value = numeroCollaborateur},
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByDemandeur_Mois(int numeroCollaborateur, int mois, int annee)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Demandeur", Value = numeroCollaborateur},
                        new Filtre { Name = "@Mois", Value = mois},
                        new Filtre { Name = "@Annee", Value = annee}
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByDate(String dateDebut, String dateFin)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@DateDemandeDebut", Value = Convert.ToDateTime(dateDebut)},
                        new Filtre { Name = "@DateDemandeFin", Value = Convert.ToDateTime(dateFin)}
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByResponsable(int numeroCollaborateur)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Responsable", Value = numeroCollaborateur},
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByResponsable(int numeroCollaborateur, String dateDebut, String dateFin)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Responsable", Value = numeroCollaborateur},
                        new Filtre { Name = "@DateDemandeDebut", Value = Convert.ToDateTime(dateDebut)},
                        new Filtre { Name = "@DateDemandeFin", Value = Convert.ToDateTime(dateFin)}
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByStatut(StatutEnum statut)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Statut", Value = statut},
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByStatut(StatutEnum statut, String dateDebut, String dateFin)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Statut", Value = statut},
                        new Filtre { Name = "@DateDemandeDebut", Value = Convert.ToDateTime(dateDebut)},
                        new Filtre { Name = "@DateDemandeFin", Value = Convert.ToDateTime(dateFin)}
                });
            return daoDemande.PsSelect(Params);
        }

        //public DataTable GetByResponsable_Statut(int numeroCollaborateur, StatutEnum statut)
        //{
        //    daoDemande = new DaoConge();
        //    var Params = new List<Filtre>(
        //        new[] {
        //                new Filtre { Name = "@Statut", Value = statut},
        //                new Filtre { Name = "@Responsable", Value = numeroCollaborateur},
        //        });
        //    return daoDemande.PsSelect(Params);
        //}

        public DataTable GetByEnregistreur_Statut(int numeroCollaborateur, StatutEnum statut)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Enregistreur", Value = numeroCollaborateur},
                        new Filtre { Name = "@Statut", Value = statut}
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByStatutsAvecResponsable(int numeroCollaborateur, List<StatutEnum> statuts)
        {
            var sBuilder = new System.Text.StringBuilder();
            var i = 0;
            statuts.ForEach(p =>
            {
                sBuilder.Append((int)p);
                i++;
                if (i < statuts.Count)
                    sBuilder.Append(",");
            });
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Responsable", Value = numeroCollaborateur},
                        new Filtre { Name = "@ListStatuts", Value = sBuilder.ToString()}
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByStatutsAvecResponsable(int numeroCollaborateur, List<StatutEnum> statuts, String dateDebut, String dateFin)
        {
            var sBuilder = new System.Text.StringBuilder();
            var i = 0;
            statuts.ForEach(p =>
            {
                sBuilder.Append((int)p);
                i++;
                if (i < statuts.Count)
                    sBuilder.Append(",");
            });
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Responsable", Value = numeroCollaborateur},
                        new Filtre { Name = "@ListStatuts", Value = sBuilder.ToString()},
                        new Filtre { Name = "@DateDemandeDebut", Value = dateDebut},
                        new Filtre { Name = "@DateDemandeFin", Value = dateFin}
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByStatuts(List<StatutEnum> statuts)
        {
            var sBuilder = new System.Text.StringBuilder();
            var i = 0;
            statuts.ForEach(p =>
            {
                sBuilder.Append((int)p);
                i++;
                if (i < statuts.Count)
                    sBuilder.Append(",");
            });
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@ListStatuts", Value = sBuilder.ToString()}
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByStatuts(List<StatutEnum> statuts, DateTime? dateDebut, DateTime? dateFin, String dateRecherche)
        {
            var sBuilder = new System.Text.StringBuilder();
            var i = 0;
            statuts.ForEach(p =>
            {
                sBuilder.Append((int)p);
                i++;
                if (i < statuts.Count)
                    sBuilder.Append(",");
            });
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@ListStatuts", Value = sBuilder.ToString()},
                        new Filtre { Name = "@DateDemandeDebut", Value = dateDebut},
                        new Filtre { Name = "@DateDemandeFin", Value = dateFin},
                        new Filtre { Name = "@DateRecherche", Value = dateRecherche}
                });
            return daoDemande.PsSelect(Params);
        }

        public DataTable GetByStatuts(List<StatutEnum> statuts, DateTime? dateDebut, DateTime? dateFin, int numeroCollaborateur, String dateRecherche)
        {
            var sBuilder = new System.Text.StringBuilder();
            var i = 0;
            statuts.ForEach(p =>
            {
                sBuilder.Append((int)p);
                i++;
                if (i < statuts.Count)
                    sBuilder.Append(",");
            });
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@ListStatuts", Value = sBuilder.ToString()},
                        new Filtre { Name = "@DateDemandeDebut", Value = dateDebut},
                        new Filtre { Name = "@DateDemandeFin", Value = dateFin},
                        new Filtre { Name = "@Demandeur", Value = numeroCollaborateur},
                        new Filtre { Name = "@DateRecherche", Value = dateRecherche}
                });
            return daoDemande.PsSelect(Params);
        }

        public void ValiderDemande(CongeEntity Conge)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Decideur", Value = Conge.Decideur},
                        new Filtre { Name = "@DateDecision", Value = DateTime.Now},
                        new Filtre { Name = "@numeroConge", Value = Conge.NumeroDemande},
                        new Filtre { Name = "@Statut", Value = StatutEnum.Validée},
                        new Filtre { Name = "@MotifDecision", Value = Conge.MotifDecision}
                });
            daoDemande.PsUpdate(Params);

            NotificationManager.CurrentSession.ValidationConge(Conge);

        }

        public void ModifierDemande(int numeroDemande, String motifDecision)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@NumeroConge", Value = numeroDemande},
                        new Filtre { Name = "@MotifDecision", Value = motifDecision}
                });
            daoDemande.PsUpdate(Params);
        }

        public bool ModifierDemande(CongeEntity Conge, out List<String> retourMsg)
        {
            retourMsg = new List<String>();
            var retour = false;
            if (IsAllHoliday(Conge.DateDebut, Conge.DateFin))
            {
                retourMsg.Add("Le congé couvre en totalité des jours fériés");
                return retour;
            }

            if (IsSimilar(Conge.DateDebut, Conge.DebutMA, Conge.DateFin, Conge.FinMA, Conge.Demandeur.NumeroCollaborateur, Conge.NumeroDemande))
            {
                retourMsg.Add("Le congé comporte d'autres jours de congé demandés !");
                return retour;
            }
            retour = true;
            var nbrJours = NbrJoursConge(Conge.DateDebut, Conge.DebutMA, Conge.DateFin, Conge.FinMA);
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@NumeroConge", Value = Conge.NumeroDemande},
                        new Filtre { Name = "@TypeConge", Value = Conge.TypeConge},
                        new Filtre { Name = "@DateDebut", Value = Conge.DateDebut},
                        new Filtre { Name = "@Demandeur", Value = Conge.Demandeur.NumeroCollaborateur},
                        new Filtre { Name = "@DateFin", Value = Conge.DateFin},
                        new Filtre { Name = "@debutMA", Value = Conge.DebutMA},
                        new Filtre { Name = "@finMA", Value = Conge.FinMA},
                        new Filtre { Name = "@NbrJours", Value = nbrJours}
                });
            daoDemande.PsUpdate(Params);

            NotificationManager.CurrentSession.Modificationconge(Conge);

            return true;
        }

        public void EnregistrerDemande(int numeroConge, int Enregistreur)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Enregistreur", Value = Enregistreur},
                        new Filtre { Name = "@numeroConge", Value = numeroConge},
                        new Filtre { Name = "@DateEnregistrement", Value = DateTime.Now},
                        new Filtre { Name = "@Statut", Value = StatutEnum.Enregistrée}
                });
            daoDemande.PsUpdate(Params);
        }

        public void RejeterDemande(CongeEntity Conge)
        {
            daoDemande = new DaoConge();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Decideur", Value = Conge.Decideur},
                        new Filtre { Name = "@numeroConge", Value = Conge.NumeroDemande},
                        new Filtre { Name = "@DateDecision", Value = DateTime.Now},
                        new Filtre { Name = "@Statut", Value = StatutEnum.Rejetée},
                        new Filtre { Name = "@MotifDecision", Value = Conge.MotifDecision}
                });
            daoDemande.PsUpdate(Params);

            NotificationManager.CurrentSession.RejetConge(Conge);
        }

        private bool IsSimilar(DateTime dateDebut, string debutMA, DateTime dateFin,
            string finMA, int Demandeur)
        {
            var bl = false;
            var dt = GetByDemandeur(Demandeur);
            foreach (DataRow dr in dt.Rows)
            {
                if ((dateDebut >= dr.Field<DateTime>("DateDebut") && dateDebut <= dr.Field<DateTime>("DateFin")
                    || dateFin <= dr.Field<DateTime>("DateFin") && dateFin >= dr.Field<DateTime>("DateDebut")
                    || dateDebut <= dr.Field<DateTime>("DateDebut") && dateFin >= dr.Field<DateTime>("DateFin"))
                    && dr["Statut"].ToString() != "2")
                {
                    bl = true;
                }

                if (dateDebut == dr.Field<DateTime>("DateFin") && debutMA == "AM" && dr.Field<String>("FinMA") == "M")
                {
                    bl = false;
                }
                if (dateFin == dr.Field<DateTime>("DateDebut") && debutMA == "M" && dr.Field<String>("DebutMA") == "AM")
                {
                    bl = false;
                }

                if (bl)
                    break;
            }
            return bl;
        }

        private bool IsSimilar(DateTime dateDebut, string debutMA, DateTime dateFin,
            string finMA, int Demandeur, long numeroDemande)
        {
            var bl = false;
            var dt = GetByDemandeur(Demandeur);
            var rows = dt.Select("NumeroDemande <> " + numeroDemande + " AND Statut <> 2");
            foreach (DataRow dr in rows)
            {
                if (dateDebut >= dr.Field<DateTime>("DateDebut") && dateDebut <= dr.Field<DateTime>("DateFin")
                    || dateFin <= dr.Field<DateTime>("DateFin") && dateFin >= dr.Field<DateTime>("DateDebut")
                    || dateDebut <= dr.Field<DateTime>("DateDebut") && dateFin >= dr.Field<DateTime>("DateFin"))
                {
                    bl = true;
                }

                if (dateDebut == dr.Field<DateTime>("DateFin") && debutMA == "AM" && dr.Field<String>("FinMA") == "M")
                {
                    bl = false;
                }
                if (dateFin == dr.Field<DateTime>("DateDebut") && debutMA == "M" && dr.Field<String>("DebutMA") == "AM")
                {
                    bl = false;
                }

                if (bl)
                    break;
            }
            return bl;
        }

        /// <summary>
        /// Vérifie si le congés demandé ne couvre pas en totalité des jours fériés
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <returns></returns>
        private bool IsAllHoliday(DateTime dateDebut, DateTime dateFin)
        {
            var bl = true;
            foreach (DateTime day in General.EachDay(dateDebut, dateFin))
            {
                if (!BizHoliday.Instance.IsHoliday(day))
                {
                    bl = false;
                    break;
                }
            }
            return bl;
        }

        /// <summary>
        /// Retourne le nombre de jours de congé pris pour une demande
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <param name="debutMA"></param>
        /// <param name="dateFin"></param>
        /// <param name="finMA"></param>
        /// <returns></returns>
        public Double NbrJoursConge(DateTime dateDebut, string debutMA, DateTime dateFin, string finMA)
        {
            Double nbr = (dateFin - dateDebut).Days + 1;
            if (debutMA == "AM")
                nbr = nbr - 0.5;
            if (finMA == "M")
                nbr = nbr - 0.5;

            foreach (DateTime day in General.EachDay(dateDebut, dateFin))
            {
                if (BizHoliday.Instance.IsHoliday(day))
                {
                    if ((day == dateDebut && debutMA == "AM") || (day == dateFin && finMA == "M"))
                        nbr = nbr - 0.5;
                    else
                        nbr--;
                }
            }

            return nbr;
        }
    }
}