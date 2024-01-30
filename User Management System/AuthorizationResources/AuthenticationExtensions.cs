using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using User_Management_System.SD;
using static User_Management_System.AuthorizationResources.AuthorizationRequirements;

namespace User_Management_System.AuthorizationResources
{
    public static class AuthenticationExtensions
    {
        public static void ConfigureJwtAuthentication(this IServiceCollection services, byte[] key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void ConfigureCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(SDPolicies.SupremeLevel, x =>
                {
                    x.RequireAuthenticatedUser();
                    x.AddRequirements(new SupremeLevelRequirement());
                });
                options.AddPolicy(SDPolicies.AuthorityLevel, x =>
                {
                    x.RequireAuthenticatedUser();
                    x.AddRequirements(new AuthorityLevelRequirement());
                });
                options.AddPolicy(SDPolicies.IntermediateLevel, x =>
                {
                    x.RequireAuthenticatedUser();
                    x.AddRequirements(new IntermediateLevelRequirement());
                });
                options.AddPolicy(SDPolicies.SecondaryLevel, x =>
                {
                    x.RequireAuthenticatedUser();
                    x.AddRequirements(new SecondaryLevelRequirement());
                });
                options.AddPolicy(SDPolicies.IsAccess, x =>
                {
                    x.RequireAuthenticatedUser();
                    x.AddRequirements(new IsAccssRequirement());
                });
            });
        }

    }
}
