using Godot;
using TappyPlane.globals;

namespace TappyPlane.scripts.components;

public partial class GameUi : Control
{

    [Export] private Label _gameOverLabel;
    [Export] private Label _pressSpaceLabel;
    [Export] private Label _scoreLabel;
    
    [Export] private Timer _timer;
    [Export]private AudioStreamPlayer _gameOverSound;
    
    private int _score = 0;
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel")) GameManager.LoadMainScreen();

        if (_pressSpaceLabel.IsVisible() && @event.IsActionPressed("ui_accept"))
        {
            GameManager.LoadMainScreen();
            
            GD.Print("New Run");
            if (_score > ScoreManager.Instance.HighSore) GD.Print($"New High Score: {_score}");
        }
    }

    public override void _Ready()
    {
        SignalHub.Instance.OnTappyDied += OnTappyDied;
        SignalHub.Instance.OnPointScored += OnPointScored;
    }

    public override void _ExitTree()
    {
        SignalHub.Instance.OnTappyDied -= OnTappyDied;
        SignalHub.Instance.OnPointScored -= OnPointScored;
    }

    private void OnTappyDied()
    {
        _gameOverLabel.Show();
        _gameOverSound.Play();
        _timer.Start();
    }

    private void OnPointScored()
    {
        ++_score;
        GD.Print($"Score: {_score}");
        _scoreLabel.Text = _score.ToString();
        ScoreManager.Instance.HighSore = _score;
    }

    private void OnTimerTimeout()
    {
        _gameOverLabel.Hide();
        _pressSpaceLabel.Show();
    }

}
