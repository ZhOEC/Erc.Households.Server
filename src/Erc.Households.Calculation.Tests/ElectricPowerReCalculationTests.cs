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
    public class ElectricPowerReCalculationTests
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
            var usageT1 = new Usage(400, 1);
            var invalidCalculationsT1 = new[] { new UsageCalculation
            {
                Units = 400,
                PriceValue = _regularPrice,
                Charge = 400 * _regularPrice
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
                UsageT1 = new Usage(-300, 1),
                ZoneRecord = Domain.Shared.ZoneRecord.None
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = -504,
                Units = -300,
                Kz = 1,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 100, Charge = 168m},
                    new { PriceValue = _regularPrice, Units = -400, Charge = -400 * _regularPrice }
                }
            });
        }

        [Fact]
        public void Calculate_Corrective_Invoice_With_Regular_Tariff_Two_Zone()
        {
            var usageT1 = new Usage(0, 0.5m);
            var invalidCalculationsT1 = new[] { new UsageCalculation
            {
                Units = 100,
                PriceValue = _regularPrice,
                Charge = 100 * _regularPrice*0.5m
            }};

            foreach (var calc in invalidCalculationsT1)
                usageT1.AddCalculation(calc, true);

            var usageT2 = new Usage(0, 1);
            var invalidCalculationsT2 = new[] { new UsageCalculation
            {
                Units = 1000,
                PriceValue = _regularPrice,
                Charge = 1000 * _regularPrice
            }};

            foreach (var calc in invalidCalculationsT2)
                usageT2.AddCalculation(calc, true);

            var calculateStrategy = new ElectricPowerCalculateStrategy((id, date) => Task.FromResult(new (Usage, Usage, Usage)[] { (usageT1, usageT2, null) }.AsEnumerable()));
            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Recalculation,
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
                UsageT1 = new Usage(-90, 0.5m),
                UsageT2 = new Usage(-300, 1),
                ZoneRecord = Domain.Shared.ZoneRecord.Two
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = -90 * _regularPrice * 0.5m,
                Units = -90,
                Kz = 0.5m,
                Calculations = new[]
                {
                    new { PriceValue = _regularPrice, Units = 10, Charge = 10 * _regularPrice * 0.5m },
                    new { PriceValue = 1.68m, Units = -100, Charge = -100 * 1.68m * 0.5m }
                }
            });
            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = -300 * _regularPrice,
                Units = -300,
                Kz = 1,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 700, Charge = 700 * _regularPrice},
                    new { PriceValue = _regularPrice, Units = -1000,  Charge = -1000 * _regularPrice }
                }
            });
        }

        [Fact]
        public void Calculate_Corrective_Invoice_With_Regular_Tariff_Three_Zone()
        {
            var usageT1 = new Usage(0, 0.4m);
            var invalidCalculationsT1 = new[] { new UsageCalculation
            {
                Units = 100,
                PriceValue = _regularPrice,
                Charge = 100 * _regularPrice*0.4m
            }};

            foreach (var calc in invalidCalculationsT1)
                usageT1.AddCalculation(calc, true);

            var usageT2 = new Usage(0, 1);
            var invalidCalculationsT2 = new[] { new UsageCalculation
            {
                Units = 500,
                PriceValue = _regularPrice,
                Charge = 500 * _regularPrice
            }};

            foreach (var calc in invalidCalculationsT2)
                usageT2.AddCalculation(calc, true);

            var usageT3 = new Usage(0, 1.5m);
            var invalidCalculationsT3 = new[] { new UsageCalculation
            {
                Units = 600,
                PriceValue = _regularPrice,
                Charge = 600 * _regularPrice*1.5m
            }};

            foreach (var calc in invalidCalculationsT3)
                usageT3.AddCalculation(calc, true);

            var calculateStrategy = new ElectricPowerCalculateStrategy((id, date) => Task.FromResult(new (Usage, Usage, Usage)[] { (usageT1, usageT2, usageT3) }.AsEnumerable()));

            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Recalculation,
                Tariff = new Tariff
                {
                    Rates = new[]
                    {
                        new TariffRate
                        {
                            StartDate = new DateTime(2021, 1, 1),
                            Value = 1.68m
                        }
                    }
                },
                ToDate = new DateTime(2021, 2, 1),
                UsageT1 = new Usage(-5, 0.4m),
                UsageT2 = new Usage(-100, 1),
                UsageT3 = new Usage(10, 1.5m),
                ZoneRecord = Domain.Shared.ZoneRecord.Three
            };

            calculateStrategy.Calculate(calculationRequest).Wait();

            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = -5 * _regularPrice * 0.4m,
                Units = -5,
                Kz = 0.4m,
                Calculations = new[]
                {
                    new { PriceValue = _regularPrice, Units = -100, Charge = -100 * _regularPrice * 0.4m },
                    new { PriceValue = _regularPrice, Units = 95, Charge = 95 * _regularPrice * 0.4m }
                }
            });

            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = -100 * _regularPrice,
                Units = -100,
                Kz = 1,
                Calculations = new[]
                {
                    new { PriceValue = _regularPrice, Units = -500,  Charge = -500 * _regularPrice },
                    new { PriceValue = _regularPrice, Units = 400,  Charge = 400 * _regularPrice }
                }
            });

            calculationRequest.UsageT3.Should().BeEquivalentTo(new
            {
                Charge = 10 * _regularPrice * 1.5m,
                Units = 10,
                Kz = 1.5m,
                Calculations = new[]
                {
                    new  { Units = -600, PriceValue = _regularPrice, Charge = -600 * _regularPrice * 1.5m },
                    new  { Units = 610, PriceValue = _regularPrice, Charge = 610 * _regularPrice * 1.5m }
                }
            });
        }

        [Fact]
        public void Calculate_Corrective_Invoice_With_Two_blocks_Tariff_One_Zone()
        {
            var usageT1 = new Usage(0, 1);
            var invalidCalculationsT1 = new[]
            {
                new UsageCalculation
                {
                    Units = 60,
                    PriceValue = _regularPrice,
                    Charge = 60 * _regularPrice
                },
                new UsageCalculation
                {
                    Units = 100,
                    PriceValue = _discountPrice,
                    Charge = 100 * _discountPrice
                }
            };

            foreach (var calc in invalidCalculationsT1)
                usageT1.AddCalculation(calc, true);

            var calculateStrategy = new ElectricPowerCalculateStrategy((id, date) => Task.FromResult(new (Usage, Usage, Usage)[] { (usageT1, null, null) }.AsEnumerable()));

            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Recalculation,
                Tariff = _twoBlocksTariff,
                ToDate = new DateTime(2021, 2, 1),
                UsageT1 = new Usage(-10, 1),
                ZoneRecord = Domain.Shared.ZoneRecord.None
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = -10 * _regularPrice,
                Units = -10,
                Calculations = new[]
                {
                    new {  Units = -60, PriceValue = _regularPrice, Charge = -60 * _regularPrice },
                    new {  Units = -100, PriceValue = _discountPrice, Charge = -100 * _discountPrice },
                    new {  Units = 50, PriceValue = _regularPrice, Charge = 50 * _regularPrice },
                    new {  Units = 100, PriceValue = _discountPrice, Charge = 100 * _discountPrice },
                }
            });
            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));
        }

        [Fact]
        public void Calculate_corrective_invoice_with_two_blocks_tariff_two_zones()
        {
            var usageT1 = new Usage(0, 0.5m);
            var invalidCalculationsT1 = new[]
            {
                new UsageCalculation { PriceValue = _regularPrice, Units = 371, Charge = 311.64m },
                new UsageCalculation { PriceValue = _discountPrice, Units = 29, Charge = 13.05m }
            };

            foreach (var calc in invalidCalculationsT1)
                usageT1.AddCalculation(calc, true);

            var usageT2 = new Usage(0, 1);
            var invalidCalculationsT2 = new[]
            {
                new UsageCalculation { PriceValue = _regularPrice, Units = 929, Charge = 1560.72m },
                new UsageCalculation { PriceValue = _discountPrice, Units = 71, Charge = 63.9m }
            };

            foreach (var calc in invalidCalculationsT2)
                usageT2.AddCalculation(calc, true);

            var calculateStrategy = new ElectricPowerCalculateStrategy((id, date) => Task.FromResult(new (Usage, Usage, Usage)[] { (usageT1, usageT2, null) }.AsEnumerable()));

            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Recalculation,
                Tariff = _twoBlocksTariff,
                ToDate = new DateTime(2021, 2, 1),
                UsageT1 = new Usage(-250, 0.5m),
                UsageT2 = new Usage(100, 1),
                ZoneRecord = Domain.Shared.ZoneRecord.Two
            };

            calculateStrategy.Calculate(calculationRequest).Wait();

            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = -203.37,
                Units = -250,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 12, Charge = 5.4m },
                    new { PriceValue = 1.68m, Units = 138, Charge = 115.92m },
                    new { PriceValue = _regularPrice, Units = -371, Charge = -311.64m },
                    new { PriceValue = _discountPrice, Units = -29, Charge = -13.05m }
                }
            });

            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));
            calculationRequest.UsageT1.Units.Should().Be(-250);

            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = 154.74m,
                Units = 100,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 88, Charge = 79.2m },
                    new { PriceValue = 1.68m, Units = 1012, Charge = 1700.16m },
                    new { PriceValue = _regularPrice, Units = -929, Charge = -1560.72m },
                    new { PriceValue = _discountPrice, Units = -71, Charge = -63.9m }
                }
            });
            calculationRequest.UsageT2.Charge.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT2.Units.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Units));

            (
               calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
               + calculationRequest.UsageT2.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
               + invalidCalculationsT1.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
               + invalidCalculationsT2.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
            )
            .Should().Be(calculationRequest.Tariff.Rates.First(t => t.Value == 0.9m).ConsumptionLimit);
        }

        [Fact]
        public void Calculate_corrective_invoice_with_two_blocks_tariff_three_zones()
        {
            var usageT1 = new Usage(0, 0.4m);
            var invalidCalculationsT1 = new[]
            {
                new UsageCalculation { PriceValue = _regularPrice, Units = 450, Charge = 302.4m },
                new UsageCalculation { PriceValue = _discountPrice, Units = 50, Charge = 18m }
            };

            foreach (var calc in invalidCalculationsT1)
                usageT1.AddCalculation(calc, true);

            var usageT2 = new Usage(0, 1);
            var invalidCalculationsT2 = new[]
            {
                new UsageCalculation { PriceValue = _regularPrice, Units = 270, Charge = 453.6m },
                new UsageCalculation { PriceValue = _discountPrice, Units = 30, Charge = 27m }
            };

            foreach (var calc in invalidCalculationsT2)
                usageT2.AddCalculation(calc, true);

            var usageT3 = new Usage(0, 1.5m);
            var invalidCalculationsT3 = new[]
            {
                new UsageCalculation { PriceValue = _regularPrice, Units = 180, Charge = 453.6m },
                new UsageCalculation { PriceValue = _discountPrice, Units = 20, Charge = 27m }
            };

            foreach (var calc in invalidCalculationsT3)
                usageT3.AddCalculation(calc, true);

            var calculateStrategy = new ElectricPowerCalculateStrategy((id, date) => Task.FromResult(new (Usage, Usage, Usage)[] { (usageT1, usageT2, usageT3) }.AsEnumerable()));

            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Recalculation,
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
                UsageT1 = new Usage(-333, 0.4m),
                UsageT2 = new Usage(-200, 1),
                UsageT3 = new Usage(-100, 1.5m),
                ZoneRecord = Domain.Shared.ZoneRecord.Three
            };

            calculateStrategy.Calculate(calculationRequest).Wait();

            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = -222.53m,
                Units = -333,
                Calculations = new[]
                {
                    new { PriceValue = _regularPrice, Units = -450, Charge = -302.4m },
                    new { PriceValue = _discountPrice, Units = -50, Charge = -18m },
                    new { PriceValue = _discountPrice, Units = 46, Charge = 16.56m },
                    new { PriceValue = _regularPrice, Units = 121, Charge = 81.31m}
                }
            });
            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));

            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = -333.66m,
                Units = -200,
                Calculations = new[]
                {
                    new { PriceValue = _regularPrice, Units = -270, Charge = -453.6m },
                    new { PriceValue = _discountPrice, Units = -30, Charge = -27m },
                    new { PriceValue = _discountPrice, Units = 27, Charge = 24.3m },
                    new { PriceValue = _regularPrice, Units = 73, Charge = 122.64m }
                }
            });
            calculationRequest.UsageT2.Charge.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT2.Units.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Units));

            calculationRequest.UsageT3.Should().BeEquivalentTo(new
            {
                Charge = -260.19m,
                Units = -100,
                Calculations = new[]
                {
                    new { PriceValue = _regularPrice, Units = -180, Charge = -453.6m },
                    new { PriceValue = _discountPrice, Units = -20, Charge = -27m },
                    new { PriceValue = _discountPrice, Units = 27, Charge = 36.45m },
                    new { PriceValue = _regularPrice, Units = 73, Charge = 183.96m}
                }
            });
            calculationRequest.UsageT3.Charge.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT3.Units.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.Units));

            (
                calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
                + calculationRequest.UsageT2.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
                + calculationRequest.UsageT3.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
                + invalidCalculationsT1.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
               + invalidCalculationsT2.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
               + invalidCalculationsT3.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
             )
             .Should().Be(calculationRequest.Tariff.Rates.First(t => t.Value == 0.9m).ConsumptionLimit);
        }
    }
}
