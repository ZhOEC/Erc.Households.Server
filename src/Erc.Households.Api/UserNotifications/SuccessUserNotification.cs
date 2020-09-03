using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.UserNotifications
{
    public class SuccessUserNotification : IUserNotification
    {
        public string Title { get; set; }
        public string Type { get; } = "success";
        public string Text { get; set; }
        public string UiModule { get; set ; }
    }
}
