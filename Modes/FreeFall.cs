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
    private int score = 0;
    private RandomNumberGenerator _rand = new RandomNumberGenerator();
    private Vector2 _window;
    private RainPod _pod;

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

        GenerateNextObstacleWave(ObstacleStartY);

        _pod = Util.LoadNode("RainPod") as RainPod;
        _pod.Position = new Vector2(_window.x / 2, -400);
        _pod.Connect("HitObstacle", this, nameof(OnDropHitObstacle));
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
                GenerateNextObstacleWave(lastOb.Position.y);
            }
        }
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().ChangeScene("res://Modes/StartMenu.tscn");
        }
    }

    private void GenerateNextObstacleWave(float startY)
    {
        for (int i = 0; i < ObstaclesPerLayer; i++)
        {
            Obstacle ob = Util.LoadNode("Obstacles/Obstacle") as Obstacle;
            ob.SetRandomObstacleType();
            AddChild(ob);

            float posX = _rand.RandiRange(100, (int)OS.GetRealWindowSize().x - 100);
            float posY = startY + VerticalObstacleSpace * i;

            ob.Position = new Vector2(posX, posY);
            _spawnedObstacles.Add(ob);
        }

    }

    private void ClearSpawnedObstacles()
    {
        foreach(Node2D ob in _spawnedObstacles)
        {
            RemoveChild(ob);
        }
    }

    private void OnDropHitObstacle(RainPod r, KinematicCollision2D collision)
    {
        StaticBody2D obstacle = collision.Collider as StaticBody2D;
        Node obp = obstacle.GetParent().GetParent();
        obp.RemoveChild(obstacle.GetParent());
        score -= 10;
    }
}
