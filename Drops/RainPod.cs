using Godot;
using RainDrop;
using RainDrop.Enums;

public class RainPod : KinematicBody2D
{
    [Signal]
    public delegate void HitSomething();
    [Signal]
    public delegate void DropTypeChanged();

    private Sprite _sprite;
    private Sprite _rainSprite;
    private Sprite _hailSprite;
    private Sprite _snowSprite;
    private DropType _currentDropType;
    private Node2D _collisionShape;
    private AudioStreamPlayer _convertSound;
    private AudioStreamPlayer _powerUpSound;
    private AudioStreamPlayer _obstacleSound;

    [Export]
    public float MinDropScale = RainDrop.Settings.GetFloat("RainPod.MinDropScale", 0.5f);

    // Called when the node enters the scene tree for the first time.
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
        TransformDrop(DropType.Rain, false);
    }
    public override void _Process(float delta)
    {
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
    public override void _PhysicsProcess(float delta)
    {
    }

    public void Grow(float amount)
    {
        Vector2 sScale = _sprite.Transform.Scale;
        Vector2 bScale = _collisionShape.Transform.Scale;

        sScale.Set(sScale.x + amount, bScale.y + amount);
        bScale.Set(bScale.x + amount, bScale.y + amount);

        _sprite.SetScale(sScale);
        _collisionShape.SetScale(bScale);

        if(sScale.x <= MinDropScale)
        {
            GetTree().ChangeScene("res://Modes/GameOver.tscn");
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
        TransformDrop(pickUp.DropType);
    }

    private void SwitchSprite(Sprite newSprite)
    {
        _rainSprite.Visible =
        _snowSprite.Visible =
        _hailSprite.Visible = false;

        newSprite.Visible = true;    
    }

}
