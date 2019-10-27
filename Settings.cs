using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

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

    class SaveFile
    {
        private string _fileName = "raindrop.save";
        private string _sharedSecret = "happygo  lucky bears with icecream sdfasdfasd 12343489 on the china";
        private string _fileContents;
        private SaveFileStructure _contents = new SaveFileStructure();
        private bool _encrypted;

        public SaveFileStructure Contents
        {
            get { return _contents; }
        }

        public SaveFile(bool encrypted = false)
        {
            _encrypted = encrypted;
            OpenFile();
        }

        private void OpenFile()
        {
            StreamReader r;
            if (File.Exists(_fileName))
            {
                r = new StreamReader(_fileName);
                string contents = r.ReadToEnd();
                r.Close();
                if(_encrypted)
                {
                    if (contents != null && contents.Length > 0)
                    {
                        contents = Crypto.DecryptStringAES(contents, _sharedSecret);
                    }
                }

                _fileContents = contents;
                _contents = JsonConvert.DeserializeObject<SaveFileStructure>(contents);
            }
            else
            {
                _contents = new SaveFileStructure();
            }
        }

        public void Save()
        {
            _contents.LastEdit = DateTime.Now;
            StreamWriter w = new StreamWriter(_fileName);
            w.WriteLine(JsonConvert.SerializeObject(_contents));
            w.Close();
        }
    }

    class SaveFileStructure
    {
        public DateTime LastEdit;
        public int Orbs;
    }
}
