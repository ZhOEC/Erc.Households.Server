using System;
using Microsoft.AspNetCore.Http;

namespace Erc.Households.Server.Api.Requests
{
    public class NewPaymentBatch
    {
        public int BranchOfficeid { get; set; }
        public int PaymentChannelId { get; set; }
        public DateTime DateComing { get; set; }
        public IFormFile UploadFile { get; set; }
    }
}