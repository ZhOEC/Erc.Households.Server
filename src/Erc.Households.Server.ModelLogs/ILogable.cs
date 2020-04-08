using System.Collections.Generic;

namespace Erc.Households.Server.ModelLogs
{
    public interface ILogable
    {
        IReadOnlyCollection<ObjectLog> Logs { get; }
    }
}
