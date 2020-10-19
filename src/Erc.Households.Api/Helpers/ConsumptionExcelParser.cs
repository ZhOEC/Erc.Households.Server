using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Events.AccountingPoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            using var package = new ExcelPackage(stream);
            var rowNumber = package.Workbook.Worksheets.First().Dimension.End.Row;
            var cells = package.Workbook.Worksheets.First().Cells;

            var listRecord = new List<DataFile>();
            //var probRow = new List<DataFile>();
            //var dict = new Dictionary<string, List<DataFile>>();
            
            //var ac = await _ercContext.AccountingPoints.FindAsync(cells[2, 26].Value.ToString());
            //var branchOffice = _branchOfficeService.GetOne(ac.BranchOfficeId);
            //var branchOfficePeriod = await _ercContext.Periods.FindAsync(branchOffice.CurrentPeriodId);

            for (int row = startRow; row < rowNumber; row++)
            {
                if (!(cells[row, 17].Value is null))
                {
                    //System.Diagnostics.Debug.WriteLine($"{row} - {cells[row, 15].Value}");
                    try
                    {
                        var record = new DataFile()
                        {
                            Row = row,
                            Eic = cells[row, 14].GetValue<string>(),
                            //PeriodId = branchOffice.CurrentPeriodId,
                            //FromDate = branchOfficePeriod.StartDate,
                            //ToDate = branchOfficePeriod.EndDate,

                            PreviousMeterReadingT1 = cells[row, 15].GetValue<int>(),
                            PresentMeterReadingT1 = cells[row, 16].GetValue<int>(),
                            UsageT1 = cells[row, 17].GetValue<int>(),
                            MeterNumber = cells[row, 26].GetValue<string>()
                        };

                        if (cells[row, 18].Text.ToLower().Equals("день") || cells[row, 18].Text.ToLower().Equals("пік"))
                        {
                            for (int residualRow = startRow; residualRow < rowNumber; residualRow++)
                            {
                                if (cells[residualRow, 14].Text.Equals(cells[row, 14].Text) && (cells[residualRow, 18].Text.Equals("напівпік") || cells[residualRow, 18].Text.ToLower().Equals("ніч")))
                                {
                                    if (cells[residualRow, 18].Text.Equals("напівпік"))
                                    {
                                        record.PreviousMeterReadingT2 = cells[row, 15].GetValue<int>();
                                        record.PresentMeterReadingT2 = cells[row, 16].GetValue<int>();
                                        record.UsageT2 = cells[row, 17].GetValue<int>();

                                        cells[residualRow, 17].Clear();
                                    }
                                    else if (cells[residualRow, 18].Text.ToLower().Equals("ніч"))
                                    {
                                        record.PreviousMeterReadingT3 = cells[row, 15].GetValue<int>();
                                        record.PresentMeterReadingT3 = cells[row, 16].GetValue<int>();
                                        record.UsageT3 = cells[row, 17].GetValue<int>();

                                        cells[residualRow, 17].Clear();
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
            using var package = new ExcelPackage(stream);
            var rowNumber = package.Workbook.Worksheets.First().Dimension.End.Row;
            var cells = package.Workbook.Worksheets.First().Cells;

            var listRecord = new List<DataFile>();
            var probRow = new List<DataFile>();
            //var dict = new Dictionary<string, List<DataFile>>();

            //var ac = await _ercContext.AccountingPoints.FindAsync(cells[2, 26].Value.ToString());
            //var branchOffice = _branchOfficeService.GetOne(ac.BranchOfficeId);
            //var branchOfficePeriod = await _ercContext.Periods.FindAsync(branchOffice.CurrentPeriodId);

            for (int row = startRow; row < rowNumber; row++)
            {
                if (!(cells[row, 17].Value is null))
                {
                    try
                    {
                        var record = new DataFile()
                        {
                            Row = row,
                            Eic = cells[row, 14].GetValue<string>(),

                            PreviousMeterReadingT1 = cells[row, 15].GetValue<int>(),
                            PresentMeterReadingT1 = cells[row, 16].GetValue<int>(),
                            UsageT1 = cells[row, 17].GetValue<int>(),
                            MeterNumber = cells[row, 26].GetValue<string>()
                        };

                        foreach (var r in listRecord)
                        {
                            if (r.Eic.Equals(record.Eic))
                            {
                                if (cells[row, 18].Text.Equals("напівпік"))
                                {
                                    r.PreviousMeterReadingT2 = cells[row, 15].GetValue<int>();
                                    r.PresentMeterReadingT2 = cells[row, 16].GetValue<int>();
                                    r.UsageT2 = cells[row, 17].GetValue<int>();
                                }
                                else if (cells[row, 18].Text.ToLower().Equals("ніч"))
                                {
                                    r.PreviousMeterReadingT3 = cells[row, 15].GetValue<int>();
                                    r.PresentMeterReadingT3 = cells[row, 16].GetValue<int>();
                                    r.UsageT3 = cells[row, 17].GetValue<int>();
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
