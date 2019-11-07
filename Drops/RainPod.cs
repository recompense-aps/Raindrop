using System;
using Godot;
using RainDrop;
using RainDrop.Enums;

public class RainPod : KinematicBody2D
{
    #region Signals
    [Signal]
    public delegate void HitSomething();
    [Signal]
    public delegate void DropTypeChanged();
    [Signal]
    public delegate void DropDied();
    #endregion

    #region Members
    private int _health;
    private Sprite _sprite;
    private Sprite _rainSprite;
    private Sprite _hailSprite;
    private Sprite _snowSprite;
    private DropType _currentDropType;
    private Node2D _collisionShape;
    private AudioStreamPlayer _convertSound;
    private AudioStreamPlayer _powerUpSound;
    private AudioStreamPlayer _obstacleSound;
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
    #endregion

    #region Exports
    [Export]
    public float MinDropScale = RainDrop.GameSettings.GetFloat("RainPod.MinDropScale", 0.5f);
    [Export]
    public int MaxHealth = RainDrop.GameSettings.GetInt("RainPod.MaxHealth", 10);
    [Export]
    public int StartHealth = RainDrop.GameSettings.GetInt("RainPod.StartHealth", 5);
    [Export]
    public float Speed = RainDrop.GameSettings.GetFloat("DropMover.Speed", 100);
    [Export]
    public float MaxSpeed = RainDrop.GameSettings.GetFloat("DropMover.MaxSpeed", 200);
    [Export]
    public float RainSpeedMultiplier = RainDrop.GameSettings.GetFloat("DropMover.RainSpeedMultiplier", 1.0f);
    [Export]
    public float HailSpeedMultiplier = RainDrop.GameSettings.GetFloat("DropMover.HailSpeedMultiplier", 4f);
    [Export]
    public float SnowSpeedMultiplier = RainDrop.GameSettings.GetFloat("DropMover.SnowSpeedMultiplier", 0.5f);
    [Export]
    public float AccelerationBase = RainDrop.GameSettings.GetFloat("DropMover.AccelerationBase", 60);
    [Export]
    public float AccelerationMagnitude = RainDrop.GameSettings.GetFloat("DropMover.AccelerationMagnitude", 60);
    [Export]
    public float DecelBaseMultiplier = RainDrop.GameSettings.GetFloat("DropMover.DecelBaseMultiplier", 0.2f);
    [Export]
    public float SmallWindMultiplier = RainDrop.GameSettings.GetFloat("DropMover.SmallWindMultiplier", 0.5f);
    [Export]
    public float RegularWindMultiplier = RainDrop.GameSettings.GetFloat("DropMover.RegularWindMultiplier", 1f);
    [Export]
    public float PowerWindMultiplier = RainDrop.GameSettings.GetFloat("DropMover.PowerWindMultiplier", 3f);
    [Export]
    public float PowerCost = RainDrop.GameSettings.GetFloat("DropMover.PowerCost", 10);

    #endregion
    
