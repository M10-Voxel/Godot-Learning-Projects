using Godot;

/// <summary>
/// Moves a platform from one position to another using tween.
/// This is "safer" than using a path: Player position aligns better with the platform.
/// </summary>
public partial class MovingPlatformAnimated : AnimatableBody2D
{
    // EXPORTS:
    [Export] private Marker2D _from;
    [Export] private Marker2D _to;
    [Export] private float _speed = 50.0f;

	
	
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override async void _Ready()
    {
        if (_from == null || _to == null)
        {
            QueueFree();
            return;
        }
        
        GlobalPosition = _from.GlobalPosition;
        
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        
        MoveToTarget(_to.GlobalPosition);
    }

    
    //* ________________________________________________________________________________________________
    //* SUB METHODS:

    /// <summary>
    /// Recursive method to move from one position to another. On completion, calls itself and moves to next/previous target.
    /// </summary>
    /// <param name="target">The current position targeted for the animation</param>
    private void MoveToTarget(Vector2 target)
    {
        float totalTime = GlobalPosition.DistanceTo(target) / _speed;
        
        Tween tween = CreateTween();

        tween.TweenProperty(
            this, 
            Node2D.PropertyName.GlobalPosition.ToString(),
            target,
            totalTime);

        tween.Finished += () =>
        {
            Vector2 nextTarget = target == _to.GlobalPosition ? _from.GlobalPosition : _to.GlobalPosition;
            MoveToTarget(nextTarget);
        };
    }
}
