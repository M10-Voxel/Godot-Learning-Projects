using Godot;

/// <summary>
/// The eagle enemy. Descending from the sky, in zigzag pattern - turns to the player after delay and shoots when over him.
/// Starts movement when entering Screen (no collision with other objects)
/// </summary>
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
        Velocity = _flyDirection;   // constant down movement without gravity
        MoveAndSlide();
        if (_playerDetect.IsColliding()) _shooter.Shoot(GlobalPosition.DirectionTo(PlayerRef.GlobalPosition));
    }


    //* ________________________________________________________________________________________________
    //* OWN METHODS:
    
    /// <summary>
    /// Flips sprite as well as flight direction depending on player position
    /// </summary>
    private void FlyToPlayer()
    {
        FlipSprite();
        float xDirection = AnimatedSprite.FlipH ? 1.0f : -1.0f;
        _flyDirection = new Vector2(_flySpeed.X * xDirection, _flySpeed.Y);
    }
    
    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:

    /// <summary>
    /// Animation and movement start when entering screen
    /// </summary>
    protected override void OnScreenEntered()
    {
        base.OnScreenEntered();
        AnimatedSprite.Play("eagle");
        FlyToPlayer();
    }

    /// <summary>
    /// Fly to player after delay
    /// </summary>
    protected override void OnTimerTimeout()
    {
        FlyToPlayer();
    }
}
