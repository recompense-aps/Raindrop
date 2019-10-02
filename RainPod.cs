using Godot;
using System;
using System.Diagnostics;

public class RainPod : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    int Speed = 100;

    [Export]
    int LateralAcceleration = 5;
    [Export]
    int LateralDeceleration = 1;

    private Vector2 _velocity;
    private Vector2 _acceleration = new Vector2(0, 0);
    private Vector2 _decel;
    private bool decelRight = false;
    private bool decelLeft = false;

    [Signal]
    public delegate void HitObstacle();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _velocity = new Vector2(0, Speed);
        _decel = new Vector2(LateralDeceleration, 0);
    }

    public void GetInput()
    {
        if(Input.IsActionJustPressed("right"))
        {
            _acceleration.Set(LateralAcceleration, 0);
            decelRight = true;
            decelLeft = false;
        }
        if(Input.IsActionJustPressed("left"))
        {
            _acceleration.Set(-LateralAcceleration, 0);
            decelRight = false;
            decelLeft = true;
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        _velocity += _acceleration;

        KinematicCollision2D c = MoveAndCollide(_velocity * delta);
        if(c != null)
        {
            // there was a collision
            EmitSignal(nameof(HitObstacle), this);
        }

        if (decelRight)
        {
            _acceleration += -1 * _decel;

            if (_velocity.x <= 0)
            {
                _acceleration = new Vector2(0, 0);
                decelRight = false;
                _velocity.Set(0, Speed);
            }
        }

        if(decelLeft)
        {
            _acceleration += _decel;

            if(_velocity.x >= 0)
            {
                _acceleration = new Vector2(0, 0);
                decelLeft = false;
                _velocity.Set(0, Speed);
            }
        }
    }
}
