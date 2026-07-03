using Godot;

public partial class MovingPlatformPath : PathFollow2D
{

    [Export] private float _speed = 100.0f;
    
    
    
        
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:


    public override void _PhysicsProcess(double delta)
    {
        Progress += _speed * (float)delta;
    }
}
