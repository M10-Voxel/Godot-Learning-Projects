using Godot;

/// <summary>
/// Visual scene to display each block for the Hiscore consisting of score and date
/// </summary>
public partial class HighScoreDisplay : VBoxContainer
{
    // EXPORTS:
    [Export] private Label _scoreLabel;
    [Export] private Label _dateLabel;

    // PRIVATE VARIABLES:
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

    /// <summary>
    /// Assigns given HighScore to local variable
    /// to be used in _Ready to assign the score and date to the labels
    /// </summary>
    /// <param name="highScore">The passed in HighScore to be used</param>
    public void SetHighScore(HighScore highScore)
    {
        _highScore = highScore;
    }
}
