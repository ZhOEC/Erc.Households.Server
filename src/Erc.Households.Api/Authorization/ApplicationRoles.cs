﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.Authorization
{
    public static class ApplicationRoles
    {
        public const string User = "User";
        public const string Operator = "Operator";
        public const string BranchOfficeEngineer = "BranchOfficeEngineer";
        public const string Administrator = "Administrator";
        
        public static IEnumerable<string> AsEnumerable()
        {
            yield return User;
            yield return Operator;
            yield return BranchOfficeEngineer;
            yield return Administrator;
        }
    }
}
