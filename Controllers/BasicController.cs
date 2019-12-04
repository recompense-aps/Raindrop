using Godot;
using System;

public class BasicController : Node
{
    public float Speed { get; set; }
    private Obstacle _obstacle;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Speed = 1;
        _obstacle = GetParent<Obstacle>();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _obstacle.Position = new Vector2(_obstacle.Position.x, _obstacle.Position.y - Speed);
    }
}
