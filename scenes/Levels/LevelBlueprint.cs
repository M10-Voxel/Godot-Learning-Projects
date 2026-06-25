using Godot;

public partial class LevelBlueprint : Node
{
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        // When quit (Esc) is pressed - Change to Main Screen
        if (@event.IsActionPressed("quit"))
        {
            GameManager.ChangeToMainScreen();
        }
    }
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:
}
