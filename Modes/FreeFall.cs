using Godot;
using RainDrop;
using System;

public class FreeFall : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private double standardVelocity = 500;
    private int score = 0;
    private RandomNumberGenerator _rand = new RandomNumberGenerator();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GenerateNextObstacleWave();

        AddChild(Util.LoadNode("RainPod"));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    private void GenerateNextObstacleWave()
    {
        float startX = 0;
        float startY = 0;// OS.GetWindowSize().y;
        float ySpace = 100;
        int obstaclesToGenerate = 10;

        for (int i = 0; i < obstaclesToGenerate; i++)
        {
            Node2D ob = Util.LoadNode("Obstacle");
            AddChild(ob);
            ob.Position = new Vector2(_rand.RandiRange(0, (int)OS.GetRealWindowSize().x), startY + ySpace * i);
        }

    }
}
