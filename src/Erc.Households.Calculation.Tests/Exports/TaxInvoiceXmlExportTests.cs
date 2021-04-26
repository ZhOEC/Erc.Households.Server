using Erc.Households.Domain.Taxes;
using Org.XmlUnit.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Erc.Households.Tests.Exports
{
    public class TaxInvoiceXmlExportTests
    {
        public TaxInvoiceXmlExportTests()=> Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        [Fact]
        public void Export_taxinvoice_electricity()
        {
            var ti = new TaxInvoice
            {
                Id = 111046,
                QuantityTotal = 2342352,
                FullSum = 4329482.36m,
                LiabilityDate = new DateTime(2021, 3, 31),
                LiabilitySum = 3607901.97m,
                TaxSum = 721580.39m,
                Type = TaxInvoiceType.Electricity,
                BranchOfficeId = 8,
                BranchOffice = new Domain.BranchOffice
                {
                    Name = "Баранівський ЦОК",
                    Company = new Domain.Company
                    {
                        Name = "ТОВАРИСТВО З ОБМЕЖЕНОЮ ВІДПОВІДАЛЬНІСТЮ \"ЖИТОМИРСЬКА ОБЛАСНА ЕНЕРГОПОСТАЧАЛЬНА КОМПАНІЯ\"",
                        BookkeeperName = "Алла ІВЧУК",
                        StateRegistryCode = "42095943",
                        TaxpayerNumber = "420959406258",
                        BookkeeperTaxNumber = "2778207983",
                        TaxpayerPhone = "0412402109",
                        Address = "10003, майдан Перемоги, буд. 10 м. Житомир"
                    }
                },
                TabLines = new[] { new TaxInvoiceTabLine {Price = 1.54029026m, LiabilitiesAmount = 3607901.97m, Quantity = 2342352, Tax = 721580.394m, ProductCode= "2716000000", ProductName= "електрична енергія", RowNumber=1, Unit= "кВт·год", UnitCode= "0415" } }
            };
            var generated = ti.ExportToXml();
            var d = DiffBuilder.Compare(Input.FromFile("Exports/reference_electricity.xml"))
                .WithNodeFilter(node=>node.Name!= "DOCID")
                .WithTest(Input.FromString(generated)).Build();
            
            Assert.False(d.HasDifferences());
        }

        [Fact]
        public void Export_taxinvoice_dso_compensation()
        {
            var ti = new TaxInvoice
            {
                Id = 111047,
                QuantityTotal = 1,
                FullSum = 3432.82m,
                LiabilityDate = new DateTime(2021, 3, 31),
                LiabilitySum = 2860.68m,
                TaxSum = 572.14m,
                Type = TaxInvoiceType.CompensationDso,
                BranchOfficeId = 8,
                BranchOffice = new Domain.BranchOffice
                {
                    Name = "Баранівський ЦОК",
                    Company = new Domain.Company
                    {
                        Name = "ТОВАРИСТВО З ОБМЕЖЕНОЮ ВІДПОВІДАЛЬНІСТЮ \"ЖИТОМИРСЬКА ОБЛАСНА ЕНЕРГОПОСТАЧАЛЬНА КОМПАНІЯ\"",
                        BookkeeperName = "Алла ІВЧУК",
                        StateRegistryCode = "42095943",
                        TaxpayerNumber = "420959406258",
                        BookkeeperTaxNumber = "2778207983",
                        TaxpayerPhone = "0412402109",
                        Address = "10003, майдан Перемоги, буд. 10 м. Житомир"
                    }
                },
                TabLines = new[] { new TaxInvoiceTabLine { Price = 2860.68m, LiabilitiesAmount = 2860.68m, Quantity = 1, Tax = 572.136m, ProductCode = "35.13", ProductName = "Компенсація за недотримання гарантованих стандартів якості надання послуг ОСР", RowNumber = 1, Unit = "грн", UnitCode = "2454" } }
            };
            var generated = ti.ExportToXml();
            var d = DiffBuilder.Compare(Input.FromFile("Exports/reference_compensation.xml"))
                .WithNodeFilter(node => node.Name != "DOCID")
                .WithTest(Input.FromString(generated)).Build();

            Assert.False(d.HasDifferences());
        }
    }
}
