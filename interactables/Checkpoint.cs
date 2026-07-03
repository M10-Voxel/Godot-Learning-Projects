using Godot;

public partial class Checkpoint : Area2D
{
    
    [Export] private VisibleOnScreenNotifier2D _onScreenNotifier;
    [Export] private AnimationTree _animationTree;
    
    
    
    
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override void _Ready()
    {
        _onScreenNotifier.ScreenEntered += OnScreenEntered;
        _animationTree.AnimationFinished += OnAnimationFinished;
        AreaEntered += OnCheckpointTouched;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }


    private void OnCheckpointTouched(Area2D area)
    {
        SignalHub.EmitOnLevelCompleted(true);
        AreaEntered -= OnCheckpointTouched;
    }

    private void OnScreenEntered()
    {
        _animationTree.Set("parameters/conditions/boss_killed", true);
        Monitoring = true;
    }
    
    private void OnAnimationFinished(StringName animName)
    {
        if (animName == "open")
        {
            SetDeferred(Area2D.PropertyName.Monitoring, true);
        }
    }
}

