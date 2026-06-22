using Godot;

public partial class SignalHub : Node
{
    // EXPORTS:
    [Signal] public delegate void OnAnimalDiedEventHandler();
    [Signal] public delegate void OnAttemptMadeEventHandler();
    [Signal] public delegate void OnCupDestroyedEventHandler(int numRemaining);
    
    // INSTANCE:
    public static SignalHub Instance { get; private set; }

    //* ________________________________________________________________________________________________
    //* STANDARD GODOT METHODS:
    
    public override void _Ready()
    {
        Instance = this;
    }

    //* ________________________________________________________________________________________________
    //* SIGNAL EMITTER:
    
    public static void EmitOnAnimalDied()
    {
        Instance.EmitSignalOnAnimalDied();
    }

    public static void EmitOnAttemptMade()
    {
        Instance.EmitSignalOnAttemptMade();
    }

    public static void EmitOnCupDestroyed(int numRemaining)
    {
        Instance.EmitSignalOnCupDestroyed(numRemaining);
    }

}
