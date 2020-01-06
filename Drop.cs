using Godot;
using RainDrop;

public class Drop : Area2D
{
    [Signal]
    public delegate void HitPlatform(Platform p);

    public float Speed = 1;
    private float _powerUpDuration = 12;
    private float _recoveryDuration = 2.5f;
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
    private Color BasicModulate = Color.Color8(255, 255, 255, 255);
    private Color CurrentModulate;

    private bool IsInvincible
    {
        get
        {
            return  _invincible;
        }
    }

    private bool IsGhost
    {
        get
        {
            return _ghost;
        }
    }

    private bool IsPoweredUp
    {
        get
        {
            return _ghost || _invincible;
        }
    }

    public override void _Ready()
    {
        Global.Drop = this;
        PauseMode = PauseModeEnum.Stop;
        GetParent().Connect("TeleportStarted", this, nameof(OnTeleportStarted));
        GetParent().Connect("TeleportFinished", this, nameof(OnTeleportFinished));
        _originalScale = new Vector2(Scale);
        _spriteTrail = FindNode("SpriteTrail") as SpriteTrail;
        CurrentModulate = BasicModulate;
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
        Global.GameState = GameState.GameOver;
        if(Global.SaveFile.Contents.PlaySounds)
        {
            Global.Playlist.Mute();
            Global.SoundEffects.Play("GameOver").Connect("finished", this, nameof(OnGameOverSoundFinished));
        }
        else
        {
            OnGameOverSoundFinished();
        }
        Portal.PortalIsCurrentlySpawned = false;
        GetTree().Paused = true;
    }

    private void Hurt()
    {
        _spriteTrail.On = false;
        _recovering = true;
        ApplyRecoveryBlinkEffect();
        Particles2D n = Global.Instance("DropBurst") as Particles2D;
        GetParent().AddChild(n);
        n.Position = new Vector2(Position);
        n.OneShot = true;

        _health -= _obstacleDamage;
        Scale = new Vector2(Scale.x - _scaleDelta, Scale.x - _scaleDelta);
        if (_health <= 0)
        {
            Lose();
        }
        Global.HUD.SetHealth(_health);
    }

    private void ApplyRecoveryBlinkEffect()
    {
        BlinkerEffect blink = Global.Instance("Effects/BlinkerEffect") as BlinkerEffect;
        blink.TimeToLive = _recoveryDuration;
        blink.Period = 0.05f;
        blink.Die = true;
        blink.Simple = true;
        blink.Connect("Died", this, nameof(OnRecoveryBlinkerDied));
        AddChild(blink);
    }

    private void ApplyPowerUpEffect(Color c)
    {
        RemoveBlinkers();
        Modulate = c;
        _ghost = _invincible = false;
        BlinkerEffect blink = Global.Instance("Effects/BlinkerEffect") as BlinkerEffect;
        blink.TimeToLive = _powerUpDuration;
        blink.Period = 0.1f;
        blink.Die = true;
        blink.Simple = true;
        blink.Delay = _powerUpDuration - 3;
        AddChild(blink);
        blink.SetCustomBlink(BasicModulate);
        blink.Connect("Died", this, nameof(OnPowerUpBlinkerDied));      
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
        Modulate = CurrentModulate;
    }

    private void _on_Drop_area_entered(Area2D area)
    {
        if(area.Name == "DeathArea")
        {
            Position = new Vector2(300, 50);
            if(IsInvincible == false)
            {
                Global.SoundEffects.Play("HitGround");
                Hurt();
            }
        }
        if(area is Obstacle)
        {
            if (!_recovering)
            {
                (area as Obstacle).Fall();
                Global.SoundEffects.Play("HitObstacle");
            }
            if (IsInvincible == false && _recovering == false)
            {
                Hurt();               
            }      
        }
        if(area is Platform && IsGhost == false)
        {
            Global.SoundEffects.Play("HitPlatform");
            EmitSignal(nameof(HitPlatform), area as Platform);
            (area as Platform).Impact(this);
        }
        if(area is Portal)
        {
            Global.HUD.Score += 25;
            (area as Portal).Teleport();
        }
        if(area is PowerUp)
        {
            HandleCollision_PowerUp(area as PowerUp);
        }
    }

    private void _on_Drop_area_exited(object area)
    {
        if(area is Platform)
        {
            (area as Platform).UnImpact();
        }
    }

    private void HandleCollision_PowerUp(PowerUp p)
    {
        Global.SoundEffects.Play("PowerUp");
        _recovering = false;
        switch (p.Type)
        {
            case PowerUpType.Health:
                _health = 1;
                Scale = _originalScale;
                Global.HUD.SetHealth(_health);
                break;
            case PowerUpType.Ghost:
                ApplyPowerUpEffect(p.Modulate);
                _ghost = true;
                break;
            case PowerUpType.Invincibility:
                ApplyPowerUpEffect(p.Modulate);
                _invincible = true;
                break;
        }
        p.QueueFree();
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

    private void OnRecoveryBlinkerDied(BlinkerEffect blinker)
    {
        if(blinker.IsInsideTree())
        {
            _recovering = false;
            _spriteTrail.On = true;
            if(IsPoweredUp == false)
            {
                Modulate = BasicModulate;
            }          
        }
    }

    private void OnPowerUpBlinkerDied(BlinkerEffect blinker)
    {
        if (blinker.IsInsideTree())
        {
            _ghost = _invincible = false;
            Modulate = BasicModulate;
        }
    }

    private void OnGameOverSoundFinished()
    {
        QueueFree();
        Global.GameOver.ApplyValues();
        Global.GameOver.Show();        
        GetTree().Paused = false;       
    }
}
