using Godot;

public partial class ScoreManager : Node
{
    
    // PROPERTIES:
    public static ScoreManager Instance { get; private set; }
    public HighScores ScoresHistory { get; private set; } = new HighScores();
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

    // Used to generate clearer messages in console
    private static void LogInfo(string msg)
    {
        GD.Print($"[ScoreManager] {msg}");
    }

    public void AddScore(int score)
    {
        LogInfo($"AddScore | {score}");
        LogInfo($"AddScore | pre-add score count: {ScoresHistory.Scores.Count}");
        ScoresHistory.AddNewScore(score);
        LogInfo($"AddScore | done score count: {ScoresHistory.Scores.Count}");
        SaveScoresToFile();
    }

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

    private void SaveScoresToFile()
    {
        Error error = ResourceSaver.Save(ScoresHistory, ScoreFilePath);

        LogInfo(error == Error.Ok ? "SaveScoresToFile | ok" : "SaveScoresToFile | failed");
    }
}
