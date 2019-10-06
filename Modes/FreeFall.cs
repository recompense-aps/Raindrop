using Godot;
using RainDrop;
using System;
using System.Collections.Generic;

public class FreeFall : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private double standardVelocity = 500;
    private RandomNumberGenerator _rand = new RandomNumberGenerator();
    private Vector2 _window;
    private RainPod _pod;
    private HUD _hud;

    private List<Node2D> _spawnedObstacles = new List<Node2D>();

    // Exported attributes
    [Export]
    public float ObstacleStartY = 0;
    [Export]
    public float VerticalObstacleSpace = 100;
    [Export]
    public int ObstaclesPerLayer = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _window = OS.GetRealWindowSize();
        _hud = (Util.FindNode(this, "HUD") as HUD);
        GenerateNextWave(ObstacleStartY);

        _pod = Util.LoadNode("RainPod") as RainPod;
        _pod.Position = new Vector2(_window.x / 2, -400);
        _pod.Connect("HitSomething", this, nameof(OnDropHitSomething));
        AddChild(_pod);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_spawnedObstacles.Count > 0)
        {
            Node2D lastOb = _spawnedObstacles[_spawnedObstacles.Count - 1];
            if (_pod.Position.y > lastOb.Position.y)
            {
                GenerateNextWave(lastOb.Position.y);
            }
        }
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().ChangeScene("res://Modes/StartMenu.tscn");
        }
    }

    private void GenerateNextWave(float startY)
    {
        for (int i = 0; i < ObstaclesPerLayer; i++)
        {
            _rand.Randomize();

            float posX = _rand.RandiRange(100, (int)OS.GetRealWindowSize().x - 100);
            float posY = startY + VerticalObstacleSpace * i;

            if (_rand.RandiRange(1,10) < 9)
            {
                PowerUp p = Util.LoadNode("PowerUp") as PowerUp;
                p.Position = new Vector2(posX, posY);
                AddChild(p);
            }
            else
            {
                Obstacle ob = Util.LoadNode("Obstacles/Obstacle") as Obstacle;
                ob.SetRandomObstacleType();
                AddChild(ob);

                ob.Position = new Vector2(posX, posY);
                _spawnedObstacles.Add(ob);
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

    private void OnDropHitSomething(RainPod r, KinematicCollision2D collision)
    {
        StaticBody2D obj = collision.Collider as StaticBody2D;
        Node objP = obj.GetParent();

        _hud.Debug = collision.Collider.GetType().ToString();

        if (collision.Collider is PowerUp)
        {
            RemoveChild(collision.Collider as PowerUp);
            _hud.Power += 1;
        }
        else
        {
            Node obp = obj.GetParent().GetParent();
            obp.RemoveChild(obj.GetParent()); 
            _hud.Score -= 10;
        }

    }
}
