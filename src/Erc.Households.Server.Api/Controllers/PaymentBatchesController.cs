using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dBASE.NET;
using Erc.Households.Domain.Helpers;
using Erc.Households.Domain.Payments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Api.Requests;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentBatchesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ErcContext _ercContext;

        public PaymentBatchesController(IWebHostEnvironment environment, ErcContext ercContext)
        {
            _hostingEnvironment = environment;
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _ercContext.PaymentBatches
                            .Select(x => new { paymentChannelName = _ercContext.PaymentChannels.Where(p => p.Id == x.ChannelId).Select(p => p.Name).First(), x.TotalAmount, x.TotalCount, x.IsClosed })
                            .ToListAsync()
                );

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] NewPaymentBatch paymentBatch)
        {
            var paymentChannel = _ercContext.PaymentChannels.Where(x => x.Id == paymentBatch.PaymentChannelId).FirstOrDefault();

            if (paymentChannel is null)
                return NotFound("Payment channel not found!");

            var paymentList = new List<Payment>();
            if (paymentBatch.UploadFile != null)
            {
                var extFile = Path.GetExtension(paymentBatch.UploadFile.FileName);
                if (extFile == ".dbf") paymentList = ParseFileDbf(paymentChannel, paymentBatch.UploadFile);
                else if (extFile == ".xls" || extFile == ".xlsx") return Ok("Excel files not yet supported");
            }

            _ercContext.PaymentBatches.Add(
                new PaymentBatch(
                    paymentBatch.PaymentChannelId,
                    paymentList
                )
            );
            await _ercContext.SaveChangesAsync();

            return Ok(paymentChannel);
        }

        public List<Payment> ParseFileDbf(PaymentChannel payChannel, IFormFile file)
        {
            var listPayment = new List<Payment>();
            var filePath = SaveFileToDisk(file);

            var dbf = new Dbf(Encoding.GetEncoding(866));
            dbf.Read(filePath);

            var recordList = payChannel.TotalRecord == FileTotalRow.Last
                ? dbf.Records.GetRange(0, dbf.Records.Count - 1)
                : dbf.Records.GetRange(Convert.ToInt16(payChannel.TotalRecord), dbf.Records.Count - Convert.ToInt16(payChannel.TotalRecord));

            foreach (DbfRecord record in recordList)
            {
                listPayment.Add(
                    new Payment(
                        DateTime.ParseExact(record[payChannel.DateFieldName].ToString(), payChannel.TextDateFormat, CultureInfo.InvariantCulture),
                        Convert.ToDecimal(record[payChannel.SumFieldName].ToString()),
                        1, //PeriodId for test set 1
                        string.Join(" ", payChannel.PersonFieldName.Split("+").Select(x => record[x]).ToList()),
                        _ercContext.AccountingPoints.Where(x => x.Name.Equals(record[payChannel.RecordpointFieldName.Trim()].ToString())).Select(x => x.Id).FirstOrDefault()
                    )
                );
            }

            System.IO.File.Delete(filePath);
            return listPayment;
        }

        private string SaveFileToDisk(IFormFile file)
        {
            var tempFolder = Directory.CreateDirectory($"{_hostingEnvironment.ContentRootPath}/Temp");
            var filePath = Path.Combine(tempFolder.FullName, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                file.CopyTo(fileStream);
            }

            return filePath;
        }
    }
}

