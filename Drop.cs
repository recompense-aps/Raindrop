using Godot;
using RainDrop;

public class Drop : Area2D
{
    [Signal]
    public delegate void HitObstacle(Obstacle o);
    [Signal]
    public delegate void HitPlatform(Platform p);

    public float Speed = 1;
    private float _health = 1;
    private float _scaleDelta = 0.1f;
    private Vector2 _pausePosition = new Vector2();
    private Vector2 _originalScale;
    private bool paused = false;
    private bool _invincible = false;
    PackedScene _gameOverScene;
    PackedScene _blinkerScene;
    SpriteTrail _spriteTrail;
    BlinkerEffect _lastBlinker;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global.Drop = this;
        GetParent().Connect("TeleportStarted", this, nameof(OnTeleportStarted));
        GetParent().Connect("TeleportFinished", this, nameof(OnTeleportFinished));
        _gameOverScene = GD.Load<PackedScene>("res://Locations/GameOver.tscn");
        _blinkerScene = GD.Load<PackedScene>("res://Effects/BlinkerEffect.tscn");
        _originalScale = new Vector2(Scale);
        _spriteTrail = FindNode("SpriteTrail") as SpriteTrail;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(paused)
        {
            Position = _pausePosition;
        }
    }

    private void Lose()
    {
        QueueFree();
        var children =  GetParent().GetChildren();
        foreach(Node n in children)
        {
            if(n is SoundEffects == false)
            {
                n.QueueFree();
            }
        }
        Global.SoundEffects.Play("GameOver");
        Global.FinalScore = Global.HUD.Score;
        Portal.PortalIsCurrentlySpawned = false;
        GetParent().AddChild(_gameOverScene.Instance());
    }

    private void Hurt()
    {
        _spriteTrail.On = false;
        _invincible = true;
        ApplyBlinkEffect();

        Global.HUD.Score -= 1;
        _health -= 0.25f;
        Scale = new Vector2(Scale.x - _scaleDelta, Scale.x - _scaleDelta);
        Global.SoundEffects.Play("HitObstacle");
        if (_health <= 0)
        {
            Lose();
        }
        Global.HUD.SetHealth(_health);
    }

    private void ApplyBlinkEffect()
    {
        RemoveBlinkers();
        BlinkerEffect blink = _blinkerScene.Instance() as BlinkerEffect;
        blink.Period = 0.05f;
        blink.Die = true;
        blink.Simple = true;
        blink.Connect("Died", this, nameof(OnBlinkerDied));
        AddChild(blink);
    }

    private void RemoveBlinkers()
    {
        foreach(Node n in GetChildren())
        {
            if(n is BlinkerEffect)
            {
                n.QueueFree();
            }
        }
        Modulate = new Color(1, 1, 1, 1);
    }

    private void _on_Drop_area_entered(Area2D area)
    {
        if(area.Name == "DeathArea")
        {
            Position = new Vector2(300, 50);
            if(!_invincible)
            {
                Hurt();
            }
        }
        if(area is Obstacle)
        {
            if(_invincible)
            {

            }
            else
            {
                Hurt();
                EmitSignal(nameof(HitObstacle));
                (area as Obstacle).Fall();
            }
        }
        if(area is Platform)
        {
            Global.SoundEffects.Play("HitPlatform");
            EmitSignal(nameof(HitPlatform), area as Platform);
        }
        if(area is Portal)
        {
            Scale = _originalScale;
            _health = 1;
            (area as Portal).Teleport();
        }
    }

    private void OnTeleportStarted()
    {
        Position = new Vector2(Position.x, 50);
        _pausePosition = new Vector2(Position);
        paused = true;
        Visible = false;
    }

    private void OnTeleportFinished()
    {
        paused = false;
        Visible = true;
    }

    private void OnBlinkerDied()
    {
        _invincible = false;
        _spriteTrail.On = true;
        _lastBlinker = null;
        Modulate = new Color(1, 1, 1, 1);
    }
}



