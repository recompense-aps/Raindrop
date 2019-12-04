using Godot;
using RainDrop;

public class Drop : Area2D
{
    public float Speed = 1;
    private float _health = 1;
    private Vector2 _pausePosition = new Vector2();
    private bool paused = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetParent().Connect("TeleportStarted", this, nameof(OnTeleportStarted));
        GetParent().Connect("TeleportFinished", this, nameof(OnTeleportFinished));
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
        Global.HUD.Reset();
        Global.SoundEffects.Play("Lose");
    }

    private void _on_Drop_area_entered(Area2D area)
    {
        if(area.Name == "DeathArea")
        {
            Lose();
        }
        if(area is Obstacle)
        {
            (area as Obstacle).QueueFree();
            Global.HUD.Score -= 1;
            _health -= 0.25f;
            Scale = new Vector2(Scale.x - 0.25f, Scale.x - 0.25f);
            Global.SoundEffects.Play("HitObstacle");
            if(_health <= 0)
            {
                Lose();
            }
        }
        if(area is Portal)
        {
            (area as Portal).Teleport();
        }
    }

    private void OnTeleportStarted()
    {
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



