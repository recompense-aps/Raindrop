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
    [Export]
    public bool SpawnPowerUps = false;

    private static int _scoreToSpawnPortal = 20;
    private static int _scoreToSpawnPortalIncrement = 20;
    private static int _scoreToPowerUp = 5;
    private static int _scoreToPowerUpIncrement = 5;

    private float _elapsed;
    private PickBag<string> _obstaclePickBag;
    private PickBag<float> _obstacleSizes;
    private PickBag<PowerUpType> _powerUpPickBag;
    private RandomNumberGenerator _random = new RandomNumberGenerator();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PauseMode = PauseModeEnum.Stop;
        FillObstaclePickBag(Global.CurrentLocation);
        _obstacleSizes = new PickBag<float>();
        _obstacleSizes.Add(50, 1);
        _obstacleSizes.Add(25, 1.2f);
        _obstacleSizes.Add(25, 0.8f);

        _powerUpPickBag = new PickBag<PowerUpType>();
        _powerUpPickBag.Add(33, PowerUpType.Ghost);
        _powerUpPickBag.Add(33, PowerUpType.Invincibility);
        _powerUpPickBag.Add(34, PowerUpType.Health);
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
        if (Input.IsActionJustPressed("dev_powerup"))
        {
            SpawnPowerUp();
        }
        if(Input.IsActionJustPressed("dev_obstacle"))
        {
            SpawnDevObstacle();
        }

        if (Global.HUD.Score >= _scoreToSpawnPortal)
        {
            _scoreToSpawnPortal += _scoreToSpawnPortalIncrement;
            SpawnPortal();
        }
        if(Global.HUD.Score >= _scoreToPowerUp && SpawnPowerUps)
        {
            _scoreToPowerUp += _scoreToPowerUpIncrement;
            SpawnPowerUp();
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
        Obstacle o = Global.Instance("Obstacle") as Obstacle;
        o.Position = new Vector2(Position);
        GetParent().AddChild(o);
        o.Spawn(_obstaclePickBag.Pick());
        o.Scale *= new Vector2(pickedScale, pickedScale);      
    }

    private void SpawnPortal()
    {
        if (On == false || Portal.PortalIsCurrentlySpawned) return;
        Portal p = Global.Instance("Portal") as Portal;
        p.Position = new Vector2(300, 600);
        GetParent().AddChild(p);
        p.Spawn(Global.NextLocation);
    }

    private void SpawnPowerUp()
    {
        if(On)
        {
            PowerUp pu = Global.Instance("PowerUp") as PowerUp;
            GetParent().AddChild(pu);
            pu.Spawn(_powerUpPickBag.Pick());
        }
    }

    private void SpawnDevObstacle()
    {
        if(On)
        {
            float pickedScale = _obstacleSizes.Pick();
            Obstacle o = Global.Instance("Obstacle") as Obstacle;
            o.Position = new Vector2(Position);
            GetParent().AddChild(o);
            o.Spawn("ufo");
            o.Scale *= new Vector2(pickedScale, pickedScale);
        }
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
