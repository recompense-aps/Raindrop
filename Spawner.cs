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

    public static bool SpawnedPortal = false;
    private static int _scoreToSpawnPortal = 10;
    private static int _scoreToSpawnPortalIncrement = 10;

    private float _elapsed;
    private PackedScene _obstacleScene;
    private PackedScene _portalScene;
    private PickBag<string> _obstaclePickBag;
    private RandomNumberGenerator _random = new RandomNumberGenerator();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _obstacleScene = GD.Load<PackedScene>("res://Obstacle.tscn");
        _portalScene = GD.Load<PackedScene>("res://Portal.tscn");
        //FillBagTest();
        FillBagNormal(Global.CurrentLocation);
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
        FillBagNormal(location);
    }
    
    private void SpawnObstacle()
    {
        if (On == false) return;
        Obstacle o = _obstacleScene.Instance() as Obstacle;
        o.Position = new Vector2(Position);
        GetParent().AddChild(o);
        o.Spawn(_obstaclePickBag.Pick());      
    }

    private void SpawnPortal()
    {
        if (On == false) return;

        System.Diagnostics.Debug.WriteLine($"{Global.CurrentLocation},{Global.NextLocation}");

        Portal p = _portalScene.Instance() as Portal;
        p.Position = new Vector2(400, 300);
        GetParent().AddChild(p);
        p.Spawn(Global.NextLocation);
        SpawnedPortal = true;
    }

    private void FillBagNormal(string location)
    {
        _obstaclePickBag = new PickBag<string>();
        switch(location)
        {
            case "Jungle":
                _obstaclePickBag.Add(12, "arrow");
                _obstaclePickBag.Add(12, "log");
                _obstaclePickBag.Add(12, "monkey");
                _obstaclePickBag.Add(12, "pterydactal");
                _obstaclePickBag.Add(12, "rainbow");
                _obstaclePickBag.Add(12, "seaplane");
                _obstaclePickBag.Add(14, "snake");
                _obstaclePickBag.Add(14, "toucan");
                break;
            case "Ocean":
                _obstaclePickBag.Add(12, "fish");
                _obstaclePickBag.Add(12, "surfer");
                _obstaclePickBag.Add(12, "iceplatform");
                _obstaclePickBag.Add(12, "seagull");
                _obstaclePickBag.Add(12, "shark");
                _obstaclePickBag.Add(12, "speedboat");
                _obstaclePickBag.Add(14, "submarine");
                _obstaclePickBag.Add(14, "parachute");
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
        _obstaclePickBag.Add(100, "superhero");
    }
}
