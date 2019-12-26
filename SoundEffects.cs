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
        PauseMode = PauseModeEnum.Process;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public AudioStreamPlayer Play(string soundId)
    {
        AudioStreamPlayer p = (FindNode(soundId) as AudioStreamPlayer);
        if (Mute || Global.Settings.PlaySounds != false)
        {
            p.Play();
        }
        return p;      
    }
}
