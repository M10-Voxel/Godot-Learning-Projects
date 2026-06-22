using Godot;

public partial class Cup : StaticBody2D
{
    [Export] private AnimationPlayer _vanishAnimation;
    
    public static int NumCoups { get; set; } = 0;

    public override void _Ready()
    {
        NumCoups++;
    }
    
    public void Die()
    {
        _vanishAnimation.Play("vanish");
    }
    
    private void RemoveCup()
    {
        QueueFree();
        NumCoups--;
        SignalHub.EmitOnCupDestroyed(NumCoups);
    }
}
