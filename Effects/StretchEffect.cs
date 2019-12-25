using Godot;
using RainDrop;

public class StretchEffect : Node
{
    [Export]
    public Vector2 ToScale = new Vector2(2, 2);
    [Export]
    public float Period = 1;

    private Tween _tween;
    private Node2D _parent;
    private Vector2 _origScale;
    private bool _direction = true;

    public override void _Ready()
    {
        _tween = FindNode("Tween") as Tween;
        _parent = GetParent<Node2D>();
        _origScale = new Vector2(1, 1);// new Vector2(_parent.Scale);
        SetUpTween(ToScale);
    }

    public override void _Process(float delta)
    {

    }

    private void SetUpTween(Vector2 deltaScaleVector)
    {
        _tween.InterpolateProperty(_parent, "scale", new Vector2(_parent.Scale), deltaScaleVector, Period, Tween.TransitionType.Linear, Tween.EaseType.In);
        _direction = !_direction;
        _tween.Start();
        Global.Log("Tweening to: ");
        Global.Log(deltaScaleVector);
    }

    private void _on_Tween_tween_completed(Godot.Object @object, NodePath key)
    {
        if(_direction == false)
        {
            SetUpTween(_origScale);
        }
        else
        {
            SetUpTween(ToScale);
        }
    }
}
