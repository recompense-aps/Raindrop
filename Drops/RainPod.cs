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

    private Node2D _collisionShape;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _rainSprite = Util.FindNode(this, "Sprite") as Sprite;
        _snowSprite = Util.FindNode(this, "SnowflakeSprite") as Sprite;
        _hailSprite = Util.FindNode(this, "HailstoneSprite") as Sprite;
        _sprite = _rainSprite;

        _hailSprite.Visible = false;
        _snowSprite.Visible = false;
    }
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_select"))
        {
            TransformDrop(DropType.Hail);
        }
    }
    public override void _PhysicsProcess(float delta)
    {
    }

    public void Grow(float amount)
    {
        Vector2 sScale = _sprite.Transform.Scale * amount;
        Vector2 bScale = _collisionShape.Transform.Scale * amount;
        _sprite.SetTransform(_sprite.Transform.Scaled(sScale));
        _collisionShape.SetTransform(_collisionShape.Transform.Scaled(bScale));
    }
    public void TransformDrop(DropType dropType)
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

        EmitSignal(nameof(DropTypeChanged), dropType);
    }

    private void SwitchSprite(Sprite newSprite)
    {
        _rainSprite.Visible =
        _snowSprite.Visible =
        _hailSprite.Visible = false;

        newSprite.Visible = true;    
    }
}
