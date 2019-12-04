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
        _startButton.Visible = true;
        _gameTitle.Visible = true;
        Score = 0;
        _scoring = false;
        _scoreText.Visible = false;
    }

    private void _on_StartButton_pressed()
    {
        EmitSignal(nameof(StartButtonPressed));
        _startButton.Visible = false;
        _gameTitle.Visible = false;
        _scoring = true;
        _scoreText.Visible = true;
        GetTree().CallGroup("obstacles", "queue_free");
        Global.SoundEffects.Play("Ready");
    }

    private void _on_StartButton_mouse_entered()
    {
        Global.SoundEffects.Play("MouseHover");
    }

}
