using Godot;
using System;
using System.Diagnostics;

public class RainDropPod : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Vector2 _velocity = new Vector2(0, 100);
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
        _velocity = GetChild<KinematicBody2D>(1).MoveAndSlide(_velocity);
        Debug.WriteLine(_velocity.y);
    }
}
