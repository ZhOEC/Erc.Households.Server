using Erc.Households.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.Shared.Tariffs
{
    public class Tariff
    {
        ICollection<TariffRate> _rates = new HashSet<TariffRate>();
        private readonly Action<object, string> LazyLoader;

        protected Tariff(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Commodity Commodity { get; set; }
        public IEnumerable<TariffRate> Rates
        {
            get => LazyLoader.Load(this, ref _rates);
            private set { _rates = value.ToList(); }
        }
        public void AddRate(TariffRate tariffRate) => _rates.Add(tariffRate);
        public void RemoveRate(TariffRate tariffRate) => _rates.Remove(tariffRate);
    }
}
