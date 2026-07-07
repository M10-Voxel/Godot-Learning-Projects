using Godot;

/// <summary>
/// A singleton node that manages the player's score
/// </summary>
public partial class ScoreManager : Node
{
    
    // PROPERTIES:
    public static ScoreManager Instance { get; private set; }
    public HighScores ScoresHistory { get; private set; } = new();
    public int CachedScore { get; set; }

    // CONSTANTS:
    private const string ScoreFilePath = "user://foxy.res";
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override void _EnterTree()
    {
        LoadScoresFromFile();
    }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        SaveScoresToFile();
    }


    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    /// <summary>
    /// Used to generate clearer messages in console
    /// </summary>
    /// <param name="msg">The message added to the log</param>
    private static void LogInfo(string msg)
    {
        GD.Print($"[ScoreManager] {msg}");
    }

    /// <summary>
    /// Adds a new score to the existing HighScores list
    /// </summary>
    /// <param name="score">The integer value of the score to add</param>
    public void AddScore(int score)
    {
        LogInfo($"AddScore | {score}");
        LogInfo($"AddScore | pre-add score count: {ScoresHistory.Scores.Count}");
        ScoresHistory.AddNewScore(score);
        LogInfo($"AddScore | done score count: {ScoresHistory.Scores.Count}");
        SaveScoresToFile();
    }
    
    /// <summary>
    /// Loads the HighScores list from the saved file
    /// </summary>
    private void LoadScoresFromFile()
    {
        LogInfo("LoadScoresFromFile");

        if (!ResourceLoader.Exists(ScoreFilePath))
        {
            LogInfo("LoadScoresFromFile | !ResourceLoader.Exists");
            return;
        }
        
        HighScores highScores = ResourceLoader.Load<HighScores>(ScoreFilePath);
        if(highScores != null)
        {
            ScoresHistory = highScores;
            LogInfo($"LoadScoresFromFile | Load ok, score count: {ScoresHistory.Scores.Count}");
        }
        else
        {
            LogInfo("LoadScoresFromFile | Load failed");
        }
    }

    /// <summary>
    /// Saves the HighScores list to a file in given directory
    /// </summary>
    private void SaveScoresToFile()
    {
        Error error = ResourceSaver.Save(ScoresHistory, ScoreFilePath);

        LogInfo(error == Error.Ok ? "SaveScoresToFile | ok" : "SaveScoresToFile | failed");
    }
}
