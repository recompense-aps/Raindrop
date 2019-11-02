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
    private Vector2 _window;
    private RainPod _pod;
    private StormCloud _stormCloud;
    private HUD _hud;
    private ScoreKeeper _scoreKeeper = new ScoreKeeper();
    private Spawner _spawner;
    private PickBag<LevelSpawnType> _waveBag = new PickBag<LevelSpawnType>();
    private float _spawnElapsed = 0;

    #region Exports
    //Drop
    [Export]
    public float DropStartY = RainDrop.Settings.GetFloat("FreeFall.DropStartY", 150);
    [Export]
    public float DropVolumeIncrease = RainDrop.Settings.GetFloat("FreeFall.DropVolumeIncrease", 0.10f);
    [Export]
    public float DropScale = RainDrop.Settings.GetFloat("FreeFall.DropScale", 2);
    [Export]
    public int HailStoneUpgradeCost = RainDrop.Settings.GetInt("HailStoneUpgradeCost", 0);

    //Storm cloud
    [Export]
    public float StormCloudStartY = RainDrop.Settings.GetFloat("FreeFall.StormCloudStartY", 0);

    //Obstacles
    [Export]
    public float CrawlSpeed = RainDrop.Settings.GetFloat("FreeFall.CrawlSpeed", 150);
    [Export]
    public float PowerUpCrawlSpeed = RainDrop.Settings.GetFloat("FreeFall.PowerUpCrawlSpeed", 0.9f);
    [Export]
    public float DropCrawlSpeed = RainDrop.Settings.GetFloat("FreeFall.DropCrawlSpeed", 0.8f);
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
        _hud = (Util.FindNode(this, "HUD") as HUD);
        Util.HUD = _hud;

        SetUpDrop();
        SetUpSpawning();
        ConnectSignals();
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
            Util.SaveFile.Contents.TotalScore += _scoreKeeper.Score;
            Util.SaveFile.Contents.Orbs += _scoreKeeper.PowerUpsCollected;
            Util.SaveFile.Save();
            GetTree().ChangeScene("res://Modes/StartMenu.tscn");
        }
    }
    private void GenerateNextWave(float startY)
    {
        startY += Window.Height / 2;

        for (int i = 0; i < ObstaclesPerLayer; i++)
        {
            float posX = _rand.RandiRange(-100, (int)OS.GetRealWindowSize().x);
            float posY = startY + VerticalObstacleSpace * i;
            Vector2 position = new Vector2(posX, posY);
            Vector2 crawlVector = new Vector2(0, -CrawlSpeed);

            switch(_waveBag.Pick())
            {
                case LevelSpawnType.PowerUp:
                    PowerUp p = _spawner.SpawnPowerUp(position, crawlVector * PowerUpCrawlSpeed);
                    _scoreKeeper.ScorePowerUp(p);
                    break;
                case LevelSpawnType.Obstacle:
                    Obstacle ob = _spawner.SpawnObstacle(position, crawlVector);
                    ob.TrackPlayer(_pod);
                    _scoreKeeper.ScoreObstacle(ob);
                    break;
                case LevelSpawnType.Drop:
                    RainDropPickUp rainDrop = _spawner.SpawnRainDropPickUp(position, crawlVector * DropCrawlSpeed);
                    break;
            }
        }

    }

    #region Initialization
    private void ConnectSignals()
    {
        _pod.Connect("HitSomething", this, nameof(OnDropHitSomething));
        _scoreKeeper.Connect("ScoreChanged", this, nameof(OnScoreChanged));
    }

    private void SetUpSpawning()
    {
        _spawner = new Spawner(this);       

        _waveBag.Add(50, LevelSpawnType.Obstacle);
        _waveBag.Add(25, LevelSpawnType.PowerUp);
        _waveBag.Add(25, LevelSpawnType.Drop);      
    }

    private void SetUpDrop()
    {
        _pod = Util.LoadNode("Drops/RainPod") as RainPod;
        _pod.Position = new Vector2(Window.Width / 2, DropStartY);
        _pod.Scale.Set(new Vector2(DropScale, DropScale));
        AddChild(_pod);
    }

    private void SetUpStormCloud()
    {
        _stormCloud = Util.LoadNode("StormCloud") as StormCloud;
        _stormCloud.Position = new Vector2(0, StormCloudStartY);
        //AddChild(_stormCloud);
    }
    #endregion

    #region Signal Handles
    private void OnDropHitSomething(KinematicCollision2D collision)
    {
        // TODO: Refactor pod-specific stuff into the pod
        //       Honestly, refector this whole method

        if (collision.Collider is PowerUp)
        {
            PowerUp p = collision.Collider as PowerUp;
            p.EmitSignal("Collected");
            RemoveChild(p);
            _hud.Power += _pod.PowerCost;
            _pod.HitPowerUp();
        }
        else if (collision.Collider is RainDropPickUp)
        {
            _pod.Grow(DropVolumeIncrease);
            RainDropPickUp pu = collision.Collider as RainDropPickUp;
            RemoveChild(pu);
            _pod.HitRainDropPickUp(pu);
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
            _pod.HitObstacle();
        }

    }
    private void OnScoreChanged()
    {
        _hud.Score = _scoreKeeper.Score;
    }

    #endregion
}
