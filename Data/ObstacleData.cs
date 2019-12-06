using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrop.Data
{
    static class ObstacleData
    {
        public static Dictionary<string, ObstacleContainer> Data = new Dictionary<string, ObstacleContainer>()
        {
            {"superhero", new ObstacleContainer("Streak") },
            {"football", new ObstacleContainer("Arc") },
            {"slide", new ObstacleContainer("Arc") },
            {"car", new ObstacleContainer("Streak") },
            {"ufo", new ObstacleContainer("BackAndForth") },
            {"bird", new ObstacleContainer("BackAndForth") },
            {"airplane", new ObstacleContainer("Streak") },

            { "log", new ObstacleContainer("Basic") },
            {"monkey", new ObstacleContainer("BackAndForth") },
            {"snake", new ObstacleContainer("Streak") },
            {"seaplane", new ObstacleContainer("Streak") },
            {"pterydactal", new ObstacleContainer("Streak") },
            {"rainbow", new ObstacleContainer("BackAndForth") },
            {"toucan", new ObstacleContainer("Streak") },
            {"arrow", new ObstacleContainer("Arc") },

            {"fish", new ObstacleContainer("Basic") },
            {"surfer", new ObstacleContainer("BackAndForth") },
            {"iceplatform", new ObstacleContainer("Streak") },
            {"seagull", new ObstacleContainer("Streak") },
            {"shark", new ObstacleContainer("Streak") },
            {"speedboat", new ObstacleContainer("BackAndForth") },
            {"submarine", new ObstacleContainer("Streak") },
            {"parachute", new ObstacleContainer("Arc") }
        };
    }

    class ObstacleContainer
    {
        public string Controller { get;  }
        public ObstacleContainer(string controller)
        {
            Controller = controller;
        }
    }
}
