using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace SiteMonitR.WindowsClient
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(this, propertyName);
        }

        protected void OnPropertyChanged(object sender, string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MonitoredSite : BaseViewModel
    {
        private int _id;

        [JsonProperty("SiteId")]
        public int Id
        {
            get { return _id; }
            set 
            { 
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _url;

        [JsonProperty]
        public string Url
        {
            get { return _url; }
            set 
            { 
                _url = value;
                OnPropertyChanged("Url");
            }
        }

        private string _status;

        [JsonProperty]
        public string Status
        {
            get { return _status; }
            set 
            { 
                _status = value;
                OnPropertyChanged("Status");
                var color = _status == "Up" 
                    ? Colors.DarkGreen 
                    : _status == "Checking" 
                        ? Colors.DarkGoldenrod 
                        : Colors.Red;
                this.Color = color; 
            }
        }

        Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }
    }

    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            this.Sites = new ObservableCollection<MonitoredSite>();
            this.Sites.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (MonitoredSite item in e.OldItems)
                {
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (MonitoredSite item in e.NewItems)
                {
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }       
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(sender, e.PropertyName);
        }

        private ObservableCollection<MonitoredSite> _sites;

        public ObservableCollection<MonitoredSite> Sites
        {
            get { return _sites; }
            set
            { 
                _sites = value;
                OnPropertyChanged("Sites");
            }
        }

    }
}
