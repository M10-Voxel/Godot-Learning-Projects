using System;
using Godot;

public partial class ScoreManager : Node
{
    public ScoreManager Instance { get; private set; }
    public HighScores ScoresHistory { get; private set; } = new HighScores();
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        Instance = this;
        MakeScores();
    }








    public void LogInfo(string msg)
    {
        GD.Print($"[ScoreManager] {msg}");
    }

    private void MakeScores()
    {
        Random rng = new Random();

        for (int i = 0; i < 12; i++)
        {
            var randNum = rng.Next(10 , 200);
            ScoresHistory.AddNewScore(randNum);
            LogInfo($"Added score: {randNum}");
        }
        
        LogInfo("Scores Created");

        foreach (var score in ScoresHistory.Scores)
        {
            LogInfo($"Score: {score.Score}");
        }
    }
}
