using Godot;
using RainDrop;

public class HUD : CanvasLayer
{
    Label _scoreText;
    TextureButton _startButton;
    TextureRect _gameTitle;
    private int _score = 0;
    private bool _scoring = false;

    [Signal]
    public delegate void StartButtonPressed();

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            if (_score < 0 || _scoring == false) _score = 0;
            _scoreText.Text = "Score:" + _score;
        }

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global.HUD = this;
        _scoreText = FindNode("ScoreText") as Label;
        _startButton = FindNode("StartButton") as TextureButton;
        _gameTitle = FindNode("GameTitle") as TextureRect;
        _scoreText.Visible = false;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void Reset()
    {
        Show();
        Score = 0;
        _scoring = false;
    }

    private void _on_StartButton_pressed()
    {
        EmitSignal(nameof(StartButtonPressed));
        _scoring = true;
        Hide();
        GetTree().CallGroup("obstacles", "queue_free");
        Global.SoundEffects.Play("Ready");
    }

    private void _on_StartButton_mouse_entered()
    {
        Global.SoundEffects.Play("MouseHover");
    }

    private void Hide()
    {
        foreach (Node n in GetChildren())
        {
            if(n is Control)
            {
                (n as Control).Visible = false;
            }
        }
    }

    private void Show()
    {
        foreach (Node n in GetChildren())
        {
            if (n is Control)
            {
                (n as Control).Visible = true;
            }
        }
    }

}
