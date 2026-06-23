using Godot;

public partial class SignalHub : Node
{
    public SignalHub Instance { get; private set; }

    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        Instance = this;
    }
    
    
    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:
}
