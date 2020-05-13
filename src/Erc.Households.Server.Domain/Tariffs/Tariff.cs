using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Tariffs
{
    public class Tariff
    {
        ICollection<TariffRate> _rates = new HashSet<TariffRate>();

        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TariffRate> Rates => _rates;

        public void AddRate(TariffRate tariffRate) => _rates.Add(tariffRate);
        public void RemoveRate(TariffRate tariffRate) => _rates.Remove(tariffRate);
    }
}
