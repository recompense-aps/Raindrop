using Godot;
using System;

public class ScoreChangeEffect : Node2D
{
    private Tween _tween;
    private Label _label;

    public override void _Ready()
    {
        _tween = FindNode("Tween") as Tween;
        _label = FindNode("Label") as Label;
        _tween.InterpolateProperty(this, "position", new Vector2(Position.x, Position.y),
                                    new Vector2(Position.x, Position.y + 50), 1, Tween.TransitionType.Linear, Tween.EaseType.In);
        _tween.Start();
    }

    public override void _Process(float delta)
    {

    }

    public void SetColor(Color c)
    {
        if (_label == null) return;
        _label.Modulate = c;
    }

    public void SetText(string text)
    {
        if (_label == null) return;
        _label.Text = text;
    }

    private void _on_Tween_tween_completed(Godot.Object @object, NodePath key)
    {
        QueueFree();
    }
}
