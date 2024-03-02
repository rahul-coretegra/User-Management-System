namespace User_Management_System.SD
{
    public static class SDRoutes
    {
        public const string baseUrl = "api";

        public const string Management = baseUrl + "/management";

        public const string ProjectManagement = Management + "/project";

        public const string SupremeUserManagement = Management + "/supremeuser";

        public const string ConfigureServiceManagement = Management + "/configureservice";

        public const string ItemManagement = Management + "/item";

        public const string ServiceManagement = Management + "/service";



        public const string Project = baseUrl + "/project";

        public const string UserRole = Project + "/userrole";

        public const string Route = Project + "/route";

        public const string RouteAccess = Project + "/routeaccess";

        public const string Menu = Project + "/menu";

        public const string MenuAccess = Project + "/menuaccess";

        public const string Item = Project + "/item";

        public const string Service = Project + "/service";

        public const string ConfigureService = Project + "/configureservice";

        public const string User = baseUrl + "/user";

        public const string GetAll = "getall";

        public const string Get = "get";



        public const string MenusWithSubMenus = Get + "menuswithsubmenus";


        public const string Authenticate = "authenticate";

        public const string Register = "register";

        public const string Create = "create";


        public const string Update = "update";

        public const string Upsert = "upsert";

        public const string UpSertRoleAccess = Upsert + "roleaccess";

        public const string UpSertMenuAccess = Upsert + "menuaccess";

        public const string UpSertRouteAccess = Upsert + "routeaccess";

        public const string ActivateDeactivateUser = "activatedeactivateuser";

        public const string Delete = "delete";















    }
}
