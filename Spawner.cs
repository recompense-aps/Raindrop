using System;
using Godot;
using Guero;
using RainDrop;
using System.Collections.Generic;

public class Spawner : Node2D
{
    [Export]
    public float SpawnInterval = 1;
    [Export]
    public bool RandomizeInterval = true;
    [Export]
    public bool On = true;

    private static int _scoreToSpawnPortal = 10;
    private static int _scoreToSpawnPortalIncrement = 10;

    private float _elapsed;
    private PackedScene _obstacleScene;
    private PackedScene _portalScene;
    private PickBag<string> _obstaclePickBag;
    private PickBag<float> _obstacleSizes;
    private RandomNumberGenerator _random = new RandomNumberGenerator();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _obstacleScene = GD.Load<PackedScene>("res://Obstacle.tscn");
        _portalScene = GD.Load<PackedScene>("res://Portal.tscn");
        FillObstaclePickBag(Global.CurrentLocation);
        _obstacleSizes = new PickBag<float>();
        _obstacleSizes.Add(50, 1);
        _obstacleSizes.Add(25, 1.2f);
        _obstacleSizes.Add(25, 0.8f);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _elapsed += delta;
        if(_elapsed >= SpawnInterval)
        {
            SpawnObstacle();
            _elapsed = 0;

            if(RandomizeInterval)
            {
                SpawnInterval = _random.RandiRange(1, 5);
            }
        }

        if (Input.IsActionJustPressed("ui_accept"))
        {
            SpawnPortal();
        }

        if(Global.HUD.Score >= _scoreToSpawnPortal)
        {
            _scoreToSpawnPortal += _scoreToSpawnPortalIncrement;
            SpawnPortal();
        }
    }

    public void ChangeSpawnLocation(string location)
    {
        FillObstaclePickBag(location);
    }
    
    private void SpawnObstacle()
    {
        if (On == false) return;
        float pickedScale = _obstacleSizes.Pick();
        Obstacle o = _obstacleScene.Instance() as Obstacle;
        o.Position = new Vector2(Position);
        GetParent().AddChild(o);
        o.Spawn(_obstaclePickBag.Pick());
        o.Scale *= new Vector2(pickedScale, pickedScale);      
    }

    private void SpawnPortal()
    {
        if (On == false || Portal.PortalIsCurrentlySpawned) return;
        Portal p = _portalScene.Instance() as Portal;
        p.Position = new Vector2(300, 600);
        GetParent().AddChild(p);
        p.Spawn(Global.NextLocation);
    }

    private void FillObstaclePickBag(string location)
    {
        _obstaclePickBag = new PickBag<string>();
        //FillBagTest();return;
        switch(location)
        {
            case "Desert":
                _obstaclePickBag.Add(19, "snake");
                _obstaclePickBag.Add(16, "pyramid");
                _obstaclePickBag.Add(16, "vulture");
                _obstaclePickBag.Add(16, "skull");
                _obstaclePickBag.Add(16, "cactus");
                _obstaclePickBag.Add(16, "camel");
                break;
            case "Ocean":
                _obstaclePickBag.Add(12, "fish");
                _obstaclePickBag.Add(16, "surfer");
                _obstaclePickBag.Add(12, "seagull");
                _obstaclePickBag.Add(12, "shark");
                _obstaclePickBag.Add(12, "speedboat");
                _obstaclePickBag.Add(18, "submarine");
                _obstaclePickBag.Add(18, "parachute");
                break;
            default:
                _obstaclePickBag.Add(14, "superhero");
                _obstaclePickBag.Add(14, "ufo");
                _obstaclePickBag.Add(14, "airplane");
                _obstaclePickBag.Add(14, "bird");
                _obstaclePickBag.Add(14, "car");
                _obstaclePickBag.Add(14, "slide");
                _obstaclePickBag.Add(16, "football");
                break;
        }
    }

    private void FillBagTest()
    {
        _obstaclePickBag.Add(100, "speedboat");
    }
}
