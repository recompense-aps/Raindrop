using Godot;
using RainDrop;
using System;

public class Obstacle : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    RandomNumberGenerator rand = new RandomNumberGenerator();
    private string _obstacleType = "Football";
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void SetObstacleType(string type)
    {
        AddChild(Util.LoadNode("Obstacles/" + type + "Obstacle"));
    }

    public void SetRandomObstacleType()
    {
        string[] types = new string[] { "Football", "Pigeon", "Airplane"};

        rand.Randomize();
        string type = types[rand.RandiRange(0, types.Length-1)];

        SetObstacleType(type);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
