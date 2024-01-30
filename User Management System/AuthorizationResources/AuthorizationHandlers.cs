
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using User_Management_System.DbModule;
using User_Management_System.Models.EnumModels;
using static User_Management_System.AuthorizationResources.AuthorizationRequirements;

namespace User_Management_System.AuthorizationResources
{
    public class SupremeLevelAuthorizationHandler : AuthorizationHandler<SupremeLevelRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        public SupremeLevelAuthorizationHandler(ApplicationDbContext dbContext) 
        {
                _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SupremeLevelRequirement requirement)
        {

            var userRoleInClaim = context.User.FindFirst(ClaimTypes.Role);

            var supremeLevelRoles = _dbContext.UserRoles.ToList().Where(d => d.roleLevel == RoleLevels.SupremeLevel);

            if (userRoleInClaim != null && supremeLevelRoles.Any(role => role.roleUniqueCode == userRoleInClaim.Value))
            {
                context.Succeed(requirement); // Authorization requirement is met
            }
            return Task.CompletedTask;
        }
    }

    public class AuthorityLevelAuthorizationHandler : AuthorizationHandler<AuthorityLevelRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthorityLevelAuthorizationHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorityLevelRequirement requirement)
        {
            var userRoleInClaim = context.User.FindFirst(ClaimTypes.Role);

            var supremeLevelRoles = _dbContext.UserRoles.ToList().Where(d => d.roleLevel == RoleLevels.SupremeLevel || d.roleLevel == RoleLevels.Authority);

            if (userRoleInClaim != null && supremeLevelRoles.Any(role => role.roleUniqueCode == userRoleInClaim.Value))
            {
                context.Succeed(requirement); // Authorization requirement is met
            }
            return Task.CompletedTask;
        }
    }

    public class IntermediateLevelAuthorizationHandler : AuthorizationHandler<IntermediateLevelRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        public IntermediateLevelAuthorizationHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IntermediateLevelRequirement requirement)
        {
            var userRoleInClaim = context.User.FindFirst(ClaimTypes.Role);

            var supremeLevelRoles = _dbContext.UserRoles.ToList().Where(d => d.roleLevel == RoleLevels.SupremeLevel || d.roleLevel == RoleLevels.Authority || d.roleLevel ==RoleLevels.Intermediate);

            if (userRoleInClaim != null && supremeLevelRoles.Any(role => role.roleUniqueCode == userRoleInClaim.Value))
            {
                context.Succeed(requirement); // Authorization requirement is met
            }
            return Task.CompletedTask;
        }
    }

    public class SecondaryLevelAuthorizationHandler : AuthorizationHandler<SecondaryLevelRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        public SecondaryLevelAuthorizationHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SecondaryLevelRequirement requirement)
        {
            var userRoleInClaim = context.User.FindFirst(ClaimTypes.Role);

            var supremeLevelRoles = _dbContext.UserRoles.ToList().Where(d => d.roleLevel != RoleLevels.Primary);

            if (userRoleInClaim != null && supremeLevelRoles.Any(role => role.roleUniqueCode == userRoleInClaim.Value))
            {
                context.Succeed(requirement); // Authorization requirement is met
            }
            return Task.CompletedTask;
        }
    }
    public class IsAccssAuthorizationHandler : AuthorizationHandler<IsAccssRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsAccssAuthorizationHandler(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAccssRequirement requirement)
        {

            var RoleInClaim = context.User.FindFirst(ClaimTypes.Role);

            var CurrentRoute = _httpContextAccessor.HttpContext?.Request.Path;

            var RolesWithAccessToRead = _dbContext.RoleAndAccess.ToList().Where(d => d.routePath == CurrentRoute && d.isAccess == TrueFalse.True);

            if (RoleInClaim != null && RolesWithAccessToRead.Any(role => role.roleUniqueCode == RoleInClaim.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
