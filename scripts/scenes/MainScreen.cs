using Godot;
using TappyPlane.globals;

namespace TappyPlane.scripts.scenes;

public partial class MainScreen : Control
{
    [Export] private Label _highScoreLabel;
    
   public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            GameManager.LoadGame();
        }
    }

    public override void _Ready()
    {
        GetTree().Paused = false;
        _highScoreLabel.Text = ScoreManager.Instance.HighSore.ToString();
    }
}
