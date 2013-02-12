using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HubDemo
{
    [HubName("hitCounter")]
    public class HitCounterHub : Hub
    {
        static int _hitCount;

        public void RecordHit()
        {
            _hitCount += 1;
            Clients.All.receiveHit(_hitCount);
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            _hitCount -= 1;
            Clients.All.receiveHit(_hitCount);
            return base.OnDisconnected();
        }
    }
}