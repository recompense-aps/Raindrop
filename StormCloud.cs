using Godot;
using RainDrop;
using System;

public class StormCloud : KinematicBody2D
{
    private Node2D _nodeToFollow;

    #region Exports
    [Export]
    public float Speed = 100;
    #endregion



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

}
