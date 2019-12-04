using Godot;
using RainDrop;
using System.Collections.Generic;

public class Playlist : Node
{
    List<AudioStreamPlayer> _songs = new List<AudioStreamPlayer>();
    AudioStreamPlayer _current;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void Load(List<string> songs, string root)
    {
        foreach(string song in songs)
        {
            string path = root + song;
            AudioStream audioStream = GD.Load<AudioStream>(path);
            AudioStreamPlayer player = new AudioStreamPlayer();
            player.Stream = audioStream;
            AddChild(player);
            _songs.Add(player);
        }
    }

    public void Shuffle()
    {
        Global.ShuffleList(_songs);
    }

    public void Start()
    {
        if (Global.PlaySound == false) return;
        _current = _songs[0];
        _current.Play();
        _current.Connect("finished", this, nameof(OnFinished));
    }

    private void OnFinished()
    {
        _current.Stop();
        int index = _songs.IndexOf(_current);
        _current = index == _songs.Count - 1 ? _songs[0] : _songs[index + 1];
        _current.Play();
        System.Diagnostics.Debug.WriteLine("next song");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
