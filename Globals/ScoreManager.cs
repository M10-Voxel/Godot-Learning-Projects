using Godot;
using System;

public partial class ScoreManager : Node
{
	public static ScoreManager Instance { get; private set; }

	private const string ScoresPath = "user://animals.res";

	public LevelScores LevelScores { get; private set; } = new();
	
	public static int LevelSelected { get; set; } = 1;
	
	public override void _Ready()
	{
		Instance = this;
		LoadScoresFromFile();
	}

	
	public static int GetBestScoreForLevel(int levelNumber) => Instance.LevelScores.GetBestScore(levelNumber);
	
	public static void SetScoreForCurrentLevel(int score)
	{
		Instance.LevelScores.SetBestScore( LevelSelected, score);
		Instance.SaveScoresToFile();
	}

	private void LoadScoresFromFile()
	{
		LevelScores = new LevelScores();
		if (ResourceLoader.Exists(ScoresPath))
		{
			var data = ResourceLoader.Load<LevelScores>(ScoresPath);
			if (data != null) LevelScores = data;
		}
	}

	private void SaveScoresToFile()
	{
		Error err = ResourceSaver.Save(LevelScores, ScoresPath);
		if (err != Error.Ok) GD.PrintErr("Failed to save Scores: " + err);
	}

}

