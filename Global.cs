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
        public static GameOver GameOver { get; set; }
        public static SoundEffects SoundEffects {get; set;}
        public static MainScene MainScene { get; set; }
        public static Playlist Playlist { get; set; }
        public static string CurrentLocation { get; set; }
        public static GameState GameState { get; set; }
        public static string NextLocation
        {
            get
            {
                int index = _locations.IndexOf(CurrentLocation);
                if(index < 0 )
                {
                    throw new Exception($"Location: {CurrentLocation} is invalid!");
                }
                if(index + 1 == _locations.Count)
                {
                    return _locations[0];
                }
                return _locations[index+1];
            }
        }
        public static int PreviousHighScore = 0;
        public static GlobalSettings Settings = new GlobalSettings();
        public static GlobalColors Colors = new GlobalColors();

        private static List<string> _locations = new List<string>() { "City", "Desert", "Ocean" };
        private static string _saveFilePath = "save.raindrop";
        private static uint _gameStartTicks;
        private static SaveFile<SaveFileContents> _saveFile;
        private static Dictionary<string, PackedScene> _sceneCache;
        private static RandomNumberGenerator _randomGen = new RandomNumberGenerator();
        private static Node _anchor;
        private const float DIFFICULTY_TIME_CONSTANT = 0.2f;

        public static SaveFile<SaveFileContents> SaveFile
        {
            get
            {
                if(_saveFile == null)
                {
                    _saveFile = new SaveFile<SaveFileContents>(_saveFilePath, true);
                }
                return _saveFile;
            }
        }

        public static Dictionary<string, PackedScene> SceneCache
        {
            get
            {
                return _sceneCache;
            }
        }

        public static void StartGame(Node context)
        {
            context.GetTree().CallGroup("obstacles", "queue_free");
            context.GetTree().Paused = false;
            SoundEffects.Play("Ready");
            GameState = GameState.Playing;
            CreateDrop(context);
            HUD.Score = 0;
            HUD.SetHealth(1);
            HUD.HideHUD();
            Playlist.Shuffle();
            Playlist.Start();
            _gameStartTicks = OS.GetTicksMsec();
            Portal.DumpInstance();
        }

        public static void CreateDrop(Node context)
        {
            Drop drop = Instance("Drop") as Drop;
            drop.Position = new Vector2(300, 50);
            MainScene.AddChild(drop);
        }

        public static float GetRandomFloat(float from, float to)
        {
            _randomGen.Randomize();
            return _randomGen.RandfRange(from, to);
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

        public static void LoadSceneCache(List<string> scenes)
        {
            _sceneCache = new Dictionary<string, PackedScene>();
            foreach(string scene in scenes)
            {
                string path = "res://" + scene + ".tscn";
                PackedScene ps = GD.Load<PackedScene>(path);
                _sceneCache.Add(scene, ps);

                if(ps == null)
                {
                    Log("Unable to load: " + path);
                }
            }
        }

        public static Node Instance(string scene)
        {
            return _sceneCache[scene].Instance();
        }

        public static void ChangeLocation(string location, Node context)
        {
            Node anchor;
            if (_anchor == null)
            {
                anchor = context.FindNode("LocationAnchor");
                if (anchor == null)
                {
                    anchor = context.GetTree().Root.FindNode("LocationAnchor");
                    if (anchor == null)
                    {
                        throw new Exception("Unable to find anchor!");
                    }
                }
                _anchor = anchor;
            }
            else
            {
                anchor = _anchor;
            }
            anchor.RemoveChild(anchor.GetChild(0));
            anchor.AddChild(Instance("Locations/" + location));
            CurrentLocation = location;

            foreach(Node child in context.GetViewport().GetChild<MainScene>(0).GetChildren())
            {
                if(child is Spawner)
                {
                    (child as Spawner).ChangeSpawnLocation(CurrentLocation);
                }
            }
        }

        public static void RemoveChildren(Node n)
        {
            foreach(Node nd in n.GetChildren())
            {
                nd.QueueFree();
            }
        }

        public static float GetTimeDelta()
        {
            float baseInput = (OS.GetTicksMsec() - _gameStartTicks);
            float output = Mathf.Log(baseInput) * DIFFICULTY_TIME_CONSTANT;
            return output;
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
        public bool DevLogActive = true;
        public bool TraceLog = true;
    }

    class GlobalColors
    {
        public Color Red = new Color(1, 0, 0, 1);
        public Color Green = new Color(0, 1, 0, 1);
        public Color Blue = new Color(0, 0, 1, 1);
        public Color Gray = new Color(0.5f, 0.5f, 0.5f, 1);
    }

    class SaveFileContents
    {
        public int Score = 0;
        public bool PlaySounds = true;
    }

    enum GameState
    {
        MainMenu,
        Playing,
        GameOver,
        RestartMenu
    }
}