    public DropType DropType
    {
        get
        {
            return _currentDropType;
        }
    }
    public override void _Ready()
    {
        _rainSprite = Util.FindNode(this, "Sprite") as Sprite;
        _snowSprite = Util.FindNode(this, "SnowflakeSprite") as Sprite;
        _hailSprite = Util.FindNode(this, "HailstoneSprite") as Sprite;
        _convertSound = Util.FindNode(this, "ConvertSound") as AudioStreamPlayer;
        _powerUpSound = Util.FindNode(this, "PowerUpSound") as AudioStreamPlayer;
        _obstacleSound = Util.FindNode(this, "ObstacleSound") as AudioStreamPlayer;
        _sprite = _rainSprite;
        _collisionShape = Util.FindNode(this, "CollisionShape2D") as Node2D;
        _hailSprite.Visible = false;
        _snowSprite.Visible = false;
        _health = StartHealth;
        _velocity = new Vector2(0,0);
        _acceleration = new Vector2(0, 0);
        _deceleration = new Vector2(0, 0);
        _accelerationTransform = new Vector2(0, 0);
        _windMultiplier = RegularWindMultiplier;
        _maxSpeed = MaxSpeed;
        TransformDrop(DropType.Rain, false);
    }
    public override void _Process(float delta)
    {
        Move(delta);
        HandleCollision(delta);
        HandleWind();
        GetInput();

        if(Input.IsActionJustPressed("ui_select"))
        {
            _currentDropType++;
            if ((int)_currentDropType > (int)DropType.Hail)
            {
                _currentDropType = DropType.Rain;
            }
            TransformDrop(_currentDropType);
        }
    }
    public void Grow(float amount)
    {
        if (CheckHealth(amount))
        {
            Vector2 sScale = _sprite.Transform.Scale;
            Vector2 bScale = _collisionShape.Transform.Scale;

            sScale.Set(sScale.x + amount, bScale.y + amount);
            bScale.Set(bScale.x + amount, bScale.y + amount);

            _sprite.SetScale(sScale);
            _collisionShape.SetScale(bScale);
        }
    }
    public void TransformDrop(DropType dropType, bool playSound = true)
    {
        switch(dropType)
        {
            case DropType.Rain:
                SwitchSprite(_rainSprite);
                break;
            case DropType.Snow:
                SwitchSprite(_snowSprite);
                break;
            case DropType.Hail:
                SwitchSprite(_hailSprite);
                break;
        }
        if(playSound)
        {
            _convertSound.Play();
        }
        EmitSignal(nameof(DropTypeChanged), dropType);
    }
    public void HitPowerUp()
    {
        _powerUpSound.Play();
    }
    public void HitObstacle()
    {
        _obstacleSound.Play();
    }
    public void HitRainDropPickUp(RainDropPickUp pickUp)
    {
        _powerUpSound.Play();
        if(pickUp.MutateDrop)
        {
            TransformDrop(pickUp.DropType);
        }       
    }
    private void SwitchSprite(Sprite newSprite)
    {
        _rainSprite.Visible =
        _snowSprite.Visible =
        _hailSprite.Visible = false;

        newSprite.Visible = true;    
    }
    private bool CheckHealth(float amount)
    {
        if(amount < 0)
        {
            _health--;
        }
        else
        {
            _health++;
        }

        if(_health <= 0)
        {
            EmitSignal(nameof(DropDied));
            return false;
        }

        if (_health > MaxHealth)
        {
            _health = MaxHealth;
            return false;
        }

        return true;
    }
    private void Move(float delta)
    {
         if(Math.Abs(_velocity.x) > _maxSpeed && _acelX)
        {
            _acceleration.x = _acceleration.x * -1 * DecelBaseMultiplier;
            _acelX = false;
            _decelX = true;           
        }
        if(Math.Abs(_velocity.y) > _maxSpeed && _acelY)
        {
            _acceleration.y = _acceleration.y * -1 * DecelBaseMultiplier;
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
            SetRotation(_velocity.Angle() - (float)Math.PI / 2);
        }
        else
        {
            SetRotation(_velocity.Angle());
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
            _acceleration.Set(AccelerationMagnitude, 0);
            _acelX = true;
        }
        if (Input.IsActionJustPressed("move_left"))
        {
            _acceleration.Set(-1 * AccelerationMagnitude, 0);
            _acelX = true;
        }
        if (Input.IsActionJustPressed("move_up"))
        {
            _acceleration.y = -1 * AccelerationMagnitude;
            _acelY = true;
        }
        if (Input.IsActionJustPressed("move_down"))
        {
            _acceleration.y = AccelerationMagnitude;
            _acelY = true;
        }

        if (Input.IsActionJustPressed("move_right_up"))
        {
            _acceleration.Set(AccelerationMagnitude, -AccelerationMagnitude);
            _acelX = true;
            _acelY = true;
        }
        if (Input.IsActionJustPressed("move_right_down"))
        {
            _acceleration.Set(AccelerationMagnitude, AccelerationMagnitude);
            _acelX = true;
            _acelY = true;
        }
        if (Input.IsActionJustPressed("move_left_down"))
        {
            _acceleration.Set(-AccelerationMagnitude, AccelerationMagnitude);
            _acelX = true;
            _acelY = true;
        }
        if (Input.IsActionJustPressed("move_left_up"))
        {
            _acceleration.Set(-AccelerationMagnitude, -AccelerationMagnitude);
            _acelX = true;
            _acelY = true;
        }
    }
    private void HandleWind()
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
        KinematicCollision2D c = MoveAndCollide(_velocity * delta);
        if (c != null)
        {
            EmitSignal("HitSomething", c);
        }
        Position = new Vector2(Mathf.Clamp(Position.x, 0, OS.GetRealWindowSize().x - 32), 
            Mathf.Clamp(Position.y, 0, Window.Height - 64));
    }
}
