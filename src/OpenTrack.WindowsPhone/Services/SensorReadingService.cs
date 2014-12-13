using System;
using Windows.Devices.Sensors;
using Windows.UI.Xaml;

namespace OpenTrack.WindowsPhone.Services
{
    public delegate void NewReadingEventHandler(InclinometerReading reading);

    public class SensorReadingService
    {
        private Inclinometer _inclinometer;
        private DispatcherTimer _timer;

        public event NewReadingEventHandler NewReadingEvent;

        public SensorReadingService()
        {
            _inclinometer = Inclinometer.GetDefault();
            _timer = new DispatcherTimer();
            _timer.Tick += ReadSensor;
        }

        public void StartReading(int delay)
        {
            _timer.Interval = TimeSpan.FromMilliseconds(delay);
            _timer.Start();
        }

        public void StopReading()
        {
            _timer.Stop();
        }

        private void ReadSensor(object sender, object o)
        {
            NewReadingEvent(GetCurrentReading());
        }

        private InclinometerReading GetCurrentReading()
        {
            InclinometerReading reading = _inclinometer.GetCurrentReading();
            return reading;
        }

    }
}
