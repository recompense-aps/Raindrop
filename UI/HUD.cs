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
    private Label _powerText;
    private Label _debugText;
    private Label _fpsLabel;
    private TextEdit _textEdit;
    private ColorRect _powerBar;

    private int _score = 0;
    private float _power = 100;
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

    public float Power
    {
        get
        {
            return _power;
        }
        set
        {
            _power = value;
            if (_power <= 0)
            {
                _power = 0;
            }
            else if (_power < 100)
            {
                _powerStopWatch.Start();
            }
            if(_power > 100)
            {
                _power = 100;
            }
            _powerText.Text = _power.ToString();
            _powerBar.SetScale(new Vector2(_power / 100, 1));
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

    public string ConsoleInput
    {
        get
        {
            return _textEdit.Text;
        }
        set
        {
            _textEdit.Text = value;
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
        _fpsLabel = Util.FindNode(this, "FpsText") as Label;
        _textEdit = Util.FindNode(this, "TextEdit") as TextEdit;
        _powerBar = Util.FindNode(this, "Powerbar") as ColorRect;

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _timeText.Text = _powerStopWatch.Elapsed.Seconds.ToString();
        _fpsLabel.Text = Engine.GetFramesPerSecond().ToString() + "/" + Engine.TargetFps.ToString();

        CheckConsole();

        if(_powerStopWatch.Elapsed.Seconds >= 1)
        {
            _powerStopWatch.Reset();
            Power += 2;
        }
    }

    public void ToggleConsole(bool tog = true)
    {
        _textEdit.Visible = tog;
        _consoleOn = tog;
    }

    private void CheckConsole()
    {
        if (!_consoleOn) return;
        if (Input.IsActionJustPressed("ui_focus_next"))
        {
            string command = ConsoleInput;
            ConsoleInput = "";

            string[] commandSplit = command.Split(' ');

            EmitSignal(nameof(ConsoleInputEntered), new ConsoleCommand(commandSplit[1], commandSplit[2], commandSplit[3]));
        }
    }
}

class ConsoleCommand
{
    string _key;
    string _value;
    string _category;

    public string Category
    {
        get
        {
            return _category;
        }
    }

    public ConsoleCommand(string category, string key, string value)
    {
        _key = key;
        _value = value;
        _category = category;
    }

    public void Execute(object instance)
    {

    }
}
