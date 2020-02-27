using System;
using System.Collections.Generic;

namespace Erc.Households.Server.ModelLogs
{
    public abstract class LogableObjectBase : ILogable
    {
        readonly List<ObjectLog> _logs = new List<ObjectLog>();

        public IReadOnlyCollection<ObjectLog> Logs => _logs.AsReadOnly();

        protected void AddLog(string operation, DateTime date, string user) => _logs.Add(new ObjectLog("Open", DateTime.Now, user));
    }
}
