using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using OpenTrack.WindowsPhone.Services.SensorReaders;

namespace OpenTrack.WindowsPhone.Providers
{
    public class SettingsProvider
    {
        public string OpenTrackIp
        {
            get { return GetStringValue("OpenTrackIp"); }
            set { ApplicationData.Current.LocalSettings.Values["OpenTrackIp"] = value; }
        }

        public string OpenTrackPort
        {
            get { return GetStringValue("OpenTrackPort"); }
            set { ApplicationData.Current.LocalSettings.Values["OpenTrackPort"] = value; }
        }
        public int RefreshRate
        {
            get { return GetIntValue("RefreshRate"); }
            set { ApplicationData.Current.LocalSettings.Values["RefreshRate"] = value; }
        }
        public SensorReaderType SensorType
        {
            get { return GetEnumValue<SensorReaderType>("SensorType"); }
            set { ApplicationData.Current.LocalSettings.Values["SensorType"] = value.ToString(); }
        }

        private T GetEnumValue<T>(string key) where T : struct
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                return (T) Enum.Parse(typeof (T), ApplicationData.Current.LocalSettings.Values[key].ToString());
            return default(T);
        }

        private string GetStringValue(string key)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(key) ? ApplicationData.Current.LocalSettings.Values[key].ToString() : "";
        }

        private int GetIntValue(string key)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(key) ? (int)ApplicationData.Current.LocalSettings.Values[key] : 0;
        }
    }
}
