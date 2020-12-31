using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.Domain.Helpers;
using Erc.Households.Domain.Payments;
using Erc.Households.EF.PostgreSQL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using dBASE.NET;

namespace Erc.Households.Api.Helpers
{
    public class PaymentsDbfParser
    {
        private readonly ErcContext _ercContext;
        readonly IBranchOfficeService _branchOfficeService;

        public PaymentsDbfParser(ErcContext ercContext, IBranchOfficeService branchOfficeService)
        {
            _ercContext = ercContext;
            _branchOfficeService = branchOfficeService;
        }

        public List<Payment> Parser(int branchOfficeId, PaymentChannel paymentChannel, IFormFile file)
        {
            var listPayments = new List<Payment>();
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var dbf = new Dbf(Encoding.GetEncoding(866));
            dbf.Read(ms);

            var recordList = paymentChannel.TotalRecord == FileTotalRow.Last
                ? dbf.Records.GetRange(0, dbf.Records.Count - 1)
                : dbf.Records.GetRange(Convert.ToInt16(paymentChannel.TotalRecord), dbf.Records.Count - Convert.ToInt16(paymentChannel.TotalRecord));

            foreach (DbfRecord record in recordList)
            {
                listPayments.Add(
                    new Payment(
                        DateTime.ParseExact(record[paymentChannel.DateFieldName].ToString(), paymentChannel.TextDateFormat, new CultureInfo(CultureInfo.CurrentCulture.ToString())),
                        Convert.ToDecimal(record[paymentChannel.SumFieldName].ToString()),
                        _branchOfficeService.GetOne(branchOfficeId).CurrentPeriodId,
                        paymentChannel.PaymentsType,
                        string.Join(" ", paymentChannel.PersonFieldName.Split("+").Select(x => record[x]).ToList()),
                        _ercContext.AccountingPoints.FirstOrDefault(x => x.Name == record[paymentChannel.RecordpointFieldName.Trim()].ToString())?.Id,
                        record[paymentChannel.RecordpointFieldName].ToString()
                    )
                );
            }

            return listPayments;
        }
    }
}
