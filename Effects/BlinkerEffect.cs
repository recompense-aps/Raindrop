using Godot;
using RainDrop;

public class BlinkerEffect : Node
{
    [Export]
    public float Period = 1;
    [Export]
    public float TimeToLive = 5;
    [Export]
    public bool Die = false;
    [Export]
    public bool Simple = false;

    [Signal]
    public delegate void Died();

    private float _time = 0;
    private float _blinkTime = 0;
    private Tween _tween;
    private CanvasItem _parent;
    private Color _origMod;
    private Color _fadeMod = new Color(1, 1, 1, 0);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _tween = FindNode("Tween") as Tween;
        _parent = GetParent<CanvasItem>();
        _origMod = _parent.Modulate;
        FadeOut();
        if(Simple == false)
        {
            _tween.Start();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Die)
        {
            _time += delta;
            if(_time >= TimeToLive)
            {
                EmitSignal(nameof(Died));
                _parent.Modulate = _origMod;
                QueueFree();
            }
        }

        if(Simple)
        {
            _blinkTime += delta;
            if (_blinkTime >= Period)
            {
                if (_parent.Modulate.a == 0)
                {
                    _parent.Modulate = _origMod;
                }
                else
                {
                    _parent.Modulate = _fadeMod;
                }
                _blinkTime = 0;
            }
        }
    }

    private void FadeIn()
    {
        _tween.InterpolateProperty(_parent, "modulate", _fadeMod, _origMod, Period, Tween.TransitionType.Linear, Tween.EaseType.InOut);
    }

    private void FadeOut()
    {
        _tween.InterpolateProperty(_parent, "modulate", _origMod, _fadeMod, Period, Tween.TransitionType.Linear, Tween.EaseType.InOut);
    }

    private void _on_Tween_tween_completed(Godot.Object @object, NodePath key)
    {
        if(_parent.Modulate.a == 0)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
        _tween.Start();
    }
}
