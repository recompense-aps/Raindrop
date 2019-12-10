using Godot;
using RainDrop;

public class SoundEffects : Node
{
    [Export]
    public bool Mute = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global.SoundEffects = this;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void Play(string soundId)
    {
        if (Mute || Global.Settings.PlaySounds == false) return;
        (FindNode(soundId) as AudioStreamPlayer).Play();
    }
}
