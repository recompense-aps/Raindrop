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
    [Export]
    public float Delay = 0;

    [Signal]
    public delegate void Died();

    private float _time = 0;
    private float _blinkTime = 0;
    private Tween _tween;
    private CanvasItem _parent;
    private Color _origMod;
    private Color _fadeMod;
    private Color _nextMod;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _tween = FindNode("Tween") as Tween;
        _parent = GetParent<CanvasItem>();
        _origMod = _parent.Modulate;
        _fadeMod = new Color(_origMod.r, _origMod.g, _origMod.b, 0);
        _nextMod = _fadeMod;
        FadeOut();
        if(Simple == false)
        {
            _tween.Start();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _time += delta;
        if(Simple && _time >= Delay)
        {
            _blinkTime += delta;
            if (_blinkTime >= Period)
            {
                Color temp = new Color(_parent.Modulate.ToRgba32());
                _parent.Modulate = _nextMod;
                _nextMod = temp;
                _blinkTime = 0;
            }
        }
        if (Die)
        {
            if (_time >= TimeToLive)
            {
                _parent.Modulate = _origMod;
                EmitSignal(nameof(Died), this);               
                QueueFree();
            }
        }
    }

    public void SetCustomBlink(Color c)
    {
        _fadeMod = c;
        _nextMod = _fadeMod;
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
