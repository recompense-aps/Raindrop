using Godot;
using System;

public class StartMenu : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Button levelsButton = GetNode(new NodePath("LevelsButton")) as Button;
        levelsButton.Connect("Click", this, nameof(OnLevelsButtonClick));

        (GetNode(new NodePath("FreeFallButton")) as Button).Connect("Click", this, nameof(OnFreeFallButtonClick));
    }

    private void OnLevelsButtonClick(Button button)
    {
        GetTree().ChangeScene("res://Modes/Levels.tscn");
    }

    private void OnFreeFallButtonClick(Button button)
    {
        GetTree().ChangeScene("res://Modes/FreeFall.tscn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
