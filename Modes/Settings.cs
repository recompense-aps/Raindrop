using Godot;
using RainDrop;

public class Settings : Node2D
{
    private CheckButton _musicVolumeOn;
    private CheckButton _soundEffectsVolumeOn;
    public override void _Ready()
    {
        Ready_FindNodes();

        _musicVolumeOn.Pressed = Util.SaveFile.Contents.MusicVolumeOn;
        _soundEffectsVolumeOn.Pressed = Util.SaveFile.Contents.SoundEffectsVolumeOn;
        _musicVolumeOn.Connect("toggled", this, nameof(OnMusicVolumeOnToggled));
        _soundEffectsVolumeOn.Connect("toggled", this, nameof(OnSoundEffectsVolumeOnToggled));
    }
    public override void _Process(float delta)
    {
    }
    private void Ready_FindNodes()
    {
        _musicVolumeOn = FindNode("MusicCheckButton") as CheckButton;
        _soundEffectsVolumeOn = FindNode("SoundEffectsCheckButton") as CheckButton;
    }

    #region Signal Connections
    private void OnMusicVolumeOnToggled(bool buttonPressed)
    {
        Util.SaveFile.Contents.MusicVolumeOn = buttonPressed;
        Util.SaveFile.Save();
    }
    private void OnSoundEffectsVolumeOnToggled(bool buttonPressed)
    {
        Util.SaveFile.Contents.SoundEffectsVolumeOn = buttonPressed;
        Util.SaveFile.Save();
    }
    #endregion
}
