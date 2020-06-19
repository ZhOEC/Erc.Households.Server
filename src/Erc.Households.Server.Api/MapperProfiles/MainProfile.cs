using AutoMapper;
using Erc.Households.Domain;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Payments;
using System.Linq;

namespace Erc.Households.Api.MapperProfiles
{
	public class MainProfile : Profile
	{
		public MainProfile()
		{
			CreateMap<AccountingPoint, Responses.AccountingPoint>();
			CreateMap<Person, Responses.Person>();
			CreateMap<BranchOffice, Responses.BranchOffice>();
			CreateMap<PaymentsBatch, Responses.PaymentsBatch>()
				.ForMember(x => x.TotalAmount, x => x.MapFrom(y => y.Payments.Sum(t => t.Amount)))
				.ForMember(x => x.TotalCount, x => x.MapFrom(y => y.Payments.Count()));
			CreateMap<Payment, Responses.Payment>()
				.ForMember(x => x.AccountingPointName, x => x.MapFrom(y => y.AccountingPoint.Name));
		}
	}
}
