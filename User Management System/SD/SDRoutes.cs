namespace User_Management_System.SD
{
    public static class SDRoutes
    {
        public const string baseUrl = "api";


        public const string ProjectManagement = baseUrl + "/project";

        public const string Projects = "projects";

        public const string Project =  "project";

        public const string CreateProject =  "create";

        public const string UpadateProject =  "update";


        public const string SupremeUserManagement = baseUrl + "/supremeuser";

        public const string SupremeUsers = "supremeusers";

        public const string SupremeUser =  "supremeuser";

        public const string RegisterSupremeUser =  "create";

        public const string Authenticate =  "authenticate";

        public const string UpdateSupremeUser =  "update";


        public const string UserRoleManagement = baseUrl + "/userrole";

        public const string UserRoles = "userroles";

        public const string UserRole =  "userrole";

        public const string CreateUserRole =  "create";

        public const string UpadateUserRole =  "update";

        public const string DeleteUserRole = "delete";


        public const string RoleAndAccessManagement = baseUrl + "/roleandaccess";
        public const string Routes = "routes";
        public const string Route = "route";
        public const string CreateRoute = "create";
        public const string UpdateRoute = "update";
        public const string DeleteRoute = "delete";

        public const string AccessByRoleId = "accessbyroleid";
        public const string RolesAndAccesses =  "rolesandaccesses";
        public const string RoleAndAccess =  "roleandaccess";
        public const string UpsertRolesAndAccesses = "upsertrolesandaccesses";


        public const string RoleAndMenusManagement = baseUrl + "/roleandmenus";

        public const string Menus = "menus";
        public const string Menu = "menu";
        public const string MenusByParentId = "menusbyparentid";
        public const string CreateMenu = "create";
        public const string UpdateMenu = "update";
        public const string DeleteMenu = "delete";

        public const string MenusWithSubMenus = "menuswithsubmenus";

        public const string MenusByRoleId = "menusbyroleid";
        public const string RolesAndMenus = "rolesandmenus";
        public const string RoleAndMenu = "roleandmenu";
        public const string UpsertRolesAndMenus = "upsertrolesandmenus";



        public const string UserManagement = baseUrl + "/user";

        public const string Users ="users";

        public const string User = "user";

        public const string RegisterUser = "registeruser";

        public const string UpdateUser ="updateuser";

        public const string UpsertUserAndRoles =  "upsertuserandroles";

        public const string ActivateDeactivateUser =  "activatedeactivateuser";

    }
}
