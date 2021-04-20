using Erc.Households.Domain.Taxes;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Erc.Households.Api.Helpers
{
    public class TaxInvoiceXmlExporter
    {
        public Stream Export(Requests.ExportTaxInvoice taxInvoice) => taxInvoice.Type == TaxInvoiceType.CompensationDso ? ExportByCompensationPayments(taxInvoice) : ExportByCustomerPayments(taxInvoice);

        protected Stream ExportByCustomerPayments(Requests.ExportTaxInvoice taxInvoice)
        {
            MemoryStream ms = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = System.Text.Encoding.GetEncoding(1251),
                Indent = true
            };

            XmlWriter writer = XmlWriter.Create(ms, settings);

            writer.WriteStartDocument();

            writer.WriteStartElement("ZVIT");       // start ZVIT element

            writer.WriteStartElement("TRANSPORT");  //start TRANSPORT
            writer.WriteElementString("CREATEDATE", taxInvoice.LiabilityDate.ToShortDateString());
            writer.WriteElementString("VERSION", "4.1");
            writer.WriteEndElement();   //end TRANSPORT

            writer.WriteStartElement("ORG"); //ORG stert

            writer.WriteStartElement("FIELDS");
            writer.WriteElementString("EDRPOU", taxInvoice.CompanyStateRegistryCode);
            writer.WriteEndElement(); // FIELDS end 

            writer.WriteStartElement("CARD");

            writer.WriteStartElement("FIELDS");
            writer.WriteElementString("PERTYPE", "0");
            writer.WriteElementString("PERDATE", new DateTime(taxInvoice.LiabilityDate.Year, taxInvoice.LiabilityDate.Month, 1).ToShortDateString());
            writer.WriteElementString("CHARCODE", "J1201010");
            writer.WriteElementString("DOCID", Guid.NewGuid().ToString().ToUpper());
            writer.WriteEndElement(); // FIELDS end 

            writer.WriteStartElement("DOCUMENT"); // DOCUMENT start

            WriteTabElement(writer, "N13", "1");
            WriteTabElement(writer, "N27", "4");
            WriteTabElement(writer, "N14", "02");
            WriteTabElement(writer, "FIRM_ADR", taxInvoice.CompanyAddress);
            WriteTabElement(writer, "FIRM_NAME", $"ТОВАРИСТВО З ОБМЕЖЕНОЮ ВІДПОВІДАЛЬНІСТЮ \"ЖИТОМИРСЬКА ОБЛАСНА ЕНЕРГОПОСТАЧАЛЬНА КОМПАНІЯ\", { taxInvoice.BranchOfficeName }"); //");
            WriteTabElement(writer, "FIRM_PHON", taxInvoice.CompanyTaxpayerPhone);
            WriteTabElement(writer, "N2_1", taxInvoice.Id + "/" + taxInvoice.BranchOfficeId);
            WriteTabElement(writer, "N2_13", taxInvoice.BranchOfficeId.ToString());
            WriteTabElement(writer, "N25", "1");
            WriteTabElement(writer, "N3", "Неплатник");
            WriteTabElement(writer, "N4", "100000000000");
            WriteTabElement(writer, "N8", "Договір про постачання");
            WriteTabElement(writer, "N10", taxInvoice.CompanyBookkeeperName);
            WriteTabElement(writer, "A5_7", taxInvoice.LiabilitySum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A6_7", taxInvoice.TaxSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A6_11", taxInvoice.TaxSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A7_7", taxInvoice.FullSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A7_8", "0");
            WriteTabElement(writer, "A7_9", "0");
            WriteTabElement(writer, "A7_10", "0");
            WriteTabElement(writer, "A7_11", taxInvoice.FullSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "FIRM_INN", taxInvoice.CompanyTaxpayerNumber);
            WriteTabElement(writer, "N11", taxInvoice.LiabilityDate.ToShortDateString());
            WriteTabElement(writer, "N2_11", taxInvoice.Id.ToString());
            WriteTabElement(writer, "INN", taxInvoice.CompanyBookkeeperTaxNumber);
            WriteTabElement(writer, "FIRM_EDRPOU", taxInvoice.CompanyStateRegistryCode);
            WriteTabElement(writer, "EDRPOU", taxInvoice.CompanyStateRegistryCode);
            WriteTabElement(writer, "N81", "1");
            WriteTabElement(writer, "N82", new DateTime(taxInvoice.LiabilityDate.Year, 1, 1).ToShortDateString());
            WriteTabElement(writer, "PHON", taxInvoice.CompanyTaxpayerPhone);
            WriteTabElement(writer, "A7_71", "0");
            WriteTabElement(writer, "TAB1_A1", "I", "1");
            WriteTabElement(writer, "TAB1_A8", "20", "1");
            WriteTabElement(writer, "TAB1_A10", taxInvoice.LiabilitySum.ToString("#.00").Replace(',', '.'), "1");
            WriteTabElement(writer, "TAB1_A20", taxInvoice.TaxSum.ToString("#.000000").Replace(',', '.'), "1");
            WriteTabElement(writer, "TAB1_A13", taxInvoice.Type == TaxInvoiceType.Gas ? "газ" : "електрична енергія", "1");
            WriteTabElement(writer, "TAB1_A131", taxInvoice.Type == TaxInvoiceType.Gas ? "2711210000" : "2716000000", "1");
            WriteTabElement(writer, "TAB1_A14", taxInvoice.Type == TaxInvoiceType.Gas ? "м³" : "кВт·год", "1");
            WriteTabElement(writer, "TAB1_A141", "0415", "1");
            WriteTabElement(writer, "TAB1_A15", taxInvoice.EnergyAmount.ToString(), "1");
            WriteTabElement(writer, "TAB1_A16", taxInvoice.TariffValue.ToString().Replace(',', '.'), "1");
            WriteTabElement(writer, "TAB1_A17", taxInvoice.LiabilitySum.ToString("#.00").Replace(',', '.'), "1");

            writer.WriteEndElement(); //DOCUMENT end

            writer.WriteEndElement(); // CARD end

            writer.WriteEndElement(); // ORG end

            writer.WriteEndElement();   // end ZVIT element;
            writer.WriteEndDocument();

            writer.Flush();

            return ms;
        }

        protected Stream ExportByCompensationPayments(Requests.ExportTaxInvoice taxInvoice)
        {
            MemoryStream ms = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = System.Text.Encoding.GetEncoding(1251),
                Indent = true
            };

            XmlWriter writer = XmlWriter.Create(ms, settings);

            writer.WriteStartDocument();

            writer.WriteStartElement("ZVIT");       // start ZVIT element

            writer.WriteStartElement("TRANSPORT");  //start TRANSPORT
            writer.WriteElementString("CREATEDATE",  taxInvoice.LiabilityDate.ToShortDateString());
            writer.WriteElementString("VERSION", "4.1");
            writer.WriteEndElement();   //end TRANSPORT

            writer.WriteStartElement("ORG"); //ORG stert

            writer.WriteStartElement("FIELDS");
            writer.WriteElementString("EDRPOU", taxInvoice.CompanyStateRegistryCode);
            writer.WriteEndElement(); // FIELDS end 

            writer.WriteStartElement("CARD");

            writer.WriteStartElement("FIELDS");
            writer.WriteElementString("PERTYPE", "0");
            writer.WriteElementString("PERDATE", new DateTime(taxInvoice.LiabilityDate.Year, taxInvoice.LiabilityDate.Month, 1).ToShortDateString());
            writer.WriteElementString("CHARCODE", "J1201010");
            writer.WriteElementString("DOCID", Guid.NewGuid().ToString().ToUpper());
            writer.WriteEndElement(); // FIELDS end 

            writer.WriteStartElement("DOCUMENT"); // DOCUMENT start

            WriteTabElement(writer, "N13", "1");
            WriteTabElement(writer, "N14", "02");
            WriteTabElement(writer, "FIRM_ADR", taxInvoice.CompanyAddress);
            WriteTabElement(writer, "FIRM_NAME", $"ТОВАРИСТВО З ОБМЕЖЕНОЮ ВІДПОВІДАЛЬНІСТЮ \"ЖИТОМИРСЬКА ОБЛАСНА ЕНЕРГОПОСТАЧАЛЬНА КОМПАНІЯ\", { taxInvoice.BranchOfficeName }");
            WriteTabElement(writer, "FIRM_PHON", taxInvoice.CompanyTaxpayerPhone);
            WriteTabElement(writer, "N2_1", taxInvoice.Id + "/" + taxInvoice.BranchOfficeId);
            WriteTabElement(writer, "N2_13", taxInvoice.BranchOfficeId.ToString());
            WriteTabElement(writer, "N25", "1");
            WriteTabElement(writer, "N3", "Неплатник");
            WriteTabElement(writer, "N4", "100000000000");
            WriteTabElement(writer, "N10", taxInvoice.CompanyBookkeeperName);
            WriteTabElement(writer, "A5_7", taxInvoice.LiabilitySum.ToString().Replace(',', '.'));
            WriteTabElement(writer, "A6_7", taxInvoice.TaxSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A6_11", taxInvoice.TaxSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A7_7", taxInvoice.FullSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "A7_8", "0");
            WriteTabElement(writer, "A7_9", "0");
            WriteTabElement(writer, "A7_10", "0");
            WriteTabElement(writer, "A7_11", taxInvoice.FullSum.ToString("#.00").Replace(',', '.'));
            WriteTabElement(writer, "FIRM_INN", taxInvoice.CompanyTaxpayerNumber);
            WriteTabElement(writer, "N11", taxInvoice.LiabilityDate.ToShortDateString());
            WriteTabElement(writer, "N2_11", taxInvoice.Id.ToString());
            WriteTabElement(writer, "INN", taxInvoice.CompanyBookkeeperTaxNumber);
            WriteTabElement(writer, "FIRM_EDRPOU", taxInvoice.CompanyStateRegistryCode);
            WriteTabElement(writer, "EDRPOU", taxInvoice.CompanyStateRegistryCode);
            WriteTabElement(writer, "PHON", taxInvoice.CompanyTaxpayerPhone);
            WriteTabElement(writer, "A7_71", "0");
            WriteTabElement(writer, "TAB1_A1", "I", "1");
            WriteTabElement(writer, "TAB1_A8", "20", "1");
            WriteTabElement(writer, "TAB1_A10", taxInvoice.LiabilitySum.ToString("#.00").Replace(',', '.'), "1");
            WriteTabElement(writer, "TAB1_A20", taxInvoice.TaxSum.ToString("#.000000").Replace(',', '.'), "1");
            WriteTabElement(writer, "TAB1_A13", "Компенсація за недотримання гарантованих стандартів якості надання послуг ОСР", "1");
            WriteTabElement(writer, "TAB1_A133", "35.13", "1");
            WriteTabElement(writer, "TAB1_A14", "грн", "1");
            WriteTabElement(writer, "TAB1_A141", "2454", "1");
            WriteTabElement(writer, "TAB1_A15", "1", "1");
            WriteTabElement(writer, "TAB1_A16", taxInvoice.LiabilitySum.ToString().Replace(',', '.'), "1");
            WriteTabElement(writer, "TAB1_A17", taxInvoice.LiabilitySum.ToString().Replace(',', '.'), "1");

            writer.WriteEndElement(); //DOCUMENT end

            writer.WriteEndElement(); // CARD end

            writer.WriteEndElement(); // ORG end

            writer.WriteEndElement();   // end ZVIT element;
            writer.WriteEndDocument();

            writer.Flush();

            return ms;
        }

        protected void WriteTabElement(XmlWriter writer, string name, string value, string tab = "0")
        {
            writer.WriteStartElement("ROW");

            writer.WriteAttributeString("TAB", tab);
            writer.WriteAttributeString("LINE", "0");
            writer.WriteAttributeString("NAME", name);
            writer.WriteElementString("VALUE", value);

            writer.WriteEndElement();
        }
    }
}
