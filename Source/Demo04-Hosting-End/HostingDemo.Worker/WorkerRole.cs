using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.Owin.Hosting;

namespace HostingDemo.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private IDisposable server;

        public override void Run()
        {
            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override void OnStop()
        {
            if (server != null)
                server.Dispose();

            base.OnStop();
        }

        public override bool OnStart()
        {
            server = WebApplication
                .Start<OwinHost>("http://localhost:8080");

            return base.OnStart();
        }
    }
}
