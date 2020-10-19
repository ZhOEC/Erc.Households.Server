using Erc.Households.ConsumptionParser.Core;
using Erc.Households.UsageParser.Core;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Erc.Households.UsageParser.Xlsx.NaturalGas
{
    public class XlsxNaturalGasConsumptionParser : IConsumptionParser
    {
        public IReadOnlyCollection<ParsedConsumption> Parse(Stream stream)
        {
            var consumptionList = new List<ParsedConsumption>();            
            using var package = new ExcelPackage(stream);
            var rowCount = package.Workbook.Worksheets.First().Dimension.End.Row;
            var cells = package.Workbook.Worksheets.First().Cells;
            var generationId = Guid.NewGuid();
            for (int i = 1; i <= rowCount; i++)
            {
                try
                {
                    var consumption = new ParsedConsumption
                    {
                        GenerationId = generationId,
                        Eic = cells[i, 1].Value.ToString(),
                        UsageT1 = cells[i, 2].GetValue<decimal>(),
                        IsParsesd = true,
                        RowNumber = i
                    };

                    consumptionList.Add(consumption);
                }
                catch
                {
                    var consumption = new ParsedConsumption
                    {
                        GenerationId = generationId,
                        Eic = cells[i, 1].Value.ToString(),
                        RowNumber = i
                    };
                }
            }
            return consumptionList;
        }
    }
}
