using Godot;
using RainDrop;

public class HUD : CanvasLayer
{
    Label _scoreText;
    Label _healthText;
    Label _highScoreText;
    LabelButton _muteButton;
    private int _score = 0;
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
            if (_score < 0 || Global.GameState != GameState.Playing)
            {
                _score = 0;
            }
            else if(delta != 0)
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
        Global.PreviousHighScore = Global.SaveFile.Score;
        PauseMode = PauseModeEnum.Process;
        _scoreText = FindNode("ScoreText") as Label;
        _healthText = FindNode("HealthText") as Label;
        _highScoreText = FindNode("HighScoreText") as Label;
        _muteButton = FindNode("MuteButton") as LabelButton;
        _highScoreText.Text = Global.PreviousHighScore.ToString();
        _scoreText.Visible = false;
        _healthText.Visible = false;
        _highScoreText.Visible = false;

        if(Global.SaveFile.PlaySounds == true)
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
        if(_score > Global.PreviousHighScore)
        {
            _highScoreText.Text = _score.ToString();
            Global.PreviousHighScore = _score;
            Global.SaveFile.Score = _score;
        }
    }

    public void Reset()
    {
        ShowHUD();
        Score = 0;
    }

    public void SetHealth(float health)
    {
        //pretty wak like this
        float prev = float.Parse(_healthText.Text.Split(":")[1]);
        float n = health * 100;
        _healthText.Text = "Health:" + n;
        if(n-prev != 0)
        {
            ScoreChangeEffect ef = Global.Instance("Effects/ScoreChangeEffect") as ScoreChangeEffect;
            _healthText.AddChild(ef);
            if (n < prev)
            {
                ef.SetColor(Color.ColorN("red"));
                ef.SetText((n - prev).ToString());
            }
            else
            {
                ef.SetColor(Color.ColorN("green"));
                ef.SetText((n - prev).ToString());
            }
        }
    }

    private void _on_StartButton_Pressed(object labelButton)
    {
        Global.StartGame(this);
        HideHUD();
        _scoreText.Visible = true;
        _healthText.Visible = true;
        _highScoreText.Visible = true;
    }

    private void _on_MuteButton_Pressed(object labelButton)
    {
        if(_muteButton.BaseText == "MUTE")
        {
            Global.SaveFile.PlaySounds = false;
            _muteButton.Text = _muteButton.BaseText = "UNMUTE";
            Global.Playlist.Mute();
        }
        else
        {
            Global.SaveFile.PlaySounds = true;
            _muteButton.Text = _muteButton.BaseText = "MUTE";
            Global.Playlist.UnMute();
        }
        Global.SaveFile.Save();
    }

    private void _on_LabelButton_Pressed(object labelButton)
    {
        Global.MainScene.DisplayMenu("Locations/Tutorial");
        CompleteHideHUD();
    }

    private void _on_CreditsButton_Pressed(object labelButton)
    {
        Global.MainScene.DisplayMenu("Locations/Credits");
        CompleteHideHUD();
    }


    public void HideHUD()
    {
        foreach (Node n in GetChildren())
        {
            if(n is Control)
            {
                Control c = n as Control;
                if(c.Name != "ScoreContainer" && c.Name != "ScoreText" && c.Name != "HighScoreContainer"
                   && c.Name != "HealthText" && c.Name != "HighScoreText")
                {
                    (n as CanvasItem).Visible = false;
                }
            }
        }
    }

    public void ShowHUD()
    {
        foreach (Node n in GetChildren())
        {
            if (n is Control)
            {
                (n as CanvasItem).Visible = true;
            }
        }
    }

    public void CompleteHideHUD()
    {
        foreach (Node n in GetChildren())
        {
            if (n is CanvasItem)
            {
                (n as CanvasItem).Visible = false;
            }
        }
    }

}
