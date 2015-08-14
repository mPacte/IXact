using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EntityModele;
using EntityModele.Criteres;
using EntityModele.Enums;
using mPacte.Orm.DAL;
using System.Xml.Linq;

namespace DAO
{
    public class DaoCollaborateur : DaoBase<CollaborateurEntity>
    {
        public int GetNbrResponsabilites(int numero, DataTable dt)
        {
            //int nbr;
            return dt.AsEnumerable().Count(row => row.Field<int?>("Responsable") == numero);
            //var requete = "SELECT Count(*) FROM Collaborateur WHERE Responsable = " + numero;
            //nbr = Convert.ToInt32(OrmDal.Instance.DAL.ExecuteScalar(requete));
            //return nbr;
        }

        public override List<CollaborateurEntity> PsSelectEntities(List<Filtre> psParameters)
        {
            var dt = base.PsSelect(psParameters);
            var collaborateurs = new List<CollaborateurEntity>();
            var password = (psParameters.Count == 1 && psParameters[0].Name == "@Username");
            collaborateurs = LoadCollaborateurs(dt, password);
            return collaborateurs;
        }

        public override List<CollaborateurEntity> PsSelectEntities()
        {
            var dt = base.PsSelect();
            var collaborateurs = new List<CollaborateurEntity>();
            collaborateurs = LoadCollaborateurs(dt, false);
            return collaborateurs;
        }

        private List<CollaborateurEntity> LoadCollaborateurs(DataTable dt, bool password)
        {
            var collaborateurs = new List<CollaborateurEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                var collaborateur = new CollaborateurEntity();
                collaborateur.NumeroCollaborateur = Convert.ToInt16(dr["NumeroCollaborateur"]);
                collaborateur.Nom = dr["Nom"].ToString();
                collaborateur.Prenom = dr["Prenom"].ToString();
                collaborateur.NomComplet = collaborateur.Nom + " " + collaborateur.Prenom;
                collaborateur.Username = dr["Username"].ToString();
                collaborateur.Fonction = dr["Fonction"].ToString();
                collaborateur.Photo = !String.IsNullOrEmpty(dr["Photo"].ToString()) ? General.RepertoirePhotos + dr["Photo"].ToString() : null;
                collaborateur.CV = !String.IsNullOrEmpty(dr["CV"].ToString()) ? dr["CV"].ToString() : null;
                collaborateur.Adresse = dr["Adresse"].ToString();
                collaborateur.Telephone = dr["Telephone"].ToString();
                if(password) collaborateur.Password = dr["Password"].ToString();
                collaborateur.Niveau = dr["Niveau"].ToString();
                collaborateur.Situation = dr["Situation"].ToString();
                collaborateur.Diplome = dr["Diplome"].ToString();
                collaborateur.Statut = dr["Statut"].ToString();
                collaborateur.Etudes = dr["Etudes"].ToString();
                if (!Convert.IsDBNull(dr["DateEntree"]))
                    collaborateur.DateEntree = Convert.ToDateTime(dr["DateEntree"]);
                if (!Convert.IsDBNull(dr["DateNaissance"]))
                    collaborateur.DateNaissance = Convert.ToDateTime(dr["DateNaissance"]);
                collaborateur.LieuNaissance = dr["LieuNaissance"].ToString();
                collaborateur.Matricule = dr["Matricule"].ToString();
                collaborateur.Statut = dr["Statut"].ToString();
                collaborateur.Etudes = dr["Etudes"].ToString();
                if (!Convert.IsDBNull(dr["LastUpdateCv"]))
                    collaborateur.LastUpdateCV = Convert.ToDateTime(dr["LastUpdateCv"]);
                if (!Convert.IsDBNull(dr["DerniereConnexion"]))
                    collaborateur.DerniereConnexion = Convert.ToDateTime(dr["DerniereConnexion"]);

                //var emails = dr["Emails"].ToString().Split(';').Where(p => p != String.Empty).ToList();
                collaborateur.Emails = dr["Emails"].ToString(); // new List<Email>(emails.Count);
                //emails.ForEach(p => collaborateur.Emails.Add(new Email() { email = p }));

                collaborateur.Profil = new List<ProfilEnum>();
                //Charger le(s) profils
                collaborateur.Profil.Add((ProfilEnum)Enum.Parse(typeof(ProfilEnum), dr["Profil"].ToString()));
                if (GetNbrResponsabilites(collaborateur.NumeroCollaborateur, dt) > 0)
                {
                    //collaborateur.Profil.Add(Profil.Chef);
                    collaborateur.Profil = new List<ProfilEnum>(new[] { ProfilEnum.Chef });
                    collaborateur.IsResponsable = true;
                }
                var responsable = dr["Responsable"];
                if (responsable != DBNull.Value)
                {
                    collaborateur.Responsable = Convert.ToInt32(responsable);
                }
                else
                {
                    collaborateur.Responsable = null;
                }

                collaborateur.ActiviteActuelle = dr["ActiviteActuelle"].ToString();
                collaborateur.ClientActuel = dr["ClientActuel"].ToString();
                collaborateur.DateSortie = dr.Field<DateTime?>("DateSortie");
                collaborateur.CongeActuelDebut = dr.Field<DateTime?>("CongeActuelDebut");
                collaborateur.CongeActuelFin = dr.Field<DateTime?>("CongeActuelFin");
                collaborateur.SecteurClient = dr.Field<String>("Secteur");
                collaborateur.Competences = dr.Field<String>("Competences");

                collaborateur.CompetencesList = new List<CompetenceEntity>();
                var list = XElement.Parse(collaborateur.Competences).Descendants("comp").ToList();
                list.ForEach(elem =>
                    {
                        var comp = new CompetenceEntity();
                        comp.LibelleCompetence = elem.Attribute("lib").Value;
                        comp.NiveauCompetence = Convert.ToInt32(elem.Attribute("niv").Value);
                        if (!collaborateur.CompetencesList.Any(c => c.LibelleCompetence == comp.LibelleCompetence))
                            collaborateur.CompetencesList.Add(comp);
                    });
                collaborateurs.Add(collaborateur);
            }

            return collaborateurs;
        }
    }
}