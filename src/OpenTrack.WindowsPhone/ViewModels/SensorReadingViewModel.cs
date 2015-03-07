using Caliburn.Micro;

namespace OpenTrack.WindowsPhone.ViewModels
{
    public class SensorReadingViewModel: PropertyChangedBase
    {
        private string _yaw;
        private string _pitch;
        private string _roll;
        private string _accuracy;

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
    }
}