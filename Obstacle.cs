using Godot;
using RainDrop;
using RainDrop.Data;
using System.Collections.Generic;

public class Obstacle : Area2D
{
    private static Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
    private static Dictionary<string, PackedScene> _controllers = new Dictionary<string, PackedScene>();
    private Sprite _sprite;
    private Tween _tween;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sprite = FindNode("Sprite") as Sprite;
        _tween = FindNode("Tween") as Tween;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Position.y <= -32 || Position.y >= 1500)
        {
            QueueFree();
            Global.HUD.Score += 1;
        }
        if (_sprite.Texture == null)
        {
            QueueFree(); //TODO: Fix this
        }
    }

    public void Spawn(string id)
    {
        ObstacleContainer c;
        if (ObstacleData.Data.ContainsKey(id))
        {
            c = ObstacleData.Data[id];
        }
        else
        {
            c = new ObstacleContainer("Basic");
        }
        
        if(_textures.ContainsKey(id))
        {
            _sprite.Texture = _textures[id];
        }
        else
        {
            Texture t = LoadTexture(id);
            _textures.Add(id, t);
        }
        if(_controllers.ContainsKey(c.Controller))
        {
            AddChild(_controllers[c.Controller].Instance());
        }
        else
        {
            PackedScene ps = LoadController(c.Controller);
            _controllers.Add(c.Controller, ps);
            AddChild(ps.Instance());
        }

        Scale = new Vector2(2, 2);
        AddToGroup("obstacles");
    }

    public void Fall()
    {
        foreach(Node n in GetChildren())
        {
            if (n is Sprite || n is Tween) continue;
            RemoveChild(n);
        }
        _tween.InterpolateProperty(this, "position", new Vector2(Position), 
            new Vector2(Position.x, 1600), 2, Tween.TransitionType.Linear, Tween.EaseType.In);
        _tween.Start();
    }

    private Texture LoadTexture(string id)
    {
        string rootPath = "res://Graphics/Obstacles/";
        Texture t = GD.Load<Texture>(rootPath + id + ".png");
        return t;
    }

    private PackedScene LoadController(string id)
    {
        string rootPath = "res://Controllers/";
        PackedScene ps = GD.Load<PackedScene>(rootPath + id + "Controller.tscn");
        return ps;
    }
}
