using System.IO;
using Newtonsoft.Json;

namespace TourPlannerApp.Models
{
    public class SettingsManager
    {
        private static SettingsManager _settingsManager { get; set; } = null;

        private static object _lockobject = new object();

        private Settings _currentSettings { get; }
        
        private SettingsManager()
        {
            _currentSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("appsettings.json"));
        }

        public static Settings GetSettings()
        {
            // threadsafety reasons
            lock (_lockobject)
            {
                _settingsManager ??= new SettingsManager();
            }

            return _settingsManager._currentSettings;
        }
        

    }
}