using Godot;
using System;

public class StartMenu : Node2D
{
    public override void _Ready()
    {
        (GetNode(new NodePath("FreeFallButton")) as Button).Connect("Click", this, nameof(OnFreeFallButtonClick));
        (GetNode(new NodePath("SettingsButton")) as Button).Connect("Click", this, nameof(OnSettingsButtonClick));
    }

    private void OnFreeFallButtonClick(Button button)
    {
        GetTree().ChangeScene("res://Modes/FreeFall.tscn");
    }

    private void OnSettingsButtonClick(Button button)
    {
        GetTree().ChangeScene("res://Modes/Settings.tscn");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
