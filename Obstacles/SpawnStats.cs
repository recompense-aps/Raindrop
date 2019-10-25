using Godot;
using RainDrop;
using System;
using System.Collections.Generic;

public class ObstacleSpawner
{
    private RandomNumberGenerator _random = new RandomNumberGenerator();
    private PickBag<string> _cityPickBag = new PickBag<string>();
    private PickBag<string> _junglePickBag = new PickBag<string>();
    private PickBag<string> _oceanPickBag = new PickBag<string>();

    private static Dictionary<string, int> _cityStats = new Dictionary<string, int>()
    {
        {"Pigeon", 11 },
        {"BrickPlatform", 11 },
        {"SuperHero", 11 },
        {"Airplane", 11 },
        {"PlayGroundSlide",11 },
        {"Javelin", 11 },
        {"Football", 11 },
        {"Ufo", 11 },
        {"Car", 12 }
    };

    private Dictionary<string, int> _jungleStats = new Dictionary<string, int>()
    {
        {"LogPlatform", 12 },
        {"Snake", 12 },
        {"Toucan", 12 },
        {"Arrow", 12 },
        {"DinoBird", 12 },
        {"JunglePlane", 12 },
        {"Rainbow", 12 },
        {"MonkeyHead", 16 }
    };

    private Dictionary<string, int> _oceanStats = new Dictionary<string, int>()
    {
        {"Seagull", 12 },
        {"IcePlatform", 12 },
        {"SeaPlane", 12 },
        {"Shark", 12 },
        {"Surfboard", 12 },
        {"Submarine", 12 },
        {"Fish", 12 },
        {"Parachute", 16 }
    };

    public ObstacleSpawner()
    {
        _cityPickBag.Add(_cityStats);
        _junglePickBag.Add(_jungleStats);
        _oceanPickBag.Add(_oceanStats);
    }

    //TODO: This should be a struct
    public Obstacle Spawn(string levelType)
    {
        switch (levelType)
        {
            case "city":
                return SpawnRandomObstacle(_cityPickBag.Pick());
            case "jungle":
                return SpawnRandomObstacle(_junglePickBag.Pick());
            case "ocean":
                return SpawnRandomObstacle(_oceanPickBag.Pick());
            default:
                throw new Exception("Invalid Spawn! '" + levelType + "' is not a valid level type!");
        }
    }

    private Obstacle SpawnRandomObstacle(string pick)
    {
        Obstacle ob = Util.LoadNode("Obstacles/Obstacle") as Obstacle;
        ob.SetObstacleType(pick);
        return ob;
    }
}
