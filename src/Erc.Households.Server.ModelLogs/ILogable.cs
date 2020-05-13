using System.Collections.Generic;

namespace Erc.Households.ModelLogs
{
    public interface ILogable
    {
        IReadOnlyCollection<ObjectLog> Logs { get; }
    }
}
