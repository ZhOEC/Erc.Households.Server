using Erc.Households.CalculateStrategies.Core;
using Erc.Households.CalculateStrategies.ElectricPower;
using Erc.Households.Domain.Shared.Billing;
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
                .Where(x => x.Name == "GetConsumptionMonthLimit" && x.IsPrivate)
                .First();

            var limit = (int)method.Invoke(calculateStrategy, new object[] { _twoBlocksTariff.Rates.First(tr => tr.HeatingConsumptionLimit.HasValue), new DateTime(2021, 2, 1), new DateTime(2021, 3, 1) });
            limit.Should().Be(3000);

            limit = (int)method.Invoke(calculateStrategy, new object[] { _twoBlocksTariff.Rates.First(tr => tr.HeatingConsumptionLimit.HasValue), new DateTime(2021, 5, 1), new DateTime(2021, 6, 1) });
            limit.Should().Be(100);
        }

        [Fact]
        public void Test_exemption_limit_for_heating_full_month()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);

            MethodInfo method = typeof(ElectricPowerCalculateStrategy)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.Name == "GetExemtionLimit" && x.IsPrivate)
                .First();

            var limit = (int)method.Invoke(calculateStrategy, new object[] {
                new ExemptionData
                {
                    IsElectricHeatig = true,
                    HeatingCorrection = 1.094m,
                    NumberOfPeople = 4,
                    ExemptionDiscountNorms = new ExemptionDiscountNorms[]
                    {
                        new ExemptionDiscountNorms(
                            effectiveDate: new DateTime(2019, 1, 1),
                            baseUnits: 110,
                            baseUnitsWithoutHotWater: 130,
                            basePerson: 1,
                            unitsPerPerson:30,
                            maxUnits: 230,
                            maxUnitsWithoutHotWater: 250,
                            baseSquareMeter: 10.5m,
                            squareMeterPerPerson: 21m,
                            unitsPerSquareMeter: 30)
                    }
                }, new DateTime(2021, 2, 1), new DateTime(2021, 3, 1) });
            limit.Should().Be(3101);
        }

        [Fact]
        public void Test_exemption_limit_without_water_supply_full_month()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);

            MethodInfo method = typeof(ElectricPowerCalculateStrategy)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.Name == "GetExemtionLimit" && x.IsPrivate)
                .First();

            var limit = (int)method.Invoke(calculateStrategy, new object[] {
                new ExemptionData
                {
                    IsElectricHeatig = false,
                    HeatingCorrection = 1.094m,
                    NumberOfPeople = 5,
                    IsCentralizedHotWaterSupply = false,
                    CanBeUsedElectricWaterHeater = false,
                    IsGasWaterHeaterInstalled = false,
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
                }, new DateTime(2021, 2, 1), new DateTime(2021, 3, 1) });
            limit.Should().Be(190);
        }

        [Fact]
        public void Test_exemption_limit_with_water_supply_full_month()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);

            MethodInfo method = typeof(ElectricPowerCalculateStrategy)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.Name == "GetExemtionLimit" && x.IsPrivate)
                .First();

            var limit = (int)method.Invoke(calculateStrategy, new object[] {
                new ExemptionData
                {
                    IsElectricHeatig = false,
                    HeatingCorrection = 1.094m,
                    NumberOfPeople = 6,
                    IsCentralizedHotWaterSupply = false,
                    CanBeUsedElectricWaterHeater = true,
                    IsGasWaterHeaterInstalled = false,
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
                            maxUnitsWithoutHotWater: 220,
                            baseSquareMeter: 10.5m,
                            squareMeterPerPerson: 21m,
                            unitsPerSquareMeter: 30)
                    }
                }, new DateTime(2021, 2, 1), new DateTime(2021, 3, 1) });
            limit.Should().Be(220);
        }

        [Fact]
        public void Test_exemption_limit_with_water_supply_and_gas_heater_full_month()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);

            MethodInfo method = typeof(ElectricPowerCalculateStrategy)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.Name == "GetExemtionLimit" && x.IsPrivate)
                .First();

            var limit = (int)method.Invoke(calculateStrategy, new object[] {
                new ExemptionData
                {
                    IsElectricHeatig = false,
                    HeatingCorrection = 1.094m,
                    NumberOfPeople = 2,
                    IsCentralizedHotWaterSupply = false,
                    CanBeUsedElectricWaterHeater = true,
                    IsGasWaterHeaterInstalled = true,
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
                            maxUnitsWithoutHotWater: 220,
                            baseSquareMeter: 10.5m,
                            squareMeterPerPerson: 21m,
                            unitsPerSquareMeter: 30)
                    }
                }, new DateTime(2021, 2, 1), new DateTime(2021, 3, 1) });
            limit.Should().Be(100);
        }

        [Fact]
        public void Test_exemption_limit_for_heating_october()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);

            MethodInfo method = typeof(ElectricPowerCalculateStrategy)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.Name == "GetExemtionLimit" && x.IsPrivate)
                .First();

            var limit = (int)method.Invoke(calculateStrategy, new object[] {
                new ExemptionData
                {
                    IsElectricHeatig = true,
                    HeatingCorrection = 1.094m,
                    NumberOfPeople = 4,
                    ExemptionDiscountNorms = new ExemptionDiscountNorms[]
                    {
                        new ExemptionDiscountNorms(
                            effectiveDate: new DateTime(2019, 1, 1),
                            baseUnits: 110,
                            baseUnitsWithoutHotWater: 130,
                            basePerson: 1,
                            unitsPerPerson:30,
                            maxUnits: 230,
                            maxUnitsWithoutHotWater: 250,
                            baseSquareMeter: 10.5m,
                            squareMeterPerPerson: 21m,
                            unitsPerSquareMeter: 30)
                    }
                }, new DateTime(2021, 10, 1), new DateTime(2021, 11, 1) });
            limit.Should().Be(1601);
        }

        [Fact]
        public void Test_exemption_limit_for_heating_april()
        {
            var calculateStrategy = new ElectricPowerCalculateStrategy(null);

            MethodInfo method = typeof(ElectricPowerCalculateStrategy)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(x => x.Name == "GetExemtionLimit" && x.IsPrivate)
                .First();

            var limit = (int)method.Invoke(calculateStrategy, new object[] {
                new ExemptionData
                {
                    IsElectricHeatig = true,
                    HeatingCorrection = 1.094m,
                    NumberOfPeople = 4,
                    ExemptionDiscountNorms = new ExemptionDiscountNorms[]
                    {
                        new ExemptionDiscountNorms(
                            effectiveDate: new DateTime(2019, 1, 1),
                            baseUnits: 110,
                            baseUnitsWithoutHotWater: 130,
                            basePerson: 1,
                            unitsPerPerson:30,
                            maxUnits: 230,
                            maxUnitsWithoutHotWater: 250,
                            baseSquareMeter: 10.5m,
                            squareMeterPerPerson: 21m,
                            unitsPerSquareMeter: 30)
                    }
                }, new DateTime(2021, 4, 1), new DateTime(2021, 5, 1) });
            limit.Should().Be(1551);
        }
    }
}
