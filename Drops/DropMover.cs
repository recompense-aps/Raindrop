using Godot;
using System;
using RainDrop;
using RainDrop.Enums;

public class DropMover : Node
{
    private RainPod _drop;
    [Export]
    float Speed = 10000;
    [Export]
    float RainSpeedMultiplier = 1.0f;
    [Export]
    float HailSpeedMultiplier = 1.5f;
    [Export]
    float SnowSpeedMultiplier = 0.5f;
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
    private Vector2 _usualDirectionNormal = new Vector2(0, 1);
    private WindType _currentWindType = WindType.Regular;
    private float _windMultiplier;

    public override void _Ready()
    {
        _drop = GetParent() as RainPod;

        _velocity = new Vector2(0, Speed);
        _acceleration = new Vector2(0, 0);
        _deceleration = new Vector2(0, 0);
        _accelerationTransform = new Vector2(0, 0);
        _windMultiplier = RegularWindMultiplier;

        _drop.Connect("DropTypeChanged", this, nameof(OnDropTypeChanged));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetInput();
        _acceleration += _deceleration;
        _velocity += _acceleration * _windMultiplier;

        _drop.SetRotation(_velocity.Angle() - (float)Math.PI / 2);

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
            _drop.EmitSignal("HitSomething", c);
        }
        _drop.Position = new Vector2(Mathf.Clamp(_drop.Position.x, 0, 1024), _drop.Position.y);
        Wind();

        if (Input.IsActionJustPressed("ui_focus_prev"))
        {
            _velocity /= 100;
        }
    }

    private void GetInput()
    {
        if (_acceleration.Length() != 0)
        {
           return;
        }
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

    private void OnDropTypeChanged(DropType dropType)
    {
        _velocity = _velocity.Normalized();
        switch(dropType)
        {
            case DropType.Rain:
                _velocity *= Speed * RainSpeedMultiplier;
                break;
            case DropType.Snow:
                _velocity *= Speed * SnowSpeedMultiplier;
                break;
            case DropType.Hail:
                _velocity *= Speed * HailSpeedMultiplier;
                break;
        }
    }
}

