using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityModele;
using EntityModele.Enums;
using System.Net.Mail;
using System.Globalization;

namespace BIZ
{
    public class NotificationManager
    {
        log4net.ILog logger = log4net.LogManager.GetLogger("NotificationManager");

        private NotificationManager()
        {

        }

        private static NotificationManager _currentSession;

        public static NotificationManager CurrentSession
        {
            get
            {
                if (_currentSession == null)
                    _currentSession = new NotificationManager();
                return _currentSession;
            }
        }

        public void Init()
        {
            _currentSession = new NotificationManager();
        }

        public void Notifier(CongeEntity Conge)
        {

        }

        public void NouveauConge(CongeEntity Conge)
        {
            var msg = SetNotification(Conge, TypeMessageEnum.NouveauCongeToConsultant);
            this.Envoyer(msg);

            msg = SetNotification(Conge, TypeMessageEnum.NouveauCongeToAdministratif);
            this.Envoyer(msg);
        }

        public void NouveauCongeParAdministration(CongeEntity Conge)
        {
            var msg = SetNotification(Conge, TypeMessageEnum.NouveauCongeParAdministrationToConsultant);
            this.Envoyer(msg);

            msg = SetNotification(Conge, TypeMessageEnum.NouveauCongeParAdministrationToAdministratif);
            this.Envoyer(msg);
        }

        public void Modificationconge(CongeEntity Conge)
        {
            var msg = SetNotification(Conge, TypeMessageEnum.ModificationCongeConsultant);
            this.Envoyer(msg);

            msg = SetNotification(Conge, TypeMessageEnum.ModificationCongeAdministratif);
            this.Envoyer(msg);
        }

        public void ValidationConge(CongeEntity Conge)
        {
            var msg = SetNotification(Conge, TypeMessageEnum.ValidationCongeConsultant);
            this.Envoyer(msg);

            msg = SetNotification(Conge, TypeMessageEnum.ValidationCongeAdministratif);
            this.Envoyer(msg);
        }

        public void RejetConge(CongeEntity Conge)
        {
            var msg = SetNotification(Conge, TypeMessageEnum.RejetCongeConsultant);
            this.Envoyer(msg);

            msg = SetNotification(Conge, TypeMessageEnum.RejetCongeAdministratif);
            this.Envoyer(msg);
        }

        public void ConfirmationCram(CramEntity Cram)
        {
            var msg = SetNotification(Cram, TypeMessageEnum.Confirmationcram);
            this.Envoyer(msg);
        }

        public void AlertValidationCram(CramEntity Cram)
        {
            var msg = SetNotification(Cram, TypeMessageEnum.AlertValidationCram);
            this.Envoyer(msg);
        }

        public void AlertJustificatifCram(CramEntity Cram)
        {
            var msg = SetNotification(Cram, TypeMessageEnum.AlertJustificatifCram);
            this.Envoyer(msg);
        }

        public void AlertValidationEtJustificatifCram(CramEntity Cram)
        {
            var msg = SetNotification(Cram, TypeMessageEnum.AlertValidationEtJustificatif);
            this.Envoyer(msg);
        }

        public void InformerDirectionAlertes(List<CramEntity> listAlerted)
        {
            var msg = SetNotification(listAlerted, TypeMessageEnum.AlertDirection);
            this.Envoyer(msg);
        }

