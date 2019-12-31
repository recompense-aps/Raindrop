using Godot;
using System;

public class LabelButton : Label
{
    [Export]
    public string SceneTarget = null;
    [Export]
    public string Hover_SoundEffect = null;
    [Export]
    public string Pressed_SoundEffect = null;
    [Export]
    public string BaseText = "";
    [Signal]
    public delegate void Pressed(LabelButton labelButton);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Text = "" + BaseText;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    private void _on_LabelButton_mouse_entered()
    {
        //Text = ">" + BaseText;
        Modulate = Color.ColorN("white", 0.6f);
    }


    private void _on_LabelButton_mouse_exited()
    {
        //Text = "" + BaseText;
        Modulate = Color.ColorN("white", 1f);
    }

    private void _on_LabelButton_gui_input(object @event)
    {
        InputEvent ev = @event as InputEvent;
        if(ev.IsPressed())
        {
            EmitSignal(nameof(Pressed), this);
            if(SceneTarget != null)
            {
                if(SceneTarget == "EXIT")
                {
                    GetTree().Quit();
                }
                GetTree().ChangeScene(SceneTarget);
            }
        }
    }
}
