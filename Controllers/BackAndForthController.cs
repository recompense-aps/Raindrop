using Godot;
using System;

public class BackAndForthController : Node
{
    public float Speed { get; set; }
    private Obstacle _obstacle;
    private int directionX = 1;
    private int directionY = -1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Speed = 2;
        _obstacle = GetParent<Obstacle>();
        
        if(_obstacle.Position.y <= 0)
        {
            directionY = 1;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _obstacle.Position = new Vector2(_obstacle.Position.x + Speed * directionX, _obstacle.Position.y + Speed * directionY);
        if(_obstacle.Position.x <= 5 || _obstacle.Position.x >= 600)
        {
            directionX *= -1;
        }
    }
}
