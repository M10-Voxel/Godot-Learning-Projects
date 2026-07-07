using Godot;

public partial class EnemyBlueprint : CharacterBody2D
{
    // EXPORTS:
    [Export] private VisibleOnScreenNotifier2D _screenNotifier;
    [Export] private Hitbox _hitbox;
    [Export] protected AnimatedSprite2D AnimatedSprite;
    [Export] protected Timer Timer;

    [Export] protected float Speed = 30.0f;
    [Export] private float _maxAnimationStartDelay = 0.8f;
    [Export] private int _points = 20;


    // PRIVATE VARIABLES:
    private float _gravity = 800.0f;
    private float _fallenOff = 200.0f;
    
    // PROTECTED VARIABLES:
    protected Player PlayerRef;


    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override void _Ready()
    {
        _screenNotifier.ScreenEntered += OnScreenEntered;
        Timer.Timeout += OnTimerTimeout;
        _hitbox.AreaEntered += Die;
        AssignPlayerRef();
    }

    public override void _Process(double delta)
    {
        EnemyFallenOff();
    }


    //* ________________________________________________________________________________________________
    //* SUB METHODS:

    /// <summary>
    /// Searches scene for node matching the player group name and assigns it to PlayerRef
    /// </summary>
    private void AssignPlayerRef()
    {
        PlayerRef = GetTree().GetFirstNodeInGroup(GameConstants.GroupPlayer) as Player;
        if (PlayerRef == null)
        {
            GD.PrintErr("No Player Ref");
            QueueFree();
        }
    }

    /// <summary>
    /// When enemy reaches bottom limit, it is removed from the scene
    /// </summary>
    private void EnemyFallenOff()
    {
        if (GlobalPosition.Y > _fallenOff)
        {
            CallDeferred(Node.MethodName.QueueFree);
        }
    }


    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    /// <summary>
    /// Starts the animation after a random delay
    /// </summary>
    protected async void DelayInitialAnimation()
    {
        AnimatedSprite.Stop();

        float delay = GD.Randf() * _maxAnimationStartDelay;
        await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);

        AnimatedSprite.Play();

    }

    /// <summary>
    /// Helper Method to shorten application of gravity to velocity of the enemy
    /// </summary>
    /// <param name="delta">The time from starting the scene</param>
    /// <returns>New velocity after applying gravity</returns>
    protected Vector2 ApplyGravity(double delta)
    {
        Vector2 velocity = Velocity;
        velocity.Y += _gravity * (float)delta;
        return velocity;
    }

    /// <summary>
    /// Helper Method to flip sprite to player position
    /// </summary>
    protected void FlipSprite()
    {
        AnimatedSprite.FlipH = PlayerRef.GlobalPosition.X > GlobalPosition.X;
    }


    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:

    /// <summary>
    /// General Method to start timer after first time entering screen
    /// </summary>
    protected virtual void OnScreenEntered()
    {
        Timer.Start();
        _screenNotifier.ScreenEntered -= OnScreenEntered;
    }

    /// <summary>
    /// Empty Method to be overridden by child classes
    /// </summary>
    protected virtual void OnTimerTimeout() {}

    /// <summary>
    /// Called when enemy hit - signal enemy death, add points, and remove from scene
    /// </summary>
    /// <param name="area">In this case only a player bullet - _hitbox is set to only recognize player bullets</param>
    private void Die(Area2D area)
    {
        SignalHub.EmitOnEnemyDied(GlobalPosition);
        SignalHub.EmitOnPointScored(_points);
        QueueFree();
    }
}
