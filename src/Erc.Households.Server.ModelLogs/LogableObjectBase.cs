using System;
using System.Collections.Generic;

namespace Erc.Households.Server.ModelLogs
{
    public abstract class LogableObjectBase : ILogable
    {
        readonly List<ObjectLog> _logs = new List<ObjectLog>();

        public IReadOnlyCollection<ObjectLog> Logs => _logs.AsReadOnly();

        protected void AddLog(string operation, string user) => _logs.Add(new ObjectLog(operation, DateTime.Now, user));
    }
}
