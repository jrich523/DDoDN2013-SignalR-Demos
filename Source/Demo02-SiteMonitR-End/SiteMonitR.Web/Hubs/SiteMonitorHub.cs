using Microsoft.AspNet.SignalR;
using SiteMonitR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Timers;
using System.Web;

namespace SiteMonitR.Web.Hubs
{
    public class SiteMonitorHub : Hub
    {
        public Timer Timer { get; set; }

        public void StartMonitoring()
        {
            if (Timer == null)
            {
                Timer = new Timer();
                Timer.Interval = 3000;
                Timer.Elapsed += OnTimerElapsed;
                Timer.Start();
            }
        }

        void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Timer.Stop();

            using (var db = new SiteMonitRContext())
            {
                var sites = db.MonitoredSites;

                foreach (var site in sites)
                {
                    var result = new SiteStatusResult
                    {
                        SiteId = site.Id,
                        Url = site.Url,
                        Status = "Checking"
                    };

                    Clients.All.receiveSiteStatus(result);

                    try
                    {
                        new WebClient().DownloadString(site.Url);
                        result.Status = SiteStatus.Up.ToString();
                    }
                    catch
                    {
                        result.Status = SiteStatus.Down.ToString();
                    }

                    Clients.All.receiveSiteStatus(result);
                }
            }

            Timer.Start();
        }
    }
}