using Godot;
using System;

public class GlobalControls : Node
{
    public override void _Ready()
    {
        
    }
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().ChangeScene("res://Modes/StartMenu.tscn");
        }
        if(Input.IsActionJustPressed("launch_upgrade_menu"))
        {
            GetTree().ChangeScene("res://UI/UpgradeMenu/UpgradeMenu.tscn");
        }
    }
}
