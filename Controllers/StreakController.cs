using Godot;
using System;
using RainDrop;

public class StreakController : Node
{
    private Obstacle _obstacle;
    private RandomNumberGenerator _rand = new RandomNumberGenerator();
    private float Speed { get; set; }
    private Vector2 _streakToPoint;
    private Vector2 _velocity;
    private Vector2 _accel = new Vector2(0.05f, -0.05f);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _rand.Randomize();
        _obstacle = GetParent<Obstacle>();
        _velocity = new Vector2(_rand.RandfRange(-1f, 2f), _rand.RandfRange(-5f, -6f));

        if(_velocity.x < 0)
        {
            _accel = new Vector2(_accel.x * -1, _accel.y);
        }

        if(_obstacle.Position.y <= 0)
        {
            _accel = new Vector2(_accel.x, _accel.y * -1);
            _velocity = new Vector2(_velocity.x, _velocity.y * -1);
        }

        SpriteTrail st = new SpriteTrail();
        _obstacle.AddChild(st);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _obstacle.Position = new Vector2(_obstacle.Position + _velocity * Global.GetTimeDelta());
        _velocity = _velocity + _accel;
        _obstacle.Rotation = _velocity.Angle() + (float)Math.PI/2;
    }
}
