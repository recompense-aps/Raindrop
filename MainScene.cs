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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        OS.WindowFullscreen = true;
        PauseMode = PauseModeEnum.Process;
        Global.Playlist = new Playlist();
        AddChild(Global.Playlist);
        Global.Playlist.Load(GetSongs(), "Audio/Music/");
        Global.Playlist.Shuffle();
        Global.Playlist.Start();

        Global.LoadSceneCache(new List<string>()
        {
            "Locations/GameOver",
            "Locations/City",
            "Locations/Desert",
            "Locations/Ocean",
            "Effects/BlinkerEffect",
            "Effects/ScoreChangeEffect",
            "DropBurst",
            "Obstacle",
            "Portal",
            "PowerUp",
            "Drop"
        });
        Global.MainScene = this;
        Global.ChangeLocation("City", this);
        Global.GameState = GameState.MainMenu;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("pause"))
        {
            GetTree().Paused = !GetTree().Paused;
        }
    }

    private List<string> GetSongs()
    {
        return new List<string>()
        {
            "Viscosity.wav",
            "Cosmic Strings.wav",
        };
    }    
}
