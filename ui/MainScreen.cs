using Godot;

public partial class MainScreen : Control
{
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // When quit (Esc) is pressed - Change to Level Scene
        if (@event.IsActionPressed("quit"))
        {
            GameManager.ChangeToLevel();
        }
    }

    //* ________________________________________________________________________________________________
    //* OWN METHODS:
}
