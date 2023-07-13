using System.Security.Claims;
using ToolProgramCore.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace ToolProgramCore.Policies.Handlers
{
    public class AdminRequirementHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, AdminRequirement requirement)
        {

            // TODO see if I can get a certificate for more security
            //var dateOfBirthClaim = context.User.FindFirst(
            //    c => c.Type == ClaimTypes.DateOfBirth && c.Issuer == "http://contoso.com");

            string username = context.User.Identities.ElementAt(0).Name ?? "";


            foreach (string authorizedUser  in requirement.AuthorizedUsers)
            {
                if (username.Equals( "TTUSA\\" + authorizedUser))
                {
                    context.Succeed(requirement);
                }

            }
            

            return Task.CompletedTask;
        }
    }
}
