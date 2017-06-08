using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChapter.AspNetCore.MvcDemo.Security
{
    public static class AuthorizationPolicies
    {
        public const string EmployeeOnly = "EmployeeOnly";
        public const string Over21 = "Over21";
        public const string Over18 = "Over18";
    }
}
