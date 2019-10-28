using Godot;
using System;
using RainDrop;
using RainDrop.Enums;

public class DropMover : Node
{
    private RainPod _drop;
    [Export]
    public float Speed = RainDrop.Settings.GetFloat("DropMover.Speed", 100);
    [Export]
    public float MaxSpeed = RainDrop.Settings.GetFloat("DropMover.MaxSpeed", 200);
    [Export]
    public float RainSpeedMultiplier = RainDrop.Settings.GetFloat("DropMover.RainSpeedMultiplier", 1.0f);
    [Export]
    public float HailSpeedMultiplier = RainDrop.Settings.GetFloat("DropMover.HailSpeedMultiplier", 4f);
    [Export]
    public float SnowSpeedMultiplier = RainDrop.Settings.GetFloat("DropMover.SnowSpeedMultiplier", 0.5f);
    [Export]
    public float AccelerationBase = RainDrop.Settings.GetFloat("DropMover.AccelerationBase", 60);
    [Export]
    public float AccelerationMagnitude = RainDrop.Settings.GetFloat("DropMover.AccelerationMagnitude", 60);
    [Export]
    public float SmallWindMultiplier = RainDrop.Settings.GetFloat("DropMover.SmallWindMultiplier", 0.5f);
    [Export]
    public float RegularWindMultiplier = RainDrop.Settings.GetFloat("DropMover.RegularWindMultiplier", 1f);
    [Export]
    public float PowerWindMultiplier = RainDrop.Settings.GetFloat("DropMover.PowerWindMultiplier", 3f);
    [Export]
    public float PowerCost = RainDrop.Settings.GetFloat("DropMover.PowerCost", 10);

    private Vector2 _velocity;
    private Vector2 _acceleration;
    private Vector2 _deceleration;
    private Vector2 _accelerationTransform;
    private Vector2 _usualDirectionNormal = new Vector2(0, 1);
    private WindType _currentWindType = WindType.Regular;
    private float _windMultiplier;
    private float _maxSpeed;
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
        _maxSpeed = MaxSpeed;
        _drop.Connect("DropTypeChanged", this, nameof(OnDropTypeChanged));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetInput();
        if(Math.Abs(_velocity.x) > _maxSpeed && _acelX)
        {
            _acceleration.x = _acceleration.x * -0.2f;
            _acelX = false;
            _decelX = true;           
        }
        if(Math.Abs(_velocity.y) > _maxSpeed && _acelY)
        {
            _acceleration.y = _acceleration.y * -0.2f;
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

        _velocity += _acceleration;

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
        if (_acceleration.Length() != 0 || Util.HUD.Power < 1)
        {
           return;
        }
        if (Input.IsActionJustPressed("move_right"))
        {
            _acceleration.Set(AccelerationMagnitude, 0);
            _acelX = true;
            HandlePower();
        }
        if (Input.IsActionJustPressed("move_left"))
        {
            _acceleration.Set(-1 * AccelerationMagnitude, 0);
            _acelX = true;
            HandlePower();
        }
        if (Input.IsActionJustPressed("move_up"))
        {
            _acceleration.y = -1 * AccelerationMagnitude;
            _acelY = true;
            HandlePower();
        }
        if (Input.IsActionJustPressed("move_down"))
        {
            _acceleration.y = AccelerationMagnitude;
            _acelY = true;
            HandlePower();
        }

        if (Input.IsActionJustPressed("move_right_up"))
        {
            _acceleration.Set(AccelerationMagnitude, -AccelerationMagnitude);
            _acelX = true;
            _acelY = true;
            HandlePower();
        }
        if (Input.IsActionJustPressed("move_right_down"))
        {
            _acceleration.Set(AccelerationMagnitude, AccelerationMagnitude);
            _acelX = true;
            _acelY = true;
            HandlePower();
        }
        if (Input.IsActionJustPressed("move_left_down"))
        {
            _acceleration.Set(-AccelerationMagnitude, AccelerationMagnitude);
            _acelX = true;
            _acelY = true;
            HandlePower();
        }
        if (Input.IsActionJustPressed("move_left_up"))
        {
            _acceleration.Set(-AccelerationMagnitude, -AccelerationMagnitude);
            _acelX = true;
            _acelY = true;
            HandlePower();
        }
    }

    private void Wind()
    {
        if (Input.IsActionJustPressed("switch_wind"))
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
        _drop.Position = new Vector2(Mathf.Clamp(_drop.Position.x, 0, OS.GetRealWindowSize().x - 32), 
            Mathf.Clamp(_drop.Position.y, 0, OS.GetRealWindowSize().y - 64));
    }

    private void OnDropTypeChanged(DropType dropType)
    {
       switch (dropType)
       {
            case DropType.Hail:
                AccelerationMagnitude = AccelerationBase * HailSpeedMultiplier;
                break;
            case DropType.Rain:
                AccelerationMagnitude = AccelerationBase * RainSpeedMultiplier;
                break;
            case DropType.Snow:
                AccelerationMagnitude = AccelerationBase * SnowSpeedMultiplier;
                break;
        }
        Util.HUD.Debug = dropType.ToString();
        Util.Log(dropType + "," + AccelerationMagnitude / AccelerationBase);
    }
    
    private void ConsoleInputEntered(ConsoleCommand c)
    {

    }
    
    private void HandlePower()
    {
        switch (_currentWindType)
        {
            case WindType.Regular:
                Util.HUD.Power -= PowerCost;
                break;
            case WindType.Power:
                Util.HUD.Power -= PowerCost * 2;
                break;
            case WindType.Small:
                Util.HUD.Power -= PowerCost * 0.5f;
                break;
        }
    }
}

