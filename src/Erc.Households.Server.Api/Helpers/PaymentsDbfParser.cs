using dBASE.NET;
using Erc.Households.BranchOfficeManagment.EF;
using Erc.Households.Domain.Helpers;
using Erc.Households.Domain.Payments;
using Erc.Households.EF.PostgreSQL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Erc.Households.Api.Helpers
{
    public class PaymentsDbfParser
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ErcContext _ercContext;

        public PaymentsDbfParser(IWebHostEnvironment environment, ErcContext ercContext)
        {
            _hostingEnvironment = environment;
            _ercContext = ercContext;
        }

        public List<Payment> Parser(IFormFile file, PaymentChannel paymentChannel, int branchOfficeId)
        {
            var listPayment = new List<Payment>();
            var filePath = SaveFileToDisk(file);

            var dbf = new Dbf(Encoding.GetEncoding(866));
            dbf.Read(filePath);

            var recordList = paymentChannel.TotalRecord == FileTotalRow.Last
                ? dbf.Records.GetRange(0, dbf.Records.Count - 1)
                : dbf.Records.GetRange(Convert.ToInt16(paymentChannel.TotalRecord), dbf.Records.Count - Convert.ToInt16(paymentChannel.TotalRecord));

            foreach (DbfRecord record in recordList)
            {
                listPayment.Add(
                    new Payment(
                        DateTime.ParseExact(record[paymentChannel.DateFieldName].ToString(), paymentChannel.TextDateFormat, CultureInfo.InvariantCulture),
                        Convert.ToDecimal(record[paymentChannel.SumFieldName].ToString()),
                        new BranchOfficeService(_ercContext).GetOne(branchOfficeId).CurrentPeriodId,
                        string.Join(" ", paymentChannel.PersonFieldName.Split("+").Select(x => record[x]).ToList()),
                        _ercContext.AccountingPoints.FirstOrDefault(x => x.Name == record[paymentChannel.RecordpointFieldName.Trim()].ToString())?.Id
                    )
                );
            }

            File.Delete(filePath);
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
