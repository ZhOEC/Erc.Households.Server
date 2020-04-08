using System;

namespace Erc.Households.Server.ModelLogs
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

        public string Operation { get; set; }
        public DateTime Time { get; set; }
        public string User { get; set; }
    }
}