        private MessageEntity SetNotification(CongeEntity Conge, TypeMessageEnum typeMessage)
        {
            var Message = new MessageEntity(typeMessage);
            //Message.NbreJoursDemandes = Conge.NbrJours;
            //Message.DebutConge = Conge.DateDebut;
            //Message.FinConge = Conge.DateFin;
            //Message.Demandeur = Conge.Demandeur;
            Message.From = "cra@diracet.com";
            switch (typeMessage)
            {
                case TypeMessageEnum.NouveauCongeToConsultant:
                    Message.To = Conge.Demandeur.Emails;
                    Message.Sujet = "Nouvelle demande de congés";
                    Message.Body = "Vous avez effectué une nouvelle demande de congé" + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.NbrJours + Environment.NewLine +
                            "Type de congé : " + Conge.TypeConge + Environment.NewLine +
                            "Elle sera traitée dans les plus brefs délais.";
                    break;
                case TypeMessageEnum.NouveauCongeToAdministratif:
                    General.Administratifs.ForEach(adm => Message.To += adm.Emails);
                    Message.Sujet = "Nouvelle demande de congés";
                    Message.Body = "Une nouvelle demande de congé a été enregistrée :" + Environment.NewLine +
                            "Congé N° : " + Conge.NumeroDemande + Environment.NewLine +
                            "Demandeur : " + Conge.Demandeur.NomComplet + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Type de congé : " + Conge.TypeConge + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.NbrJours;
                    break;
                case TypeMessageEnum.ModificationCongeConsultant:
                    Message.To = Conge.Demandeur.Emails;
                    Message.Sujet = "Modification demande de congés";
                    Message.Body = "Les modifications sur votre demande de congé ont été bien enregistrées, " +
                            "elles seront traitées dans les plus brefs délais :" + Environment.NewLine +
                            "**Anciennes dates : " + Environment.NewLine +
                            "Date début : " + Conge.Old.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.Old.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.Old.DateFin.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.Old.FinMA) + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.Old.NbrJours + Environment.NewLine +

                            "**Nouvelles dates : " + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.NbrJours;
                    break;
                case TypeMessageEnum.ModificationCongeAdministratif:
                    General.Administratifs.ForEach(adm => Message.To += adm.Emails);
                    Message.Sujet = "Modification demande de congés";
                    Message.Body = "Des modifications ont été apportées à une demande de congé :" + Environment.NewLine +
                            "Congé N° : " + Conge.NumeroDemande + Environment.NewLine +
                            "Demandeur : " + Conge.Demandeur.NomComplet + Environment.NewLine +
                            "**Anciennes dates : " + Environment.NewLine +
                            "Date début : " + Conge.Old.DateDebut.ToString("dd/MM/yyyy") + Environment.NewLine +
                            "Date fin : " + Conge.Old.DateFin.ToString("dd/MM/yyyy") + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.Old.NbrJours + Environment.NewLine +

                            "**Nouvelles date : " + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.NbrJours;
                    break;
                case TypeMessageEnum.ValidationCongeConsultant:
                    Message.To = Conge.Demandeur.Emails;
                    Message.Sujet = "Validation d'une demande de congés";
                    Message.Body = "Votre demande de congé a été validée :" + Environment.NewLine +
                            "Demandeur : " + Conge.Demandeur.NomComplet + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Type de congé : " + Conge.TypeConge + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.NbrJours;
                    break;
                case TypeMessageEnum.ValidationCongeAdministratif:
                    General.Administratifs.ForEach(adm => Message.To += adm.Emails);
                    Message.Sujet = "Validation d'une demande de congés";
                    Message.Body = "Vous venez de valider une demande de congé :" + Environment.NewLine +
                            "Congé N° : " + Conge.NumeroDemande + Environment.NewLine +
                            "Demandeur : " + Conge.Demandeur.NomComplet + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Type de congé : " + Conge.TypeConge + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.NbrJours;
                    break;
                case TypeMessageEnum.RejetCongeConsultant:
                    Message.To = Conge.Demandeur.Emails;
                    Message.Sujet = "Rejet d'une demande de congés";
                    Message.Body = "Votre demande de congé a été rejetée :" + Environment.NewLine +
                            "Demandeur : " + Conge.Demandeur.NomComplet + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Motif rejet : " + Conge.MotifDecision;
                    break;
                case TypeMessageEnum.RejetCongeAdministratif:
                    General.Administratifs.ForEach(adm => Message.To += adm.Emails);
                    Message.Sujet = "Rejet d'une demande de congés";
                    Message.Body = "Vous venez de rejeter une demande de congé :" + Environment.NewLine +
                            "Congé N° : " + Conge.NumeroDemande + Environment.NewLine +
                            "Demandeur : " + Conge.Demandeur.NomComplet + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Type de congé : " + Conge.TypeConge + Environment.NewLine +
                            "Motif rejet : " + Conge.MotifDecision;
                    break;
                case TypeMessageEnum.NouveauCongeParAdministrationToConsultant:
                    Message.To = Conge.Demandeur.Emails;
                    Message.Sujet = "Nouvelle demande de congés";
                    Message.Body = "Une nouvelle demande de congé / absence a été effectuée par l'administration pour vous :" + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.NbrJours + Environment.NewLine +
                            "Type de congé : " + Conge.TypeConge;
                    break;
                case TypeMessageEnum.NouveauCongeParAdministrationToAdministratif:
                    General.Administratifs.ForEach(adm => Message.To += adm.Emails);
                    Message.Sujet = "Nouvelle demande de congés";
                    Message.Body = "Une nouvelle demande de congé / absence a été effectuée par l'administration :" + Environment.NewLine +
                            "Congé N° : " + Conge.NumeroDemande + Environment.NewLine +
                            "Demandeur : " + Conge.Demandeur.NomComplet + Environment.NewLine +
                            "Date début : " + Conge.DateDebut.ToString("dd/MM/yyyy") + General.GetmaDebut(Conge.DebutMA) + Environment.NewLine +
                            "Date fin : " + Conge.DateFin.ToString("dd/MM/yyyy") + General.GetmaFin(Conge.FinMA) + Environment.NewLine +
                            "Type de congé : " + Conge.TypeConge + Environment.NewLine +
                            "Nombre de jours pris : " + Conge.NbrJours;
                    break;
            }

