using Godot;
using RainDrop;
using System;

public class Obstacle : Node2D
{
    [Signal]
    public delegate void PassedPlayer();

    RandomNumberGenerator rand = new RandomNumberGenerator();
    private string _obstacleType = "Football";
    private Node2D _player;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    public void SetObstacleType(string type)
    {
        AddChild(Util.LoadNode("Obstacles/" + type + "Obstacle"));
    }
    public void TrackPlayer(Node2D player)
    {
        _player = player;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_player != null)
        {
            if (_player.Position.y > Position.y + 25)
            {
                EmitSignal(nameof(PassedPlayer));
            }
        }
    }
}
