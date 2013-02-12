using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostingDemo.Worker
{
    public class OwinHost
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapHubs();
        }
    }
}
