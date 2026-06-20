using Godot;

namespace TappyPlane.globals;

public partial class SignalHub : Node
{
    public static SignalHub Instance { get; private set; }
    
    [Signal] public delegate void OnTappyDiedEventHandler();
    [Signal] public delegate void OnPointScoredEventHandler();


    public override void _Ready()
    {
        Instance = this;
    }

    public static void EmitOnTappyDied()
    {
        Instance.EmitSignal(SignalName.OnTappyDied);
    }

    public static void EmitOnPointScored()
    {
        Instance.EmitSignal(SignalName.OnPointScored);
    }
}
