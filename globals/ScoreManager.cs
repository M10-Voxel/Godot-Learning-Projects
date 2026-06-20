using Godot;
using TappyPlane.classes;

namespace TappyPlane.globals;

public partial class ScoreManager : Node
{
    private const string ScoreFilePath = "user://tappy.res";
    public static ScoreManager Instance { get; private set; }

    private int _highScore = 0;
    
    public int HighSore
    {
        get => _highScore;
        set
        {
            if (value > _highScore)
            {
                SaveScoreToFile();
                _highScore = value;
            }
        }
    }

    public override void _Ready()
    {
        Instance = this;
        LoadScoreFromFile();
    }

    private void SaveScoreToFile()
    {
        var hrs = new HighScoreResource();
        hrs.HighScore = _highScore;
        ResourceSaver.Save(hrs, ScoreFilePath);
    }

    private void LoadScoreFromFile()
    {
        if (!ResourceLoader.Exists(ScoreFilePath)) return;
        
        var hsr = ResourceLoader.Load<HighScoreResource>(ScoreFilePath);
        if (hsr != null) _highScore = hsr.HighScore;
    }
}
