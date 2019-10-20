using System;
using System.IO;
using System.Collections.Generic;

namespace RainDrop
{
    public static class Settings
    {
        private static RainDropSettings _rainDropSettings;

        public static string Get(string key)
        {
            CheckFile();
            return _rainDropSettings.Get(key);
        }

        public static float GetFloat(string key, float defValue)
        {
            CheckFile();
            if(_rainDropSettings.Get(key) != null)
            {
                return float.Parse(_rainDropSettings.Get(key));
            }
            else
            {
                return defValue;
            }
        }

        public static int GetInt(string key, int defValue)
        {
            CheckFile();
            if (_rainDropSettings.Get(key) != null)
            {
                return int.Parse(_rainDropSettings.Get(key));
            }
            else
            {
                return defValue;
            }
        }

        private static void CheckFile()
        {
            if (_rainDropSettings == null)
            {
                _rainDropSettings = new RainDropSettings();
            }
        }
    }

    public class RainDropSettings
    {
        private string _settingsFilePath = "raindrop.settings";
        private Dictionary<string, string> _settings = new Dictionary<string, string>();

        public RainDropSettings()
        {
            LoadSettingsFile();
        }

        private void LoadSettingsFile()
        {
            string settingsFile;
            if (File.Exists(_settingsFilePath))
            {
                StreamReader r = new StreamReader(_settingsFilePath);
                settingsFile = r.ReadToEnd();
                r.Close();

                string[] lines = settingsFile.Split('\n');
                foreach(string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    if (line[0] == '#')
                    {
                        continue;
                    }
                    string cleanLine = line.Trim();
                    string key = cleanLine.Split('=')[0].Trim();
                    string value = cleanLine.Split('=')[1].Trim();
                    
                    if (value != "[value]")
                    {
                        _settings.Add(key, value);
                    }
                }
            }
        }

        public string Get(string key)
        {
            if (_settings.ContainsKey(key))
            {
                return _settings[key];
            }
            else return null;
        }
    }
}
