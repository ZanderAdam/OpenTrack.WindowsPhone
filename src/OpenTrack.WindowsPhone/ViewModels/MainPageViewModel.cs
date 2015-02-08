using System;
using Windows.Devices.Sensors;
using Windows.UI.Xaml;
using Caliburn.Micro;
using OpenTrack.WindowsPhone.Providers;
using OpenTrack.WindowsPhone.Services;

namespace OpenTrack.WindowsPhone.ViewModels
{
    public class MainPageViewModel : PropertyChangedBase
    {
        private readonly OpenTrackService _openTrackService;
        private readonly SettingsProvider _settingsProvider;
        private bool _isInputEnabled = true;
        private bool _isPolling;
        private string _openTrackIp;
        private string _openTrackPort;
        private int _refreshRate = 100;
        private string _yaw;
        private string _pitch;
        private string _roll;
        private string _accuracy;
        private string _accelX;
        private string _accelY;
        private string _accelZ;

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

        public string AccelX
        {
            get { return _accelX; }
            set
            {
                _accelX = value;
                NotifyOfPropertyChange(() => AccelX);
            }
        }
        public string AccelY
        {
            get { return _accelY; }
            set
            {
                _accelY = value;
                NotifyOfPropertyChange(() => AccelY);
            }
        }
        public string AccelZ
        {
            get { return _accelZ; }
            set
            {
                _accelZ = value;
                NotifyOfPropertyChange(() => AccelZ);
            }
        }
        public MainPageViewModel(OpenTrackService openTrackService, SettingsProvider settingsProvider)
        {
            _openTrackService = openTrackService;
            _settingsProvider = settingsProvider;
            _openTrackService.NewReadingEvent += NewReadingEvent;

            ReadSettings();
        }

        private void NewReadingEvent(SensorReading sensorReading)
        {
            if (sensorReading.HasInclinometer)
            {
                Yaw = String.Format("Yaw: {0}", sensorReading.InclinometerReading.YawDegrees);
                Pitch = String.Format("Pitch: {0}", sensorReading.InclinometerReading.PitchDegrees);
                Roll = String.Format("Roll: {0}", sensorReading.InclinometerReading.RollDegrees);
                Accuracy = String.Format("Accuracy: {0}", sensorReading.InclinometerReading.YawAccuracy);
            }
            else
            {
                AccelX = String.Format("Accel X: {0}",sensorReading.AccelerometerReading.AccelerationX);
                AccelY = String.Format("Accel Y: {0}",sensorReading.AccelerometerReading.AccelerationY);
                AccelZ = String.Format("Accel Z: {0}", sensorReading.AccelerometerReading.AccelerationZ);
            }
        }

        public void StartPolling()
        {
            IsInputEnabled = false;
            IsPolling = true;

            _openTrackService.Start(OpenTrackIp, OpenTrackPort, RefreshRate);

            SaveSettings();
        }


        public void EndPolling()
        {
            IsInputEnabled = true;
            IsPolling = false;

            _openTrackService.Stop();
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

        private void ReadSettings()
        {
            OpenTrackIp = _settingsProvider.OpenTrackIp;
            OpenTrackPort = _settingsProvider.OpenTrackPort;
            RefreshRate = _settingsProvider.RefreshRate;
        }

        private void SaveSettings()
        {
            _settingsProvider.OpenTrackIp = OpenTrackIp;
            _settingsProvider.OpenTrackPort = OpenTrackPort;
            _settingsProvider.RefreshRate = RefreshRate;
        }
    }
}
