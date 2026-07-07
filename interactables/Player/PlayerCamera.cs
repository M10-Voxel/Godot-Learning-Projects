using Godot;

public partial class PlayerCamera : Camera2D
{
    // EXPORTS:
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
        // Changes X and Y coordinate of Camera each frame, creating a camera shake effect
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

    /// <summary>
    /// When shake is true, set Process to true, enabling the camera shake.
    /// Then create a timer, after its timeout Process is stopped, with that the camera shake
    /// Then Camera is reset, to ensure it's at the root position
    /// </summary>
    /// <param name="lives">Irrelevant</param>
    /// <param name="shake">The boolean to determine if camera should shake</param>
    private async void ShakeCamera(int lives, bool shake)
    {
        if (!shake) return;
        SetProcess(true);
        
        await ToSignal(GetTree().CreateTimer(_shakeDuration), Timer.SignalName.Timeout);
        
        SetProcess(false);
        Offset = Vector2.Zero;
    }
}
