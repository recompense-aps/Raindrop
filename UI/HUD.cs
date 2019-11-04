using Godot;
using RainDrop;
using System.Diagnostics;
using System;
using System.Reflection;

public class HUD : CanvasLayer
{
    [Signal]
    public delegate void ConsoleInputEntered();

    private Stopwatch _stopWatch = new Stopwatch();
    private Stopwatch _powerStopWatch = new Stopwatch();
    private Label _scoreText;
    private Label _timeText;
    private Label _debugText;
    private Label _fpsLabel;

    private int _score = 0;
    private string _debug = "";
    private bool _consoleOn = false;

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            _scoreText.Text = "Score: " + _score.ToString();
        }
    }

    public string Debug
    {
        get
        {
            return _debug;
        }
        set
        {
            _debug = value;
            _debugText.Text = "Debug: " + _debug;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _stopWatch.Start();
        _scoreText = GetNode(new NodePath("Bottom/ScoreText")) as Label;
        _timeText = GetNode(new NodePath("Bottom/TimeText")) as Label;
        _debugText = GetNode(new NodePath("Bottom/DebugText")) as Label;
        _fpsLabel = Util.FindNode(this, "Bottom/FpsText") as Label;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _timeText.Text = _powerStopWatch.Elapsed.Seconds.ToString();
        _fpsLabel.Text = Engine.GetFramesPerSecond().ToString() + "/" + Engine.TargetFps.ToString();
    }

    public void ToggleConsole(bool tog = true)
    {
        _consoleOn = tog;
    }
}
