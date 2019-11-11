using Godot;
using System;

public class PowerUpEffect : Node
{
    private FreeFall _freeFall;
    private RainPod _player;
    private Timer _timer;
    public override void _Ready()
    {
        _timer = FindNode("Timer") as Timer;
    }
    public override void _Process(float delta)
    {
    }

    public void Spawn(FreeFall freeFall, RainPod player)
    {
        freeFall.AddChild(this);
        AddToGroup("power-up-effects");
        _player = player;
        _timer.Start();
    }
}
