using System.Collections.Generic;
using Godot;
using System;

namespace RainDrop
{
    static class Global
    {
        public static bool PlaySound = true;
        public static HUD HUD { get; set; }
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

        private static List<string> _locations = new List<string>() { "City", "Jungle", "Ocean" };

        public static void ChangeLocation(string location, Node context)
        {
            PackedScene scene = GD.Load<PackedScene>("res://Locations/" + location + ".tscn");
            Node anchor = context.FindNode("LocationAnchor");
            if(anchor == null)
            {
                throw new System.Exception("Unable to find anchor!");
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


}
