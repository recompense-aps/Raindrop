using Godot;
using RainDrop;

public class HUD : CanvasLayer
{
    Label _scoreText;
    Label _healthText;
    LabelButton _muteButton;
    private int _score = 0;
    private bool _scoring = false;
    private float _powerUpTimer = 0;

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
            float prevScore = _score;
            _score = value;
            float delta = _score - prevScore;
            if (_score < 0 || _scoring == false)
            {
                _score = 0;
            }
            else
            {
                ScoreChangeEffect ef = Global.Instance("Effects/ScoreChangeEffect") as ScoreChangeEffect;
                _scoreText.AddChild(ef);
                if(_score < prevScore)
                {
                    ef.SetColor(Color.ColorN("red"));
                    ef.SetText(delta.ToString());
                }
                else
                {
                    ef.SetColor(Color.ColorN("green"));
                    ef.SetText(delta.ToString());
                    Global.SoundEffects.Play("Score");
                }
            }
            _scoreText.Text = "Score:" + _score;
        }

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global.HUD = this;
        PauseMode = PauseModeEnum.Process;
        _scoreText = FindNode("ScoreText") as Label;
        _healthText = FindNode("HealthText") as Label;
        _muteButton = FindNode("MuteButton") as LabelButton;
        _scoreText.Visible = false;
        _healthText.Visible = false;
        if(Global.SaveFile.Contents.PlaySounds == true)
        {
            _muteButton.Text = _muteButton.BaseText = "MUTE";
        }
        else
        {
            _muteButton.Text = _muteButton.BaseText = "UNMUTE";
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    public void Reset()
    {
        Show();
        Score = 0;
        _scoring = false;
    }

    public void SetHealth(float health)
    {
        //pretty wak like this
        float prev = float.Parse(_healthText.Text.Split(":")[1]);
        float n = health * 100;
        _healthText.Text = "Health:" + n;
        ScoreChangeEffect ef = Global.Instance("Effects/ScoreChangeEffect") as ScoreChangeEffect;
        _healthText.AddChild(ef);
        if (n < prev)
        {
            ef.SetColor(Color.ColorN("red"));
            ef.SetText((n-prev).ToString());
        }
        else
        {
            ef.SetColor(Color.ColorN("green"));
            ef.SetText((n-prev).ToString());
        }
    }

    public void SetScoring(bool scoring)
    {
        _scoring = scoring;
    }

    private void _on_StartButton_Pressed(object labelButton)
    {
        EmitSignal(nameof(StartButtonPressed));
        _scoring = true;
        Hide();
        GetTree().CallGroup("obstacles", "queue_free");
        Global.SoundEffects.Play("Ready");
        Global.GameState = GameState.Playing;
        _scoreText.Visible = true;
        _healthText.Visible = true;
    }

    private void _on_MuteButton_Pressed(object labelButton)
    {
        if(_muteButton.BaseText == "MUTE")
        {
            Global.SaveFile.Contents.PlaySounds = false;
            _muteButton.Text = _muteButton.BaseText = "UNMUTE";
        }
        else
        {
            Global.SaveFile.Contents.PlaySounds = true;
            _muteButton.Text = _muteButton.BaseText = "MUTE";
        }
        Global.SaveFile.Save();
    }

    private void Hide()
    {
        foreach (Node n in GetChildren())
        {
            if(n is Control)
            {
                Control c = n as Control;
                if(c.Name != "ScoreContainer" && c.Name != "ScoreText" && c.Name != "HealthText")
                {
                    (n as CanvasItem).Visible = false;
                }
            }
        }
    }

    private void Show()
    {
        foreach (Node n in GetChildren())
        {
            if (n is Control)
            {
                (n as CanvasItem).Visible = true;
            }
        }
    }

}
