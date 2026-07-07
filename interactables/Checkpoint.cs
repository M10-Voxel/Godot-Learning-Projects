using Godot;

public partial class Checkpoint : Area2D
{
    // EXPORTS:
    [Export] private VisibleOnScreenNotifier2D _onScreenNotifier;
    [Export] private AnimationTree _animationTree;
    
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override void _Ready()
    {
        _onScreenNotifier.ScreenEntered += OnScreenEntered;
        AreaEntered += OnCheckpointTouched;
    }

    
    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:
    
    /// <summary>
    /// When touching the checkpoint emit signal to complete level
    /// </summary>
    /// <param name="area">This checkpoint</param>
    private void OnCheckpointTouched(Area2D area)
    {
        SignalHub.EmitOnLevelCompleted(true);
        AreaEntered -= OnCheckpointTouched;
    }

    /// <summary>
    /// Set new state to show open animation and then waving animation.
    /// Also start detecting bodies (entering, exiting)
    /// </summary>
    private void OnScreenEntered()
    {
        _animationTree.Set("parameters/conditions/boss_killed", true);
        Monitoring = true;
    }
}

