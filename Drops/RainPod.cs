using Godot;
using RainDrop;

public class RainPod : KinematicBody2D
{
    [Signal]
    public delegate void HitSomething();

    private Sprite _sprite;
    private Node2D _collisionShape;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sprite = Util.FindNode(this, "Sprite") as Sprite;
        _collisionShape = Util.FindNode(this, "CollisionShape2D") as Node2D;
        if (_collisionShape == null)
        {
            System.Diagnostics.Debug.WriteLine("xp");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("no");
        }
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
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
}
