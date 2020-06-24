using Erc.Households.Events;
using System.Collections.Generic;

namespace Erc.Households.Domain
{
    public interface IEntity
    {
        int Id { get; }
        ICollection<IEntityEvent> Events { get; }
    }
}
