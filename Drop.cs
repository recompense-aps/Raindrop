using Godot;
using RainDrop;

public class Drop : Area2D
{
    [Signal]
    public delegate void HitPlatform(Platform p);

    public float Speed = 1;
    private float _health = 1;
    private float _scaleDelta = 0.1f;
    private float _obstacleDamage = 0.25f;
    private Vector2 _pausePosition = new Vector2();
    private Vector2 _originalScale;
    private bool paused = false;
    private bool _invincible = false;
    private bool _ghost = false;
    private bool _recovering = false;
    private SpriteTrail _spriteTrail;
    private BlinkerEffect _lastBlinker;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global.Drop = this;
        GetParent().Connect("TeleportStarted", this, nameof(OnTeleportStarted));
        GetParent().Connect("TeleportFinished", this, nameof(OnTeleportFinished));
        _originalScale = new Vector2(Scale);
        _spriteTrail = FindNode("SpriteTrail") as SpriteTrail;
    }

    public override void _Process(float delta)
    {
        if(paused)
        {
            Position = _pausePosition;
        }
    }

    public void ToggleDevMode()
    {
        _invincible = !_invincible;
        _ghost = !_ghost;
    }

    private void Lose()
    {
        if(Global.SaveFile.Contents.PlaySounds)
        {
            Global.SoundEffects.Play("GameOver").Connect("finished", this, nameof(OnGameOverSoundFinished));
        }
        else
        {
            OnGameOverSoundFinished();
        }
        Global.FinalScore = Global.HUD.Score;
        Portal.PortalIsCurrentlySpawned = false;
        GetTree().Paused = true;
    }

    private void Hurt()
    {
        _spriteTrail.On = false;
        _recovering = true;
        ApplyBlinkEffect();

        Global.HUD.Score -= 1;
        _health -= _obstacleDamage;
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
        BlinkerEffect blink = Global.Instance("Effects/BlinkerEffect") as BlinkerEffect;
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
            if(_invincible == false && _recovering == false)
            {
                Hurt();
                (area as Obstacle).Fall();
                Particles2D n = Global.Instance("DropBurst") as Particles2D;
                GetParent().AddChild(n);
                n.Position = new Vector2(Position);
                n.OneShot = true;
            }
        }
        if(area is Platform && _ghost == false)
        {
            Global.SoundEffects.Play("HitPlatform");
            EmitSignal(nameof(HitPlatform), area as Platform);
            (area as Platform).Impact(this);
        }
        if(area is Portal)
        {
            Scale = _originalScale;
            _health = 1;
            (area as Portal).Teleport();
        }
        if(area is PowerUp)
        {
            PowerUp p = area as PowerUp;
            switch(p.Type)
            {
                case PowerUpType.Health:
                    _health = 1;
                    Global.HUD.SetHealth(_health);
                    break;
                case PowerUpType.Ghost:
                    _ghost = true;
                    break;
                case PowerUpType.Invincibility:
                    _invincible = true;
                    break;
            }
            p.QueueFree();
        }
    }

    private void _on_Drop_area_exited(object area)
    {
        if(area is Platform)
        {
            (area as Platform).UnImpact();
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
        _recovering = false;
        _spriteTrail.On = true;
        _lastBlinker = null;
        Modulate = new Color(1, 1, 1, 1);
    }

    private void OnGameOverSoundFinished()
    {
        Global.GameOver.ApplyValues();
        Global.GameOver.Show();
        QueueFree();        
        GetTree().Paused = false;
        Global.HUD.QueueFree();       
    }
}
