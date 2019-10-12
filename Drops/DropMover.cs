using Godot;
using System;

public class DropMover : Node
{
    private KinematicBody2D _drop;
    [Export]
    float Speed = 100;
    [Export]
    float AccelerationMagnitudeBase = 20;
    [Export]
    float DecelerationMagnitudeBase = 1;
    [Export]
    float SmallWindMultiplier = 0.5f;
    [Export]
    float RegularWindMultiplier = 1f;
    [Export]
    float PowerWindMultiplier = 3f;

    private Vector2 _velocity;
    private Vector2 _acceleration;
    private Vector2 _deceleration;
    private Vector2 _accelerationTransform;
    private WindType _currentWindType = WindType.Regular;
    private float _windMultiplier;

    public override void _Ready()
    {
        _drop = GetParent() as KinematicBody2D;

        _velocity = new Vector2(0, Speed);
        _acceleration = new Vector2(0, 0);
        _deceleration = new Vector2(0, 0);
        _accelerationTransform = new Vector2(0, 0);
        _windMultiplier = RegularWindMultiplier;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetInput();
        _acceleration += _deceleration;
        _velocity += _acceleration * _windMultiplier;

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
        _drop.Position = new Vector2(Mathf.Clamp(_drop.Position.x, 0, 1024), _drop.Position.y);
        Wind();
    }

    private void GetInput()
    {
        if (Input.IsActionJustPressed("move_right"))
        {
            _acceleration.Set(AccelerationMagnitudeBase, 0);
            _accelerationTransform.Set(_acceleration);
            _deceleration.Set(DecelerationMagnitudeBase * -1, 0);
        }
        if (Input.IsActionJustPressed("move_left"))
        {
            _acceleration.Set(-1 * AccelerationMagnitudeBase, 0);
            _accelerationTransform.Set(_acceleration);
            _deceleration.Set(DecelerationMagnitudeBase, 0);
        }
        if (Input.IsActionJustPressed("move_up"))
        {
            _acceleration.y = -1 * AccelerationMagnitudeBase;
            _deceleration.y = DecelerationMagnitudeBase;
            _accelerationTransform.Set(_acceleration);
        }
        if (Input.IsActionJustPressed("move_down"))
        {
            _acceleration.y = AccelerationMagnitudeBase;
            _deceleration.y = -1 * DecelerationMagnitudeBase;
            _accelerationTransform.Set(_acceleration);
        }

        if (Input.IsActionJustPressed("move_right_up"))
        {
            _acceleration.Set(AccelerationMagnitudeBase, -AccelerationMagnitudeBase);
            _deceleration.Set(-DecelerationMagnitudeBase, DecelerationMagnitudeBase);
            _accelerationTransform.Set(_acceleration);
        }
        if (Input.IsActionJustPressed("move_right_down"))
        {
            _acceleration.Set(AccelerationMagnitudeBase, AccelerationMagnitudeBase);
            _deceleration.Set(-DecelerationMagnitudeBase, -DecelerationMagnitudeBase);
            _accelerationTransform.Set(_acceleration);
        }
        if (Input.IsActionJustPressed("move_left_down"))
        {
            _acceleration.Set(-AccelerationMagnitudeBase, AccelerationMagnitudeBase);
            _deceleration.Set(DecelerationMagnitudeBase, -DecelerationMagnitudeBase);
            _accelerationTransform.Set(_acceleration);
        }
        if (Input.IsActionJustPressed("move_left_up"))
        {
            _acceleration.Set(-AccelerationMagnitudeBase, -AccelerationMagnitudeBase);
            _deceleration.Set(DecelerationMagnitudeBase, DecelerationMagnitudeBase);
            _accelerationTransform.Set(_acceleration);
        }
    }

    private void Wind()
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            _currentWindType += 1;
            
            if (_currentWindType > WindType.Power)
            {
                _currentWindType = WindType.Small;
            }

            switch (_currentWindType)
            {
                case WindType.Small:
                    _windMultiplier = SmallWindMultiplier;
                    break;
                case WindType.Regular:
                    _windMultiplier = RegularWindMultiplier;
                    break;
                case WindType.Power:
                    _windMultiplier = PowerWindMultiplier;
                    break;
                default:
                    throw new Exception("Wind type");
            }
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


public enum WindType
{
    Small, Regular, Power
}

