using Microsoft.AspNetCore.Authorization;

namespace WebChapter.AspNetCore.MvcDemo.Security
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        public int RequiredAge { get; }

        public AgeRequirement(int requiredAge)
        {
            RequiredAge = requiredAge;
        }
    }
}
