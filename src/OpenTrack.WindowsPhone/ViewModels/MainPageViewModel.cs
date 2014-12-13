using System;
using Windows.UI.Xaml;
using Caliburn.Micro;

namespace OpenTrack.WindowsPhone.ViewModels
{
    public class MainPageViewModel : PropertyChangedBase
    {
        private bool _isInputEnabled = true;
        private bool _isPolling;
        private string _openTrackIp;
        private string _openTrackPort;
        private int _refreshRate = 100;

        public bool IsInputEnabled
        {
            get { return _isInputEnabled; }
            set
            {
                _isInputEnabled = value;
                NotifyOfPropertyChange(() => IsInputEnabled);
            }
        }
        public bool IsPolling
        {
            get { return _isPolling; }
            set
            {
                _isPolling = value;
                NotifyOfPropertyChange(() => IsPolling);
                NotifyOfPropertyChange(() => CanStartPolling);
                NotifyOfPropertyChange(() => CanEndPolling);
            }
        }
        public string OpenTrackIp
        {
            get { return _openTrackIp; }
            set
            {
                _openTrackIp = value;
                NotifyOfPropertyChange(() => OpenTrackIp);
                NotifyOfPropertyChange(() => CanStartPolling);
            }
        }
        public string OpenTrackPort
        {
            get { return _openTrackPort; }
            set
            {
                _openTrackPort = value;
                NotifyOfPropertyChange(() => OpenTrackPort);
                NotifyOfPropertyChange(() => CanStartPolling);
            }
        }
        public int RefreshRate
        {
            get { return _refreshRate; }
            set
            {
                _refreshRate = value;
                NotifyOfPropertyChange(() => RefreshRate);
                NotifyOfPropertyChange(() => CanStartPolling);
            }
        }

        public void StartPolling()
        {
            IsInputEnabled = false;
            IsPolling = true;
        }


        public void EndPolling()
        {
            IsInputEnabled = true;
            IsPolling = false;
        }

        public bool CanStartPolling
        {
            get
            {
                return !String.IsNullOrEmpty(OpenTrackIp)
                       && !String.IsNullOrEmpty(OpenTrackPort)
                       && !IsPolling;
            }
        }

        public bool CanEndPolling
        {
            get { return IsPolling; }
        }
    }
}
