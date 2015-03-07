using System;
using Windows.UI.Popups;
using Caliburn.Micro;
using OpenTrack.WindowsPhone.Services;
using OpenTrack.WindowsPhone.Services.SensorReaders;

namespace OpenTrack.WindowsPhone.ViewModels
{
    public class MainPageViewModel : PropertyChangedBase
    {
        private readonly OpenTrackService _openTrackService;
        private SensorReadingViewModel _sensorReadingViewModel;
        private SettingsViewModel _settingsViewModel;

        private bool _isInputEnabled = true;
        private bool _isPolling;

        public bool IsInputEnabled
        {
            get { return _isInputEnabled; }
            set
            {
                _isInputEnabled = value;
                NotifyOfPropertyChange(() => IsInputEnabled);
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

        public SettingsViewModel Settings
        {
            get { return _settingsViewModel; }
            set
            {
                _settingsViewModel = value;
                NotifyOfPropertyChange(() => Settings);
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

        public bool CanStartPolling
        {
            get
            {
                return !String.IsNullOrEmpty(Settings.OpenTrackIp)
                       && !String.IsNullOrEmpty(Settings.OpenTrackPort)
                       && !IsPolling;
            }
        }

        public bool CanEndPolling
        {
            get { return IsPolling; }
        }

        public MainPageViewModel(OpenTrackService openTrackService, SettingsViewModel settingsViewModel,
            SensorReadingViewModel sensorReadingViewModel)
        {
            _openTrackService = openTrackService;
            _openTrackService.NewReadingEvent += NewReadingEvent;
            _openTrackService.ExceptionEvent += ExceptionEvent;

            Settings = settingsViewModel;
            SensorReading = sensorReadingViewModel;

            SubscribeToSettingsChanged();
        }

        private void SubscribeToSettingsChanged()
        {
            Settings.PropertyChanged += (sender, args) =>
            {
                NotifyOfPropertyChange(() => CanStartPolling);
                NotifyOfPropertyChange(() => CanEndPolling);
            };
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

            _openTrackService.Start(Settings.OpenTrackIp, Settings.OpenTrackPort, Settings.RefreshRate, Settings.SelectedSensorType);

            Settings.Save();
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
    }
}
