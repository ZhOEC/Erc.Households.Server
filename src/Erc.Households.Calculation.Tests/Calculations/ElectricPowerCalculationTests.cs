using Erc.Households.CalculateStrategies.Core;
using Erc.Households.CalculateStrategies.ElectricPower;
using Erc.Households.Domain.Shared.Billing;
using Erc.Households.Domain.Shared.Tariffs;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Erc.Households.Calculation.Tests
{
    public class ElectricPowerCalculationTests
    {
        [Fact]
        public void Calculate_Regular_Invoice_With_Regular_Tariff_One_Zone()
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
                UsageT1 = new Usage(1000),
                ZoneRecord = Domain.Shared.ZoneRecord.None
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 1680,
                Units = 1000,
                Kz = 1,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 1000, Charge = 1680}
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
                UsageT1 = new Usage(1000) 
                {
                    Kz = 0.5m,
                    DiscountWeight = .67m
                },
                UsageT2 = new Usage(1000)
                {
                    Kz = 1, 
                    DiscountWeight = .33m
                },
                ZoneRecord = Domain.Shared.ZoneRecord.Two
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 840,
                Units = 1000,
                Kz = 0.5m,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 1000, Charge = 840}
                }
            });
            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = 1680,
                Units = 1000,
                Kz = 1,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 1000, Charge = 1680}
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
                UsageT1 = new Usage(1001)
                {
                    Kz = .4m,
                    DiscountWeight = .46m
                },
                UsageT2 = new Usage(1000)
                {
                    Kz = 1,
                    DiscountWeight = .33m
                },
                UsageT3 = new Usage(1000)
                {
                    Kz = 1.5m,
                    DiscountWeight = .21m
                },
                ZoneRecord = Domain.Shared.ZoneRecord.Three
            };

            calculateStrategy.Calculate(calculationRequest).Wait();

            calculationRequest.UsageT1.Should().BeEquivalentTo(new 
            { 
                Charge = 672.67m, 
                Units = 1001 ,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 1001, Charge = 672.67m}
                }
            });

            calculationRequest.UsageT2.Should().BeEquivalentTo(new 
            { 
                Charge = 1680, 
                Units = 1000,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 1000, Charge = 1680}
                }
            });

            calculationRequest.UsageT3.Should().BeEquivalentTo(new 
            { 
                Charge = 2520,
                Units = 1000,
                Calculations = new[]
                {
                    new { PriceValue = 1.68m, Units = 1000, Charge = 2520}
                }
            });
        }

        [Fact]
        public void Calculate_Regular_Invoice_With_Regular_Tariff_Three_Zone_with_zero_consumption()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);
            var calculationRequest = new CalculationRequest
            {
                AccountingPointId = 1,
                FromDate = new DateTime(2021, 1, 1),
                InvoiceType = Domain.Shared.InvoiceType.Common,
                Tariff = new Tariff
                {
                    Rates = new[] { 
                        new TariffRate
                        {
                            StartDate = new DateTime(2021, 1, 1),
                            Value = 1.68m
                        },
                        new TariffRate
                        {
                            StartDate = new DateTime(2021, 1, 1),
                            Value = .9m,
                            ConsumptionLimit = 100
                        }
                    }
                },
                ToDate = new DateTime(2021, 2, 1),
                UsageT1 = new Usage(0)
                {
                    Kz = .4m,
                    DiscountWeight = .46m
                },
                UsageT2 = new Usage(24)
                {
                    Kz = 1,
                    DiscountWeight = .33m
                },
                UsageT3 = new Usage(6)
                {
                    Kz = 1.5m,
                    DiscountWeight = .21m
                },
                ZoneRecord = Domain.Shared.ZoneRecord.Three
            };

            calculateStrategy.Calculate(calculationRequest).Wait();

            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 0,
                Units = 0,
            });

            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = .9m * 24,
                Units = 24,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 24, Charge = .9m*24}
                }
            });

            calculationRequest.UsageT3.Should().BeEquivalentTo(new
            {
                Charge = 6 * .9m * 1.5m,
                Units = 6,
                Calculations = new[]
                {
                    new { PriceValue = .9m, Units = 6, Charge = 6*.9m*1.5m}
                }
            });
        }


        [Fact]
        public void Calculate_Regular_Invoice_With_Two_blocks_Tariff_One_Zone()
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
                UsageT1 = new Usage(1000),
                ZoneRecord = Domain.Shared.ZoneRecord.None
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new 
            { 
                Charge = 1602, 
                Units = 1000, 
                Calculations = new[] 
                { 
                    new { PriceValue = 0.9m, Units = 100, Charge = 90 },
                    new { PriceValue = 1.68m, Units = 900, Charge = 1512}
                }
            });
            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));
        }

        [Fact]
        public void Calculate_Regular_Invoice_With_Two_blocks_Tariff_Two_Zone_Over_Limit()
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
                UsageT1 = new Usage(333)
                {
                    Kz = 0.5m,
                    DiscountWeight = .67m
                },
                UsageT2 = new Usage(444)
                {
                    Kz = 1,
                    DiscountWeight = .33m
                },
                ZoneRecord = Domain.Shared.ZoneRecord.Two
            };

            calculateStrategy.Calculate(calculationRequest).Wait();

            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 262.95m,
                Units = 333,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 43, Charge = 19.35m },
                    new { PriceValue = 1.68m, Units = 290, Charge = 243.6m}
                }
            });
            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));

            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = 701.46m,
                Units = 444,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 57, Charge = 51.3m },
                    new { PriceValue = 1.68m, Units = 387, Charge = 650.16m}
                }
            });
            calculationRequest.UsageT2.Charge.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT2.Units.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Units));


            (
                calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units) 
                + calculationRequest.UsageT2.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
             )
             .Should().Be(calculationRequest.Tariff.Rates.First(t => t.Value == 0.9m).ConsumptionLimit);
        }

        [Fact]
        public void Calculate_Regular_Invoice_With_Two_blocks_Tariff_Two_Zone_Less_Limit()
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
                UsageT1 = new Usage(33)
                {
                    Kz = 0.5m,
                    DiscountWeight = .67m
                },
                UsageT2 = new Usage(44)
                {
                    Kz = 1,
                    DiscountWeight = .33M
                },
                ZoneRecord = Domain.Shared.ZoneRecord.Two
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new { Charge = 14.85m, Units = 33 });
            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));
            
            calculationRequest.UsageT2.Should().BeEquivalentTo(new { Charge = 39.6m, Units = 44 });
            calculationRequest.UsageT2.Charge.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT2.Units.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Units));
        }

        [Fact]
        public void Calculate_Regular_Invoice_With_Two_blocks_Tariff_Three_Zone_Over_Limit()
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
                UsageT1 = new Usage(333)
                {
                    Kz = 0.4m,
                    DiscountWeight = .46m
                },
                UsageT2 = new Usage(200)
                {
                    Kz = 1,
                    DiscountWeight = .33m
                },
                UsageT3 = new Usage(100)
                {
                    Kz = 1.5m,
                    DiscountWeight = .21m
                },
                ZoneRecord = Domain.Shared.ZoneRecord.Three
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            
            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 207.24m,
                Units = 333,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 53, Charge = 19.08m },
                    new { PriceValue = 1.68m, Units = 280, Charge = 188.16m}
                }
            });
            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));
            
            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = 311.04m,
                Units = 200,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 32, Charge = 28.8m },
                    new { PriceValue = 1.68m, Units = 168, Charge = 282.24m}
                }
            });
            calculationRequest.UsageT2.Charge.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT2.Units.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Units));

            calculationRequest.UsageT3.Should().BeEquivalentTo(new
            {
                Charge = 234.45m,
                Units = 100,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 15, Charge = 20.25m },
                    new { PriceValue = 1.68m, Units = 85, Charge = 214.2m}
                }
            });
            calculationRequest.UsageT3.Charge.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT3.Units.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.Units));

            (
                calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
                + calculationRequest.UsageT2.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
                + calculationRequest.UsageT3.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
             )
             .Should().Be(calculationRequest.Tariff.Rates.First(t => t.Value == 0.9m).ConsumptionLimit);
        }

        [Fact]
        public void Calculate_Regular_Invoice_With_Two_blocks_Tariff_Three_Zone_Less_Limit()
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
                UsageT1 = new Usage(33)
                {
                    Kz = 0.4m,
                    DiscountWeight = .46m
                },
                UsageT2 = new Usage(44)
                {
                    Kz = 1,
                    DiscountWeight = .33m
                },
                UsageT3 = new Usage(14)
                {
                    Kz = 1.5m,
                    DiscountWeight = .21m
                },
                ZoneRecord = Domain.Shared.ZoneRecord.Three
            };

            calculateStrategy.Calculate(calculationRequest).Wait();
            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 11.88m,
                Units = 33,
                Calculations = new[]
                 {
                    new { PriceValue = 0.9m, Units = 33, Charge = 11.88m },
                }
            });
            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));

            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = 39.6m,
                Units = 44,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 44, Charge = 39.6m },
                }
            });
            calculationRequest.UsageT2.Charge.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT2.Units.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Units));

            calculationRequest.UsageT3.Should().BeEquivalentTo(new
            {
                Charge = 18.9m,
                Units = 14,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 14, Charge = 18.9m },
                }
            });
            calculationRequest.UsageT3.Charge.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT3.Units.Should().Be(calculationRequest.UsageT3.Calculations.Sum(c => c.Units));
        }

        [Fact]
        public void Calculate_Regular_Invoice_With_Two_blocks_Tariff_Three_Zone_Over_Limit_with_zero_consumption_in_one_zone ()
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
                UsageT1 = new Usage(53)
                {
                    Kz = 0.4m,
                    DiscountWeight = .46m
                },
                UsageT2 = new Usage(49)
                {
                    Kz = 1,
                    DiscountWeight = .33m
                },
                UsageT3 = new Usage(0)
                {
                    Kz = 1.5m,
                    DiscountWeight = .21m
                },
                ZoneRecord = Domain.Shared.ZoneRecord.Three
            };

            calculateStrategy.Calculate(calculationRequest).Wait();

            calculationRequest.UsageT1.Should().BeEquivalentTo(new
            {
                Charge = 19.39m,
                Units = 53,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 52, Charge = 18.72m },
                    new { PriceValue = 1.68m, Units = 1, Charge = 0.67m}
                }
            });
            calculationRequest.UsageT1.Charge.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT1.Units.Should().Be(calculationRequest.UsageT1.Calculations.Sum(c => c.Units));

            calculationRequest.UsageT2.Should().BeEquivalentTo(new
            {
                Charge = 44.88m,
                Units = 49,
                Calculations = new[]
                {
                    new { PriceValue = 0.9m, Units = 48, Charge = 43.2m },
                    new { PriceValue = 1.68m, Units = 1, Charge = 1.68m}
                }
            });
            calculationRequest.UsageT2.Charge.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Charge));
            calculationRequest.UsageT2.Units.Should().Be(calculationRequest.UsageT2.Calculations.Sum(c => c.Units));

            calculationRequest.UsageT3.Should().BeEquivalentTo(new
            {
                Charge = 0,
                Units = 0
            });
            calculationRequest.UsageT3.Calculations.Should().BeNullOrEmpty();
            
            (
                calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
                + calculationRequest.UsageT2.Calculations.Where(c => c.PriceValue == 0.9m).Sum(c => c.Units)
             )
             .Should().Be(calculationRequest.Tariff.Rates.First(t => t.Value == 0.9m).ConsumptionLimit);
        }
    }
}
