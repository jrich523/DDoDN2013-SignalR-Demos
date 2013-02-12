using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MappingSite
{
    public class MappingHub : Hub
    {
        public void CheckIn(dynamic location)
        {
            Clients.All.showCheckIn(location);
        }
    }
}