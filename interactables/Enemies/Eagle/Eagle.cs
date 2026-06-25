using Godot;

public partial class Eagle : EnemyBlueprint
{
    // EXPORTS:
    [Export] private RayCast2D _playerDetect;
    [Export] private Shooter _shooter;
    
    
    // PRIVATE VARIABLES:
    private readonly Vector2 _flySpeed = new(35.0f, 15.0f);
    private Vector2 _flyDirection = Vector2.Zero;
    
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override void _Ready()
    {
        base._Ready();
        Timer.OneShot = false;
        
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Velocity = _flyDirection;
        MoveAndSlide();
        if (_playerDetect.IsColliding()) Shoot();
    }

    //* ________________________________________________________________________________________________
    //* SUB METHODS:

    private void Shoot()
    {
        _shooter.Shoot(GlobalPosition.DirectionTo(PlayerRef.GlobalPosition));
    }

    //* ________________________________________________________________________________________________
    //* OWN METHODS:
    private void FlyToPlayer()
    {
        FlipSprite();
        float xDirection = AnimatedSprite.FlipH ? 1.0f : -1.0f;
        _flyDirection = new Vector2(_flySpeed.X * xDirection, _flySpeed.Y);
    }
    
    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:

    protected override void OnScreenEntered()
    {
        base.OnScreenEntered();
        AnimatedSprite.Play("eagle");
        FlyToPlayer();
    }

    protected override void OnTimerTimeout()
    {
        FlyToPlayer();
    }
}
