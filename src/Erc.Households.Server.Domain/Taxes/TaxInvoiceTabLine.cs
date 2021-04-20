using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Taxes
{
    public class TaxInvoiceTabLine
    {
        public int RowNumber { get; init; } //"TAB1_A1", "1"
        public string ProductName { get; init; } // "TAB1_A13", "електрична енергія"
        public string ProductCode { get; init; } // "TAB1_A131", "2716000000"
        public string Unit { get; init; } // "TAB1_A14", "кВт·год"
        public string UnitCode { get; init; } // "TAB1_A141", "0415" 
        public decimal Quantity { get; init; } // "TAB1_A15"
        public decimal Price { get; init; } //"TAB1_A16"
        public decimal LiabilitiesAmount { get; init; } //"TAB1_A10" ToString("0.00").Replace(',', '.')
        public decimal Tax { get; init; } // "TAB1_A20", .ToString("0.000000").Replace(',', '.'), "1") 6-digit after coma!!!!;
    }
}
