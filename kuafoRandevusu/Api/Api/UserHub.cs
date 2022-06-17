using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class UserHub : Microsoft.AspNet.SignalR.Hub
    {
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<UserHub>();
            context.Clients.All.displayStatus();
        }
    }
}
