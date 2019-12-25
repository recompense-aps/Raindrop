using Godot;
using RainDrop;
using System.Collections.Generic;

public class MainScene : Node2D
{
    [Signal]
    public delegate void TeleportStarted();
    [Signal]
    public delegate void TeleportFinished();
    [Signal]
    public delegate void Message(string message);
    PackedScene _dropScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        FindNode("HUD").Connect("StartButtonPressed", this, nameof(OnStartButtonPressed));
        _dropScene = GD.Load<PackedScene>("res://Drop.tscn");
        Global.ChangeLocation("City", this);
        Global.Settings.PlaySounds = true;

        Playlist pl = new Playlist();
        AddChild(pl);
        pl.Load(GetSongs(), "Audio/Music/");
        pl.Shuffle();
        pl.Start();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void OnStartButtonPressed()
    {
        Drop d = _dropScene.Instance() as Drop;
        d.Position = new Vector2(300, 50);
        AddChild(d);
    }

    private List<string> GetSongs()
    {
        return new List<string>()
        {
            "destiny-loop.wav",
            "funky-loop.wav",
            "mellow-garden-loop.wav",
            "someday.ogg"
        };
    }
}
