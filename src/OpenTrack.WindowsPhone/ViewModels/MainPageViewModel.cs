using System;
using Windows.Devices.Sensors;
using Windows.UI.Xaml;
using Caliburn.Micro;
using OpenTrack.WindowsPhone.Services;

namespace OpenTrack.WindowsPhone.ViewModels
{
    public class MainPageViewModel : PropertyChangedBase
    {
        private readonly SensorReadingService _sensorReadingService;
        private bool _isInputEnabled = true;
        private bool _isPolling;
        private string _openTrackIp;
        private string _openTrackPort;
        private int _refreshRate = 100;
        private string _yaw;
        private string _pitch;
        private string _roll;
        private string _accuracy;

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
        public string Yaw
        {
            get { return _yaw; }
            set
            {
                _yaw = value; 
                NotifyOfPropertyChange(() => Yaw);
            }
        }
        public string Pitch
        {
            get { return _pitch; }
            set
            {
                _pitch = value;
                NotifyOfPropertyChange(() => Pitch);
            }
        }
        public string Roll
        {
            get { return _roll; }
            set
            {
                _roll = value;
                NotifyOfPropertyChange(() => Roll);
            }
        }
        public string Accuracy
        {
            get { return _accuracy; }
            set
            {
                _accuracy = value;
                NotifyOfPropertyChange(() => Accuracy);
            }
        }

        public MainPageViewModel(SensorReadingService sensorReadingService)
        {
            _sensorReadingService = sensorReadingService;
            _sensorReadingService.NewReadingEvent += NewReadingEvent;
        }

        private void NewReadingEvent(InclinometerReading reading)
        {
            Yaw = String.Format("Yaw: {0}", reading.YawDegrees);
            Pitch = String.Format("Pitch: {0}", reading.PitchDegrees);
            Roll = String.Format("Roll: {0}", reading.RollDegrees);
            Accuracy = String.Format("Accuracy: {0}", reading.YawAccuracy);
        }

        public void StartPolling()
        {
            IsInputEnabled = false;
            IsPolling = true;

            _sensorReadingService.StartReading(RefreshRate);
        }


        public void EndPolling()
        {
            IsInputEnabled = true;
            IsPolling = false;

            _sensorReadingService.StopReading();
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
