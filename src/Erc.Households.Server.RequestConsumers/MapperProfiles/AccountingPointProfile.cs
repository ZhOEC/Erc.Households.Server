using AutoMapper;
using Erc.Households.Backend.Data.AccountingPoints;
using Erc.Households.Backend.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Backend.RequestConsumers.MapperProfiles
{
    public class AccountingPointProfile: Profile
    {
        public AccountingPointProfile()
        {
            CreateMap<AccountingPoint, AccountingPoint>();
        }
    }
}
