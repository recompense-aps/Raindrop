using Godot;
using System;

public class Platform : Area2D
{
    [Export]
    public string PlatformType = "City";

    private string[] _types = new string[] { "City", "Desert", "Ocean" };
    private float _impact = 10f;
    private float _unimpactY;
    private float _positiveImpactY;
    private float _negativeImpactY;

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
        _unimpactY = Position.y;
        _positiveImpactY = Position.y + _impact;
        _negativeImpactY = Position.y - _impact;
    }

    public void Impact(Area2D area)
    {
        if(area.Position.y > Position.y)
        {
            Position = new Vector2(Position.x, _negativeImpactY);
        }
        else
        {
            Position = new Vector2(Position.x, _positiveImpactY);
        }    
    }

    public void UnImpact()
    {
        Position = new Vector2(Position.x, _unimpactY);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
