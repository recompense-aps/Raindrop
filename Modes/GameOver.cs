using Godot;
using System;

public class GameOver : Node2D
{
    public override void _Ready()
    {
        FindNode("RestartButton").Connect("pressed", this, nameof(OnRestartButtonClicked));
        FindNode("LevelSelectButton").Connect("pressed", this, nameof(OnLevelSelectButtonClicked));
        FindNode("QuitButton").Connect("pressed", this, nameof(QuitButtonClicked));
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
