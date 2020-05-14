using System;

namespace Erc.Households.ModelLogs
{
    public class ObjectLog
    {
        protected ObjectLog()
        {

        }

        public ObjectLog(string operation, DateTime time, string user)
        {
            Operation = operation;
            Time = time;
            User = user;
        }

        public string Operation { get; private set; }
        public DateTime Time { get; private set; }
        public string User { get; private set; }
    }
}
