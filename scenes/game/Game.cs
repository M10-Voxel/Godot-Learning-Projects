using DiceCatcher.components.Dice;
using DiceCatcher.components.Fox;
using Godot;

namespace DiceCatcher.scenes.Game;

public partial class Game : Node2D
{
	[Export] private PackedScene _diceScene;
	[Export] private Fox _fox;
	[Export] private Label _score;
	[Export] private AudioStreamPlayer2D _audioPlayer;
	[Export] private Timer _spawnTimer;

	private const string StoppableGroup = "stoppable";
	private const float Margin = 80.0f;
	private readonly AudioStream _gameOverSound = GD.Load<AudioStream>("res://assets/sounds/game_over.wav");
	private int _points = 0;

	
	public override void _Ready()
	{
		_fox.PointScored += OnPointScored;
		SpawnDice();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("restart"))
		{
			GetTree().ReloadCurrentScene();
		}
	}

	private void SpawnDice()
	{
		Dice newDice = _diceScene.Instantiate<Dice>();

		Rect2 vpr = GetViewportRect();
		float newX = (float)GD.RandRange(vpr.Position.X + Margin, vpr.End.X - Margin);
		newDice.Position = new Vector2( newX, -Margin);
		newDice.GameOver += GameOver;	// Assign every new dice the signal handler
		AddChild(newDice);
	}
	
	private void OnPointScored()
	{
		_points++;
		_score.Text = _points.ToString();
		GD.Print($"Points: {_points}");
	}

	
	private void GameOver()
	{
		GD.Print("Game Over!");
		_audioPlayer.Stop();
		_audioPlayer.Stream = _gameOverSound;
		_audioPlayer.Play();
		StopAll();
	}
	
	private void StopAll()
	{
		_spawnTimer.Stop();
		
		var toStop = GetTree().GetNodesInGroup(StoppableGroup);
		foreach (Node node in toStop)
		{
			node.SetPhysicsProcess(false);
			node.SetProcess(false);
		}
	}
}
