using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.ModelLogs
{
    public abstract class LogableObjectBase : ILogable
    {
        protected List<ObjectLog> _logs;

        protected LogableObjectBase()
        {
            _logs = new List<ObjectLog>();
        }

        public IReadOnlyCollection<ObjectLog> Logs 
        { 
            get => _logs;
            private set { _logs = value.ToList(); } 
        }
        

        protected void AddLog(string operation, string user) => _logs.Add(new ObjectLog(operation, DateTime.Now, user));
    }
}
