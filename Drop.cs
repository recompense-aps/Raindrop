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
    private Vector2 _pausePosition = new Vector2();
    private bool paused = false;
    PackedScene _gameOverScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetParent().Connect("TeleportStarted", this, nameof(OnTeleportStarted));
        GetParent().Connect("TeleportFinished", this, nameof(OnTeleportFinished));
        _gameOverScene = GD.Load<PackedScene>("res://Locations/GameOver.tscn");
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

    private void _on_Drop_area_entered(Area2D area)
    {
        if(area.Name == "DeathArea")
        {
            Lose();
        }
        if(area is Obstacle)
        {
            Global.HUD.Score -= 1;
            _health -= 0.25f;
            Scale = new Vector2(Scale.x - 0.25f, Scale.x - 0.25f);
            Global.SoundEffects.Play("HitObstacle");
            if(_health <= 0)
            {
                Lose();
            }
            EmitSignal(nameof(HitObstacle));
            (area as Obstacle).Fall();
        }
        if(area is Platform)
        {
            EmitSignal(nameof(HitPlatform), area as Platform);
        }
        if(area is Portal)
        {
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
}



