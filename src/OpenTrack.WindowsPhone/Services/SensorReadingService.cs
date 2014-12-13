using System;
using Windows.Devices.Sensors;
using Windows.UI.Xaml;

namespace OpenTrack.WindowsPhone.Services
{

    public class SensorReadingService
    {
        private Inclinometer _inclinometer;

        public SensorReadingService()
        {
            _inclinometer = Inclinometer.GetDefault();
        }

        public InclinometerReading GetCurrentReading()
        {
            InclinometerReading reading = _inclinometer.GetCurrentReading();
            return reading;
        }

    }
}
