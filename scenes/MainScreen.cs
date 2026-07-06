using Godot;

public partial class MainScreen : Control
{

    [Export] private PackedScene _highSoreScene;
    [Export] private GridContainer _highScoreContainer;
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        GetTree().Paused = false;
        ScoreManager.Instance.CachedScore = 0;
        SetupScores();
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
            GameManager.ChangeToNextLevel();
        }
    }

    //* ________________________________________________________________________________________________
    //* SUB METHODS:

    private void SetupScores()
    {
        foreach (var score in ScoreManager.Instance.ScoresHistory.Scores)
        {
            var highscoreDisplay = _highSoreScene.Instantiate<HighScoreDisplay>();
            highscoreDisplay.SetHighScore(score);
            _highScoreContainer.AddChild(highscoreDisplay);
        }
        {
            
        }
    }
}
