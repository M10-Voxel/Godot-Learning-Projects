using Godot;

public partial class PlayerCamera : Camera2D
{
    [Export] private double _shakeAmount = 5.0;
    [Export] private double _shakeDuration = 0.3;
        
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        SignalHub.Instance.OnPlayerHit += ShakeCamera;
        SetProcess(false);
    }

    public override void _Process(double delta)
    {
        Offset = new Vector2(
            (float)GD.RandRange(-_shakeAmount, _shakeAmount),
            (float)GD.RandRange(-_shakeAmount, _shakeAmount)
        );
    }

    public override void _ExitTree()
    {
        SignalHub.Instance.OnPlayerHit -= ShakeCamera;
    }

    
    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:



    private async void ShakeCamera(int lives, bool shake)
    {
        if (!shake) return;
        SetProcess(true);
        
        await ToSignal(GetTree().CreateTimer(_shakeDuration), Timer.SignalName.Timeout);
        
        SetProcess(false);
        Offset = Vector2.Zero;
    }
}
