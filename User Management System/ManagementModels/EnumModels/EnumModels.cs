namespace User_Management_System.ManagementModels.EnumModels
{
    public enum RoleLevels
    {
        Primary = 1,
        Secondary,
        Intermediate,
        Authority,
        SupremeLevel
    }
    public enum TrueFalse
    {
        True=1,
        False
    }

    public enum TypeOfDatabase
    {
        PostgreSql=1,
        MicrosoftSqlServer,
        MongoDb
    }
    public enum TypeOfService
    {
        AwsSimpleEmail = 1,
        AwsSimpleNotification,
        ElasticMailEmail,
        ElasticMailSms,
        ErrorHandler,
        NexmoSms,
        OutLookEmail,
        PostMarkEmail,
        TwilioSendGridEmail,
        TwilioSms
    }

}
