using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostingDemo.Worker
{
    public class MappingHub : Hub
    {
        public void CheckIn(dynamic location)
        {
            Clients.All.showCheckIn(location);
        }
    }
}
