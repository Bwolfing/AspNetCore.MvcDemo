using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;
using WebChapter.AspNetCore.MvcDemo.Models;

namespace WebChapter.AspNetCore.MvcDemo.Security
{
    public class AgeRequirementHandler : AuthorizationHandler<AgeRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AgeRequirementHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            ApplicationUser user = await _userManager.GetUserAsync(context.User);

            if (user == null)
            {
                context.Fail();
                return;
            }

            if (user.Age < requirement.RequiredAge)
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
    }
}
