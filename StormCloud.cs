using Godot;
using System;

public class StormCloud : KinematicBody2D
{
    private Node2D _nodeToFollow;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(_nodeToFollow != null)
        {
            Vector2 v = (Position - _nodeToFollow.Position).Normalized();

            Position += v * 100 * delta * -1;
        }
    }

    public void Follow(Node2D node)
    {
        _nodeToFollow = node;
    }
}
