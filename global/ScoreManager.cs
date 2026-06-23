using Godot;

public partial class ScoreManager : Node
{
    public ScoreManager Instance { get; private set; }

    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        Instance = this;
    }
}
