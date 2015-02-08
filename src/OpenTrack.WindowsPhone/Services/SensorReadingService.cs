using System;
using Windows.Devices.Sensors;
using Windows.UI.Xaml;

namespace OpenTrack.WindowsPhone.Services
{

    public class SensorReadingService
    {
        private Inclinometer _inclinometer;
        private Accelerometer _accelerometer;

        public SensorReadingService()
        {
            _inclinometer = null; //Inclinometer.GetDefault();

            if (_inclinometer == null)
            {
                _accelerometer = Accelerometer.GetDefault();
            }
        }

        public SensorReading GetCurrentReading()
        {
            InclinometerReading inclinometerReading = null;
            AccelerometerReading accelerometerReading = null;

            if(_inclinometer != null)
                inclinometerReading = _inclinometer.GetCurrentReading();
            else
                accelerometerReading = _accelerometer.GetCurrentReading();

            return new SensorReading()
            {
                InclinometerReading = inclinometerReading,
                AccelerometerReading = accelerometerReading
            };
        }

    }

    public class SensorReading
    {
        public InclinometerReading InclinometerReading { get; set; }
        public AccelerometerReading AccelerometerReading { get; set; }

        public bool HasInclinometer
        {
            get { return InclinometerReading != null; }
        }
    }
}
