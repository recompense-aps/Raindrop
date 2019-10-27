using Godot;
using System;

public class StartMenu : Node2D
{
    public override void _Ready()
    {
        RainDrop.SaveFile saveFile = new RainDrop.SaveFile();
        saveFile.Save();
    }

    private void OnFreeFallButtonClick(Button button)
    {
        
    }

    private void OnSettingsButtonClick(Button button)
    {
        
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    private void OnFreeFallButtonPressed()
    {
        GetTree().ChangeScene("res://Modes/LevelSelect.tscn");
    }
    private void OnSettingsButtonPressed()
    {
        GetTree().ChangeScene("res://Modes/Settings.tscn");
    }
}
