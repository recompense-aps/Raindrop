using Godot;
using RainDrop;

public class PowerUp : Area2D
{
    private Vector2 _velocity = new Vector2(0, 5);
    private Sprite _cross;

    public PowerUpType Type { get; set; }

    public override void _Ready()
    {
        PauseMode = PauseModeEnum.Stop;
        Position = new Vector2(Global.GetRandomFloat(200,500), -100);
        AddToGroup("powerups");
        _cross = FindNode("Cross") as Sprite;
        _cross.Hide();
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
                Modulate = Global.Colors.Gray;
                _cross.Hide();
                break;
            case PowerUpType.Health:
                Modulate = Global.Colors.White;
                _cross.Show();
                break;
            case PowerUpType.Invincibility:
                Modulate = Global.Colors.Red;
                _cross.Hide();
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
