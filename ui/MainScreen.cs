using Godot;

public partial class MainScreen : Control
{
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        GetTree().Paused = false;
    }

    public override void _Input(InputEvent @event)
    {
        // When quit (Esc) is pressed - Change to Level Scene
        if (@event.IsActionPressed("quit"))
        {
            GetTree().Quit();
        }
        if (@event.IsActionPressed("shoot"))
        {
            GameManager.ChangeToLevel();
        }
    }

    //* ________________________________________________________________________________________________
    //* OWN METHODS:
}
