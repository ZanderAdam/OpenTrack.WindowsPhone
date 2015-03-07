using Windows.Devices.Sensors;

namespace OpenTrack.WindowsPhone.Services.SensorReaders
{
    public class GyroscopeReadingService : ISensorReadingService
    {
        private readonly Inclinometer _inclinometer;

        public GyroscopeReadingService()
        {
            _inclinometer = Inclinometer.GetDefault();
        }

        public SensorReading GetCurrentReading()
        {
            var reading = _inclinometer.GetCurrentReading();

            return new SensorReading()
            {
                YawDegrees = reading.YawDegrees,
                PitchDegrees = reading.PitchDegrees,
                RollDegrees = reading.RollDegrees,
                Accuracy = reading.YawAccuracy.ToString()
            };
        }

    }
}
