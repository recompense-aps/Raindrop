using Godot;
using RainDrop;
using System.Collections.Generic;

public class MainScene : Node2D
{
    private AudioStreamPlayer _titleMusic;
    [Signal]
    public delegate void TeleportStarted();
    [Signal]
    public delegate void TeleportFinished();
    [Signal]
    public delegate void Message(string message);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //OS.WindowFullscreen = true;
        _titleMusic = FindNode("TitleMusic") as AudioStreamPlayer;
        PauseMode = PauseModeEnum.Process;
        Global.Playlist = new Playlist();
        AddChild(Global.Playlist);
        Global.Playlist.Load(GetSongs(), "Audio/Music/");

        Global.LoadSceneCache(new List<string>()
        {
            "Locations/GameOver",
            "Locations/City",
            "Locations/Desert",
            "Locations/Ocean",
            "Locations/Tutorial",
            "Locations/Credits",
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
        if(Input.IsActionJustPressed("pause") && Global.GameState == GameState.Playing)
        {
            GetTree().Paused = !GetTree().Paused;
        }
        if(Global.GameState != GameState.MainMenu)
        {
            _titleMusic.Stop();
        }
        else if (_titleMusic.Playing == false && Global.SaveFile.PlaySounds)
        {
            _titleMusic.Play();
            Global.Playlist.Mute();
        }
        if(_titleMusic.Playing && Global.SaveFile.PlaySounds == false)
        {
            _titleMusic.Stop();
        }
    }

    public void DisplayMenu(string locationPath)
    {
        FindNode("Menus").AddChild(Global.Instance(locationPath));
    }

    private List<string> GetSongs()
    {
        return new List<string>()
        {
            "Viscosity.wav",
            "Cosmic Strings.wav",
            "Reverie.wav",
            "Isolated.wav"
        };
    }

    private void _on_MoveRightButton_ready()
    {
        Global.MoveRightButton = FindNode("MoveRightButton") as Button;
    }

    private void _on_MoveLeftButton_ready()
    {
        Global.MoveLeftButton = FindNode("MoveLeftButton") as Button;
    }


    private void _on_MoveUpButton_ready()
    {
        Global.MoveUpButton = FindNode("MoveUpButton") as Button;
    }

    private void _on_MoveDownButton_ready()
    {
        Global.MoveDownButton = FindNode("MoveDownButton") as Button;
    }

}
