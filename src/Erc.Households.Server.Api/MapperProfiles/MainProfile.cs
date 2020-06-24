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
			CreateMap<Domain.Payments.PaymentsBatch, Responses.PaymentsBatch>()
				.ForMember(x => x.TotalAmount, x => x.MapFrom(y => y.Payments.Sum(t => t.Amount)))
				.ForMember(x => x.TotalCount, x => x.MapFrom(y => y.Payments.Count()));

			CreateMap<Domain.Billing.Invoice, Responses.Invoice>()
				.ForMember(i => i.Type, s => s.MapFrom(_ => "Звичайний"));
			CreateMap<Domain.AccountingPoints.AccountingPointExemption, Responses.AccountingPointExemption>();
			//CreateMap<Domain.Billing.Usage, Responses.Usage>();
				//.ForMember(d=>d.Calculations, opt=>opt.Ignore());
				//.AfterMap((s, d) => d.CalculationItems = s.Calculations
				//));
			//CreateMap<Domain.Billing.UsageCalculation, Responses.UsageCalculation>();
		}
	}
}
