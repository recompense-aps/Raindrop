using Godot;
using System;

public class DropMover : Node
{
    private KinematicBody2D _drop;
    float Speed = 100;
    float acelMagnitude = 20;
    float decelMagnitude = 1;

    private Vector2 _velocity;
    private Vector2 _acceleration;
    private Vector2 _deceleration;
    private Vector2 _accelerationTransform;

    public override void _Ready()
    {
        _drop = GetParent() as KinematicBody2D;

        _velocity = new Vector2(0, Speed);
        _acceleration = new Vector2(0, 0);
        _deceleration = new Vector2(0, 0);
        _accelerationTransform = new Vector2(0, 0);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetInput();
        _acceleration += _deceleration;
        _velocity += _acceleration;

        if (_accelerationTransform.x > 0)
        {
            if (_acceleration.x <= 0)
            {
                ResetX();
            }
        }
        else if (_accelerationTransform.x < 0)
        {
            if (_acceleration.x >= 0)
            {
                ResetX();
            }
        }

        if (_accelerationTransform.y > 0)
        {
            if (_acceleration.y <= 0)
            {
                ResetY();
            }
        }
        else if (_accelerationTransform.y < 0)
        {
            if (_acceleration.y >= 0)
            {
                ResetY();
            }
        }

        KinematicCollision2D c = _drop.MoveAndCollide(_velocity * delta);
        if (c != null)
        {
            _drop.EmitSignal("HitSomething", _drop, c);
        }
    }

    private void GetInput()
    {
        if (Input.IsActionJustPressed("move_right"))
        {
            _acceleration.Set(acelMagnitude, 0);
            _accelerationTransform.Set(_acceleration);
            _deceleration.Set(decelMagnitude * -1, 0);
        }
        if (Input.IsActionJustPressed("move_left"))
        {
            _acceleration.Set(-1 * acelMagnitude, 0);
            _accelerationTransform.Set(_acceleration);
            _deceleration.Set(decelMagnitude, 0);
        }
        if (Input.IsActionJustPressed("move_up"))
        {
            _acceleration.y = -1 * acelMagnitude;
            _deceleration.y = decelMagnitude;
            _accelerationTransform.Set(_acceleration);
        }
        if (Input.IsActionJustPressed("move_down"))
        {
            _acceleration.y = acelMagnitude;
            _deceleration.y = -1 * decelMagnitude;
            _accelerationTransform.Set(_acceleration);
        }

        if (Input.IsActionJustPressed("move_right"))
        {

        }
        if (Input.IsActionJustPressed("move_right"))
        {

        }
        if (Input.IsActionJustPressed("move_right"))
        {

        }
        if (Input.IsActionJustPressed("move_right"))
        {

        }
    }

    private void ResetX()
    {
        _acceleration.x =
        _deceleration.x =
        _accelerationTransform.x =
        _velocity.x = 0;
    }

    private void ResetY()
    {
        _acceleration.y =
        _deceleration.y =
        _accelerationTransform.y = 0;
        _velocity.y = Speed;
    }

}
