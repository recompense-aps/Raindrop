using Godot;
using System;

public class Platform : Area2D
{
    [Export]
    public string PlatformType = "City";
    private string[] _types = new string[] { "City", "Desert", "Ocean" };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach(string t in _types)
        {
            if(t != PlatformType)
            {
                Node n = FindNode(t);
                if (n is Sprite)
                {
                    (n as Sprite).Visible = false;
                }
                else if(n is TextureRect)
                {
                    (n as TextureRect).Visible = false;
                }
            }
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
