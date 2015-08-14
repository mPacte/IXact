namespace EntityModele.Enums
{
    public enum TypeMessageEnum
    {
        NouveauCongeToConsultant = 0,
        ValidationCongeConsultant = 1,
        RejetCongeConsultant = 2,
        EnregistrementConge = 3,
        NouveauCongeToAdministratif = 4,
        ValidationCongeAdministratif = 5,
        RejetCongeAdministratif = 6,
        ModificationCongeConsultant = 7,
        ModificationCongeAdministratif = 8,
        NouveauCongeParAdministrationToConsultant = 9,
        NouveauCongeParAdministrationToAdministratif = 10,
        Confirmationcram = 11,
        AlertValidationCram = 12,
        AlertJustificatifCram = 13,
        AlertValidationEtJustificatif = 14,
        AlertDirection = 15
    }

    public enum Fonctionnalite
    {
        AnnulerDemandeCongé = 102,
        ModifierDemandeCongé = 101,
        ModifierDemandeCongéCollaborateur = 103,
        AnnulerDemandeCongéCollaborateur = 104
    }
}