using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.UserNotifications
{
    public interface IUserNotification
    {
        string Title { get; set; }
        string Type { get; }
        string Text { get; set; }
        string UiModule { get; set; }
    }
}
