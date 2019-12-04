using Godot;
using System;

public class Cloud : Sprite
{
    [Export]
    public Vector2 Velocity = new Vector2(1, 0);
    private bool _reproduced = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Position += Velocity;
        if (Position.x > 0 && !_reproduced)
        {
            _reproduced = true;
            Cloud c = Duplicate() as Cloud;
            c.Position = new Vector2(-35, Position.y);
            c.Velocity = new Vector2(Velocity);
            GetParent().AddChild(c);
        }
    }
}
