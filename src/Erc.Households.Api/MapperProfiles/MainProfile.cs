using AutoMapper;
using System.Linq;

namespace Erc.Households.Api.MapperProfiles
{
	public class MainProfile : Profile
	{
		public MainProfile()
		{
			CreateMap<Domain.AccountingPoints.AccountingPoint, Responses.AccountingPoint>()
				.ForMember(x => x.TariffId, x => x.MapFrom(y => y.CurrentTariff.Id))
				.ForMember(x => x.City, x => x.MapFrom(y => y.Address.Street.City))
				.ForMember(x => x.ContractStartDate, x => x.MapFrom(y => y.CurrentContract.StartDate));

			CreateMap<Domain.Addresses.Address, Responses.Address>()
				.ForMember(x => x.CityId, x => x.MapFrom(y => y.Street.City.Id));
			
			CreateMap<Domain.Person, Responses.Person>();
			
			CreateMap<Domain.BranchOffice, Responses.BranchOffice>();
			
			CreateMap<Domain.Payments.PaymentsBatch, Responses.PaymentsBatch>()
				.ForMember(x => x.TotalAmount, x => x.MapFrom(y => y.Payments.Sum(t => t.Amount)))
				.ForMember(x => x.TotalCount, x => x.MapFrom(y => y.Payments.Count()));
			
			CreateMap<Domain.Payments.Payment, Responses.Payment>()
				.ForMember(x => x.AccountingPointName, x => x.MapFrom(y => y.AccountingPoint.Name));
			
			CreateMap<Domain.AccountingPoints.UsageCategory, Responses.UsageCategory>();

			CreateMap<Domain.Billing.Invoice, Responses.Invoice>()
				.ForMember(i => i.Type, s => s.MapFrom(_ => "Звичайний"));
			
			CreateMap<Domain.AccountingPoints.AccountingPointExemption, Responses.AccountingPointExemption>()
				.ForMember(r=>r.CategoryName, mo=>mo.MapFrom(s=>$"{s.Category.Name} ({s.Category.Coeff:#.#}%)"));

			CreateMap<Domain.Taxes.TaxInvoice, Responses.TaxInvoice>(); 
			CreateMap<Domain.Taxes.TaxInvoice, Requests.DownloadTaxInvoice>()
				.ForMember(x => x.BranchOfficeId, x => x.MapFrom(y => y.BranchOffice.Id))
				.ForMember(x => x.BranchOfficeName, x => x.MapFrom(y => y.BranchOffice.Name))
				.ForMember(x => x.CompanyAddress, x => x.MapFrom(y => y.BranchOffice.Company.Address))
				.ForMember(x => x.CompanyStateRegistryCode, x => x.MapFrom(y => y.BranchOffice.Company.StateRegistryCode))
				.ForMember(x => x.CompanyTaxpayerNumber, x => x.MapFrom(y => y.BranchOffice.Company.TaxpayerNumber))
				.ForMember(x => x.CompanyTaxpayerPhone, x => x.MapFrom(y => y.BranchOffice.Company.TaxpayerPhone))
				.ForMember(x => x.CompanyBookkeeperName, x => x.MapFrom(y => y.BranchOffice.Company.BookkeeperName))
				.ForMember(x => x.CompanyBookkeeperTaxNumber, x => x.MapFrom(y => y.BranchOffice.Company.BookkeeperTaxNumber));

			CreateMap<Domain.Payments.Payment, Responses.AccountingPointPaymentListItem>()
				.ForMember(li => li.Source, s => s.MapFrom(p => p.Batch.PaymentChannel.Name))
				.ForMember(li => li.EnterDate, cfg => cfg.MapFrom(s => s.Batch.IncomingDate));
		}
	}
}
