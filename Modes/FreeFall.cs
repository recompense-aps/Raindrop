using Godot;
using RainDrop;
using RainDrop.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

public class FreeFall : Node2D
{
    private RandomNumberGenerator _rand = new RandomNumberGenerator();
    private ObstacleSpawner _obstacleSpawner = new ObstacleSpawner();
    private Vector2 _window;
    private RainPod _pod;
    private StormCloud _stormCloud;
    private HUD _hud;
    private ScoreKeeper _scoreKeeper = new ScoreKeeper();
    private List<Obstacle> _spawnedObstacles = new List<Obstacle>();
    private PickBag<LevelSpawnType> _waveBag = new PickBag<LevelSpawnType>();
    private string _spawnType = "city";
    private float _spawnElapsed = 0;

    #region Exports
    //Drop
    [Export]
    public float DropStartY = RainDrop.Settings.GetFloat("FreeFall.DropStartY", 150);
    [Export]
    public float DropVolumeIncrease = RainDrop.Settings.GetFloat("FreeFall.DropVolumeIncrease", 0.10f);
    [Export]
    public float DropScale = RainDrop.Settings.GetFloat("FreeFall.DropScale", 2);

    //Storm cloud
    [Export]
    public float StormCloudStartY = RainDrop.Settings.GetFloat("FreeFall.StormCloudStartY", 0);

    //Obstacles
    [Export]
    public float CrawlSpeed = RainDrop.Settings.GetFloat("FreeFall.CrawlSpeed", 150);
    [Export]
    public float RainCrawlMultiplier = RainDrop.Settings.GetFloat("DropMover.RainCrawlMultiplier", 1.5f);
    [Export]
    public float HailCrawlMultiplier = RainDrop.Settings.GetFloat("DropMover.RainCrawlMultiplier", 2.0f);
    [Export]
    public float SnowCrawlMultiplier = RainDrop.Settings.GetFloat("DropMover.RainCrawlMultiplier", 1.0f);
    [Export]
    public float ObstacleStartY = RainDrop.Settings.GetFloat("FreeFall.ObstacleStartY", OS.GetRealWindowSize().y);
    [Export]
    public float SpawnInterval = RainDrop.Settings.GetFloat("FreeFall.SpawnInterval", 1);
    [Export]
    public float VerticalObstacleSpace = RainDrop.Settings.GetFloat("FreeFall.VerticalObstacleSpace", 100);
    [Export]
    public int ObstaclesPerLayer = RainDrop.Settings.GetInt("FreeFall.ObstaclesPerLayer", 2);

    #endregion

    public override void _Ready()
    {
        Util.FlushLog();
        _window = OS.GetRealWindowSize();
        _hud = (Util.FindNode(this, "HUD") as HUD);
        Util.HUD = _hud;

        _pod = Util.LoadNode("Drops/RainPod") as RainPod;
        _pod.Position = new Vector2(_window.x / 2, DropStartY);
        _pod.Connect("HitSomething", this, nameof(OnDropHitSomething));
        _pod.Scale.Set(new Vector2(DropScale, DropScale));
        AddChild(_pod);

        _stormCloud = Util.LoadNode("StormCloud") as StormCloud;
        _stormCloud.Position = new Vector2(0, StormCloudStartY);
        AddChild(_stormCloud);

        _scoreKeeper.Connect("ScoreChanged", this, nameof(OnScoreChanged));

        _spawnType = Util.Globals.ContainsKey("SpawnType") ? Util.Globals["SpawnType"] as string : "city";

        _waveBag.Add(50, LevelSpawnType.Obstacle);
        _waveBag.Add(25, LevelSpawnType.PowerUp);
        _waveBag.Add(25, LevelSpawnType.Drop);

        GenerateNextWave(ObstacleStartY);
    }

    public override void _Process(float delta)
    {
        _spawnElapsed += delta;
        _hud.Debug = ((int)_spawnElapsed).ToString();
        if (_spawnElapsed >= SpawnInterval)
        {
            _spawnElapsed = 0;
            GenerateNextWave(ObstacleStartY);
        }
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().ChangeScene("res://Modes/StartMenu.tscn");
        }
    }

    private void GenerateNextWave(float startY)
    {
        startY += _window.y / 2;

        for (int i = 0; i < ObstaclesPerLayer; i++)
        {
            float posX = _rand.RandiRange(-100, (int)OS.GetRealWindowSize().x);
            float posY = startY + VerticalObstacleSpace * i;

            switch(_waveBag.Pick())
            {
                case LevelSpawnType.PowerUp:
                    PowerUp p = Util.LoadNode("PowerUp") as PowerUp;
                    p.Position = new Vector2(posX, posY);
                    p.Velocity = new Vector2(0, -CrawlSpeed);
                    AddChild(p);
                    _scoreKeeper.ScorePowerUp(p);
                    break;
                case LevelSpawnType.Obstacle:
                    Obstacle ob = _obstacleSpawner.Spawn(_spawnType);
                    AddChild(ob);
                    ob.TrackPlayer(_pod);
                    ob.Position = new Vector2(posX, posY);
                    ob.Velocity = new Vector2(0, -CrawlSpeed);
                    _spawnedObstacles.Add(ob);
                    _scoreKeeper.ScoreObstacle(ob);
                    break;
                case LevelSpawnType.Drop:
                    RainDropPickUp rainDrop = Util.LoadNode("Drops/RainDropPickUp") as RainDropPickUp;
                    rainDrop.Position = new Vector2(posX, posY);
                    rainDrop.Velocity = new Vector2(0, -CrawlSpeed);
                    AddChild(rainDrop);
                    break;
            }
        }

    }

    private void ClearSpawnedObstacles()
    {
        foreach(Node2D ob in _spawnedObstacles)
        {
            RemoveChild(ob);
        }
    }

    private void ChangeObstacleSpeed(Vector2 velocity)
    {
        _spawnedObstacles.ForEach(ob => ob.Velocity.Set(velocity));
    }

    private void OnDropHitSomething(KinematicCollision2D collision)
    {

        if (collision.Collider is PowerUp)
        {
            PowerUp p = collision.Collider as PowerUp;
            p.EmitSignal("Collected");
            RemoveChild(p);
        }
        else if (collision.Collider is RainDropPickUp)
        {
            _pod.Grow(DropVolumeIncrease);
            Node2D pu = collision.Collider as Node2D;
            RemoveChild(pu);
        }
        else if (collision.Collider is StormCloud)
        {
            //technically a loss, so restart the scene
            Util.ChangeScene(this, "Modes/FreeFall");
        }
        else //obstacle
        {
            Node obj = collision.Collider as Node;
            Node obp = obj.GetParent().GetParent();
            obp.RemoveChild(obj.GetParent());

            _pod.Grow(-0.1f);
            Particles2D p  = Util.LoadNode("Particles/DropBurst") as Particles2D;
            p.OneShot = true;
            _pod.AddChild(p);
        }

    }
    
    private void OnScoreChanged()
    {
        _hud.Score = _scoreKeeper.Score;
    }
}
