using Godot;
using System;

public class ArcController : Node
{
    private Obstacle _obstacle;
    private float Speed { get; set; }
    private Vector2 _accel = new Vector2(0, 0.3f);
    private Vector2 _velocity = new Vector2(1, -15);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _obstacle = GetParent<Obstacle>();
        int randX = (new Random()).Next(200, 300);
        _obstacle.Position = new Vector2(randX, _obstacle.Position.y);

        if(_obstacle.Position.y <= 0)
        {
           // just have it drop instead
            _velocity *= -1;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _velocity += _accel;
        _obstacle.Position += _velocity;
        _obstacle.Rotation = _velocity.Angle() + (float)Math.PI / 2;
    }
}
