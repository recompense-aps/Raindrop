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
    private PackedScene _dropScene;
    private Drop _drop;

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

        Global.LoadSceneCache(new List<string>()
        {
            "Locations/GameOver",
            "Effects/BlinkerEffect",
            "DropBurst",
            "Obstacle",
            "Portal"
        });
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel") && _drop != null)
        {
            _drop.ToggleDevMode();
        }
    }

    private void OnStartButtonPressed()
    {
        _drop = _dropScene.Instance() as Drop;
        _drop.Position = new Vector2(300, 50);
        AddChild(_drop);
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
