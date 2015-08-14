using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAO;
using DAO.Cache;
using EntityModele;
using EntityModele.Criteres;
using EntityModele.Enums;
using System.Xml.Linq;

namespace BIZ
{
    public class BizCollaborateur
    {
        private DaoCollaborateur daoCollaborateur;

        public CollaborateurEntity GetCollaborateurByNumero(int Numero)
        {
            daoCollaborateur = new DaoCollaborateur();
            //var dt = daoCollaborateur.GetByNumero(Numero);
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@NumeroCollaborateur", Value = Numero},
                });
            var collaborateurs = daoCollaborateur.PsSelectEntities(Params);
            if (collaborateurs != null)
            {
                var collaborateur = collaborateurs.FirstOrDefault();
                //Charger les droits
                collaborateur.DroitPage = BizDroit.Instance.GetDroitsPage(collaborateur.Profil);
                collaborateur.DroitFonctionnalite = BizDroit.Instance.GetDroitsFonctionnalite(collaborateur.Profil);
                return collaborateur;
            }
            else
            {
                throw new Exception("Collaborateur inexistant : " + Numero);
            }
        }

        public List<CollaborateurEntity> GetAll()
        {
            daoCollaborateur = new DaoCollaborateur();
            var dt = daoCollaborateur.PsSelectEntities();
            return dt;
        }

        public CollaborateurEntity GetCollaborateurByUsername(string userName)
        {
            daoCollaborateur = new DaoCollaborateur();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Username", Value = userName},
                });
            var collaborateurs = daoCollaborateur.PsSelectEntities(Params);
            var collaborateur = collaborateurs.FirstOrDefault();
            if (collaborateur != null)
            {
                //Charger les droits
                collaborateur.DroitPage = BizDroit.Instance.GetDroitsPage(collaborateur.Profil);
                collaborateur.DroitFonctionnalite = BizDroit.Instance.GetDroitsFonctionnalite(collaborateur.Profil);
                //General.CurrentUser = collaborateur;
                return collaborateur;
            }
            else
            {
                return null;
                //Log ("Collaborateur inexistant : " + userName);
            }
        }

        public List<CollaborateurEntity> GetCollaborateurByProfil(ProfilEnum profil)
        {
            daoCollaborateur = new DaoCollaborateur();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@Profil", Value = profil},
                });
            var listCollaborateur = daoCollaborateur.PsSelectEntities(Params);
            return listCollaborateur;
        }

        public List<CollaborateurEntity> GetByCriteres(List<Filtre> filtre)
        {
            daoCollaborateur = new DaoCollaborateur();
            var listCollaborateur = daoCollaborateur.PsSelectEntities(filtre);
            return listCollaborateur;
        }

        public void UpdateEmails(int numeroCollaborateur, string emails)
        {
            daoCollaborateur = new DaoCollaborateur();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@NumeroCollaborateur", Value = numeroCollaborateur},
                        new Filtre { Name = "@Emails", Value = emails},
                });
            //daoCollaborateur.Update(numeroCollaborateur, "Emails", emails);
            daoCollaborateur.PsUpdate(Params);
        }

        public void UpdatePassword(int numeroCollaborateur, string pwd)
        {
            daoCollaborateur = new DaoCollaborateur();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@NumeroCollaborateur", Value = numeroCollaborateur},
                        new Filtre { Name = "@Password", Value = pwd},
                });
            daoCollaborateur.PsUpdate(Params);
        }

        public void Update(List<Filtre> Params)
        {
            if (Params == null) return;
            var paramComp = Params.Find(f => f.Name == "@Competences");
            if (paramComp != null)
            {
                var xEle = new XElement("competences",
                            from comp in paramComp.Value as List<CompetenceEntity>
                            select new XElement("comp",
                                    new XAttribute("lib", comp.LibelleCompetence),
                                    new XAttribute("niv", comp.NiveauCompetence)));
                paramComp.Value = xEle.ToString();
            }
            daoCollaborateur = new DaoCollaborateur();
            daoCollaborateur.PsUpdate(Params);
        }
    }
}