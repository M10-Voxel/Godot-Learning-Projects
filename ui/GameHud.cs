using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameHud : Control
{

    [Export] private ColorRect _overlayScreen;
    [Export] private Label _score;
    [Export] private Label _state;
    [Export] private Label _level;
    [Export] private Label _pressButton;
    [Export] private AudioStreamPlayer2D _winSound;
    [Export] private AudioStreamPlayer2D _loseSound;
    [Export] private Timer _completeTimer;
    [Export] private Timer _pauseGameTimer;
    [Export] private HBoxContainer _heartContainer;
    
    
    
    
    private bool _canContinue = false;
    private bool _completedLevel = false;
    private int _scoreValue = 0;
    private List<TextureRect> _hearts = [];
    
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        GetTree().Paused = false;
        SignalHub.Instance.OnLevelCompleted += OnLevelCompleted;
        SignalHub.Instance.OnPointScored += OnPointScored;
        SignalHub.Instance.OnPlayerHit += OnPlayerHit;
        _completeTimer.Timeout += OnCompleteTimerTimeout;
        _pauseGameTimer.Timeout += () => GetTree().Paused = true;
        _hearts = _heartContainer.GetChildren().OfType<TextureRect>().ToList();
        _level.Text = $"Level: {GameManager.Instance.CurrentLevel + 1}";
        _scoreValue = ScoreManager.Instance.CachedScore;
        UpdateScoreLabel(_scoreValue);
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

        // When shoot (Left Click) is pressed - Change to Next Level or Main Screen depending on level completion
        if (@event.IsActionPressed("shoot") && _canContinue)
        {
            if (_completedLevel) GameManager.ChangeToNextLevel();
            else                 GameManager.ChangeToMainScreen();
        }

        // When next (E) is pressed - Change to Next Level
        if (@event.IsActionPressed("next"))
        {
            GameManager.ChangeToNextLevel();
        }

        // When reload (R) is pressed - Reload Level
        if (@event.IsActionPressed("reload"))
        {
            GameManager.ReloadLevel();
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
            _completedLevel = true;
            ScoreManager.Instance.CachedScore = _scoreValue;
            _state.Text = "Level Completed";
            _pressButton.Text = "Left Click to continue";
            _winSound.Play();
        }
        else
        {
            _state.Text = "Game Over";
            _pressButton.Text = "press R to restart";
            _loseSound.Play();
            ScoreManager.Instance.AddScore(_scoreValue);
            ScoreManager.Instance.CachedScore = 0;
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
