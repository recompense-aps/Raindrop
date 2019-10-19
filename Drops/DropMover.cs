using Godot;
using System;
using RainDrop;
using RainDrop.Enums;

public class DropMover : Node
{
    private RainPod _drop;
    [Export]
    public float Speed = 100;
    [Export]
    public float MaxSpeed = 200;
    [Export]
    public float RainSpeedMultiplier = 1.0f;
    [Export]
    public float HailSpeedMultiplier = 1.5f;
    [Export]
    public float SnowSpeedMultiplier = 0.5f;
    [Export]
    public float AccelerationMagnitudeBase = 20;
    [Export]
    public float SmallWindMultiplier = 0.5f;
    [Export]
    public float RegularWindMultiplier = 1f;
    [Export]
    public float PowerWindMultiplier = 3f;

    private Vector2 _velocity;
    private Vector2 _acceleration;
    private Vector2 _deceleration;
    private Vector2 _accelerationTransform;
    private Vector2 _usualDirectionNormal = new Vector2(0, 1);
    private WindType _currentWindType = WindType.Regular;
    private float _windMultiplier;
    private bool _acelX = false;
    private bool _decelX = false;
    private bool _acelY = false;
    private bool _decelY = false;

    public override void _Ready()
    {
        _drop = GetParent() as RainPod;

        _velocity = new Vector2(0,0);
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
        if(Math.Abs(_velocity.x) > MaxSpeed && _acelX)
        {
            _acceleration.x = _acceleration.x * -0.5f;
            _acelX = false;
            _decelX = true;           
        }
        if(Math.Abs(_velocity.y) > MaxSpeed && _acelY)
        {
            _acceleration.y = _acceleration.y *= -0.5f;
            _acelY = false;
            _decelY = true;
        }
        if (_decelX && Util.Direction(_velocity.x) == Util.Direction(_acceleration.x))
        {          
            ResetX();
            _decelX = false;
        }
        if (_decelY && Util.Direction(_velocity.y) == Util.Direction(_acceleration.y))
        {
            ResetY();
            _decelY = false;
        }
        _velocity += _acceleration * _windMultiplier;

        if (_velocity.Length() != 0)
        {
            _drop.SetRotation(_velocity.Angle() - (float)Math.PI / 2);
        }
        else
        {
            _drop.SetRotation(_velocity.Angle());
        }
        

        HandleCollision(delta);
        Wind();
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
            _acelX = true;
        }
        if (Input.IsActionJustPressed("move_left"))
        {
            _acceleration.Set(-1 * AccelerationMagnitudeBase, 0);
            _acelX = true;
        }
        if (Input.IsActionJustPressed("move_up"))
        {
            _acceleration.y = -1 * AccelerationMagnitudeBase;
            _acelY = true;
        }
        if (Input.IsActionJustPressed("move_down"))
        {
            _acceleration.y = AccelerationMagnitudeBase;
            _acelY = true;
        }

        if (Input.IsActionJustPressed("move_right_up"))
        {
            _acceleration.Set(AccelerationMagnitudeBase, -AccelerationMagnitudeBase);
            _acelX = true;
            _acelY = true;
        }
        if (Input.IsActionJustPressed("move_right_down"))
        {
            _acceleration.Set(AccelerationMagnitudeBase, AccelerationMagnitudeBase);
            _acelX = true;
            _acelY = true;
        }
        if (Input.IsActionJustPressed("move_left_down"))
        {
            _acceleration.Set(-AccelerationMagnitudeBase, AccelerationMagnitudeBase);
            _acelX = true;
            _acelY = true;
        }
        if (Input.IsActionJustPressed("move_left_up"))
        {
            _acceleration.Set(-AccelerationMagnitudeBase, -AccelerationMagnitudeBase);
            _acelX = true;
            _acelY = true;
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
        _accelerationTransform.y = 
        _velocity.y = 0;
    }

    private void HandleCollision(float delta)
    {
        KinematicCollision2D c = _drop.MoveAndCollide(_velocity * delta);
        if (c != null)
        {
            _drop.EmitSignal("HitSomething", c);
        }
        _drop.Position = new Vector2(Mathf.Clamp(_drop.Position.x, 0, 1024), _drop.Position.y);
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
    
    private void ConsoleInputEntered(ConsoleCommand c)
    {

    }
}

