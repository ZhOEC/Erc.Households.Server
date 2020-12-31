using ClosedXML.Excel;
using Erc.Households.ConsumptionParser.Core;
using Erc.Households.UsageParser.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Erc.Households.UsageParser.Xlsx.NaturalGas
{
    public class XlsxNaturalGasConsumptionParser : IConsumptionParser
    {
        public IReadOnlyCollection<ParsedConsumption> Parse(Stream stream)
        {
            var consumptionList = new List<ParsedConsumption>();
            using var workbook = new XLWorkbook(stream);
            var usedRows = workbook.Worksheets.First().RowsUsed(XLCellsUsedOptions.AllContents);
            var generationId = Guid.NewGuid();
            foreach(var row in usedRows)
            {
                try
                {
                    var consumption = new ParsedConsumption
                    {
                        GenerationId = generationId,
                        Eic = (row.Cell(1).Value.ToString().Length != 16 || Regex.Matches(row.Cell(1).Value.ToString(), @"\p{IsCyrillic}").Count > 0)
                            ? throw new Exception("Помилка EIC. Перевірте EIC на наявність кирилічних символів або на довжину (16 символів).")
                            : row.Cell(1).Value.ToString(),
                        UsageT1 = row.Cell(2).GetValue<decimal>(),
                        PeriodDate = row.Cell(3).GetValue<DateTime?>(),
                        IsParsed = true,
                        RowNumber = row.RowNumber()
                    };

                    consumptionList.Add(consumption);
                }
                catch (Exception ex)
                {
                    var consumption = new ParsedConsumption
                    {
                        GenerationId = generationId,
                        Eic = row.Cell(1).Value.ToString(),
                        RowNumber = row.RowNumber(),
                        ErrorMessage = ex.Message
                    };

                    consumptionList.Add(consumption);
                }
            }

            return consumptionList;
        }
    }
}
