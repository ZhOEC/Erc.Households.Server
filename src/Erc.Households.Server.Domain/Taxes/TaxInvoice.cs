using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Erc.Households.Domain.Taxes
{
    public class TaxInvoice
    {
        public int Id { get; init; }
        public int BranchOfficeId { get; init; }
        public int PeriodId { get; init; }
        public DateTime LiabilityDate { get; init; }
        public decimal QuantityTotal { get; init; }
        public decimal LiabilitySum { get; init; }
        public decimal TaxSum { get; init; }
        public decimal FullSum { get; init; }
        public DateTime CreationDate { get; private set; } = DateTime.Now;
        public TaxInvoiceType Type { get; init; }
        public IEnumerable<TaxInvoiceTabLine> TabLines { get; init; }
        public BranchOffice BranchOffice { get; init; }

        public string ExportToXml()
        {
            XmlWriterSettings settings = new()
            {
                Encoding = Encoding.GetEncoding(1251),
                Indent = true
            };
            var ms = new MemoryStream(); 
            var writer = XmlWriter.Create(ms, settings);

            writer.WriteStartDocument();

            writer.WriteStartElement("ZVIT");       // start ZVIT element

            writer.WriteStartElement("TRANSPORT");  //start TRANSPORT
            writer.WriteElementString("CREATEDATE", LiabilityDate.ToShortDateString());
            writer.WriteElementString("VERSION", "4.1");
            writer.WriteEndElement();   //end TRANSPORT

            writer.WriteStartElement("ORG"); //ORG stert

            writer.WriteStartElement("FIELDS");
            writer.WriteElementString("EDRPOU", BranchOffice.Company.StateRegistryCode);
            writer.WriteEndElement(); // FIELDS end 

            writer.WriteStartElement("CARD");

            writer.WriteStartElement("FIELDS");
            writer.WriteElementString("PERTYPE", "0");
            writer.WriteElementString("PERDATE", new DateTime(LiabilityDate.Year, LiabilityDate.Month, 1).ToShortDateString());
            writer.WriteElementString("CHARCODE", "J1201011");
            writer.WriteElementString("DOCID", Guid.NewGuid().ToString().ToUpper());
            writer.WriteEndElement(); // FIELDS end 

            writer.WriteStartElement("DOCUMENT"); // DOCUMENT start

            WriteTabElement(writer, "N13", "1");
            if (Type != TaxInvoiceType.CompensationDso)
                WriteTabElement(writer, "N27", "4");
            
            WriteTabElement(writer, "N14", "02");

            WriteTabElement(writer, "FIRM_ADR", BranchOffice.Company.Address);
            WriteTabElement(writer, "FIRM_NAME", $"{BranchOffice.Company.Name}{(string.Equals(BranchOffice.StringId, "co", StringComparison.OrdinalIgnoreCase) ? string.Empty : ", " + BranchOffice.Name)}"); 
            WriteTabElement(writer, "FIRM_PHON", BranchOffice.Company.TaxpayerPhone);
            WriteTabElement(writer, "N2_1", Id + "/" + BranchOfficeId);
            if (BranchOfficeId < 100) 
                WriteTabElement(writer, "N2_13", BranchOfficeId.ToString());
            WriteTabElement(writer, "N25", "1");
            WriteTabElement(writer, "N3", "Неплатник");
            WriteTabElement(writer, "N4", "100000000000");
            
            if (Type != TaxInvoiceType.CompensationDso)
                WriteTabElement(writer, "N8", "Договір про постачання");
            
            WriteTabElement(writer, "N10", BranchOffice.Company.BookkeeperName);
            WriteTabElement(writer, "A5_7", LiabilitySum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A6_7", TaxSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A6_11", TaxSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A7_7", FullSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A7_8", "0");
            WriteTabElement(writer, "A7_9", "0");
            WriteTabElement(writer, "A7_10", "0");
            WriteTabElement(writer, "A7_11", FullSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "FIRM_INN", BranchOffice.Company.TaxpayerNumber);
            WriteTabElement(writer, "N11", LiabilityDate.ToShortDateString());
            WriteTabElement(writer, "N2_11", Id.ToString());
            WriteTabElement(writer, "INN", BranchOffice.Company.BookkeeperTaxNumber);
            WriteTabElement(writer, "FIRM_EDRPOU", BranchOffice.Company.StateRegistryCode);
            WriteTabElement(writer, "EDRPOU", BranchOffice.Company.StateRegistryCode);
            WriteTabElement(writer, "KS", "1");
            if (Type != TaxInvoiceType.CompensationDso)
            {
                WriteTabElement(writer, "N81", "1");
                WriteTabElement(writer, "N82", new DateTime(LiabilityDate.Year, 1, 1).ToShortDateString());
            }
            WriteTabElement(writer, "PHON", BranchOffice.Company.TaxpayerPhone);
            WriteTabElement(writer, "A7_71", "0");
            
            foreach(var tabLine in TabLines)
            {
                WriteTabElement(writer, "TAB1_A1", tabLine.RowNumber.ToString(), "1", (tabLine.RowNumber - 1).ToString());
                WriteTabElement(writer, "TAB1_A8", "20", "1", (tabLine.RowNumber - 1).ToString());
                WriteTabElement(writer, "TAB1_A10", tabLine.LiabilitiesAmount.ToString("0.00").Replace(',', '.'), "1", (tabLine.RowNumber - 1).ToString());
                WriteTabElement(writer, "TAB1_A20", tabLine.Tax.ToString("0.000000").Replace(',', '.'), "1", (tabLine.RowNumber - 1).ToString());
                WriteTabElement(writer, "TAB1_A13", tabLine.ProductName, "1", (tabLine.RowNumber - 1).ToString());

                if (Type == TaxInvoiceType.CompensationDso)
                    WriteTabElement(writer, "TAB1_A133", tabLine.ProductCode, "1", (tabLine.RowNumber - 1).ToString()); // "TAB1_A133", "35.13",
                else if (Type == TaxInvoiceType.Electricity)
                    WriteTabElement(writer, "TAB1_A131", tabLine.ProductCode, "1", (tabLine.RowNumber - 1).ToString()); // "TAB1_A131", "2716000000"
                else if (Type == TaxInvoiceType.Gas)
                    WriteTabElement(writer, "TAB1_A131", tabLine.ProductCode, "1", (tabLine.RowNumber - 1).ToString()); // "TAB1_A131", "2711210000"

                if (Type == TaxInvoiceType.Gas) WriteTabElement(writer, "TAB1_A132", "1", "1", (tabLine.RowNumber - 1).ToString());
                WriteTabElement(writer, "TAB1_A14", tabLine.Unit, "1", (tabLine.RowNumber - 1).ToString()); // "TAB1_A14", "кВт·год" / компенсація: "TAB1_A14", "грн" / газ ? метр кубічний
                WriteTabElement(writer, "TAB1_A141", tabLine.UnitCode, "1", (tabLine.RowNumber - 1).ToString()); // "TAB1_A141", compensation - "2454", electricity - "0415", gas - "0134"
                WriteTabElement(writer, "TAB1_A15", tabLine.Quantity.ToString("0.000").Replace(',', '.'), "1", (tabLine.RowNumber - 1).ToString());
                WriteTabElement(writer, "TAB1_A16", tabLine.Price.ToString("0.00######").Replace(',', '.'), "1", (tabLine.RowNumber - 1).ToString());
            }

            writer.WriteEndElement(); //DOCUMENT end

            writer.WriteEndElement(); // CARD end

            writer.WriteEndElement(); // ORG end

            writer.WriteEndElement();   // end ZVIT element;
            writer.WriteEndDocument();
            
            writer.Flush();
            
            return Encoding.GetEncoding(1251).GetString(ms.ToArray()); 

            static void WriteTabElement(XmlWriter writer, string name, string value, string tab = "0", string line = "0")
            {
                writer.WriteStartElement("ROW");

                writer.WriteAttributeString("TAB", tab);
                writer.WriteAttributeString("LINE", line);
                writer.WriteAttributeString("NAME", name);
                writer.WriteElementString("VALUE", value);

                writer.WriteEndElement();
            }
        }
    }
}
