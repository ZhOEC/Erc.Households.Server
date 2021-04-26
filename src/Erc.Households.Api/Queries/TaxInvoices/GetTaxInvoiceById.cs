using MediatR;

namespace Erc.Households.Api.Queries.TaxInvoices
{
    public class GetTaxInvoiceById : IRequest<Requests.ExportTaxInvoice>
    {
        public int TaxInvoiceId { get; private set; }

        public GetTaxInvoiceById(int taxInvoiceId)
        {
            TaxInvoiceId = taxInvoiceId;
        }
    }
}
