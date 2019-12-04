using Godot;
using System;

public class SpriteTrail : Node
{
    private Node2D _parent;
    private Texture _texture;
    private float _spawnTimer = 0;
    public override void _Ready()
    {
        _parent = GetParent() as Node2D;
        _texture = (_parent.FindNode("Sprite") as Sprite).Texture;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        _spawnTimer += delta;
        if(_spawnTimer >= 0.1)
        {
            TrailSprite s = new TrailSprite(5);
            s.Texture = _texture;
            s.Position = new Vector2(_parent.Position);
            s.Scale = new Vector2(_parent.Scale);
            GetTree().Root.AddChild(s);
            _spawnTimer = 0;
        }
    }
}

class TrailSprite : Sprite
{
    private float _timeAlive = 0;
    private float _timeToLive;
    private float _alphaStep;
    private float _alpha = 150;
    
    public TrailSprite(float timeToLive)
    {
        _timeToLive = timeToLive;
        _alphaStep = _alpha / timeToLive;
    }

    public override void _Process(float delta)
    {
        _alpha -= _alphaStep;
        Modulate = Color.Color8(255, 255, 255, (byte)_alpha);
        if(_alpha <= 0)
        {
            QueueFree();
        }
    }
}
