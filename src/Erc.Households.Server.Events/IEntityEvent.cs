using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Events
{
    public interface IEntityEvent : IEvent
    {
        int Id { get; set; }
    }
}
