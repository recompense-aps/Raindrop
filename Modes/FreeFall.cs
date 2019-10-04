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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _window = OS.GetRealWindowSize();

        GenerateNextObstacleWave();

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
    }

    private void GenerateNextObstacleWave(float startY = 0)
    {
        float startX = 0;
        float ySpace = 100;
        int obstaclesToGenerate = 10;

        for (int i = 0; i < obstaclesToGenerate; i++)
        {
            Node2D ob = Util.LoadNode("Obstacle");
            AddChild(ob);
            ob.Position = new Vector2(_rand.RandiRange(0, (int)OS.GetRealWindowSize().x), startY + ySpace * i);
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
