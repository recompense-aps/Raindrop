using Godot;
using RainDrop;

public class ManualController : Node
{
    private Area2D _slave;
    private Vector2 _velocity = new Vector2(1, 1);
    private float _horizontalSpeed = 5;
    private float _accel = 0.1f;
    private const float CONSTRAINT_LEFT = -5;
    private const float CONSTRAINT_RIGHT = 605;
    private const float CONSTRAINT_TOP = 32;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _slave = GetParent() as Area2D;
        _slave.Connect("HitObstacle", this, nameof(OnHitObstacle));
        _slave.Connect("HitPlatform", this, nameof(OnHitPlatform));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionPressed("ui_left"))
        {
            _velocity.x = -_horizontalSpeed;
        }
        else if(Input.IsActionPressed("ui_right"))
        {
            _velocity.x = _horizontalSpeed;
        }

        if(Input.IsActionPressed("ui_up"))
        {
            _velocity.y = -_horizontalSpeed;
        }
        else if(Input.IsActionPressed("ui_down"))
        {
            _velocity.y += _accel * 10;
        }
        else
        {
            _velocity.y += _accel;
        }

        _slave.Position = _slave.Position + _velocity;

        if (_slave.Position.x <= CONSTRAINT_LEFT && _velocity.x < 0)
        {
            OutOfSideBounds();
        }
        if(_slave.Position.x >= CONSTRAINT_RIGHT && _velocity.x > 0)
        {
            OutOfSideBounds();
        }
        if (_slave.Position.y <= CONSTRAINT_TOP && _velocity.y < 0)
        {
            OutOfTopBounds();
        }

        float clampX = Mathf.Clamp(_slave.Position.x, 0, 600);
        float clampY = Mathf.Clamp(_slave.Position.y, 0, 800);
        _slave.Position = new Vector2(clampX, clampY);
    }

    private void OutOfSideBounds()
    {
        FlowThroughSide();
    }

    private void OutOfTopBounds()
    {
        ReverseY();
    }

    private void ReverseX()
    {
        _velocity.x *= -1;
    }

    private void ReverseY()
    {
        _velocity.y *= -1;
    }

    private void FlowThroughSide()
    {
        if(_slave.Position.x < 5)
        {
            _slave.Position = new Vector2(600, _slave.Position.y);
        }
        else
        {
            _slave.Position = new Vector2(0, _slave.Position.y);
        }
    }

    private void OnHitObstacle(Obstacle o)
    {
        _velocity = new Vector2(_velocity.x, -3);
    }

    private void OnHitPlatform(Platform p)
    {
        float vY = 6;
        if(_slave.Position.y <= p.Position.y)
        {
            vY = -6f;
        }
        _velocity = new Vector2(_velocity.x, vY);
    }
}
