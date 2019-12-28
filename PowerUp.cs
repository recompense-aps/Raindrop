using Godot;
using RainDrop;

public class PowerUp : Area2D
{
    private Vector2 _velocity = new Vector2(0, 5);
    private Color _dropGreen = Color.Color8(0, 255, 0, 255);
    private Color _dropRed = Color.Color8(255, 0, 0, 255);
    private Color _dropBlue = Color.Color8(0, 0, 255, 255);

    public PowerUpType Type { get; set; }

    public override void _Ready()
    {
        Position = new Vector2(Global.GetRandomFloat(200,500), 0);
    }

    public override void _Process(float delta)
    {
        Position += _velocity;
    }

    public void Spawn(PowerUpType powerUpType)
    {
        Type = powerUpType;
        switch(Type)
        {
            case PowerUpType.Ghost:
                Modulate = _dropBlue;
                break;
            case PowerUpType.Health:
                Modulate = _dropGreen;
                break;
            case PowerUpType.Invincibility:
                Modulate = _dropRed;
                break;
        }
    }
}

public enum PowerUpType
{
    Health,
    Invincibility,
    Ghost
}
