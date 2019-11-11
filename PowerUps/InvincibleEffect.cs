using Godot;
using RainDrop;

public class InvincibleEffect : Node
{
    private RainPod _player;
    private bool _enabled = false;
    private float _time = 0;
    private float _timeOut = 0;
    public bool Enabled 
    {
        get{return _enabled;}
    }
    public override void _Ready()
    {
        _player = GetParent() as RainPod;
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(_enabled)
        {
            _time += delta;
            if(_time >= _timeOut)
            {
                _enabled = false;
                _time = 0;
                Util.Log("--invincible effect timed out--");
            }
        }
    }

    public static InvincibleEffect Get(Node context)
    {
        return context.FindNode("InvincibleEffect") as InvincibleEffect;
    }

    public void Enable(float howLong = 5)
    {
        _enabled = true;
        _timeOut = howLong;
        Util.Log("--invincible effect enabled--");
    }
}
