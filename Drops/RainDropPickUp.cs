using Godot;
using System;

public class RainDropPickUp : KinematicBody2D
{
    public Vector2 Velocity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Position += Velocity * delta;
    }
}
