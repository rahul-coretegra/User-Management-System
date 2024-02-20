
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using User_Management_System.ManagementConfigurations;

using static User_Management_System.AuthorizationResources.AuthorizationRequirements;

namespace User_Management_System.AuthorizationResources
{
    public class SupremeAccessAuthorizationHandler : AuthorizationHandler<SupremeAccessRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        public SupremeAccessAuthorizationHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SupremeAccessRequirement requirement)
        {

            var userInDb = _dbContext.SupremeUsers.FirstOrDefault(x => x.UniqueId == context.User.FindFirstValue(ClaimTypes.SerialNumber));
            if (context.User.HasClaim(c => c.Type == "SupremeAcces" && bool.TryParse(c.Value, out bool supremeAcces) && supremeAcces) && userInDb.SupremeAccess == true)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}