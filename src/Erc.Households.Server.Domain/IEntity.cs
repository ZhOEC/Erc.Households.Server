using Erc.Households.Server.Events;
using System.Collections.Generic;

namespace Erc.Households.Server.Domain
{
    public interface IEntity
    {
        ICollection<IEvent> Events { get; }
    }
}
