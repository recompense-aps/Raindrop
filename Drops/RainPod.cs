using Godot;
using RainDrop;

public class RainPod : KinematicBody2D
{
    [Signal]
    public delegate void HitSomething();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    public override void _PhysicsProcess(float delta)
    {
    }
}
