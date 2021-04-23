using Erc.Households.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.Shared.Tariffs
{
    public class Tariff
    {
        List<TariffRate> _rates = new List<TariffRate>();

        public int Id { get; set; }
        public string Name { get; set; }
        public Commodity Commodity { get; set; }
        public IEnumerable<TariffRate> Rates
        {
            get => _rates?.OrderByDescending(tr => tr.StartDate);
            set => _rates = value.ToList();
        }

        public void AddRate(TariffRate tariffRate)
        {
            if (_rates is null)
                _rates = new List<TariffRate>();
            else
                _rates = new List<TariffRate>(_rates);

            tariffRate.Id = (_rates.Max(t => t?.Id) ?? 0) + 1;
            tariffRate.StartDate = tariffRate.StartDate.Date;
            _rates.Add(tariffRate);
        }

        public void UpdateRate(TariffRate tariffRate)
        {
            var rate = _rates.First(tr => tr.Id == tariffRate.Id);
            _rates.Remove(rate);
            _rates.Add(tariffRate);
            _rates = new List<TariffRate>(_rates);
        }

        public void RemoveRate(int id) 
        {
            var rate = _rates.FirstOrDefault(tr => tr.Id == id);
            if (rate != null)
                _rates.Remove(rate);
            _rates = new List<TariffRate>(_rates);
        }
    }
}
