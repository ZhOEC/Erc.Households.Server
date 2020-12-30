using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.EF.PostgreSQL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Erc.Households.Api.Helpers
{
    public class ConsumptionExcelParser
    {
        private readonly ErcContext _ercContext;
        private readonly IBranchOfficeService _branchOfficeService;

        public ConsumptionExcelParser(ErcContext ercContext, IBranchOfficeService branchOfficeService)
        {
            _ercContext = ercContext;
            _branchOfficeService = branchOfficeService;
        }

        public List<DataFile> ParserSecondAsync(Stream stream)
        {
            const int startRow = 2; // Start row parse data
            using var workbook = new XLWorkbook(stream);
            var usedRows = workbook.Worksheets.First().RowsUsed(XLCellsUsedOptions.AllContents);

            var listRecord = new List<DataFile>();
            //var probRow = new List<DataFile>();
            //var dict = new Dictionary<string, List<DataFile>>();

            //var ac = await _ercContext.AccountingPoints.FindAsync(cells[2, 26].Value.ToString());
            //var branchOffice = _branchOfficeService.GetOne(ac.BranchOfficeId);
            //var branchOfficePeriod = await _ercContext.Periods.FindAsync(branchOffice.CurrentPeriodId);

            foreach (var row in usedRows)
            {
                if (!(row.Cell(17).Value is null))
                {
                    try
                    {
                        var record = new DataFile()
                        {
                            Row = row.RowNumber(),
                            Eic = row.Cell(14).GetValue<string>(),
                            //PeriodId = branchOffice.CurrentPeriodId,
                            //FromDate = branchOfficePeriod.StartDate,
                            //ToDate = branchOfficePeriod.EndDate,

                            PreviousMeterReadingT1 = row.Cell(15).GetValue<int>(),
                            PresentMeterReadingT1 = row.Cell(16).GetValue<int>(),
                            UsageT1 = row.Cell(17).GetValue<int>(),
                            MeterNumber = row.Cell(26).GetValue<string>()
                        };

                        if (row.Cell(18).GetValue<string>().ToLower().Equals("день") || row.Cell(18).GetValue<string>().ToLower().Equals("пік"))
                        {

                            foreach (var residualRow in usedRows)
                            {
                                if (residualRow.Cell(14).GetValue<string>().Equals(residualRow.Cell(14).GetValue<string>()) 
                                    && (residualRow.Cell(18).GetValue<string>().ToLower().Equals("напівпік") || residualRow.Cell(18).GetValue<string>().ToLower().Equals("ніч")))
                                {
                                    if (residualRow.Cell(18).GetValue<string>().ToLower().Equals("напівпік"))
                                    {
                                        record.PreviousMeterReadingT2 = residualRow.Cell(15).GetValue<int>();
                                        record.PresentMeterReadingT2 = residualRow.Cell(16).GetValue<int>();
                                        record.UsageT2 = residualRow.Cell(17).GetValue<int>();

                                        residualRow.Cell(17).Clear();
                                    }
                                    else if (residualRow.Cell(18).GetValue<string>().ToLower().Equals("ніч"))
                                    {
                                        record.PreviousMeterReadingT3 = residualRow.Cell(15).GetValue<int>();
                                        record.PresentMeterReadingT3 = residualRow.Cell(16).GetValue<int>();
                                        record.UsageT3 = residualRow.Cell(17).GetValue<int>();

                                        residualRow.Cell(17).Clear();
                                    }
                                } 
                                else
                                {
                                    // Add record in problematic list
                                }
                            }
                        }
                        listRecord.Add(record);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

            return listRecord;
        }

        public List<DataFile> ParserThirdAsync(Stream stream)
        {
            const int startRow = 2; // Start row parse data
            using var workbook = new XLWorkbook(stream);
            var usedRows = workbook.Worksheets.First().RowsUsed(XLCellsUsedOptions.AllContents);

            var listRecord = new List<DataFile>();
            var probRow = new List<DataFile>();
            //var dict = new Dictionary<string, List<DataFile>>();

            //var ac = await _ercContext.AccountingPoints.FindAsync(cells[2, 26].Value.ToString());
            //var branchOffice = _branchOfficeService.GetOne(ac.BranchOfficeId);
            //var branchOfficePeriod = await _ercContext.Periods.FindAsync(branchOffice.CurrentPeriodId);

            foreach (var row in usedRows)
            {
                if (!(row.Cell(17).Value is null))
                {
                    try
                    {
                        var record = new DataFile()
                        {
                            Row = row.RowNumber(),
                            Eic = row.Cell(14).GetValue<string>(),

                            PreviousMeterReadingT1 = row.Cell(15).GetValue<int>(),
                            PresentMeterReadingT1 = row.Cell(16).GetValue<int>(),
                            UsageT1 = row.Cell(17).GetValue<int>(),
                            MeterNumber = row.Cell(26).GetValue<string>()
                        };

                        foreach (var r in listRecord)
                        {
                            if (r.Eic.Equals(record.Eic))
                            {
                                if (row.Cell(18).GetValue<string>().ToLower().Equals("напівпік"))
                                {
                                    r.PreviousMeterReadingT2 = row.Cell(15).GetValue<int>();
                                    r.PresentMeterReadingT2 = row.Cell(16).GetValue<int>();
                                    r.UsageT2 = row.Cell(17).GetValue<int>();
                                }
                                else if (row.Cell(18).GetValue<string>().ToLower().Equals("ніч"))
                                {
                                    r.PreviousMeterReadingT3 = row.Cell(15).GetValue<int>();
                                    r.PresentMeterReadingT3 = row.Cell(16).GetValue<int>();
                                    r.UsageT3 = row.Cell(17).GetValue<int>();
                                }
                            }
                        }

                        listRecord.Add(record);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return listRecord;
        }
    }    

    public class DataFile
    {
            public int Row { get; set; }
            public string Eic { get; set; }
            public int PreviousMeterReadingT1 { get; set; }
            public int? PreviousMeterReadingT2 { get; set; }
            public int? PreviousMeterReadingT3 { get; set; }
            public int PresentMeterReadingT1 { get; set; }
            public int? PresentMeterReadingT2 { get; set; }
            public int? PresentMeterReadingT3 { get; set; }
            public int UsageT1 { get; set; }
            public int? UsageT2 { get; set; }
            public int? UsageT3 { get; set; }
            public string MeterNumber { get; set; }
    }
}
