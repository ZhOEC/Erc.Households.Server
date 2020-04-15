using Erc.Households.Server.Core;
using Erc.Households.Server.DataAccess.EF;
using Erc.Households.Server.Domain;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Server.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ErcContext _ercContext;
        readonly IBus _bus;

        public IAccountingPointRepository AccountingPointRepository => new AccountingPointRepository(_ercContext);

        public UnitOfWork(ErcContext ercContext, IBus bus)
        {
            _ercContext = ercContext;
            _bus = bus;
        }

        public async Task<int> SaveWorkAsync()
        {
            var domainEventEntities = _ercContext.ChangeTracker.Entries<IEntity>()
                .Select(po => po.Entity)
                .Where(po => po.Events.Any())
                .ToArray();

            var rows = await _ercContext.SaveChangesAsync();

            foreach (var entity in domainEventEntities)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var @event in events)
                {
                    @event.Id = entity.Id;
                    await _bus.Publish(@event, @event.GetType());
                }
            }

            return rows;
        }
    }
}
