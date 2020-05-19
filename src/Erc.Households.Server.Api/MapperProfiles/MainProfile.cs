using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.MapperProfiles
{
	public class MainProfile : Profile
	{
		public MainProfile()
		{
			CreateMap<Domain.AccountingPoints.AccountingPoint, Responses.AccountingPoint>();
			CreateMap<Domain.Person, Responses.Person>();
			CreateMap<Domain.BranchOffice, Responses.BranchOffice>();
		}
	}
}
