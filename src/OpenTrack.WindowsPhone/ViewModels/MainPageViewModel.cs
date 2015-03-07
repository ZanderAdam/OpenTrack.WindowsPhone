using System;
using Windows.Devices.Sensors;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Caliburn.Micro;
using OpenTrack.WindowsPhone.Providers;
using OpenTrack.WindowsPhone.Services;
using OpenTrack.WindowsPhone.Services.SensorReaders;

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
        private SensorReadingViewModel _sensorReadingViewModel;

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

        public SensorReadingViewModel SensorReading
        {
            get
            {
                return _sensorReadingViewModel;
            }
            set
            {
                _sensorReadingViewModel = value;
                NotifyOfPropertyChange(() => SensorReading);
            }
        }

        public MainPageViewModel(OpenTrackService openTrackService, SettingsProvider settingsProvider)
        {
            _openTrackService = openTrackService;
            _settingsProvider = settingsProvider;
            _openTrackService.NewReadingEvent += NewReadingEvent;

            _openTrackService.ExceptionEvent += ExceptionEvent;
            _sensorReadingViewModel = new SensorReadingViewModel();

            ReadSettings();
        }

        private void ExceptionEvent(Exception exception)
        {
            EnableInterface();

            var messageDialog = new MessageDialog(exception.Message);
            messageDialog.ShowAsync();
        }

        private void NewReadingEvent(SensorReading reading)
        {
            _sensorReadingViewModel.Yaw = String.Format("Yaw: {0}", reading.YawDegrees);
            _sensorReadingViewModel.Pitch = String.Format("Pitch: {0}", reading.PitchDegrees);
            _sensorReadingViewModel.Roll = String.Format("Roll: {0}", reading.RollDegrees);
            _sensorReadingViewModel.Accuracy = String.Format("Accuracy: {0}", reading.Accuracy);
        }

        public void StartPolling()
        {
            EnableInterface(false);

            _openTrackService.Start(OpenTrackIp, OpenTrackPort, RefreshRate, SensorReaderType.Gyroscope);

            SaveSettings();
        }


        public void EndPolling()
        {
            EnableInterface();

            _openTrackService.Stop();
        }

        private void EnableInterface(bool enable = true)
        {
            IsInputEnabled = enable;
            IsPolling = !enable;
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
