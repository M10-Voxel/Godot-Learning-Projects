using Godot;
using System;

public partial class LevelButton : TextureButton
{
	[Export] private int _levelNumber = 1;
	[Export] private Label _levelLabel;
	[Export] private Label _attemptsLabel;

	public override void _Ready()
	{
		_levelLabel.Text = _levelNumber.ToString();
		_attemptsLabel.Text = ScoreManager.GetBestScoreForLevel(_levelNumber).ToString();
	}

	private void OnMouseEntered()
	{
		Scale = new Vector2(1.1f, 1.1f);
	}
	
	private void OnMouseExited()
	{
		Scale = new Vector2(1.0f, 1.0f);
	}



	private void OnPressed()
	{
		ScoreManager.LevelSelected = _levelNumber;
		GetTree().ChangeSceneToFile($"res://Scenes/Level{_levelNumber}.tscn");
	}
}
