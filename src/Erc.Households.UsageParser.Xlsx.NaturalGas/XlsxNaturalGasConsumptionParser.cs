using Erc.Households.ConsumptionParser.Core;
using Erc.Households.UsageParser.Core;
using OfficeOpenXml;
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
                        Eic = (cells[i, 1].Value.ToString().Length != 16 || Regex.Matches(cells[i, 1].Value.ToString(), @"\p{IsCyrillic}").Count > 0) 
                            ? throw new Exception("Помилка обробки EIC. Перевірте EIC на наявність кирилічних символів або на довжину (макс. 16 символів).") 
                            : cells[i, 1].Value.ToString(),
                        UsageT1 = cells[i, 2].GetValue<decimal>(),
                        PeriodDate = cells[i, 3].GetValue<DateTime?>(),
                        IsParsed = true,
                        RowNumber = i
                    };

                    consumptionList.Add(consumption);
                }
                catch (Exception ex)
                {
                    var consumption = new ParsedConsumption
                    {
                        GenerationId = generationId,
                        Eic = cells[i, 1].Value.ToString(),
                        RowNumber = i,
                        ErrorMessage = ex.Message
                    };

                    consumptionList.Add(consumption);
                }
            }
            return consumptionList;
        }
    }
}
