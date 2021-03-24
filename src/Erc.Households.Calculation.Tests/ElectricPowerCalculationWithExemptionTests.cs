using Erc.Households.CalculateStrategies.Core;
using Erc.Households.CalculateStrategies.ElectricPower;
using Erc.Households.Domain.Shared.Billing;
using Erc.Households.Domain.Shared.Tariffs;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Erc.Households.Calculation.Tests
{
    public class ElectricPowerCalculationWithExemptionTests
    {
        static readonly decimal _regularPrice = 1.68m;
        static readonly decimal _discountPrice = 0.9m;

        readonly Tariff _regularTariff = new Tariff
        {
            Rates = new[]
            {
                new TariffRate
                {
                    StartDate = new DateTime(2019, 1, 1),
                    Value = _regularPrice
                }
            }
        };

        readonly Tariff _twoBlocksTariff = new Tariff
        {
            Rates = new[]
            {
                new TariffRate { StartDate = new DateTime(2019, 1, 1), Value = _regularPrice },
                new TariffRate { StartDate = new DateTime(2019, 1, 1), Value = _discountPrice, ConsumptionLimit = 100 }
            }
        };

        [Fact]
        public void Calculate_Corrective_Invoice_With_Regular_Tariff_One_Zone()
        {
            var usageT1 = new Usage(400);
            var invalidCalculationsT1 = new[] { new UsageCalculation
            {
                Units = 400,
                PriceValue = _regularPrice,
                Charge = 400 * _regularPrice,
                Discount = 130 * _regularPrice*.5m,
                DiscountUnits = 130,
            }};

            foreach (var calc in invalidCalculationsT1)
                usageT1.AddCalculation(calc);

            var calculateStrategy = new ElectricPowerCalculateStrategy((id, date) => Task.FromResult(new (Usage, Usage, Usage)[] { (usageT1, null, null) }.AsEnumerable()));
            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Recalculation,
                Tariff = _regularTariff,
                ToDate = new DateTime(2021, 2, 1),
                UsageT1 = new Usage(-300),
                ZoneRecord = Domain.Shared.ZoneRecord.None,
                ExemptionData = new ExemptionData
                {
                    CanBeUsedElectricWaterHeater = true,
                    ExemptionPercent = 50m,
                    NumberOfPeople = 2,
                    UseDiscountLimit = true,
                    ExemptionDiscountNorms = new ExemptionDiscountNorms[]
                    {
                        new ExemptionDiscountNorms(
                            effectiveDate: new DateTime(2019, 1, 1),
                            baseUnits: 70,
                            baseUnitsWithoutHotWater: 100,
                            basePerson: 1,
                            unitsPerPerson:30,
                            maxUnits: 190,
                            maxUnitsWithoutHotWater: 250,
                            baseSquareMeter: 10.5m,
                            squareMeterPerPerson: 21m,
                            unitsPerSquareMeter: 30)
                    }
                }
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = -504,
                Units = -300,
                Discount = -30 * .5m * _regularPrice,
                DiscountUnits = -30,
                Kz = 1,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 100, Charge = 168m, Discount = 100 * _regularPrice*.5m, DiscountUnits = 100 },
                    new { PriceValue = _regularPrice, Units = -400, Charge = -400 * _regularPrice, Discount = -130 * _regularPrice*.5m, DiscountUnits = -130 }
                }
            });
        }

        [Fact]
        public void Calculate_Regular_Invoice_With_Regular_Tariff_Two_Zone()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);
            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Common,
                Tariff = new Tariff
                {
                    Rates = new[] { new TariffRate
                    {
                        StartDate = new DateTime(2021, 1, 1),
                        Value = 1.68m
                    }
                    }
                },
                ToDate = new DateTime(2021, 2, 1),
                UsageT1 = new Usage(1000, 0.5m, .67m),
                UsageT2 = new Usage(1000, 1, .33m),
                ZoneRecord = Domain.Shared.ZoneRecord.Two,
                ExemptionData = new ExemptionData
                {
                    CanBeUsedElectricWaterHeater = true,
                    ExemptionPercent = 50m,
                    NumberOfPeople = 1,
                    UseDiscountLimit = true,
                    ExemptionDiscountNorms = new ExemptionDiscountNorms[]
                    {
                        new ExemptionDiscountNorms(
                            effectiveDate: new DateTime(2019, 1, 1),
                            baseUnits: 70,
                            baseUnitsWithoutHotWater: 100,
                            basePerson: 1,
                            unitsPerPerson:30,
                            maxUnits: 190,
                            maxUnitsWithoutHotWater: 250,
                            baseSquareMeter: 10.5m,
                            squareMeterPerPerson: 21m,
                            unitsPerSquareMeter: 30)
                    }
                }
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 840,
                Units = 1000,
                Discount = 67 * .5m * _regularPrice * .5m,
                DiscountUnits = 67,
                Kz = 0.5m,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 1000, Charge = 840, Discount = 67 * .5m * _regularPrice * .5m, DiscountUnits = 67}
                }
            });
            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = 1680,
                Units = 1000,
                Discount = 33 * .5m * _regularPrice,
                DiscountUnits = 33,
                Kz = 1,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 1000, Charge = 1680,  Discount = 33 * .5m * _regularPrice, DiscountUnits = 33 }
                }
            });
        }

        [Fact]
        public void Calculate_Regular_Invoice_With_Regular_Tariff_Three_Zone()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);
            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Common,
                Tariff = new Tariff
                {
                    Rates = new[] { new TariffRate
                    {
                        StartDate = new DateTime(2021, 1, 1),
                        Value = 1.68m
                    }
                    }
                },
                ToDate = new DateTime(2021, 2, 1),
                UsageT1 = new Usage(10, .4m, .46m),
                UsageT2 = new Usage(30, 1, .33m),
                UsageT3 = new Usage(1000, 1.5m, .21m),
                ZoneRecord = Domain.Shared.ZoneRecord.Three,
                ExemptionData = new ExemptionData
                {
                    CanBeUsedElectricWaterHeater = true,
                    ExemptionPercent = 50m,
                    NumberOfPeople = 2,
                    UseDiscountLimit = true,
                    ExemptionDiscountNorms = new ExemptionDiscountNorms[]
                    {
                        new ExemptionDiscountNorms(
                            effectiveDate: new DateTime(2019, 1, 1),
                            baseUnits: 70,
                            baseUnitsWithoutHotWater: 100,
                            basePerson: 1,
                            unitsPerPerson:30,
                            maxUnits: 190,
                            maxUnitsWithoutHotWater: 250,
                            baseSquareMeter: 10.5m,
                            squareMeterPerPerson: 21m,
                            unitsPerSquareMeter: 30)
                    }
                }
            };

            calculateStrategy.Calculate(calculationRequest).Wait();

            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 10*_regularPrice*.4m,
                Units = 10,
                Discount = 10 * .5m * _regularPrice * .4m,
                DiscountUnits = 10,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 10, Charge = 10*_regularPrice*.4m, Discount = 10 * .5m * _regularPrice * .4m, DiscountUnits = 10}
                }
            });

            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = 30 * _regularPrice,
                Units = 30,
                Discount = 30 * .5m * _regularPrice,
                DiscountUnits = 30,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 30, Charge = 30*_regularPrice, Discount = 30 * .5m * _regularPrice, DiscountUnits = 30 }
                }
            });

            calculationRequest.UsageT3.Should().BeEquivalentTo(new
            {
                Charge = 2520,
                Units = 1000,
                Discount = 27 * .5m * _regularPrice * 1.5m,
                DiscountUnits = 27,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 1000, Charge = 2520, Discount = 27 * .5m * _regularPrice * 1.5m, DiscountUnits = 27}
                }
            });
        }

        [Fact]
        public void Calculate_Regular_Invoice_With_Two_blocks_Tariff_Three_Zone_Over_Limit_with_exemption()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);
            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Common,
                Tariff = new Tariff
                {
                    Rates = new[]
                    {
                        new TariffRate
                        {
                            StartDate = new DateTime(2021, 1, 1),
                            Value = 1.68m
                        },
                        new TariffRate
                        {
                            StartDate = new DateTime(2021, 1, 1),
                            Value = 0.9m,
                            ConsumptionLimit=100
                        }
                    }
                },
                ToDate = new DateTime(2021, 2, 1),
                UsageT1 = new Usage(100, 0.4m, .46m),
                UsageT2 = new Usage(100, 1, .33m),
                UsageT3 = new Usage(100, 1.5m, .21m),
                ZoneRecord = Domain.Shared.ZoneRecord.Three,
                ExemptionData = new ExemptionData
                {
                    CanBeUsedElectricWaterHeater = true,
                    ExemptionPercent = .50m,
                    NumberOfPeople = 4,
                    UseDiscountLimit = true,
                    ExemptionDiscountNorms = new ExemptionDiscountNorms[]
                    {
                        new ExemptionDiscountNorms(
                            effectiveDate: new DateTime(2019, 1, 1),
                            baseUnits: 70,
                            baseUnitsWithoutHotWater: 100,
                            basePerson: 1,
                            unitsPerPerson:30,
                            maxUnits: 190,
                            maxUnitsWithoutHotWater: 250,
                            baseSquareMeter: 10.5m,
                            squareMeterPerPerson: 21m,
                            unitsPerSquareMeter: 30)
                    }
                }
            };

            calculateStrategy.Calculate(calculationRequest).Wait();

            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 56.9m,
                Units = 100,
                Discount = 5.94m + 18.14m,
                DiscountUnits = 87,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 33, Charge = 11.88m, Discount = decimal.Round(33 * .9m * .5m * 0.4m, 2, MidpointRounding.AwayFromZero), DiscountUnits = 33},
                    new { PriceValue = 1.68m, Units = 67, Charge = 45.02m, Discount = decimal.Round(54 * 1.68m * .5m * 0.4m, 2, MidpointRounding.AwayFromZero), DiscountUnits = 54}
                }
            });
            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));
            calculationRequest.UsageT1.Discount.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Discount));
            calculationRequest.UsageT1.DiscountUnits.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.DiscountUnits));

            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = 142.26m,
                Units = 100,
                Discount = decimal.Round(33 * .9m * .5m, 2, MidpointRounding.AwayFromZero) + decimal.Round(30 * 1.68m * .5m, 2, MidpointRounding.AwayFromZero),
                DiscountUnits = 63,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 33, Charge = 29.7m, Discount = decimal.Round(33 * .9m * .5m, 2, MidpointRounding.AwayFromZero), DiscountUnits = 33},
                    new { PriceValue = 1.68m, Units = 67, Charge = 112.56m, Discount = decimal.Round(30 * 1.68m *.5m, 2, MidpointRounding.AwayFromZero), DiscountUnits = 30}
                }
            });
            calculationRequest.UsageT2.Charge.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT2.Units.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Units));
            calculationRequest.UsageT2.Discount.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Discount));
            calculationRequest.UsageT2.DiscountUnits.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.DiscountUnits));


            calculationRequest.UsageT3.Should().BeEquivalentTo(new
            {
                Charge = 212.22m,
                Units = 100,
                Discount = decimal.Round(34 * 1.5m * .9m * .5m, 2, MidpointRounding.AwayFromZero) + decimal.Round(6 * 1.5m * 1.68m * .5m, 2, MidpointRounding.AwayFromZero),
                DiscountUnits = 40,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 34, Charge = decimal.Round(34 * .9m * 1.5m, 2, MidpointRounding.AwayFromZero), Discount = decimal.Round(34 * 1.5m * .9m *.5m, 2, MidpointRounding.AwayFromZero), DiscountUnits = 34 },
                    new { PriceValue = 1.68m, Units = 66, Charge = decimal.Round(66 * 1.68m * 1.5m, 2, MidpointRounding.AwayFromZero), Discount = decimal.Round(6 * 1.5m * 1.68m * .5m, 2, MidpointRounding.AwayFromZero), DiscountUnits = 6 }
                }
            });
            calculationRequest.UsageT3.Charge.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT3.Units.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.Units));
            calculationRequest.UsageT3.Discount.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.Discount));
            calculationRequest.UsageT3.DiscountUnits.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.DiscountUnits));

            (
                calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
                + calculationRequest.UsageT2.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
                + calculationRequest.UsageT3.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
             )
             .Should().Be(calculationRequest.Tariff.Rates.First(t => t.Value == 0.9m).ConsumptionLimit);
        }
    }
}
