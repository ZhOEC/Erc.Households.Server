using Erc.Households.CalculateStrategies.ElectricPower;
using Erc.Households.Domain.Shared.Tariffs;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Erc.Households.Calculation.Tests
{
    public class ElectricPowerHelperTests
    {
        static readonly decimal _regularPrice = 1.68m;
        static readonly decimal _discountPrice = 0.9m;

        readonly Tariff _twoBlocksTariff = new Tariff
        {
            Rates = new[]
            {
                new TariffRate { StartDate = new DateTime(2019, 1, 1), Value = _regularPrice },
                new TariffRate
                {
                    StartDate = new DateTime(2019, 1, 1),
                    Value = _discountPrice,
                    ConsumptionLimit = 100,
                    HeatingConsumptionLimit=3000,
                    HeatingEndDay = new DateTime(2020,5,1),
                    HeatingStartDay = new DateTime(2019,10,1)
                }
            }
        };

        [Fact]
        public void Test_heating_tariff_limit()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);

            MethodInfo method = typeof(ElectricPowerCalculateStrategy)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.Name == "GetConsumptionMonthLimitInternal" && x.IsPrivate)
                .First();

            var limit = (int)method.Invoke(calculateStrategy, new object[] { _twoBlocksTariff.Rates.First(tr => tr.HeatingConsumptionLimit.HasValue), new DateTime(2021, 2, 1), new DateTime(2021, 3, 1) });
            limit.Should().Be(3000);

            limit = (int)method.Invoke(calculateStrategy, new object[] { _twoBlocksTariff.Rates.First(tr => tr.HeatingConsumptionLimit.HasValue), new DateTime(2021, 5, 1), new DateTime(2021, 6, 1) });
            limit.Should().Be(100);
        }
    }
}
