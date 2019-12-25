using System.Collections.Generic;
using Godot;
using Guero;
using System;
using System.Diagnostics;

namespace RainDrop
{
    static class Global
    {
        public static HUD HUD { get; set; }
        public static Drop Drop { get; set; }
        public static SoundEffects SoundEffects {get; set;}
        public static string CurrentLocation { get; set; }
        public static string NextLocation
        {
            get
            {
                int index = _locations.IndexOf(CurrentLocation);
                if(index < 0 )
                {
                    throw new System.Exception($"Location: {CurrentLocation} is invalid!");
                }
                if(index + 1 == _locations.Count)
                {
                    return _locations[0];
                }
                return _locations[index+1];
            }
        }
        public static int FinalScore = 0;
        public static GlobalSettings Settings = new GlobalSettings();

        private static List<string> _locations = new List<string>() { "City", "Desert", "Ocean" };
        private static string _saveFilePath = "save.raindrop";
        private static SaveFile<SaveFileContents> _saveFile;
        public static SaveFile<SaveFileContents> SaveFile
        {
            get
            {
                if(_saveFile == null)
                {
                    _saveFile = new SaveFile<SaveFileContents>(_saveFilePath);
                }
                return _saveFile;
            }
        }

        public static void Log(object message)
        {
            if(Settings.TraceLog)
            {
                StackTrace stackTrace = new StackTrace();
                string className = stackTrace.GetFrame(1).GetMethod().Module.Name;
                string method = stackTrace.GetFrame(1).GetMethod().Name;
                message += "\t {Called from:" +  className + "." + method + "}";
            }
            Debug.WriteLine("[RainDropLog]\t" + message);
        }

        public static void ChangeLocation(string location, Node context)
        {
            PackedScene scene = GD.Load<PackedScene>("res://Locations/" + location + ".tscn");
            Node anchor = context.FindNode("LocationAnchor");
            if(anchor == null)
            {
                anchor = context.GetTree().Root.FindNode("LocationAnchor");
                if(anchor == null)
                {
                    throw new System.Exception("Unable to find anchor!");
                }               
            }
            anchor.RemoveChild(anchor.GetChild(0));
            anchor.AddChild(scene.Instance());
            CurrentLocation = location;

            foreach(Node child in context.GetChildren())
            {
                if(child is Spawner)
                {
                    (child as Spawner).ChangeSpawnLocation(CurrentLocation);
                }
            }
        }

        public static void ShuffleList<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    class GlobalSettings
    {
        public bool PlaySounds = false;
        public bool DevLogActive = true;
        public bool TraceLog = true;
    }

    class SaveFileContents
    {
        public int Score = 0;
    }


}
