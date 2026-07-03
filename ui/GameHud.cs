using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameHud : Control
{

    [Export] private ColorRect _overlayScreen;
    [Export] private Label _score;
    [Export] private Label _state;
    [Export] private AudioStreamPlayer2D _winSound;
    [Export] private AudioStreamPlayer2D _loseSound;
    [Export] private Timer _completeTimer;
    [Export] private Timer _pauseGameTimer;
    [Export] private HBoxContainer _heartContainer;
    
    
    
    
    private bool _canContinue = false;
    private int _scoreValue = 0;
    private List<TextureRect> _hearts = [];
    
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        SignalHub.Instance.OnLevelCompleted += OnLevelCompleted;
        SignalHub.Instance.OnPointScored += OnPointScored;
        SignalHub.Instance.OnPlayerHit += OnPlayerHit;
        _completeTimer.Timeout += OnCompleteTimerTimeout;
        _pauseGameTimer.Timeout += () => GetTree().Paused = true;
        UpdateScoreLabel(_scoreValue);
        _hearts = _heartContainer.GetChildren().OfType<TextureRect>().ToList();
    }

    public override void _ExitTree()
    {
        SignalHub.Instance.OnLevelCompleted -= OnLevelCompleted;
        SignalHub.Instance.OnPointScored -= OnPointScored;
        SignalHub.Instance.OnPlayerHit -= OnPlayerHit;
    }
    
    public override void _Input(InputEvent @event)
    {
        // When quit (Esc) is pressed - Change to Main Screen
        if (@event.IsActionPressed("quit"))
        {
            GameManager.ChangeToMainScreen();
        }

        if (@event.IsActionPressed("shoot") && _canContinue)
        {
            GameManager.ChangeToMainScreen();
        }
    }
    
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    private void UpdateScoreLabel(int points)
    {
        _score.Text = "Score: " + _scoreValue;
    }

    
    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:

    private void OnCompleteTimerTimeout()
    {
        _canContinue = true;
        GD.Print("Set Can Continue to " + _canContinue);
    }

    private void OnLevelCompleted(bool isCompleted)
    {
        if (isCompleted)
        {
            _state.Text = "Level Completed";
            _winSound.Play();
        }
        else
        {
            _state.Text = "Game Over";
            _loseSound.Play();
        }
        
        _overlayScreen.Show();
        
        _completeTimer.Start();
        _pauseGameTimer.Start();
        
    }

    private void OnPointScored(int points)
    {
        _scoreValue += points;
        UpdateScoreLabel(_scoreValue);
    }

    private void OnPlayerHit(int lives, bool shake)
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].Visible = lives > i;
        }
        if (lives <= 0)
        {
            OnLevelCompleted(false);
        }
    }
}
