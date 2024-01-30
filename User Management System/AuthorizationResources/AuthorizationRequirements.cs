using Microsoft.AspNetCore.Authorization;

namespace User_Management_System.AuthorizationResources
{
    public class AuthorizationRequirements
    {
        public class SupremeLevelRequirement : IAuthorizationRequirement { }

        public class AuthorityLevelRequirement : IAuthorizationRequirement { }

        public class IntermediateLevelRequirement : IAuthorizationRequirement { }

        public class SecondaryLevelRequirement : IAuthorizationRequirement { }

        public class IsAccssRequirement : IAuthorizationRequirement { }

    }
}
