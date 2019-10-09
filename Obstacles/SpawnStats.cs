using Godot;
using RainDrop;
using System;
using System.Collections.Generic;

public class ObstacleSpawner
{
    private RandomNumberGenerator _random = new RandomNumberGenerator();
    private static Dictionary<string, int> _cityStats = new Dictionary<string, int>()
    {
        //city
        {"Ufo", 2 }
    };

    private Dictionary<string, int> _jungleStats = new Dictionary<string, int>()
    {
        {"LogPlatform", 1 }
    };

    private Dictionary<string, int> _oceanStats = new Dictionary<string, int>()
    {
        {"Seagull", 1 }
    };

    List<string> _cityPool;
    List<string> _junglePool;
    List<string> _oceanPool;
    List<string> _allPool = new List<string>();

    public ObstacleSpawner()
    {
        _cityPool = CreatePool(_cityStats);
        _junglePool = CreatePool(_jungleStats);
        _oceanPool = CreatePool(_oceanStats);

        _allPool.AddRange(_cityPool);
        _allPool.AddRange(_junglePool);
        _allPool.AddRange(_oceanPool);
    }

    //TODO: This should be a struct
    public Obstacle Spawn(string levelType)
    {
        switch (levelType)
        {
            case "all":
                return SpawnRandomObstacle(_allPool);
            case "city":
                return SpawnRandomObstacle(_cityPool);
            case "jungle":
                return SpawnRandomObstacle(_junglePool);
            case "ocean":
                return SpawnRandomObstacle(_oceanPool);
            default:
                throw new Exception("Invalid Spawn! '" + levelType + "' is not a valid level type!");
        }
    }

    private Obstacle SpawnRandomObstacle(List<string> pool)
    {
        _random.Randomize();
        int index = _random.RandiRange(0, pool.Count);
        Obstacle ob = Util.LoadNode("Obstacles/Obstacle") as Obstacle;
        ob.SetObstacleType(pool[index]);
        return ob;
    }

    private List<string> CreatePool(Dictionary<string, int> poolStats)
    {
        List<string> pool = new List<string>();

        foreach(string key in poolStats.Keys)
        {
            for (int i = 0; i < poolStats[key]; i++)
            {
                pool.Add(key);
            }
        }
        return pool;
    }
}
