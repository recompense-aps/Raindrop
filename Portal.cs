using Godot;
using RainDrop;

public class Portal : Area2D
{
    public string Destination { get; }

    private string _destination;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void Spawn(string destination)
    {
        _destination = destination;
    }

    public void Teleport()
    {
        Visible = false;
        GetTree().CallGroup("obstacles", "queue_free");
        Global.SoundEffects.Play("Teleport");

        TeleportCover teleportTransition = GetParent().FindNode("TeleportCover") as TeleportCover;
        teleportTransition.Activate();

        GetParent().EmitSignal("TeleportStarted");
        teleportTransition.Connect("TransitionFinished", this, nameof(ChangeLocation));
    }

    private void ChangeLocation()
    {
        GetParent().EmitSignal("TeleportFinished");
        Global.ChangeLocation(_destination, GetParent());
        QueueFree();
    }
}
