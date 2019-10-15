using Godot;
using System.Diagnostics;
using System;

public class HUD : CanvasLayer
{
    private Stopwatch _stopWatch = new Stopwatch();
    private Label _scoreText;
    private Label _timeText;
    private Label _powerText;
    private Label _debugText;

    private int _score = 0;
    private int _power = 0;
    private string _debug = "";

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

    public int Power
    {
        get
        {
            return _power;
        }
        set
        {
            _power = value;
            _powerText.Text = "Power: " + _power.ToString();
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
        _scoreText = GetNode(new NodePath("ScoreText")) as Label;
        _timeText = GetNode(new NodePath("TimeText")) as Label;
        _powerText = GetNode(new NodePath("PowerText")) as Label;
        _debugText = GetNode(new NodePath("DebugText")) as Label;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _timeText.Text = _stopWatch.Elapsed.Seconds.ToString();
    }
}