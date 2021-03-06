using Godot;
using RainDrop;

public class Portal : Area2D
{
    public static bool PortalIsCurrentlySpawned;
    private static Portal _instance;

    public string Destination { get; }

    private string _destination;

    public static void DumpInstance()
    {
        if(_instance != null)
        {
            _instance.QueueFree();
            _instance = null;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Tween tween = FindNode("Tween") as Tween;
        tween.InterpolateProperty(this, "scale", new Vector2(0, 0), new Vector2(4, 4), 1, Tween.TransitionType.Quad, Tween.EaseType.In);
        tween.Start();
        Global.SoundEffects.Play("PortalSpawn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void Spawn(string destination)
    {
        _destination = destination;
        if(PortalIsCurrentlySpawned)
        {
            QueueFree();
        }
        else
        {
            PortalIsCurrentlySpawned = true;
            _instance = this;
        }
    }

    public void Teleport()
    {
        Visible = false;
        GetTree().CallGroup("obstacles", "queue_free");
        GetTree().CallGroup("powerups", "queue_free");
        Global.SoundEffects.Play("Teleport");

        TeleportCover teleportTransition = GetParent().FindNode("TeleportCover") as TeleportCover;
        teleportTransition.Activate();

        GetParent().EmitSignal("TeleportStarted");
        teleportTransition.Connect("TransitionFinished", this, nameof(ChangeLocation));
    }

    private void ChangeLocation()
    {
        GetParent().EmitSignal("TeleportFinished");
        GetTree().CallGroup("obstacles", "queue_free");
        GetTree().CallGroup("powerups", "queue_free");
        Global.ChangeLocation(_destination, GetParent());
        QueueFree();
        PortalIsCurrentlySpawned = false;
    }
}
