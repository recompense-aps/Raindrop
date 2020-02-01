using Godot;
using RainDrop;

public class SideInController : Node
{
    private Node2D _slave;
    private RandomNumberGenerator _rand = new RandomNumberGenerator();
    private Vector2 _veloctiy = new Vector2(-5, 0);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        float y;
        _slave = GetParent() as Node2D;
        _rand.Randomize();
        y = _rand.RandfRange(100, 600);
        _rand.Randomize();
        _slave.Position = new Vector2(850, y);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _slave.Position += _veloctiy * Global.GetTimeDelta();
    }
}
