using Godot;
using System;

public class PowerUpEffect : Node
{
    protected FreeFall _freeFall;
    protected RainPod _player;
    public override void _Ready()
    {
        
    }
    public override void _Process(float delta)
    {
        AffectGame();
        AffectPlayer();
    }

    public void Spawn(FreeFall freeFall, RainPod player)
    {
        freeFall.AddChild(this);
        AddToGroup("power-up-effects");
        _player = player;
    }

    protected virtual void AffectGame()
    {

    }

    protected virtual void AffectPlayer()
    {

    }
}
