using Godot;

public partial class HighScoreDisplay : VBoxContainer
{

    [Export] private Label _scoreLabel;
    [Export] private Label _dateLabel;

    private HighScore _highScore;
    
        
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override void _Ready()
    {
        if (_highScore == null)
        {
            QueueFree();
        }
        else
        {
            _scoreLabel.Text = _highScore.Score.ToString("D5");
            _dateLabel.Text = _highScore.DateScored;
        }
    }
    
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    public void SetHighScore(HighScore highScore)
    {
        _highScore = highScore;
    }
}
