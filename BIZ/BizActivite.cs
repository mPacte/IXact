using System;
using System.Collections.Generic;
using System.Data;
using DAO;
using EntityModele;
using EntityModele.Criteres;

namespace BIZ
{
    public class BizActivite
    {
        private DaoActivite daoActivite;

        public List<ActiviteEntity> GetAll()
        {
            daoActivite = new DaoActivite();
            return daoActivite.PsSelectEntities();
        }

        public DataTable GetByCollaborateur(int numeroCollaborateur)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Collaborateur", Value = numeroCollaborateur},
                });

            daoActivite = new DaoActivite();
            return daoActivite.PsSelect(Params);
        }

        public DataTable GetByCommercial(int numeroCollaborateur)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@commercial", Value = numeroCollaborateur},
                });

            daoActivite = new DaoActivite();
            return daoActivite.PsSelect(Params);
        }

        public DataTable GetByClient(int numeroClient)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Client", Value = numeroClient},
                });

            daoActivite = new DaoActivite();
            return daoActivite.PsSelect(Params);
        }

        public DataTable GetByClient_Collaborateur(int numeroClient, int numeroCollaborateur)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Collaborateur", Value = numeroCollaborateur},
                        new Filtre { Name = "@Client", Value= numeroClient}
                });

            daoActivite = new DaoActivite();
            return daoActivite.PsSelect(Params);
        }

        public DataTable GetByMois(int mois)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@mois", Value = mois},
                });

            daoActivite = new DaoActivite();
            return daoActivite.PsSelect(Params);
        }

        public DataTable GetByCollaborateur_Mois(int numeroCollaborateur, int mois, int annee)
        {
            var dateDebut = new DateTime(annee, mois, 1);
            var dateFin = new DateTime(annee, mois, DateTime.DaysInMonth(annee, mois));

            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Collaborateur", Value = numeroCollaborateur},
                        new Filtre { Name = "@dateDebut", Value= dateDebut},
                        new Filtre { Name = "@dateFin", Value= dateFin},
                });

            daoActivite = new DaoActivite();
            return daoActivite.PsSelect(Params);
        }

        public List<ActiviteEntity> GetByCollaborateur_Today(int numeroCollaborateur, DateTime today)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Collaborateur", Value = numeroCollaborateur},
                        new Filtre { Name = "@dateDebut", Value= BizHoliday.Instance.NextWorkDay(today)}
                });

            daoActivite = new DaoActivite();
            return daoActivite.PsSelectEntities(Params);
        }

        public void Nouvelle(string TypeActivite, DateTime dateDebut, DateTime? dateFin, string PosteOccupe, int Collaborateur, 
            int Client, object numeroCommercial, String responsableClient, String telephone, String email, String projet)
        {
            daoActivite = new DaoActivite();
            var dateDemande = DateTime.Now;
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@TypeActivite", Value = TypeActivite},
                        new Filtre { Name = "@dateDebut", Value= dateDebut},
                        new Filtre { Name = "@dateFin", Value= dateFin},
                        new Filtre { Name = "@PosteOccupe", Value= PosteOccupe},
                        new Filtre { Name = "@Collaborateur", Value= Collaborateur},
                        new Filtre { Name = "@Client", Value= Client},
                        new Filtre { Name = "@responsableClient", Value= responsableClient},
                        new Filtre { Name = "@telephoneClient", Value= telephone},
                        new Filtre { Name = "@emailClient", Value= email},
                        new Filtre { Name = "@projet", Value= projet},
                });

            if (numeroCommercial != null && !string.IsNullOrEmpty(numeroCommercial.ToString()))
                Params.Add(new Filtre { Name = "@Commercial", Value = numeroCommercial });

            daoActivite.PsInsert(Params);
        }

        public void Modifier(int numeroActivite, string TypeActivite, DateTime dateDebut, DateTime? dateFin, string PosteOccupe, int Collaborateur,
            int Client, object numeroCommercial, String responsableClient, String telephone, String email, String projet)
        {
            daoActivite = new DaoActivite();
            var dateDemande = DateTime.Now;
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@numeroActivite", Value = numeroActivite},
                        new Filtre { Name = "@TypeActivite", Value = TypeActivite},
                        new Filtre { Name = "@dateDebut", Value= dateDebut},
                        new Filtre { Name = "@dateFin", Value= dateFin},
                        new Filtre { Name = "@PosteOccupe", Value= PosteOccupe},
                        new Filtre { Name = "@Collaborateur", Value= Collaborateur},
                        new Filtre { Name = "@Client", Value= Client},
                        new Filtre { Name = "@responsableClient", Value= responsableClient},
                        new Filtre { Name = "@telephoneClient", Value= telephone},
                        new Filtre { Name = "@emailClient", Value= email},
                        new Filtre { Name = "@projet", Value= projet},
                });

            if (numeroCommercial != null && !string.IsNullOrEmpty(numeroCommercial.ToString()))
                Params.Add(new Filtre { Name = "@Commercial", Value = numeroCommercial });

            daoActivite.PsUpdate(Params);
        }

        public List<ActiviteEntity> GetByCriteres(Object statuts, Object numeroClient, Object numeroCollaborateur, Object dateDebut, Object dateFin)
        {
            var Params = new List<Filtre>();

            if(numeroClient.ToString() != "") Params.Add(new Filtre { Name = "@Client", Value = numeroClient});
            if(numeroCollaborateur.ToString() != "") Params.Add(new Filtre { Name = "@Collaborateur", Value = numeroCollaborateur});
            if(dateDebut != "") Params.Add(new Filtre { Name = "@dateDebut", Value= dateDebut});
            if (dateFin != "") Params.Add(new Filtre { Name = "@dateFin", Value = dateFin });
            if (statuts != "") Params.Add(new Filtre { Name = "@statuts", Value = statuts });    

            daoActivite = new DaoActivite();
            return daoActivite.PsSelectEntities(Params);

        }

        public List<ActiviteEntity> GetByCriteres(List<Filtre> filtre)
        {
            daoActivite = new DaoActivite();
            var listCollaborateur = daoActivite.PsSelectEntities(filtre);
            return listCollaborateur;
        }
    }
}