using System;
using System.Linq;
using System.Net.Mail;
using EntityModele.Enums;
using System.Net;

namespace EntityModele
{
    public class MessageEntity : BaseEntity
    {
        public TypeMessageEnum Type { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Sujet { get; set; }

        public string Body { get; set; }

        //public DateTime DebutConge { get; set; }

        //public DateTime FinConge { get; set; }

        //public CollaborateurEntity Demandeur { get; set; }

        //public Double NbreJoursDemandes { get; set; }

        public MessageEntity(TypeMessageEnum typeMessage)
        {
            Type = typeMessage;
        }
        public MessageEntity()
        {

        }

    }
}