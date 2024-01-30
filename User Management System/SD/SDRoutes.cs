namespace User_Management_System.SD
{
    public static class SDRoutes
    {
        public const string baseUrl = "api";

        public const string UserRoles = baseUrl + "/userrole";
        public const string RoleAndAccess = baseUrl + "/roleandaccess";

        public const string UserManagement = baseUrl + "/usermanagement";

        public const string RegisterAndAuthenticate = baseUrl + "/registerandauthenticate";

        public const string Verification = baseUrl + "/verification";

        public const string Individuals = baseUrl + "/individual";




        public const string Get = "get";

        public const string GetUserByPhoneNumber = "getuserbyphonenumber";
        public const string GetAll = "getall";
        public const string GetUsers= "getusers";



        public const string Create = "create";

        public const string RegisterUser = "registeruser";
        public const string Authenticate = "authenticate";

        public const string CreateAndUpdateRoleAndAccess = "createandupdateroleandaccess";
        public const string CreateAndUpdateUserRoles = "createandupdateuserroles";


        public const string Update = "update";
        public const string UpdateUser = "updateuser";

        public const string ActivateDeactivateUser = "activatedeactivateuser";
        public const string RemoveAccessToRole = "removeaccesstorole";


        public const string UpdatePassword = "updatepassword";
        public const string ResetPassword = "resetpassword";

        public const string SendOtpUsingOutlookSmtp = "sendotpusingoutlooksmtp";
        public const string SendOtpUsingTwilio = "sendotpusingtwilio";

        public const string VerifyEmailsAndMessages = "verifyemailsandmessages";

        public const string Delete = "delete";

    }
}
