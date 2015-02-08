using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;

namespace OpenTrack.WindowsPhone.Services
{
    public delegate void NewReadingEventHandler(SensorReading reading);

    public class OpenTrackService
    {
        private readonly DispatcherTimer _timer;
        public event NewReadingEventHandler NewReadingEvent;
        private readonly SensorReadingService _sensorReadingService;
        private string _ipAddress;
        private string _port;

        public OpenTrackService(SensorReadingService sensorReadingService)
        {
            _sensorReadingService = sensorReadingService;

            _timer = new DispatcherTimer();
            _timer.Tick += SendNewPosition;
        }

        public void Start(string ipAddress, string port, int refreshRate)
        {
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
            NewReadingEvent(_sensorReadingService.GetCurrentReading());

            SendMessage(newSensorReading);
        }

        private async void SendMessage(SensorReading newSensorReading)
        {
            var socket = new DatagramSocket();

            var message = BuildMessage(newSensorReading);

            using (var stream = await socket.GetOutputStreamAsync(new HostName(_ipAddress), _port))
            {
                await stream.WriteAsync(message.AsBuffer());
                await stream.FlushAsync();
            }
        }

        private byte[] BuildMessage(SensorReading newSensorReading)
        {
            if (newSensorReading.HasInclinometer)
                return BuildInclinometerMessage(newSensorReading.InclinometerReading);

            return BuildAccelerometerMessage(newSensorReading.AccelerometerReading);
        }

        private byte[] BuildAccelerometerMessage(AccelerometerReading accelerometerReading)
        {
            var x = accelerometerReading.AccelerationX;
            var y = accelerometerReading.AccelerationY;
            var z = accelerometerReading.AccelerationZ;

            var yawDegrees = Math.Atan2(x, y)*180/Math.PI;
            var pitchDegrees = Math.Atan2(-x, Math.Sqrt(y*y + z + z))*180/Math.PI;
            var rollDegrees = Math.Atan2(y, z)*180/Math.PI;

            return new double[]
            {
                0,
                0,
                0,
                yawDegrees,
                pitchDegrees,
                rollDegrees
            }.SelectMany(BitConverter.GetBytes).ToArray();
        }

        private byte[] BuildInclinometerMessage(InclinometerReading inclinometerReading)
        {
            return new double[]
            {
                0,
                0,
                0,
                inclinometerReading.YawDegrees,
                inclinometerReading.PitchDegrees,
                inclinometerReading.RollDegrees
            }.SelectMany(BitConverter.GetBytes).ToArray();
        }
    }
}
