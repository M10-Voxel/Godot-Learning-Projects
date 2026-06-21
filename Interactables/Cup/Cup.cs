using Godot;

public partial class Cup : StaticBody2D
{
    [Export] private AnimationPlayer _vanishAnimation;

    public void Die()
    {
        _vanishAnimation.Play("vanish");
    }
    
    private void RemoveCup()
    {
        QueueFree();
    }
}
