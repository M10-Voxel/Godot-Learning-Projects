using System.Threading.Tasks;
using Godot;

public partial class MovingPlatformAnimated : AnimatableBody2D
{

    [Export] private Marker2D _from;
    [Export] private Marker2D _to;
    [Export] private float _speed = 50.0f;


    public override async void _Ready()
    {
        if (_from == null || _to == null) QueueFree();
        
        GlobalPosition = _from.GlobalPosition;
        
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        
        MoveToTarget(_to.GlobalPosition);
    }



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
