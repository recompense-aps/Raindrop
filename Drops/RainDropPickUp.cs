using Godot;
using RainDrop;
using RainDrop.Enums;
using System;

public class RainDropPickUp : KinematicBody2D
{
    public Vector2 Velocity;
    private Sprite _rainSprite;
    private Sprite _hailSprite;
    private Sprite _snowSprite;
    private DropType _dropType;

    public DropType DropType
    {
        get
        {
            return _dropType;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _rainSprite = Util.FindNode(this, "Sprite") as Sprite;
        _snowSprite = Util.FindNode(this, "SnowflakeSprite") as Sprite;
        _hailSprite = Util.FindNode(this, "HailStoneSprite") as Sprite;

        DropType dropToBe;
        PickBag<DropType> possibleDrops = new PickBag<DropType>();
        possibleDrops.Add(34, DropType.Rain);
        possibleDrops.Add(33, DropType.Snow);
        possibleDrops.Add(33, DropType.Hail);
        dropToBe = possibleDrops.Pick();
        TransformDrop(dropToBe);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Position += Velocity * delta;
    }

    public void TransformDrop(DropType dropType)
    {
        _dropType = dropType;
        switch (dropType)
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
    }

    private void SwitchSprite(Sprite newSprite)
    {
        // TODO: Refactor this so it isn't duplicated with rain pod
        _rainSprite.Visible =
        _snowSprite.Visible =
        _hailSprite.Visible = false;

        newSprite.Visible = true;
    }
}
