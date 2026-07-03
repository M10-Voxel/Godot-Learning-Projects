using Godot;

public partial class MovingPlatformAnimated : AnimatableBody2D
{

    [Export] private Marker2D _from;
    [Export] private Marker2D _to;
    [Export] private float _speed = 50.0f;


    public override void _Ready()
    {
        if (_from == null || _to == null) QueueFree();
        
        GlobalPosition = _from.GlobalPosition;
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
    }
}
