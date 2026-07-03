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


    // CONSTS:
    private float _gravity = 800.0f;
    private float _fallenOff = 200.0f;
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

    private void AssignPlayerRef()
    {
        PlayerRef = GetTree().GetFirstNodeInGroup(GameConstants.GroupPlayer) as Player;
        if (PlayerRef == null)
        {
            GD.PrintErr("No Player Ref");
            QueueFree();
        }
    }

    private void EnemyFallenOff()
    {
        if (GlobalPosition.Y > _fallenOff)
        {
            CallDeferred(Node.MethodName.QueueFree);
        }
    }


    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    protected async void DelayInitialAnimation()
    {
        AnimatedSprite.Stop();

        float delay = GD.Randf() * _maxAnimationStartDelay;
        await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);

        AnimatedSprite.Play();

    }

    protected Vector2 ApplyGravity(double delta)
    {
        Vector2 velocity = Velocity;
        velocity.Y += _gravity * (float)delta;
        return velocity;
    }

    protected void FlipSprite()
    {
        AnimatedSprite.FlipH = PlayerRef.GlobalPosition.X > GlobalPosition.X;
    }


    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:

    protected virtual void OnScreenEntered()
    {
        Timer.Start();
        _screenNotifier.ScreenEntered -= OnScreenEntered;
    }

    protected virtual void OnTimerTimeout() {}

    private void Die(Area2D area)
    {
        SignalHub.EmitOnEnemyDied(GlobalPosition);
        SignalHub.EmitOnPointScored(_points);
        QueueFree();
    }
}
