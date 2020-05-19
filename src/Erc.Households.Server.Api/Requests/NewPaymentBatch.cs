using System;
using Microsoft.AspNetCore.Http;

namespace Erc.Households.Api.Requests
{
    public class NewPaymentBatch
    {
        public int BranchOfficeId { get; set; }
        public int PaymentChannelId { get; set; }
        public DateTime DateComing { get; set; }
        public IFormFile UploadFile { get; set; }
    }
}