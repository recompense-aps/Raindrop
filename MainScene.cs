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
        PauseMode = PauseModeEnum.Process;
        FindNode("HUD").Connect("StartButtonPressed", this, nameof(OnStartButtonPressed));
        _dropScene = GD.Load<PackedScene>("res://Drop.tscn");
        Global.ChangeLocation("City", this);
        Global.PreviousHighScore = Global.SaveFile.Contents.Score;
        Global.GameState = GameState.MainMenu;

        Playlist pl = new Playlist();
        AddChild(pl);
        pl.Load(GetSongs(), "Audio/Music/");
        pl.Shuffle();
        pl.Start();

        Global.LoadSceneCache(new List<string>()
        {
            "Locations/GameOver",
            "Effects/BlinkerEffect",
            "Effects/ScoreChangeEffect",
            "DropBurst",
            "Obstacle",
            "Portal",
            "PowerUp"
        });
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel") && _drop != null)
        {
            _drop.ToggleDevMode();
        }
        if(Input.IsActionJustPressed("pause"))
        {
            GetTree().Paused = !GetTree().Paused;
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
            "Viscosity.wav",
            "Cosmic Strings.wav",
        };
    }    
}
