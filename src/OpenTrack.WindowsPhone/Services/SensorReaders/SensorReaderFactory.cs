namespace OpenTrack.WindowsPhone.Services.SensorReaders
{
    public class SensorReaderFactory
    {
        public ISensorReadingService GetSensorReadingService(SensorReaderType type)
        {
            switch (type)
            {
                //case SensorReaderType.OtherType:
                    //return new OtherTypeReadingService();
                default:
                    return new GyroscopeReadingService();
            }
        }
    }

    public enum SensorReaderType
    {
        Gyroscope
    }
}
