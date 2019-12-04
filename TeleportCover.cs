using Godot;
using System;

public class TeleportCover : Node2D
{
    [Signal]
    public delegate void TransitionFinished();

    AnimatedSprite _sprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sprite = FindNode("AnimatedSprite") as AnimatedSprite;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    public void Activate()
    {
        Visible = true;
        _sprite.Playing = true;
    }

    private void _on_AnimatedSprite_animation_finished()
    {
        Visible = false;
        _sprite.Playing = false;
        _sprite.Frame = 0;
        EmitSignal(nameof(TransitionFinished));
    }
}
