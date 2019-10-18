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
    private Label _scoreText;
    private Label _timeText;
    private Label _powerText;
    private Label _debugText;
    private Label _fpsLabel;
    private TextEdit _textEdit;

    private int _score = 0;
    private int _power = 0;
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

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _timeText.Text = _stopWatch.Elapsed.Seconds.ToString();
        _fpsLabel.Text = Engine.GetFramesPerSecond().ToString() + "/" + Engine.TargetFps.ToString();

        CheckConsole();
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
