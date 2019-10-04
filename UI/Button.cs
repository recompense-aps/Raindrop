using Godot;
using System;

public class Button : ColorRect
{
    [Signal]
    public delegate void Click();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private void OnGuiInput(Godot.InputEvent inputEvent)
    {
        // Replace with function body.
        if (inputEvent is InputEventMouseButton)
        {
            EmitSignal("Click", this);
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
