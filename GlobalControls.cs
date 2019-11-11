using Godot;
using System;

public class GlobalControls : Node
{
    [Signal]
    public delegate void UpgradeMenuLaunched();
    PackedScene _upgradeMenu;
    public override void _Ready()
    {
        PauseMode = PauseModeEnum.Process;
    }
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().ChangeScene("res://Modes/StartMenu.tscn");
        }
        if(Input.IsActionJustPressed("launch_upgrade_menu"))
        {

        }
    }
    public static GlobalControls Get(Node context)
    {
        return context.FindNode("GlobalControls") as GlobalControls;
    }
}
