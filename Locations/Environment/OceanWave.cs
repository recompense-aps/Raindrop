using Godot;
using System;

public class OceanWave : Node2D
{
    [Export]
    public float TweenTo = 50;

    [Export]
    public float DurationInSeconds = 1;

    private Tween _tween;
    private float _tweenTo;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _tween = FindNode("Tween") as Tween;
        SetUpTween(1);
        _tweenTo = TweenTo;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    private void SetUpTween(int direction)
    {
        TweenTo *= direction;
        _tween.InterpolateProperty(this, "position", Position, new Vector2(TweenTo, Position.y), 
                                    DurationInSeconds, Tween.TransitionType.Linear, Tween.EaseType.In);
        _tween.Start();
    }

    private void _on_Tween_tween_completed(Godot.Object @object, NodePath key)
    {
        SetUpTween(-1);
    }
}
