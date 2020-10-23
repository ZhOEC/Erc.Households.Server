using MediatR;

namespace Erc.Households.Api.Queries.TaxInvoices
{
    public class GetTaxIonviceById : IRequest<Requests.DownloadTaxInvoice>
    {
        public int TaxInvoiceId { get; private set; }

        public GetTaxIonviceById(int taxInvoiceId)
        {
            TaxInvoiceId = taxInvoiceId;
        }
    }
}
