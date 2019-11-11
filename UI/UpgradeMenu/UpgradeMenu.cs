using Godot;
using System;
using System.Collections.Generic;
public class UpgradeMenu : Node2D
{
    public delegate void UpgradePurchased(Dictionary<string,object> upgradeData);
    private VBoxContainer _menuItemContainer;
    public override void _Ready()
    {
        Ready_FindNodes();
        Ready_CreateUpgradeMenuItems();
    }
    public override void _Process(float delta)
    {
        
    }
    private void Ready_FindNodes()
    {
        _menuItemContainer = FindNode("MenuItemContainer") as VBoxContainer;
        if (_menuItemContainer == null)
        {
            throw new Exception("Unable to find MenuItemContainer");
        }
    }
    private void Ready_CreateUpgradeMenuItems()
    {
        PackedScene menuItemScene = GD.Load<PackedScene>("res://UI/UpgradeMenu/MenuItem.tscn");
        MenuItem speedBoost = menuItemScene.Instance() as MenuItem;
        MenuItem invincible = menuItemScene.Instance() as MenuItem;
        MenuItem doubleScore = menuItemScene.Instance() as MenuItem;
        
        _menuItemContainer.AddChild(speedBoost);
        _menuItemContainer.AddChild(invincible);
        _menuItemContainer.AddChild(doubleScore);
        
        speedBoost.SetDetails("Speed Boost", 10, "Boosts your speed for 1 minute");
        invincible.SetDetails("Invincibility", 50, "Makes you invincible to obstacles for 1 minute");
        doubleScore.SetDetails("Double Score", 15, "Doubles your score for 1 minute");
    }
}
