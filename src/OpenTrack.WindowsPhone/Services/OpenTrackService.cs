using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;
using OpenTrack.WindowsPhone.Services.SensorReaders;

namespace OpenTrack.WindowsPhone.Services
{
    public delegate void NewReadingEventHandler(SensorReading reading);
    public delegate void ExceptionHandler(Exception exception);

    public class OpenTrackService
    {
        private readonly DispatcherTimer _timer;

        public event NewReadingEventHandler NewReadingEvent;
        public event ExceptionHandler ExceptionEvent;

        private readonly SensorReaderFactory _sensorReaderFactory;
        private ISensorReadingService _sensorReadingService;

        private string _ipAddress;
        private string _port;

        public OpenTrackService(SensorReaderFactory sensorReaderFactory)
        {
            _sensorReaderFactory = sensorReaderFactory;
            _timer = new DispatcherTimer();
            _timer.Tick += SendNewPosition;
        }

        public void Start(string ipAddress, string port, int refreshRate, SensorReaderType sensorReaderType)
        {
            _sensorReadingService = _sensorReaderFactory.GetSensorReadingService(sensorReaderType);

            _ipAddress = ipAddress;
            _port = port;

            _timer.Interval = TimeSpan.FromMilliseconds(refreshRate);
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void SendNewPosition(object sender, object o)
        {
            var newSensorReading = _sensorReadingService.GetCurrentReading();
            NewReadingEvent(newSensorReading);

            SendMessage(newSensorReading);
        }

        private async void SendMessage(SensorReading newSensorReading)
        {
            try
            {
                _timer.Stop();

                var socket = new DatagramSocket();

                var message = BuildMessage(newSensorReading);

                using (var stream = await socket.GetOutputStreamAsync(new HostName(_ipAddress), _port))
                {
                    await stream.WriteAsync(message.AsBuffer());
                    await stream.FlushAsync();
                }

                _timer.Start();
            }
            catch (Exception ex)
            {
                ExceptionEvent(ex);
            }
        }

        private byte[] BuildMessage(SensorReading newSensorReading)
        {
            return new double[]
            {
                0,
                0,
                0,
                newSensorReading.YawDegrees,
                newSensorReading.PitchDegrees,
                newSensorReading.RollDegrees
            }.SelectMany(BitConverter.GetBytes).ToArray();
        }
    }
}
