using Godot;
using RainDrop;

public class GameOver : Node2D
{
    private Label _scoreText;
    private Label _powerUpText;
    public override void _Ready()
    {
        FindNode("RestartButton").Connect("pressed", this, nameof(OnRestartButtonClicked));
        FindNode("LevelSelectButton").Connect("pressed", this, nameof(OnLevelSelectButtonClicked));
        FindNode("QuitButton").Connect("pressed", this, nameof(QuitButtonClicked));
        _scoreText = FindNode("ScoreText") as Label;
        _powerUpText = FindNode("PowerUpText") as Label;
        _scoreText.Text = "Total Score: " + Util.SaveFile.Contents.TotalScore;
        _powerUpText.Text = "Total Power Ups: " + Util.SaveFile.Contents.Orbs;
    }
    public override void _Process(float delta)
    {
        
    }

    private void OnRestartButtonClicked()
    {
        GetTree().ChangeScene("res://Modes/FreeFall.tscn");
    }
    private void OnLevelSelectButtonClicked()
    {
        GetTree().ChangeScene("res://Modes/LevelSelect.tscn");
    }
    private void QuitButtonClicked()
    {
        GetTree().Quit();
    }
}
