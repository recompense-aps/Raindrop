using Godot;
using RainDrop;
using System;

public class SpriteTrail : Node
{
    [Export]
    public bool On = true;

    private Node2D _parent;
    private Texture _texture;
    private float _spawnTimer = 0;
    public override void _Ready()
    {
        _parent = GetParent() as Node2D;
        Sprite sprite = (_parent.FindNode("Sprite") as Sprite);

        if(sprite == null)
        {
            Global.Log("Unable to add sprite trail, could not find sprite");
            QueueFree();
            _spawnTimer = -1;
        }
        else
        {
            _texture = sprite.Texture;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(_spawnTimer >= 0 && On)
        {
            TrailSprite s = new TrailSprite(3, _parent);
            s.Texture = _texture;
            s.Position = new Vector2(_parent.Position);
            s.Scale = new Vector2(_parent.Scale);
            s.Rotation = _parent.Rotation;
            s.Modulate = _parent.Modulate;
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
    private CanvasItem _trailedNode;
    
    public TrailSprite(float timeToLive, CanvasItem parent)
    {
        _timeToLive = timeToLive;
        _alphaStep = _alpha / timeToLive;
        _trailedNode = parent;
        _trailedNode.Connect("tree_exiting", this, nameof(OnTrailedNodeExitingTree));
    }

    public override void _Process(float delta)
    {
        _alpha -= _alphaStep;
        Modulate = Color.Color8((byte)_trailedNode.Modulate.r8, (byte)_trailedNode.Modulate.g8,
        (byte)_trailedNode.Modulate.b8, (byte)_alpha);

        if(_alpha <= 0)
        {
            QueueFree();
        }
    }

    private void OnTrailedNodeExitingTree()
    {
        QueueFree();
    }
}
