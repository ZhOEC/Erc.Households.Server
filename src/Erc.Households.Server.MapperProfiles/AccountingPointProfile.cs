using AutoMapper;
using Erc.Households.Backend.Responses;
using Erc.Households.Server.Domain.AccountingPoints;

namespace EErc.Households.Server.MapperProfiles
{
    public class AccountingPointProfile : Profile
    {
        public AccountingPointProfile()
        {
            CreateMap<AccountingPoint, AccountingPointListItem>()
                .ForMember(li => li.Address, a => a.MapFrom(o => $"{o.Address.Street.City.Name} {o.Address.Street.Name} {o.Address.Building} {(string.IsNullOrEmpty(o.Address.Apt) ? string.Empty : "кв. " + o.Address.Apt)}"))
                .ForMember(li => li.Owner, a => a.MapFrom(o => $"{o.Owner.LastName} {o.Owner.FirstName} {o.Owner.Patronymic}"))
                .ForMember(li => li.TariffName, a => a.MapFrom(o => o.CurrentTariff.Name))
                .ForMember(li => li.BranchOfficeName, a => a.MapFrom(o => o.BranchOffice.Name))
                ;
        }
    }
}
