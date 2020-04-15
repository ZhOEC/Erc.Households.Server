using Erc.Households.Server.Events;
using System.Collections.Generic;

namespace Erc.Households.Server.Domain
{
    public interface IEntity
    {
        int Id { get; }
        ICollection<IEntityEvent> Events { get; }
    }
}
