using Godot;
using RainDrop;
using RainDrop.Enums;
using System;

public class Obstacle : Node2D
{
    [Signal]
    public delegate void PassedPlayer();

    public Vector2 Velocity;

    RandomNumberGenerator rand = new RandomNumberGenerator();
    private string _obstacleType = "Football";
    private RainPod _player;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }
    public void SetObstacleType(string type)
    {
        AddChild(Util.LoadNode("Obstacles/" + type + "Obstacle"));
    }
    public void TrackPlayer(RainPod player)
    {
        _player = player;
        _player.Connect("DropTypeChanged", this, nameof(OnDropTypeChanged));
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
        Move(delta);
    }

    private void Move(float delta)
    {
        Position += Velocity * delta;
    }

    private void OnDropTypeChanged(DropType dropType)
    {
        switch(dropType)
        {
            case DropType.Rain:
                Velocity = new Vector2(0, -150);
                break;
            case DropType.Hail:
                Velocity = new Vector2(0, -200);
                break;
            case DropType.Snow:
                Velocity = new Vector2(0, -100);
                break;
        }
    }
}
