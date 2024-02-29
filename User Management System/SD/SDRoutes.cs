namespace User_Management_System.SD
{
    public static class SDRoutes
    {
        public const string baseUrl = "api";

        public const string GetAll = "getall";

        public const string Get = "get";

        public const string Create = "create";

        public const string Update = "update";

        public const string Delete = "delete";

        public const string Upsert = "upsert";

        public const string GetBy = "getby";

        public const string GetByRoleId = GetBy +"roleid";

        public const string GetByParentId = GetBy + "parentid";

        public const string Authenticate = "authenticate";

        public const string Register = "register";

        public const string UpSertRoleAccess = Upsert + "roleaccess";

        public const string UpSertUserAccess = Upsert + "useraccess";



        public const string UserManagement = baseUrl + "/user";

        public const string ActivateDeactivateUser = "activatedeactivateuser";


        public const string UserRoleManagement = baseUrl + "/userrole";

        public const string RouteManagement = baseUrl + "/route";

        public const string RoleAndAccessManagement = baseUrl + "/roleandaccess";



        public const string MenuManagement = baseUrl + "/menu";

        public const string MenusWithSubMenus = Get + "menuswithsubmenus";

        public const string RoleAndMenusManagement = baseUrl + "/roleandmenus";

        public const string ServiceConfigurationManagement = baseUrl + "/serviceconfiguration";





        public const string ProjectManagement = baseUrl + "/project";

        public const string SupremeUserManagement = baseUrl + "/supremeuser";

        public const string ItemManagement = baseUrl + "/item";

        public const string ServiceManagement = baseUrl + "/service";











    }
}
