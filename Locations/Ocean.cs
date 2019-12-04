using Godot;
using System;

using System.Collections.Generic;

public class Ocean : Node2D
{
    Tween _tween;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetUpTweens();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void SetUpTweens()
    {
        List<Node2D> waves = new List<Node2D>();
        foreach(Node n in FindNode("Waves").GetChildren())
        {
            if (n is Node2D)
            {
                Node2D wave = n as Node2D;
                Tween t = new Tween();
                wave.AddChild(t);
                t.InterpolateProperty(wave, "transform/pos", wave.Position.x, wave.Position.x + 50, 10, Tween.TransitionType.Linear, Tween.EaseType.InOut);
                t.Start();
            }
        }
    }
}
