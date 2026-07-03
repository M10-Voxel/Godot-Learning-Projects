using Godot;

public partial class BallSpike : PathFollow2D
{

    [Export] private float _speed = 50.0f;
    [Export] private float _rotationSpeed = 300.0f;
    
        
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override void _Process(double delta)
    {
        Progress += _speed * (float)delta;
        RotationDegrees += _rotationSpeed * (float)delta;
    }
}
