using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using OpenTrack.WindowsPhone.Providers;
using OpenTrack.WindowsPhone.Services.SensorReaders;

namespace OpenTrack.WindowsPhone.ViewModels
{
    public class SettingsViewModel: PropertyChangedBase
    {
        private readonly SettingsProvider _settingsProvider;
        private string _openTrackIp;
        private string _openTrackPort;
        private int _refreshRate = 20;
        private SensorReaderType _selectedSensorType;

        public SettingsViewModel(SettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
            ReadSettings();
        }

        public string OpenTrackIp
        {
            get { return _openTrackIp; }
            set
            {
                _openTrackIp = value;
                NotifyOfPropertyChange(() => OpenTrackIp);
            }
        }

        public string OpenTrackPort
        {
            get { return _openTrackPort; }
            set
            {
                _openTrackPort = value;
                NotifyOfPropertyChange(() => OpenTrackPort);
            }
        }

        public int RefreshRate
        {
            get { return _refreshRate; }
            set
            {
                _refreshRate = value;
                NotifyOfPropertyChange(() => RefreshRate);
            }
        }

        public IEnumerable<SensorReaderType> SensorType
        {
            get
            {
                return Enum.GetValues(typeof(SensorReaderType)).Cast<SensorReaderType>();
            }
        }

        public SensorReaderType SelectedSensorType
        {
            get { return _selectedSensorType; }
            set
            {
                _selectedSensorType = value; 
                NotifyOfPropertyChange(() => SelectedSensorType);
            }
        }

        public void ReadSettings()
        {
            OpenTrackIp = _settingsProvider.OpenTrackIp;
            OpenTrackPort = _settingsProvider.OpenTrackPort;
            RefreshRate = _settingsProvider.RefreshRate;
            SelectedSensorType = _settingsProvider.SensorType;
        }

        public void Save()
        {
            _settingsProvider.OpenTrackIp = OpenTrackIp;
            _settingsProvider.OpenTrackPort = OpenTrackPort;
            _settingsProvider.RefreshRate = RefreshRate;
            _settingsProvider.SensorType = SelectedSensorType;
        }
    }
}