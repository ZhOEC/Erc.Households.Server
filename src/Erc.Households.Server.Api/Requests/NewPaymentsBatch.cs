using System;
using Microsoft.AspNetCore.Http;

namespace Erc.Households.Api.Requests
{
    public class NewPaymentsBatch
    {
        public int BranchOfficeId { get; set; }
        public int PaymentChannelId { get; set; }
        public DateTime IncomingDate { get; set; }
        public IFormFile UploadFile { get; set; }
    }
}