using Microsoft.AspNet.SignalR.Client.Hubs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using System;
using Windows.UI.Core;

namespace SiteMonitR.WindowsClient
{
    public sealed partial class MainPage : Page
    {
        const string API_URL = "http://localhost:51868/";
        MainPageViewModel _viewModel;
        HubConnection _connection;
        IHubProxy _hub;

        public MainPage()
        {
            this.InitializeComponent();

            _viewModel = new MainPageViewModel();
            this.DataContext = _viewModel;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_connection == null)
            {
                _connection = new HubConnection(API_URL);
                _hub = _connection.CreateHubProxy("SiteMonitorHub");

                _hub.On<dynamic>("receiveSiteStatus", (siteStatus) =>
                    {
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                if (!_viewModel.Sites.Any(x => x.Id == (int)siteStatus.SiteId))
                                {
                                    _viewModel.Sites.Add(new MonitoredSite
                                    {
                                        Status = siteStatus.Status,
                                        Id = siteStatus.SiteId,
                                        Url = siteStatus.Url
                                    });
                                }
                                else
                                {
                                    _viewModel.Sites.First(x => x.Id == 
                                        (int)siteStatus.SiteId).Status = siteStatus.Status;
                                }
                            });
                    });
            }

            _connection.Start().ContinueWith((t) =>
                {
                    var faulted = t.IsFaulted;
                    if(!faulted)
                        _hub.Invoke("StartMonitoring");
                });
        }
    }
}