            Message.Body += General.SignatureEmail;
            return Message;
        }

        private MessageEntity SetNotification(CramEntity Cram, TypeMessageEnum typeMessage)
        {
            var Message = new MessageEntity();
            Message.From = "cra@diracet.com";
            var consultant = General.AllCollaborateurs.Find(c => c.NumeroCollaborateur == Cram.Collaborateur);
            var emails = consultant.Emails;
            switch (typeMessage)
            {
                case TypeMessageEnum.Confirmationcram:
                    Message.To = emails;
                    Message.Sujet = "Confirmation Rapport d'activités";
                    Message.Body = "Votre rapport d'activité a été bien validé." + Environment.NewLine +
                            "Consultant : " + consultant.NomComplet + Environment.NewLine +
                            "Période : " + Cram.Periode.ToString("MMMM", CultureInfo.CurrentCulture);
                    break;

                case TypeMessageEnum.AlertValidationCram:
                    Message.To = emails;
                    Message.Sujet = "Validation de votre Rapport d'activités";
                    Message.Body = consultant.NomComplet + "," + Environment.NewLine +
                                    "Merci de bien vouloir valider votre rapport d'activité pour le mois de " +
                                    Cram.Periode.ToString("MMMM", CultureInfo.CurrentCulture) + " " + Cram.Periode.Year;
                    break;

                case TypeMessageEnum.AlertValidationEtJustificatif:
                    Message.To = emails;
                    Message.Sujet = "Validation de votre Rapport d'activités";
                    Message.Body = consultant.NomComplet + "," + Environment.NewLine +
                                    "Merci de bien vouloir valider votre rapport d'activité et de joindre le justificatif pour le mois de " +
                                    Cram.Periode.ToString("MMMM", CultureInfo.CurrentCulture) + " " + Cram.Periode.Year;
                    break;

                case TypeMessageEnum.AlertJustificatifCram:
                    Message.To = emails;
                    Message.Sujet = "Validation de votre Rapport d'activités";
                    Message.Body = consultant.NomComplet + "," + Environment.NewLine +
                                    "Merci de bien vouloir de joindre le justificatif de votre rapport d'activité pour le mois de " +
                                    Cram.Periode.ToString("MMMM", CultureInfo.CurrentCulture) + " " + Cram.Periode.Year;
                    break;
            }

            Message.Body += General.SignatureEmail;
            return Message;
        }

        private MessageEntity SetNotification(List<CramEntity> Crams, TypeMessageEnum typeMessage)
        {
            var Message = new MessageEntity();
            Message.From = "cra@diracet.com";
            switch (typeMessage)
            {
                case TypeMessageEnum.AlertDirection:
                    Message.To = string.Join(";", General.Administratifs.Select(e => e.Emails));
                    Message.Sujet = "Relance validation CRAM";
                    Message.Body = "Une relance de validation de CRAM pour le mois de " + Crams.First().Periode.ToString("MMMM", CultureInfo.CurrentCulture)
                                    + " a été envoyé aux consultants suivants :" + Environment.NewLine +
                                    string.Join(Environment.NewLine + "-", Crams.Select(c => c.NomCollaborateur)) + Environment.NewLine;
                    break;

            }

            Message.Body += General.SignatureEmail;
            return Message;
        }

        private void Envoyer(MessageEntity Message)
        {
            try
            {
                if (String.IsNullOrEmpty(Message.To))
                {
                    //Log
                    return;
                }
                var msg = new MailMessage();
                msg.From = new MailAddress(Message.From);
                var listDestinataires = Message.To.Split(';').Where(p => p != String.Empty).ToList();
                listDestinataires.ForEach(p => msg.To.Add(new MailAddress(p)));

                msg.Body = Message.Body;
                msg.Subject = Message.Sujet;

                //var client = new SmtpClient();
                //client.Credentials = CredentialCache.DefaultNetworkCredentials;
                //client.EnableSsl = true;
                //client.Send(msg);

                var smtp = new SmtpClient("auth.smtp.1and1.fr");
#if DEBUG
                smtp.Port = 587;
#endif
                smtp.Credentials = new System.Net.NetworkCredential("cra@diracet.com", "Cra$2014");
                smtp.Send(msg);
                //<network host="auth.smtp.1and1.fr" port="587" userName="cra@diracet.com" password="Cra$2014"/>

                logger.Info("Un email a été envoyé à la liste suivante : " + msg.To);
                logger.Info("Sujet : " + msg.Subject);
                logger.Info("Corps : " + msg.Body);
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

    }
}
