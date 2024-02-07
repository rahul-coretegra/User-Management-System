using Microsoft.AspNetCore.Authorization;

namespace User_Management_System.AuthorizationResources
{
    public class AuthorizationRequirements
    {
        public class SupremeAccessRequirement : IAuthorizationRequirement { }
    }
}
