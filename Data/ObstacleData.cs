﻿using Godot;
using System.Collections.Generic;

namespace RainDrop.Data
{
    static class ObstacleData
    {
        public static Dictionary<string, ObstacleContainer> Data = new Dictionary<string, ObstacleContainer>()
        {
            // City
            {"superhero", new ObstacleContainer("Streak") },
            {"football", new ObstacleContainer("Arc") },
            {"slide", new ObstacleContainer("Arc", false, new Vector2(0.5f,0.5f) )},
            {"car", new ObstacleContainer("Streak") },
            {"ufo", new ObstacleContainer("BackAndForth", true) },
            {"bird", new ObstacleContainer("SideIn", true) },
            {"airplane", new ObstacleContainer("Streak", false, new Vector2(2,2)) },

            // Desert
            { "snake", new ObstacleContainer("Streak", true) },
            { "pyramid", new ObstacleContainer("Arc") },
            { "vulture", new ObstacleContainer("SideIn", true) },
            { "cactus", new ObstacleContainer("Arc") },
            { "skull", new ObstacleContainer("Arc") },
            { "camel", new ObstacleContainer("Streak") },

            //Ocean
            {"fish", new ObstacleContainer("Streak") },
            {"surfer", new ObstacleContainer("BackAndForth") },
            {"seagull", new ObstacleContainer("SideIn", true) },
            {"shark", new ObstacleContainer("Streak") },
            {"speedboat", new ObstacleContainer("BackAndForth") },
            {"submarine", new ObstacleContainer("Streak") },
            {"parachute", new ObstacleContainer("BackAndForth") }
        };
    }

    class ObstacleContainer
    {
        public string Controller { get;  }
        public bool IsAnimated { get; }
        public Vector2 Scale { get; }
        public ObstacleContainer(string controller, bool isAnimated = false, Vector2? scale = null, float rotation = 0)
        {
            Controller = controller;
            IsAnimated = isAnimated;
            if(scale == null)
            {
                Scale = new Vector2(1, 1);
            }
            else
            {
                Scale = (Vector2)scale;
            }
        }
    }
}
